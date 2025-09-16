using Sprint_1.Models;

namespace Sprint_1.DTOs
{
    public class Link
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }

    public class PatioHateoasDto
    {
        public long Id { get; set; }
        public string Localizacao { get; set; }

        public List<Link> Links { get; set; } = new List<Link>();
    }
}