namespace Sprint_1.DTO;

public class MotoDTO
{
    public string Cor { get; set; }
    public string Placa { get; set; }
    public DateTime DataFabricacao { get; set; }
    public long ChaveiroId { get; set; }
    public List<long> PatioIds { get; set; }
}