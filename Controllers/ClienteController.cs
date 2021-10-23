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
        public IEnumerable<ModelCliente> GetClienteID(string parametro, string parametro2)
        {
            using (SqlConnection conexao = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")))
            {
                return conexao.Query<ModelCliente>(
                $"select *from cliente where id = {parametro}");


                //return conexao.Get<ModelCliente>(parametro);
            }
        }



        [HttpPost("insert")]
        public IActionResult InserirCliente(ModelCliente objCliente)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    try
                    {
                        var resultClientes = clienteBLL.InserirCliente(connection, objCliente, out string erro);

                        return Ok(resultClientes);

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
        

        [HttpGet("address/all")]
        public IActionResult RetornaListaClientesComplementar()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    try
                    {                        
                        var retultClientes = clienteBLL.RetornaListaClientesEndereco(connection, null, null);

                        if (retultClientes == null || retultClientes.Count == 0)
                        {
                            return NoContent();
                        }
                        else
                        {
                            return Ok(retultClientes);
                        }

                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500,ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);

            }
        }

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
                        var resultClientes = clienteBLL.RetornaListaClientes(connection, parametro, null, out string erros);

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

        [HttpGet("salesman/{parametro}/Date/{parametro2}")]
        public IActionResult RetornaListaClientesPorVendedorDataCadastroAlteracao(int parametro, DateTime parametro2)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    try
                    {
                        var resultClientes = clienteBLL.RetornaListaClientes(connection, parametro, parametro2, out string erros);

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

        [HttpGet("Date/{parametro}")]
        public IActionResult RetornaListaClientesPorDataCadastroAlteracao(DateTime parametro)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    try
                    {
                        var resultClientes = clienteBLL.RetornaListaClientes(connection, null, parametro, out string erros);

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
