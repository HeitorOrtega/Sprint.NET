using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Sprint_1.DTOs;
using Sprint_1.Models;
using Sprint_1.Helpers;


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

        [HttpGet(Name = "GetPatios")]
        public async Task<ActionResult<IEnumerable<PatioHateoasDto>>> GetTodos([FromQuery] QueryParameters parameters)
        {
            var query = _context.Patios.AsQueryable();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            Response.Headers.Add("X-Pagination", totalPages.ToString());

            var patios = await query
                .OrderBy(p => p.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var hateoasPatios = new List<PatioHateoasDto>();
            foreach (var patio in patios)
            {
                var dto = new PatioHateoasDto
                {
                    Id = patio.Id,
                    Localizacao = patio.Localizacao
                };

                dto.Links.Add(new Link
                {
                    Href = Url.Link("GetPatioById", new { id = patio.Id }),
                    Rel = "self",
                    Method = "GET"
                });

                hateoasPatios.Add(dto);
            }

            return Ok(hateoasPatios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatioDto>> GetPorId(long id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            return Ok(new PatioDto
            {
                Id = patio.Id,
                Localizacao = patio.Localizacao
            });
        }

        [HttpPost]
        public async Task<ActionResult<PatioDto>> Criar([FromBody] PatioCreateDto dto)
        {
            var novoPatio = new Patio
            {
                Localizacao = dto.Localizacao
            };

            _context.Patios.Add(novoPatio);
            await _context.SaveChangesAsync();

            var patioCriadoDTO = new PatioDto
            {
                Id = novoPatio.Id,
                Localizacao = novoPatio.Localizacao
            };

            return CreatedAtAction(nameof(GetPorId), new { id = novoPatio.Id }, patioCriadoDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(long id, [FromBody] PatioUpdateDto dto)
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
