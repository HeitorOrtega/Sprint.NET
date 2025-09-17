namespace Sprint_1.DTOs

{   
    public class LinkMoto
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }

    public class MotoHateoasDto
    {   
        public long Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public string Placa { get; set; }
        
        public List<LinkMoto> Links { get; set; } = new List<LinkMoto>();
    }
        
}