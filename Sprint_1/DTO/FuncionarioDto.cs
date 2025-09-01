namespace Sprint_1.DTOs
{
    public class FuncionarioDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Rg { get; set; }
        public string Telefone { get; set; }

        public long? PatioId { get; set; } 
    }

    public class FuncionarioCreateDto
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Rg { get; set; }
        public string Telefone { get; set; }

        public long? PatioId { get; set; } 
    }

    public class FuncionarioUpdateDto
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Rg { get; set; }
        public string Telefone { get; set; }

        public long? PatioId { get; set; } 
    }
}