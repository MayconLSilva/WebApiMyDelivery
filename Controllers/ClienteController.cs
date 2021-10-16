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
        ClienteBLL clienteBLL = new ClienteBLL();

        private IConfiguration _config;
        public ClienteController(IConfiguration configuration)
        {
            _config = configuration;
        }

        //[HttpGet("all")]
        //public IEnumerable<ModelCliente> GetClientes()
        //{
        //    using (SqlConnection conexao = new SqlConnection(
        //    _config.GetConnectionString("DefaultConnection")))
        //    {
        //        return conexao.Query<ModelCliente>(
        //        "select *from cliente");

        //        //return conexao.GetAll<Estado>();
        //    }
        //}

        [HttpGet("detalhes/{parametro}/{parametro2}")]
        public IEnumerable<ModelCliente> GetClienteID(string parametro,string parametro2)
        {
            using (SqlConnection conexao = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")))
            {
                return conexao.Query<ModelCliente>(
                $"select *from cliente where id = {parametro}");
                
                
                //return conexao.Get<ModelCliente>(parametro);
            }
        }

        //[HttpPost]
        //public IEnumerable<ModelCliente> PostCliente(ModelCliente modelCliente)
        //{
        //    using (SqlConnection conexao = new SqlConnection(
        //    _config.GetConnectionString("DefaultConnection")))
        //    {
        //        return conexao.Query<ModelCliente>(
        //        $@"select *from cliente where id = ", new Dictionary<object, string>()
        //                            {{"",""},
        //                             {"",""}
        //                            });


        //        //return conexao.Get<ModelCliente>(parametro);
        //    }

        //}


        [HttpGet("all")]
        public IActionResult RetornaListaClientes()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    try
                    {
                        var resultClientes = connection.Query<ModelCliente>("select *from cliente");

                        if (resultClientes == null)
                        {
                            return NoContent();
                        }
                        else
                        {
                            return Ok(resultClientes);
                        }

                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);//StatusCode((int)HttpStatusCode.InternalServerError, ex);

            }                        
        }

        [HttpGet("salesman/{parametro}")]
        public IActionResult RetornaListaClientesPorVendedor(int parametro)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    try
                    {
                        var resultClientes = clienteBLL.RetornaListaClientes(connection,parametro,null);

                        if (resultClientes == null)
                        {
                            return NoContent();
                        }
                        else
                        {
                            return Ok(resultClientes);
                        }

                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);

            }
        }

        //[HttpGet("salesman/Date/{parametro}/{parametro2}")]
        [HttpGet("salesman/{parametro}/Date/{parametro2}")]
        public IActionResult RetornaListaClientesPorVendedorDataCadastroAlteracao(int parametro,DateTime parametro2)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    try
                    {
                        var resultClientes = clienteBLL.RetornaListaClientes(connection, parametro, parametro2);

                        if (resultClientes == null)
                        {
                            return NoContent();
                        }
                        else
                        {
                            return Ok(resultClientes);
                        }

                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);

            }
        }


    }
}
