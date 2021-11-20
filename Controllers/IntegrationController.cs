using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIMyDelivery
{
    [ApiController]
    [Route("[controller]")]
    public class IntegrationController : Controller
    {
        private IConfiguration _config;
        public IntegrationController(IConfiguration configuration)
        {
            _config = configuration;
        }
        VendedorBLL vendedorBLL = new VendedorBLL();

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody]ModelVendedorUsuario model)
        {
            using (SqlConnection conexao = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")))
            {
                vendedorBLL.Autenticacao(model.login, model.senha, conexao, out string erro);

                if (erro.Contains("Erro"))
                    return BadRequest("Erro para consultar token! " + erro);

                else if(erro.Contains("Login"))
                    return NoContent() + erro;

                else if(erro.Contains("Senha"))
                    return NoContent() + erro;

                else
                {
                    model.role = erro;
                    var token = TokenService.GenerateToken(model);
                    model.senha = "";

                    return new
                    {
                        user = model.login,
                        token = token
                    };
                }

            } 
        }
    }
}
