using System;
using System.Collections.Generic;

namespace Sprint_1.Models;

public class Moto
{
    public long Id { get; set; }
    public string Cor { get; set; }
    public string Placa { get; set; }
    public DateTime DataFabricacao { get; set; }
    public List<Patio> Patios { get; set; }
    public Chaveiro Chaveiro { get; set; }
}