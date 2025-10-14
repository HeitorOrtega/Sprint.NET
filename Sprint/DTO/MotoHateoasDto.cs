using System;
using System.Collections.Generic;

namespace Sprint.DTOs
{
    /// <summary>
    /// Moto com links HATEOAS.
    /// </summary>
    public class MotoHateoasDto
    {
        /// <example>1</example>
        public long Id { get; set; }
        /// <example>Preto</example>
        public string Cor { get; set; }
        /// <example>123SHCV</example>
        public string Placa { get; set; }
        /// <example>2025-10-01</example>
        public DateTime DataFabricacao { get; set; }

        public List<LinkMoto> Links { get; set; } = new();
        public string? Versao { get; set; }  
    }
    /// <summary>
    /// Representa um link HATEOAS.
    /// </summary>
    public class LinkMoto   
    {
        /// <example>http://localhost:5000/v1/motos/1</example>
        public string Href { get; set; }
        /// <example>self</example>
        public string Rel { get; set; }
        /// <example>GET</example>
        public string Method { get; set; }
    }
}