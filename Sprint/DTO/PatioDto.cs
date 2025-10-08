using System.ComponentModel.DataAnnotations;

namespace Sprint.DTOs
{
    /// <summary>
    /// Representa um pátio retornado pela API.
    /// </summary>
    public class PatioDto
    {
        /// <example>1</example>
        public long Id { get; set; }

        /// <example>São Paulo - Zona Leste</example>
        public string Localizacao { get; set; }
    }

    /// <summary>
    /// DTO para criação de pátio.
    /// </summary>
    public class PatioCreateDto
    {
        [Required]
        /// <example>Rio de Janeiro - Centro</example>
        public string Localizacao { get; set; }
    }

    /// <summary>
    /// DTO para atualização de pátio.
    /// </summary>
    public class PatioUpdateDto
    {
        [Required]
        /// <example>Curitiba - Boqueirão</example>
        public string Localizacao { get; set; }
    }
}
