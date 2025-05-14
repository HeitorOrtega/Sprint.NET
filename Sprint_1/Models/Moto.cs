using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint_1.Models
{
    public class Moto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Cor { get; set; }

        [Required]
        public string Placa { get; set; }

        public DateTime DataFabricacao { get; set; }

        public List<Patio> Patios { get; set; }

        public Chaveiro Chaveiro { get; set; }
    }
}