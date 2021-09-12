using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
//using System.Web.Http;

namespace WebAPIMyDelivery
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : Controller
    {

        private IConfiguration _config;
        public ClienteController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet("todos")]
        public IEnumerable<ModelCliente> GetEstados()
        {
            using (SqlConnection conexao = new SqlConnection(
            _config.GetConnectionString("ExemplosDapper")))
            {
                return conexao.Query<ModelCliente>(
                "SELECT E.SiglaEstado, E.NomeEstado, E.NomeCapital, " +
                "R.NomeRegiao " +
                "FROM dbo.Estados E " +
                "INNER JOIN dbo.Regioes R ON R.IdRegiao = E.IdRegiao " +
                "ORDER BY E.NomeEstado");
            }
        }

        [HttpGet("dd")]
        public System.Web.Http.IHttpActionResult gettets()
        {
            using (SqlConnection conexao = new SqlConnection(
            _config.GetConnectionString("ExemplosDapper")))
            {
                var ersultado = conexao.Query<ModelCliente>(
                "SELECT E.SiglaEstado, E.NomeEstado, E.NomeCapital, " +
                "R.NomeRegiao " +
                "FROM dbo.Estados E " +
                "INNER JOIN dbo.Regioes R ON R.IdRegiao = E.IdRegiao " +
                "ORDER BY E.NomeEstado");
                
                return (System.Web.Http.IHttpActionResult)ersultado;
            }

        }

    }
}
