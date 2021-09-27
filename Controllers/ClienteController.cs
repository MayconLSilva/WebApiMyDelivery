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

        [HttpGet("all")]
        public IEnumerable<ModelCliente> GetClientes()
        {
            using (SqlConnection conexao = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")))
            {
                return conexao.Query<ModelCliente>(
                "select *from cliente");

                //return conexao.GetAll<Estado>();
            }
        }

        [HttpGet("detalhes/{parametro}")]
        public IEnumerable<ModelCliente> GetClienteID(string parametro)
        {
            using (SqlConnection conexao = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")))
            {
                return conexao.Query<ModelCliente>(
                $"select *from cliente where id = {parametro}");
                
                
                //return conexao.Get<ModelCliente>(parametro);
            }
        }

        [HttpPost]
        public IEnumerable<ModelCliente> PostCliente(ModelCliente modelCliente)
        {
            using (SqlConnection conexao = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")))
            {
                return conexao.Query<ModelCliente>(
                $@"select *from cliente where id = ", new Dictionary<object, string>()
                                    {{"",""},
                                     {"",""}
                                    });


                //return conexao.Get<ModelCliente>(parametro);
            }

        }


    }
}
