using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpotPlusApi.Models
{
    public class Veiculo
    {
        [Key]
        public int Id {get; set;}
        public int ClienteId {get; set;}
        [Required]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "A placa deve ter 7 caracteres.")]
        public string Placa {get; set;}
        [Required]
        public string Tipo {get; set;} //Carro ou Moto
    }
}