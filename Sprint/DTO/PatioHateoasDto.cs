namespace Sprint.DTOs
{
    /// <summary>
    /// Patio com links HATEOAS.
    /// </summary>
    public class LinkPatio
    {
        /// <example>http://localhost:5000/v1/patios/1</example>
        public string Href { get; set; }
        /// <example>self</example>
        public string Rel { get; set; }
        /// <example>GET</example>
        public string Method { get; set; }
    }

    public class PatioHateoasDto
    {
        /// <example>1</example>
        public long Id { get; set; }
        public string Localizacao { get; set; }

        public List<LinkPatio> Links { get; set; } = new List<LinkPatio>();
    }
}