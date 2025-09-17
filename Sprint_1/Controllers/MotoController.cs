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
    [Route("v1/motos")]
    public class MotoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetMotos")]
        public async Task<ActionResult<IEnumerable<MotoHateoasDto>>> GetTodos([FromQuery] QueryParameters parameters)
        {
            var query = _context.Motos.AsQueryable();

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

            var motos = await query
                .OrderBy(m => m.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var hateoasMotos = motos.Select(m => CriarLinks(m)).ToList();

            var paginationLinks = new List<LinkMoto>
            {
                new LinkMoto { Href = Url.Link("GetMotos", new { pageNumber = 1, pageSize = parameters.PageSize }), Rel = "first", Method = "GET" },
                new LinkMoto { Href = Url.Link("GetMotos", new { pageNumber = Math.Max(1, parameters.PageNumber - 1), pageSize = parameters.PageSize }), Rel = "prev", Method = "GET" },
                new LinkMoto { Href = Url.Link("GetMotos", new { pageNumber = Math.Min(totalPages, parameters.PageNumber + 1), pageSize = parameters.PageSize }), Rel = "next", Method = "GET" },
                new LinkMoto { Href = Url.Link("GetMotos", new { pageNumber = totalPages, pageSize = parameters.PageSize }), Rel = "last", Method = "GET" }
            };

            return Ok(new { Data = hateoasMotos, Pagination = paginationLinks });
        }

        [HttpGet("{id}", Name = "GetMotoById")]
        public async Task<ActionResult<MotoHateoasDto>> GetPorId(long id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound();

            return Ok(CriarLinks(moto));
        }

        [HttpPost(Name = "CreateMoto")]
        public async Task<ActionResult<MotoHateoasDto>> Criar([FromBody] MotoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Marca) || string.IsNullOrWhiteSpace(dto.Placa))
                return BadRequest("Marca e Placa são obrigatórias.");

            var novaMoto = new Moto
            {
                Cor = dto.Marca,
                Placa = dto.Placa,
                DataFabricacao = new DateTime(dto.Ano, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            _context.Motos.Add(novaMoto);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetMotoById", new { id = novaMoto.Id }, CriarLinks(novaMoto));
        }

        [HttpPut("{id}", Name = "UpdateMoto")]
        public async Task<ActionResult<MotoHateoasDto>> Atualizar(long id, MotoUpdateDto dto)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound();

            moto.Cor = dto.Marca;
            moto.Placa = dto.Placa;
            moto.DataFabricacao = new DateTime(dto.Ano, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            await _context.SaveChangesAsync();

            return Ok(CriarLinks(moto));
        }

        [HttpDelete("{id}", Name = "DeleteMoto")]
        public async Task<IActionResult> Deletar(long id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private MotoHateoasDto CriarLinks(Moto moto)
        {
            var dto = new MotoHateoasDto
            {
                Id = moto.Id,
                Marca = moto.Cor,
                Modelo = moto.Placa,
                Ano = moto.DataFabricacao.Year,
                Placa = moto.Placa
            };

            dto.Links.Add(new LinkMoto { Href = Url.Link("GetMotoById", new { id = moto.Id }), Rel = "self", Method = "GET" });
            dto.Links.Add(new LinkMoto { Href = Url.Link("UpdateMoto", new { id = moto.Id }), Rel = "update", Method = "PUT" });
            dto.Links.Add(new LinkMoto { Href = Url.Link("DeleteMoto", new { id = moto.Id }), Rel = "delete", Method = "DELETE" });
            dto.Links.Add(new LinkMoto { Href = Url.Link("GetMotos", null), Rel = "all", Method = "GET" });

            return dto;
        }
    }
}
