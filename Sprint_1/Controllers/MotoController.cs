using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Sprint_1.DTO;
using Sprint_1.Models;

namespace Sprint_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> GetAll([FromQuery] string? cor = null)
        {
            var query = _context.Motos.Include(m => m.Chaveiro).Include(m => m.Patios).AsQueryable();

            if (!string.IsNullOrEmpty(cor))
            {
                query = query.Where(m => m.Cor.ToLower() == cor.ToLower());
            }

            return Ok(await query.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetById(long id)
        {
            var moto = await _context.Motos
                .Include(m => m.Chaveiro)
                .Include(m => m.Patios)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (moto == null)
                return NotFound();

            return Ok(moto);
        }

        [HttpPost]
        public async Task<ActionResult<Moto>> Create([FromBody] MotoDTO motoDto)
        {
            var chaveiro = await _context.Set<Chaveiro>().FindAsync(motoDto.ChaveiroId);
            var patios = await _context.Set<Patio>().Where(p => motoDto.PatioIds.Contains(p.Id)).ToListAsync();

            if (chaveiro == null || patios.Count == 0)
                return BadRequest("Chaveiro ou Pátios inválidos.");

            var novaMoto = new Moto
            {
                Cor = motoDto.Cor,
                Placa = motoDto.Placa,
                DataFabricacao = motoDto.DataFabricacao,
                Chaveiro = chaveiro,
                Patios = patios
            };

            _context.Motos.Add(novaMoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = novaMoto.Id }, novaMoto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] MotoUpdateDTO motoAtualizada)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound();

            moto.Placa = motoAtualizada.Placa;
            moto.Cor = motoAtualizada.Cor;

            _context.Motos.Update(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
