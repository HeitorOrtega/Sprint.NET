using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_1.DTO;
using Sprint_1.Models;
using Sprint_1.Data; 

[ApiController]
[Route("api/[controller]")]
public class PatioController : ControllerBase
{
    private readonly AppDbContext _context;

    public PatioController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Patio>>> GetAll([FromQuery] long? funcionarioId = null)
    {
        var query = _context.Patios
            .Include(p => p.Funcionarios)
            .Include(p => p.Motos)
            .AsQueryable();

        if (funcionarioId.HasValue)
        {
            query = query.Where(p => p.Funcionarios.Any(f => f.Id == funcionarioId.Value));
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patio>> GetById(long id)
    {
        var patio = await _context.Patios
            .Include(p => p.Funcionarios)
            .Include(p => p.Motos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (patio == null)
            return NotFound();

        return patio;
    }

    [HttpPost]
    public async Task<ActionResult<Patio>> Create([FromBody] PatioDTO patioDto)
    {
        var funcionarios = await _context.Funcionarios
            .Where(f => patioDto.FuncionariosIds.Contains(f.Id))
            .ToListAsync();

        var motos = await _context.Motos
            .Where(m => patioDto.MotosIds.Contains(m.Id))
            .ToListAsync();

        var novoPatio = new Patio
        {
            Funcionarios = funcionarios,
            Motos = motos
        };

        _context.Patios.Add(novoPatio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = novoPatio.Id }, novoPatio);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] PatioDTO patioDto)
    {
        var patio = await _context.Patios
            .Include(p => p.Funcionarios)
            .Include(p => p.Motos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (patio == null)
            return NotFound();

        patio.Funcionarios = await _context.Funcionarios
            .Where(f => patioDto.FuncionariosIds.Contains(f.Id))
            .ToListAsync();

        patio.Motos = await _context.Motos
            .Where(m => patioDto.MotosIds.Contains(m.Id))
            .ToListAsync();

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var patio = await _context.Patios.FindAsync(id);
        if (patio == null)
            return NotFound();

        _context.Patios.Remove(patio);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
