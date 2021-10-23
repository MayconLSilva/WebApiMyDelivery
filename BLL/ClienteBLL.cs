using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace WebAPIMyDelivery
{
    public class ClienteBLL
    {

        public List<ModelCliente> RetornaListaClientes(SqlConnection connection, int? vendedor, DateTime? dataCadastroAlteracao)
        {            
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
                return null;
            }
        }

        



    }
}
