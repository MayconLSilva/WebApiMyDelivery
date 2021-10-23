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
                
                var resultClientes = connection.Query<ModelCliente>($"{consultaSQL}");
                

                return (List<ModelCliente>)resultClientes;

            }
            catch (Exception ex)
            {
               erros = ex.Message;
               return null;
            }
        }

        public List<ModelCliente> RetornaListaClientesEndereco(SqlConnection connection, int? vendedor, DateTime? dataCadastroAlteracao)
        {
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

                var resultClientes = connection.Query<dynamic>($"{consultaSQL}");

                AutoMapper.Configuration.AddIdentifier(typeof(ModelCliente), "id");
                AutoMapper.Configuration.AddIdentifier(typeof(ModelEnderecoCliente), "idEndereco");

                List<ModelCliente> clientes = (AutoMapper.MapDynamic<ModelCliente>(resultClientes) as IEnumerable<ModelCliente>).ToList();

                return clientes;

            }
            catch(Exception ex)
            {
                return null;
            }
        }
        
        public int InserirCliente(SqlConnection connection, ModelCliente objCliente, out string erro)
        {
            erro = string.Empty;

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
                Int32 idRetorno = Convert.ToInt32(resultClientes.Single());                

                return idRetorno;

            }
            catch(Exception ex)
            {
                erro = "Erro ao inserir cliente! " + ex;
                return 0;
            }
        }


    }
}
