using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sprint_1.DTOs;
using Sprint_1.Helpers;
using Sprint_1.Models;

namespace Sprint_1.Controllers
{
    [ApiController]
    [Route("v1/patios")]
    public class PatioController : ControllerBase
    {
        private readonly IPatioService _service;

        public PatioController(IPatioService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetPatios")]
        public async Task<ActionResult> GetTodos([FromQuery] QueryParameters parameters)
        {
            var (items, totalCount) = await _service.GetAllAsync(parameters);
            var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            var paginationMetadata = new
            {
                totalCount,
                pageSize = parameters.PageSize,
                currentPage = parameters.PageNumber,
                totalPages
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var hateoas = items.Select(p => CriarLinks(p)).ToList();

            return Ok(hateoas);
        }

        [HttpGet("{id}", Name = "GetPatioById")]
        public async Task<ActionResult<PatioHateoasDto>> GetPorId(long id)
        {
            var patio = await _service.GetByIdAsync(id);
            if (patio == null) return NotFound();
            return Ok(CriarLinks(patio));
        }

        [HttpPost(Name = "CreatePatio")]
        public async Task<ActionResult<PatioHateoasDto>> Criar(PatioCreateDto dto)
        {
            var entity = new Patio
            {
                Localizacao = dto.Localizacao
            };

            var created = await _service.CreateAsync(entity);
            return CreatedAtRoute("GetPatioById", new { id = created.Id }, CriarLinks(created));
        }

        [HttpPut("{id}", Name = "UpdatePatio")]
        public async Task<ActionResult<PatioHateoasDto>> Atualizar(long id, PatioUpdateDto dto)
        {
            var entity = new Patio
            {
                Localizacao = dto.Localizacao
            };

            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null) return NotFound();
            return Ok(CriarLinks(updated));
        }

        [HttpDelete("{id}", Name = "DeletePatio")]
        public async Task<IActionResult> Deletar(long id)
        {
            var removed = await _service.DeleteAsync(id);
            if (!removed) return NotFound();
            return NoContent();
        }

        private PatioHateoasDto CriarLinks(Patio patio)
        {
            var dto = new PatioHateoasDto
            {
                Id = patio.Id,
                Localizacao = patio.Localizacao
            };

            dto.Links.Add(new LinkPatio { Href = Url.Link("GetPatioById", new { id = patio.Id }), Rel = "self", Method = "GET" });
            dto.Links.Add(new LinkPatio { Href = Url.Link("UpdatePatio", new { id = patio.Id }), Rel = "update", Method = "PUT" });
            dto.Links.Add(new LinkPatio { Href = Url.Link("DeletePatio", new { id = patio.Id }), Rel = "delete", Method = "DELETE" });
            dto.Links.Add(new LinkPatio { Href = Url.Link("GetPatios", null), Rel = "all", Method = "GET" });

            return dto;
        }
    }
}
