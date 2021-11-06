using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIMyDelivery
{
    [Table("vendedor")]
    public class ModelVendedorUsuario
    {
        [Key]
        public int id { get; set; }
        public string senha { get; set; }
        public string login { get; set; }
    }
}
