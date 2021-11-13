using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIMyDelivery.Model
{
    [Table("Empresa")]
    public class Empresa
    {
        [Key]
        public int id { get; set; }
        public string  razao { get; set; }
        public string fantasia { get; set; }
        public string cpf_cnpj { get; set; }
        public string rg_ie { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
    }
}
