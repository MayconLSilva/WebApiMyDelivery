using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Slapper;

namespace WebAPIMyDelivery
{
    public class Util
    {
        public void validaEmpresa(SqlConnection sqlConection, string chaveKey, out string erro)
        {
            erro = "";           

            if (chaveKey == "" || chaveKey == null)
                erro = "A x-api-key precisa ser informada no Header";

            else
            {
                var requesEmpresa = sqlConection.Query("select *from empresa where cpf_cnpj = @cpf_cnpj",
                                                        new Dictionary<string, object>() { { "@cpf_cnpj", chaveKey.ToString().Replace("{", "").Replace("}", "") } });

                if (requesEmpresa.Count() == 0)
                    erro = "Empresa não encontrada, verifique o CPF/CNPJ! ";

                else
                    erro = "";
            }            
        }
    }
}
