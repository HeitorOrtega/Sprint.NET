using System;
using System.Collections.Generic;

namespace Sprint_1.DTOs
{
    public class MotoHateoasDto
    {
        public long Id { get; set; }
        public string Cor { get; set; }
        public string Placa { get; set; }
        public DateTime DataFabricacao { get; set; }

        public List<LinkMoto> Links { get; set; } = new();
    }

    public class LinkMoto
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }
}