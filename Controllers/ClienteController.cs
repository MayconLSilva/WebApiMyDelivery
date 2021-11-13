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
        Util util = new Util();

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

        

        [HttpPost("insert")]
        public IActionResult InserirCliente(ModelCliente objCliente)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                   var resultClientes = clienteBLL.InserirCliente(connection, objCliente, out string erro);

                    if (erro == "")
                        return Ok(resultClientes);

                    else
                        return BadRequest(erro);                                    
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro de conexão com o servidor! " + ex);
            }            
        }
        
        [HttpPut("update/{parametro}")]
        public IActionResult AtualizaCliente(int parametro,ModelCliente objCliente)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    clienteBLL.AtualizarCliente(connection, parametro,objCliente, out string erro);

                    if (erro == "")
                        return Ok();

                    else
                        return BadRequest(erro);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro para acessar o servidor! " + ex);
            }
        }

        [HttpDelete("delete/{parametro}")]
        public IActionResult DeletarCliente(int parametro)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    clienteBLL.DeletarCliente(connection, parametro, out string erro);

                    if (erro == "")
                        return Ok();
                    
                    else
                        return BadRequest(erro);

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{parametro}")]
        public IActionResult GetClienteID(string parametro)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")))
                {
                    var resultCliente = clienteBLL.ClientePorID(conexao, int.Parse(parametro), out string erro);

                    if (erro == "")
                        return Ok(resultCliente);
                    else
                        return BadRequest(erro);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro de conexão com o servidor! " + ex);
            }
        }

        [HttpGet("all")]
        public IActionResult RetornaListaClientes()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {

                    var RequesID = Request.Headers.TryGetValue("x-api-key", out var value);
                    util.validaEmpresa(connection,value, out string erro);
                    if(erro != "")
                        return BadRequest(erro);

                    var resultClientes = connection.Query<ModelCliente>("select *from cliente");

                    if (resultClientes == null)
                        return NoContent();

                    else
                        return Ok(resultClientes);                    
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao conectar ao servidor! " + ex);//StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet("Date/{parametro}")]
        public IActionResult RetornaListaClientesPorDataCadastroAlteracao(DateTime parametro)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var resultClientes = clienteBLL.RetornaListaClientes(connection, null, parametro, out string erros);

                    if (erros != "")
                        return BadRequest("Erro ao buscar cliente pelo vendedor!" + erros);
                    
                    else
                    {
                        if(resultClientes.Count == 0 || resultClientes == null)
                        {
                            return NoContent();
                        }
                        else
                        {
                            return Ok(resultClientes);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao conectar ao servidor! " +  ex);
            }
        }

        [HttpGet("salesman/{parametro}")]
        public IActionResult RetornaListaClientesPorVendedor(int parametro)
        {            
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                   var resultClientes = clienteBLL.RetornaListaClientes(connection, parametro, null, out string erros);

                    if (erros != "")
                        return BadRequest("Erro ao buscar clientes por vendedor! " + erros);

                    else
                    {
                        if (resultClientes.Count == 0 || resultClientes == null)
                            return NoContent();
                        else
                            return Ok(resultClientes);
                    }   
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao conectar ao servidor! " +  ex);
            }
        }

        [HttpGet("salesman/{parametro}/Date/{parametro2}")]
        public IActionResult RetornaListaClientesPorVendedorDataCadastroAlteracao(int parametro, DateTime parametro2)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var resultClientes = clienteBLL.RetornaListaClientes(connection, parametro, parametro2, out string erros);

                    if (erros != "")
                        return BadRequest("Erro ao buscar lista de cliente por vendedor e data! " + erros);
                    else
                    {
                        if (resultClientes.Count == 0 || resultClientes == null)
                            return NoContent();
                        else
                            return Ok(resultClientes);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao conectar ao servidor! " +  ex);
            }
        }

        [HttpGet("address/all")]
        public IActionResult RetornaListaClientesComplementar()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var retultClientes = clienteBLL.RetornaListaClientesEndereco(connection, null, null, out string erros);

                    if (erros == "")
                    {
                        if (retultClientes == null || retultClientes.Count == 0)
                            return NoContent();

                        else
                            return Ok(retultClientes);
                    }

                    else
                        return BadRequest(erros);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro de conexão com o servidor! " + ex);

            }
        }

    }
}
