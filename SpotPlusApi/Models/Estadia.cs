using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpotPlusApi.Models
{
    public class Estadia
    {
        [Key]
        public int Id {get; set;}
        public int VeiculoId {get; set;}
        public int EstacionamentoId {get; set;}
        public DateTime DataEntrada {get; set;}
        public DateTime? DataSaida {get; set;}
        public int TempoMinutos {get; set;}
        public decimal ValorTotal {get; set;}
        public string Status {get; set;} = "Ativo";
    }
}