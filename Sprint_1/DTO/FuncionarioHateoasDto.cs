namespace Sprint_1.DTOs
{
    /// <summary>
    /// Representa um link HATEOAS.
    /// </summary>
    public class LinkFuncionario
    {
        /// <example>http://localhost:5000/v1/funcionarios/1</example>
        public string Href { get; set; } = string.Empty;

        /// <example>self</example>
        public string Rel { get; set; } = string.Empty;

        /// <example>GET</example>
        public string Method { get; set; } = string.Empty;
    }

    /// <summary>
    /// Funcionário com links HATEOAS.
    /// </summary>
    public class FuncionarioHateoasDto
    {
        /// <example>1</example>
        public long Id { get; set; }

        /// <example>João da Silva</example>
        public string Nome { get; set; } = string.Empty;

        /// <example>12345678900</example>
        public string Cpf { get; set; } = string.Empty;

        /// <example>joao@email.com</example>
        public string Email { get; set; } = string.Empty;

        /// <example>MG1234567</example>
        public string Rg { get; set; }

        /// <example>(31) 99999-9999</example>
        public string Telefone { get; set; }

        /// <example>1</example>
        public long? PatioId { get; set; }

        public List<LinkFuncionario> Links { get; set; } = new List<LinkFuncionario>();
    }
}
