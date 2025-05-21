using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Sprint_1.DTOs;
using Sprint_1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    Localizacao = p.Localizacao
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
                Localizacao = patio.Localizacao
            });
        }

        [HttpPost]
        public async Task<ActionResult<PatioDTO>> Criar([FromBody] PatioCreateDTO dto)
        {
            var novoPatio = new Patio
            {
                Localizacao = dto.Localizacao
            };

            _context.Patios.Add(novoPatio);
            await _context.SaveChangesAsync();

            var patioCriadoDTO = new PatioDTO
            {
                Id = novoPatio.Id,
                Localizacao = novoPatio.Localizacao
            };

            return CreatedAtAction(nameof(GetPorId), new { id = novoPatio.Id }, patioCriadoDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(long id, [FromBody] PatioUpdateDTO dto)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            patio.Localizacao = dto.Localizacao;

            _context.Patios.Update(patio);
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
