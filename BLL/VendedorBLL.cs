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

        public void insereVendedorUsuario(SqlConnection connection, ModelVendedorUsuario modelVendedorUsuario, out string erro)
        {
            erro = "";
            Int32 idRetorno = 0;

            try
            {
                string comandoSQL = @"insert into vendedor (login, senha) values (@login, @senha);
                                      SELECT CAST(SCOPE_IDENTITY() as int)";

                var parametros = new Dictionary<string, object>() { {"@login", modelVendedorUsuario.login}, {"@senha", modelVendedorUsuario.senha} };

                var resultVendedorUsuario = connection.Query<int>(comandoSQL, parametros);

                idRetorno = Convert.ToInt32(resultVendedorUsuario.Single());
                erro = idRetorno.ToString();

            }
            catch (Exception ex)
            {
                erro = "Erro para inserir usuário/vendedor! " + ex;
            }
        }


    }
}
