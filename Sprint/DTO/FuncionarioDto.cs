namespace Sprint.DTOs
{
    /// <summary>
    /// Representa um funcionário retornado pela API.
    /// </summary>
    public class FuncionarioDto
    {
        /// <example>1</example>
        public long Id { get; set; }

        /// <example>João da Silva</example>
        public string Nome { get; set; }

        /// <example>12345678900</example>
        public string Cpf { get; set; }

        /// <example>joao@email.com</example>
        public string Email { get; set; }

        /// <example>MG1234567</example>
        public string Rg { get; set; }

        /// <example>(31) 99999-9999</example>
        public string Telefone { get; set; }

        /// <example>1</example>
        public long? PatioId { get; set; }
    }

    /// <summary>
    /// Dados necessários para criar um funcionário.
    /// </summary>
    public class FuncionarioCreateDto
    {
        /// <example>Maria Oliveira</example>
        public string Nome { get; set; }

        /// <example>98765432100</example>
        public string Cpf { get; set; }

        /// <example>maria.oliveira@email.com</example>
        public string Email { get; set; }

        /// <example>SP1234567</example>
        public string Rg { get; set; }

        /// <example>(11) 98888-7777</example>
        public string Telefone { get; set; }

        /// <example>2</example>
        public long? PatioId { get; set; }
    }

    /// <summary>
    /// Dados para atualizar um funcionário.
    /// </summary>
    public class FuncionarioUpdateDto
    {
        /// <example>Maria Oliveira</example>
        public string Nome { get; set; }

        /// <example>98765432100</example>
        public string Cpf { get; set; }

        /// <example>maria.oliveira@email.com</example>
        public string Email { get; set; }

        /// <example>SP1234567</example>
        public string Rg { get; set; }

        /// <example>(11) 98888-7777</example>
        public string Telefone { get; set; }

        /// <example>2</example>
        public long? PatioId { get; set; }
    }
}
