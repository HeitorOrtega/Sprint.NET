using System;
using System.ComponentModel.DataAnnotations;

namespace Sprint_1.DTOs
{
    /// <summary>
    /// Representa uma moto retornada pela API.
    /// </summary>
    public class MotoDto
    {
        /// <example>1</example>
        public long Id { get; set; }
        /// <example>Preto</example>
        public string Cor { get; set; }
        /// <example>123SHCV</example>
        public string Placa { get; set; }
        /// <example>2025-10-01</example>
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