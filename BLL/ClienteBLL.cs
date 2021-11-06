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
    public class ClienteBLL
    {

        public List<ModelCliente> RetornaListaClientes(SqlConnection connection, int? vendedor, DateTime? dataCadastroAlteracao, out string erros)
        {
            erros = string.Empty;
            var resultClientes = (dynamic)null;

            try
            {
                string consultaSQL = "";
                string filtrosConsultaSQL = "";
                

                if (vendedor != null)
                    filtrosConsultaSQL += $" and v.idVendedor = {vendedor} ";

                if (dataCadastroAlteracao != null)
                    filtrosConsultaSQL += $" and c.dataCadastro = '{dataCadastroAlteracao.Value.ToShortDateString()}' or c.dataAlteracao = '{dataCadastroAlteracao.Value.ToShortDateString()}' ";
                                    
                consultaSQL = $@"set dateformat dmy 
                                 select c.id, c.nomeFantasia, c.apelidoRazao, c.cpfCnpj, c.rgIE, c.telefone, c.celular, c.email, c.dataCadastro, c.dataAlteracao 
                                 from cliente c
                                   left join vendedoresCliente v on c.id = v.idCliente
                                   where 1 = 1  {filtrosConsultaSQL}";
                
                resultClientes = connection.Query<ModelCliente>($"{consultaSQL}");

            }
            catch (Exception ex)
            {
               erros = ex.Message;               
            }

            return (List<ModelCliente>)resultClientes;
        }

        public List<ModelCliente> RetornaListaClientesEndereco(SqlConnection connection, int? vendedor, DateTime? dataCadastroAlteracao,out string erro)
        {
            erro = string.Empty;
            var resultClientes = (dynamic)null;

            try
            {
                string consultaSQL = "";
                string filtrosConsultaSQL = "";


                if (vendedor != null)
                    filtrosConsultaSQL += $" and v.idVendedor = {vendedor} ";

                if (dataCadastroAlteracao != null)
                    filtrosConsultaSQL += $" and c.dataCadastro = '{dataCadastroAlteracao.Value.ToShortDateString()}' or c.dataAlteracao = '{dataCadastroAlteracao.Value.ToShortDateString()}' ";

                consultaSQL = $@"select c.id, c.nomeFantasia, c.apelidoRazao, c.cpfCnpj, c.rgIE, c.telefone, c.celular,
                                c.email, c.dataCadastro, c.dataAlteracao, ec.idEndereco as Enderecos_idEndereco, 
	                            ec.logradouro as Enderecos_logradouro, ec.numero as Enderecos_numero,
	                            ec.bairro as Enderecos_bairro, ec.cep as Enderecos_cep, ec.complemento as Enderecos_complemento, 
	                            ec.principal as Enderecos_principal, ec.idCliente as Enderecos_idCliente, 
	                            ec.cidade as Enderecos_cidade
	                            from cliente c
	                            inner join enderecosCliente ec on ec.idCliente = c.id
                                left join vendedoresCliente v on c.id = v.idCliente
	                            where 1 = 1
                                {filtrosConsultaSQL} ";

                var chamadaPrincipal = connection.Query<dynamic>($"{consultaSQL}");

                AutoMapper.Configuration.AddIdentifier(typeof(ModelCliente), "id");
                AutoMapper.Configuration.AddIdentifier(typeof(ModelEnderecoCliente), "idEndereco");

                List<ModelCliente> mapeamento = (AutoMapper.MapDynamic<ModelCliente>(chamadaPrincipal) as IEnumerable<ModelCliente>).ToList();
                
                resultClientes = mapeamento;

            }
            catch (Exception ex)
            {
                erro = "Erro ao buscar cliente com endereço! " + ex.Message;
            }

            return resultClientes;
        }
        
        public int InserirCliente(SqlConnection connection, ModelCliente objCliente, out string erro)
        {
            erro = string.Empty;
            Int32 idRetorno = 0;

            try
            {
                string sql = $@"insert into cliente (nomeFantasia,apelidoRazao,cpfCnpj,rgIE,telefone,celular,email,dataCadastro)
                                                                values (@nomeFantasia, @apelidoRazao, @cpfCnpj, @rgIE, @telefone, @celular, @email, @dataCadastro);
                                                                SELECT CAST(SCOPE_IDENTITY() as int) ";

                var resultClientes = connection.Query<int>(sql,
                                                        new Dictionary<string, object>() { { "@nomeFantasia", objCliente.nomeFantasia.ToString() },
                                                                                                   { "@apelidoRazao", objCliente.apelidoRazao.ToString() },
                                                                                                   { "@cpfCnpj", objCliente.cpfCnpj.ToString() },
                                                                                                   { "@rgIE", objCliente.rgIE.ToString() },
                                                                                                   { "@telefone", objCliente.telefone.ToString() },
                                                                                                   { "@celular", objCliente.celular.ToString() },
                                                                                                   { "@email", objCliente.email.ToString() },
                                                                                                   { "@dataCadastro", objCliente.dataCadastro.Value.Date } });
                idRetorno = Convert.ToInt32(resultClientes.Single());                 

            }
            catch(Exception ex)
            {
                erro = "Erro ao inserir cliente! " + ex;
            }

            return idRetorno;

        }

        public void AtualizarCliente(SqlConnection connection, int parametro ,ModelCliente objCliente, out string erro)
        {
            erro = string.Empty;
            try
            {
                DateTime dataAtual = DateTime.Now;
                DateTime.TryParse(objCliente.dataAlteracao.ToString(),out dataAtual);
                

                var parametrosPassado = new Dictionary<string, object>() { {"@nomeFantasia", objCliente.nomeFantasia}, {"@apelidoRazao", objCliente.apelidoRazao},
                                                                           {"@cpfCnpj", objCliente.cpfCnpj}, {"@rgIE", objCliente.rgIE},
                                                                           {"@telefone", objCliente.telefone}, {"@celular", objCliente.celular},
                                                                           {"@email", objCliente.email}, {"@dataAlteracao", dataAtual},
                                                                           {"@id", objCliente.id} };

                string sql = $@"update cliente set nomeFantasia = @nomeFantasia, apelidoRazao = @apelidoRazao, cpfCnpj = @cpfCnpj, rgIE = @rgIE,
                   telefone = @telefone, celular = @celular, email = @email, dataAlteracao = @dataAlteracao
                   where id = @id";

                var resultCliente = connection.Query(sql, parametrosPassado);

            }
            catch(Exception ex)
            {
                erro = "Erro para atualizar cliente! " + ex;
            }
        }

        public void DeletarCliente(SqlConnection connection, int parametro, out string erro)
        {
            erro = string.Empty;
            try
            {
                if (parametro <= 0)
                {
                    erro = "Id informada é inválida";
                    return;
                }
                connection.Query($"delete from cliente where id = {parametro}");

            }
            catch (Exception ex)
            {
                erro = "Erro ao deletar cliente! " + ex;
            }
        }

        public List<ModelCliente> ClientePorID(SqlConnection connection, int parametro, out string erro)
        {
            var resultClientes = (dynamic)null;
            erro = string.Empty;

            try
            {
                string consultaSQL = "";

                consultaSQL = $"select *from cliente where id = {parametro}";

                resultClientes = connection.Query<ModelCliente>(consultaSQL);                

            }
            catch(Exception ex)
            {
                erro = "Erro ao buscar cliente pela id! " + ex.Message;
            }

            return (List<ModelCliente>)resultClientes;

        }


    }
}
