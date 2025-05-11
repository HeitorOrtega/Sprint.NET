using System.Collections.Generic;

namespace Sprint_1.Models;

public class Funcionario
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rg { get; set; }
    public string Telefone { get; set; }
    public List<Patio> Patios { get; set; }
}