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
            string senhaRetornada = "";

            try
            {
                using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")))
                {
                    var resultCliente = vendedorbll.getLogin(conexao, login, out string erro);

                    if (resultCliente.Count == 0)
                        return NotFound();

                    else
                    {
                        foreach(ModelVendedorUsuario item in resultCliente)
                        {
                            senhaRetornada = item.senha;
                        }

                        if (senha != senhaRetornada)
                            return BadRequest("Senha não corresponde, verifique! ");

                        else
                            return Ok(resultCliente);
                    }



                    //var senhaRetornada = resultCliente.First(x => x.login.Equals(login)).senha.ToString();

                   // var teste2 = (from item in resultCliente where item.login.Equals(login) select item.senha).First();

                    
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro de conexão com o servidor! " + ex);
            }
        }
    }
}
