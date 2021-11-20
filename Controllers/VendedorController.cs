using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIMyDelivery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendedorController : Controller
    {
        private IConfiguration _config;
        public VendedorController(IConfiguration configuration)
        {
            _config = configuration;
        }

        VendedorBLL vendedorbll = new VendedorBLL();

        [HttpGet("{login}/{senha}")]
        public IActionResult GetLogin(string login, string senha)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")))
                {
                    var resultUsuarioVendedor = vendedorbll.GetLoginUsuarioVendedor(conexao, login, out string erro);

                    if(erro == "")
                    {
                        if (senha != resultUsuarioVendedor.senha)
                            return BadRequest("Senhas não coincidem");

                        else
                            return Ok(resultUsuarioVendedor);
                    }
                    else
                    {
                        return NoContent();
                    }

                    
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro de conexão com o servidor! " + ex);
            }
        }

        [HttpPost("insereLogin")]
        public IActionResult InsertLogin(ModelVendedorUsuario modelVendedor)
        {            
            try
            {
                using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")))
                {

                    vendedorbll.insereVendedorUsuario(conexao, modelVendedor, out string erro);

                    if (erro.Contains("Erro"))
                        return BadRequest(" " + erro);

                    else
                        return Ok(erro);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro para conectar ao servidor! " + ex.Message);

            }
        }
    }
}
