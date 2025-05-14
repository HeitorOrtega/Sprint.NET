namespace Sprint_1.DTOs
{
    public class PatioDTO
    {
        public long Id { get; set; }
        public string Localizacao { get; set; }
    }

    public class PatioCreateDTO
    {
        public string Localizacao { get; set; }
    }

    public class PatioUpdateDTO
    {
        public string Localizacao { get; set; }
    }
}