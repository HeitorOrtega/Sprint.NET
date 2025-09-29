```csharp
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

        /// <summary>
        /// Retorna todos os pátios cadastrados com suporte a paginação.
        /// </summary>
        /// <param name="parameters">Parâmetros de paginação (pageNumber, pageSize)</param>
        /// <returns>Lista paginada de pátios</returns>
        [HttpGet(Name = "GetPatios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Retorna um pátio específico pelo ID.
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <returns>Dados do pátio</returns>
        [HttpGet("{id}", Name = "GetPatioById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatioHateoasDto>> GetPorId(long id)
        {
            var patio = await _service.GetByIdAsync(id);
            if (patio == null) return NotFound();
            return Ok(CriarLinks(patio));
        }

        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        /// <param name="dto">Dados do pátio a ser criado</param>
        /// <returns>Pátio criado</returns>
        [HttpPost(Name = "CreatePatio")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<PatioHateoasDto>> Criar(PatioCreateDto dto)
        {
            var entity = new Patio
            {
                Localizacao = dto.Localizacao
            };

            var created = await _service.CreateAsync(entity);
            return CreatedAtRoute("GetPatioById", new { id = created.Id }, CriarLinks(created));
        }

        /// <summary>
        /// Atualiza os dados de um pátio existente.
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <param name="dto">Novos dados do pátio</param>
        /// <returns>Pátio atualizado</returns>
        [HttpPut("{id}", Name = "UpdatePatio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Remove um pátio existente.
        /// </summary>
        /// <param name="id">ID do pátio</param>
        [HttpDelete("{id}", Name = "DeletePatio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
```
