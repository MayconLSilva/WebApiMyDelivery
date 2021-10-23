//using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPIMyDelivery
{
    [Table("enderecosCliente")]
    public class ModelEnderecoCliente
    {
        //[DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        [Key]
        public int idEndereco { get; set; }
        //[Required(ErrorMessage = "O nome do usuário é obrigatório", AllowEmptyStrings = false)]
        //[Display(Name = "Nome do Usuário")]
        //[StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        
        public string cep { get; set; }
        public string complemento { get; set; }
        public bool principal { get; set; }
        [Required]         
        public  int idCliente { get; set; }
        public string cidade { get; set; }

        
    }
}
