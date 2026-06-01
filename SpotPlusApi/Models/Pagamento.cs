using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpotPlusApi.Models
{
    public class Pagamento
    {
       [Key]
       public int Id {get; set;}
       public int EstadiaId {get; set;}
       [Required]
       public string FormaPagamento {get; set;} = string.Empty;
       public decimal ValorPago {get; set;}
       public bool Pago {get; set;} = false;
       public DateTime? DataPagamento {get; set;}
    }
}