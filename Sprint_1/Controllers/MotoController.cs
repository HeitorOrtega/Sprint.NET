```csharp
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sprint_1.DTOs;
using Sprint_1.Helpers;
using Sprint_1.Models;

namespace Sprint_1.Controllers
{
    [ApiController]
    [Route("v1/motos")]
    public class MotoController : ControllerBase
    {
        private readonly IMotoService _service;

        public MotoController(IMotoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todas as motos com suporte a paginação.
        /// </summary>
        /// <param name="parameters">Parâmetros de paginação (pageNumber, pageSize)</param>
        /// <returns>Lista paginada de motos</returns>
        [HttpGet(Name = "GetMotos")]
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

            var hateoas = items.Select(m => CriarLinks(m)).ToList();

            var paginationLinks = new List<LinkMoto>
            {
                new LinkMoto { Href = Url.Link("GetMotos", new { pageNumber = 1, pageSize = parameters.PageSize }), Rel = "first", Method = "GET" },
                new LinkMoto { Href = Url.Link("GetMotos", new { pageNumber = Math.Max(1, parameters.PageNumber - 1), pageSize = parameters.PageSize }), Rel = "prev", Method = "GET" },
                new LinkMoto { Href = Url.Link("GetMotos", new { pageNumber = Math.Min(totalPages, parameters.PageNumber + 1), pageSize = parameters.PageSize }), Rel = "next", Method = "GET" },
                new LinkMoto { Href = Url.Link("GetMotos", new { pageNumber = totalPages, pageSize = parameters.PageSize }), Rel = "last", Method = "GET" }
            };

            return Ok(new { Data = hateoas, Pagination = paginationLinks });
        }

        /// <summary>
        /// Retorna uma moto pelo ID.
        /// </summary>
        [HttpGet("{id}", Name = "GetMotoById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MotoHateoasDto>> GetPorId(long id)
        {
            var moto = await _service.GetByIdAsync(id);
            if (moto == null) return NotFound();
            return Ok(CriarLinks(moto));
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        [HttpPost(Name = "CreateMoto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MotoHateoasDto>> Criar(MotoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Placa) || string.IsNullOrWhiteSpace(dto.Cor))
                return BadRequest("Cor e Placa são obrigatórios.");

            var entity = new Moto
            {
                Cor = dto.Cor,
                Placa = dto.Placa,
                DataFabricacao = dto.DataFabricacao
            };

            var created = await _service.CreateAsync(entity);
            return CreatedAtRoute("GetMotoById", new { id = created.Id }, CriarLinks(created));
        }

        /// <summary>
        /// Atualiza uma moto existente.
        /// </summary>
        [HttpPut("{id}", Name = "UpdateMoto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MotoHateoasDto>> Atualizar(long id, MotoUpdateDto dto)
        {
            var entity = new Moto
            {
                Cor = dto.Cor,
                Placa = dto.Placa,
                DataFabricacao = dto.DataFabricacao
            };

            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null) return NotFound();
            return Ok(CriarLinks(updated));
        }

        /// <summary>
        /// Deleta uma moto pelo ID.
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteMoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletar(long id)
        {
            var removed = await _service.DeleteAsync(id);
            if (!removed) return NotFound();
            return NoContent();
        }

        private MotoHateoasDto CriarLinks(Moto moto)
        {
            var dto = new MotoHateoasDto
            {
                Id = moto.Id,
                Cor = moto.Cor,
                Placa = moto.Placa,
                DataFabricacao = moto.DataFabricacao
            };

            dto.Links.Add(new LinkMoto { Href = Url.Link("GetMotoById", new { id = moto.Id }), Rel = "self", Method = "GET" });
            dto.Links.Add(new LinkMoto { Href = Url.Link("UpdateMoto", new { id = moto.Id }), Rel = "update", Method = "PUT" });
            dto.Links.Add(new LinkMoto { Href = Url.Link("DeleteMoto", new { id = moto.Id }), Rel = "delete", Method = "DELETE" });
            dto.Links.Add(new LinkMoto { Href = Url.Link("GetMotos", null), Rel = "all", Method = "GET" });

            return dto;
        }
    }
}
```
