using System;
using System.ComponentModel.DataAnnotations;

namespace Sprint_1.DTOs
{
    public class MotoDto
    {
        public long Id { get; set; }
        public string Cor { get; set; }
        public string Placa { get; set; }
        public DateTime DataFabricacao { get; set; }
    }

    public class MotoCreateDto
    {
        [Required]
        public string Cor { get; set; }

        [Required]
        public string Placa { get; set; }

        public DateTime DataFabricacao { get; set; }
    }

    public class MotoUpdateDto
    {
        [Required]
        public string Cor { get; set; }

        [Required]
        public string Placa { get; set; }

        public DateTime DataFabricacao { get; set; }
    }
}