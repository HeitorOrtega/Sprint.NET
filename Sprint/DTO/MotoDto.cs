using System;
using System.ComponentModel.DataAnnotations;

namespace Sprint.DTOs
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

        /// <example>ABC1234</example>
        public string Placa { get; set; }

        /// <example>2025-10-01</example>
        public DateTime DataFabricacao { get; set; }
    }

    /// <summary>
    /// DTO para criação de moto.
    /// </summary>
    public class MotoCreateDto
    {
        [Required]
        /// <example>Vermelho</example>
        public string Cor { get; set; }

        [Required]
        /// <example>XYZ9876</example>
        public string Placa { get; set; }

        /// <example>2023-05-12</example>
        public DateTime DataFabricacao { get; set; }
    }

    /// <summary>
    /// DTO para atualização de moto.
    /// </summary>
    public class MotoUpdateDto
    {
        [Required]
        /// <example>Azul</example>
        public string Cor { get; set; }

        [Required]
        /// <example>LMN4321</example>
        public string Placa { get; set; }

        /// <example>2022-08-20</example>
        public DateTime DataFabricacao { get; set; }
    }
}
