using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace SpotPlusApi.Models
{
    public class Estacionamento
    {
        [Key]
        public int Id {get; set;}
        [Required]
        public string Nome {get; set;}
        [Required]
        public int LimiteCarros {get; set;}
        [Required]
        public int LimiteMotos {get; set;}
        [Required]
        public int CarrosAtivos {get; set;}
        [Required]
        public int MotosAtivas {get; set;}
    }
}