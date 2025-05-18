using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Sprint_1.DTOs;
using Sprint_1.Models;

namespace Sprint_1.Controllers
{
    [ApiController]
    [Route("v1/motos")]
    public class MotoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotoDTO>>> GetTodos()
        {
            var motos = await _context.Motos
                .Select(m => new MotoDTO
                {
                    Id = m.Id,
                    Marca = m.Cor,
                    Modelo = m.Placa,
                    Ano = m.DataFabricacao.Year,
                    Placa = m.Placa
                })
                .ToListAsync();

            return Ok(motos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MotoDTO>> GetPorId(long id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound();

            return Ok(new MotoDTO
            {
                Id = moto.Id,
                Marca = moto.Cor,
                Modelo = moto.Placa,
                Ano = moto.DataFabricacao.Year,
                Placa = moto.Placa
            });
        }

        [HttpPost]
        public async Task<ActionResult<MotoDTO>> Criar(MotoCreateDTO dto)
        {
            var novaMoto = new Moto
            {
                Cor = dto.Marca,
                Placa = dto.Placa,
                DataFabricacao = new DateTime(dto.Ano, 1, 1)
            };

            _context.Motos.Add(novaMoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId), new { id = novaMoto.Id }, novaMoto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(long id, MotoUpdateDTO dto)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound();

            moto.Cor = dto.Marca;
            moto.Placa = dto.Placa;
            moto.DataFabricacao = new DateTime(dto.Ano, 1, 1);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(long id)
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
