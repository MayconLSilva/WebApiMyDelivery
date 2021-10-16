using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace WebAPIMyDelivery
{
    [Table("Cliente")]
    public class ModelCliente
    {
        [ExplicitKey]
        public int id { get; set; }
        public string nomeFantasia { get; set; }
        public string apelidoRazao { get; set; }
        public string cpfCnpj { get; set; }
        public string rgIE { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public DateTime ? dataCadastro { get; set; }
        public DateTime ? dataAlteracao { get; set; }

    }
}


