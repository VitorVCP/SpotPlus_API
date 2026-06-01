using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpotPlusApi.Models
{
    public class Cliente
    {
        [Key]
        public int Id {get; set;}
        [Required]
        public string Nome {get; set;}
        public decimal SaldoDebito {get; set;}
        public decimal SaldoPix {get; set;}
        public decimal LimiteCredito {get; set;}
    }
}