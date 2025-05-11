using System.Collections.Generic;

namespace Sprint_1.Models;

public class Patio
{
    public long Id { get; set; }
    public List<Funcionario> Funcionarios { get; set; }
    public List<Moto> Motos { get; set; }
}