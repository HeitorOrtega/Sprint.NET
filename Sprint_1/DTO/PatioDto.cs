namespace Sprint_1.DTOs
{
    public class PatioDto
    {
        public long Id { get; set; }
        public string Localizacao { get; set; }
    }

    public class PatioCreateDto
    {
        public string Localizacao { get; set; }
    }

    public class PatioUpdateDto
    {
        public string Localizacao { get; set; }
    }
}