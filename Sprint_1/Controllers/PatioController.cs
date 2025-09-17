using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Sprint_1.DTOs;
using Sprint_1.Models;
using Sprint_1.Helpers;
using System.Text.Json;

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

            var paginationMetadata = new
            {
                totalCount,
                pageSize = parameters.PageSize,
                currentPage = parameters.PageNumber,
                totalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var patios = await query
                .OrderBy(p => p.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var hateoasPatios = patios.Select(p => CriarLinks(p)).ToList();

            var paginationLinks = new List<LinkPatio>
            {
                new LinkPatio
                {
                    Href = Url.Link("GetPatios", new { pageNumber = 1, pageSize = parameters.PageSize }),
                    Rel = "first",
                    Method = "GET"
                },
                new LinkPatio
                {
                    Href = Url.Link("GetPatios", new { pageNumber = Math.Max(1, parameters.PageNumber - 1), pageSize = parameters.PageSize }),
                    Rel = "prev",
                    Method = "GET"
                },
                new LinkPatio
                {
                    Href = Url.Link("GetPatios", new { pageNumber = Math.Min(totalPages, parameters.PageNumber + 1), pageSize = parameters.PageSize }),
                    Rel = "next",
                    Method = "GET"
                },
                new LinkPatio
                {
                    Href = Url.Link("GetPatios", new { pageNumber = totalPages, pageSize = parameters.PageSize }),
                    Rel = "last",
                    Method = "GET"
                }
            };

            return Ok(new
            {
                Data = hateoasPatios,
                Pagination = paginationLinks
            });
        }

        [HttpGet("{id}", Name = "GetPatioById")]
        public async Task<ActionResult<PatioHateoasDto>> GetPorId(long id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            return Ok(CriarLinks(patio));
        }

        [HttpPost(Name = "CreatePatio")]
        public async Task<ActionResult<PatioHateoasDto>> Criar([FromBody] PatioCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Localizacao))
                return BadRequest("Localização é obrigatória.");

            var novoPatio = new Patio
            {
                Localizacao = dto.Localizacao
            };

            _context.Patios.Add(novoPatio);
            await _context.SaveChangesAsync();

            var dtoComLinks = CriarLinks(novoPatio);

            return CreatedAtRoute("GetPatioById", new { id = novoPatio.Id }, dtoComLinks);
        }

        [HttpPut("{id}", Name = "UpdatePatio")]
        public async Task<ActionResult<PatioHateoasDto>> Atualizar(long id, [FromBody] PatioUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Localizacao))
                return BadRequest("Localização é obrigatória.");

            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            patio.Localizacao = dto.Localizacao;

            _context.Patios.Update(patio);
            await _context.SaveChangesAsync();

            return Ok(CriarLinks(patio));
        }

        [HttpDelete("{id}", Name = "DeletePatio")]
        public async Task<IActionResult> Deletar(long id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private PatioHateoasDto CriarLinks(Patio patio)
        {
            var dto = new PatioHateoasDto
            {
                Id = patio.Id,
                Localizacao = patio.Localizacao
            };

            dto.Links.Add(new LinkPatio()
            {
                Href = Url.Link("GetPatioById", new { id = patio.Id }),
                Rel = "self",
                Method = "GET"
            });

            dto.Links.Add(new LinkPatio()
            {
                Href = Url.Link("UpdatePatio", new { id = patio.Id }),
                Rel = "update",
                Method = "PUT"
            });

            dto.Links.Add(new LinkPatio()
            {
                Href = Url.Link("DeletePatio", new { id = patio.Id }),
                Rel = "delete",
                Method = "DELETE"
            });

            dto.Links.Add(new LinkPatio()
            {
                Href = Url.Link("GetPatios", null),
                Rel = "all",
                Method = "GET"
            });

            return dto;
        }
    }
}
