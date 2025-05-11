using Microsoft.AspNetCore.Mvc;
using Sprint_1.DTO;
using Sprint_1.Models;


[ApiController]
[Route("api/[controller]")]
public class MotoController : ControllerBase
{
    private static List<Moto> motos = new();
    private static List<Chaveiro> chaveiros = new();
    private static List<Patio> patios = new();

    // Bloco de inicialização de exemplo
    static MotoController()
    {
        var chaveiroTeste = new Chaveiro { Id = 1, Dispositivo = "Chave123" };
        chaveiros.Add(chaveiroTeste);

        var patioTeste = new Patio
        {
            Id = 1,
            Funcionarios = new List<Funcionario>(),
            Motos = new List<Moto>()
        };
        patios.Add(patioTeste);
    }

    [HttpGet]
    public ActionResult<List<Moto>> GetAll()
    {
        return Ok(motos); // 200 OK
    }

    [HttpGet("{id}")]
    public ActionResult<Moto> GetById(int id)
    {
        var moto = motos.FirstOrDefault(m => m.Id == id);
        if (moto == null)
            return NotFound(); // 404 Not Found

        return Ok(moto); // 200 OK
    }

    [HttpPost]
    public ActionResult<Moto> Create([FromBody] MotoDTO motoDto)
    {
        var chaveiro = chaveiros.FirstOrDefault(c => c.Id == motoDto.ChaveiroId);
        var patiosSelecionados = patios.Where(p => motoDto.PatioIds.Contains(p.Id)).ToList();

        if (chaveiro == null || patiosSelecionados.Count == 0)
            return BadRequest("Chaveiro ou Pátios inválidos.");

        var novaMoto = new Moto
        {
            Id = motos.Count + 1,
            Cor = motoDto.Cor,
            Placa = motoDto.Placa,
            DataFabricacao = motoDto.DataFabricacao,
            Chaveiro = chaveiro,
            Patios = patiosSelecionados
        };

        motos.Add(novaMoto);
        return CreatedAtAction(nameof(GetById), new { id = novaMoto.Id }, novaMoto);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Moto motoAtualizada)
    {
        var moto = motos.FirstOrDefault(m => m.Id == id);
        if (moto == null)
            return NotFound(); // 404 Not Found

        if (motoAtualizada == null)
            return BadRequest(); // 400 Bad Request

        moto.Placa = motoAtualizada.Placa;
        moto.Cor = motoAtualizada.Cor;

        return NoContent(); // 204 No Content
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var moto = motos.FirstOrDefault(m => m.Id == id);
        if (moto == null)
            return NotFound(); // 404 Not Found

        motos.Remove(moto);
        return NoContent(); // 204 No Content
    }
}
