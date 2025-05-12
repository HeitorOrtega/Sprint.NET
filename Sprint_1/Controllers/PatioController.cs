using Microsoft.AspNetCore.Mvc;
using Sprint_1.DTO;
using Sprint_1.Models;

[ApiController]
[Route("api/[controller]")]
public class PatioController : ControllerBase
{
    private static List<Patio> patios = new();
    private static List<Funcionario> funcionarios = new();
    private static List<Moto> motos = new();

    [HttpGet]
    public ActionResult<List<Patio>> GetAll()
    {
        return Ok(patios);
    }

    [HttpGet("{id}")]
    public ActionResult<Patio> GetById(long id)
    {
        var patio = patios.FirstOrDefault(p => p.Id == id);
        if (patio == null)
            return NotFound();
        return Ok(patio);
    }

    [HttpPost]
    public ActionResult<Patio> Create([FromBody] PatioDTO patioDto)
    {
        var funcionariosSelecionados = funcionarios
            .Where(f => patioDto.FuncionariosIds.Contains(f.Id))
            .ToList();

        var motosSelecionadas = motos
            .Where(m => patioDto.MotosIds.Contains(m.Id))
            .ToList();

        var novoPatio = new Patio
        {
            Id = patios.Count + 1,
            Funcionarios = funcionariosSelecionados,
            Motos = motosSelecionadas
        };

        patios.Add(novoPatio);
        return CreatedAtAction(nameof(GetById), new { id = novoPatio.Id }, novoPatio);
    }dd

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var patio = patios.FirstOrDefault(p => p.Id == id);
        if (patio == null)
            return NotFound();

        patios.Remove(patio);
        return NoContent();
    }
}