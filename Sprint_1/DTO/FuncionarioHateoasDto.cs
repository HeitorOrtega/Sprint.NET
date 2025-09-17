namespace Sprint_1.DTOs
{
    public class LinkFuncionario
    {
        public string Href { get; set; } = string.Empty;
        public string Rel { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
    }

    public class FuncionarioHateoasDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rg { get; set; }
        public string Telefone { get; set; }
        public long? PatioId { get; set; }

        public List<LinkFuncionario> Links { get; set; } = new List<LinkFuncionario>();
    }
}