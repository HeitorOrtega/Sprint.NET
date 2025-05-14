namespace Sprint_1.DTOs
{
    public class FuncionarioDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Rg { get; set; }
        public string Telefone { get; set; }

        public long? PatioId { get; set; } 
    }

    public class FuncionarioCreateDTO
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Rg { get; set; }
        public string Telefone { get; set; }

        public long? PatioId { get; set; } 
    }

    public class FuncionarioUpdateDTO
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Rg { get; set; }
        public string Telefone { get; set; }

        public long? PatioId { get; set; } 
    }
}