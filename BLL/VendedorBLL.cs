using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Slapper;


namespace WebAPIMyDelivery
{
    public class VendedorBLL
    {
        public ModelVendedorUsuario GetLoginUsuarioVendedor(SqlConnection connection, string login, out string erro)
        {
            var resultLogin = (dynamic)null;
            erro = string.Empty;

            try
            {
                string consultaSQL = "";

                consultaSQL = $"select *from vendedor where login = '{login}'";

                resultLogin = connection.QueryFirst<ModelVendedorUsuario>(consultaSQL);

            }
            catch (Exception ex)
            {
                erro = "Erro ao buscar login ! " + ex.Message;
            }

            return resultLogin;//(List<ModelVendedorUsuario>)
        }
    }
}
