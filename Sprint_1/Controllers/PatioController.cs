using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Sprint_1.DTOs;
using Sprint_1.Models;

namespace Sprint_1.Controllers
{
    [ApiController]
    [Route("v1/patios")]
    public class PatioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatioDTO>>> GetTodos()
        {
            var patios = await _context.Patios
                .Select(p => new PatioDTO
                {
                    Id = p.Id,
                    Localizacao = "Localização padrão" // Placeholder, ajustar conforme o modelo
                })
                .ToListAsync();

            return Ok(patios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatioDTO>> GetPorId(long id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            return Ok(new PatioDTO
            {
                Id = patio.Id,
                Localizacao = "Localização padrão" // Ajustar se for necessário
            });
        }

        [HttpPost]
        public async Task<ActionResult<PatioDTO>> Criar(PatioCreateDTO dto)
        {
            var novoPatio = new Patio();

            _context.Patios.Add(novoPatio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId), new { id = novoPatio.Id }, novoPatio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(long id, PatioUpdateDTO dto)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(long id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
