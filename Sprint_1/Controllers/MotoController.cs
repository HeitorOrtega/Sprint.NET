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

        [HttpGet(Name = "GetMotos")]
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

            return Ok(hateoas);
        }

        [HttpGet("{id}", Name = "GetMotoById")]
        public async Task<ActionResult<MotoHateoasDto>> GetPorId(long id)
        {
            var moto = await _service.GetByIdAsync(id);
            if (moto == null) return NotFound();
            return Ok(CriarLinks(moto));
        }

        [HttpPost(Name = "CreateMoto")]
        public async Task<ActionResult<MotoHateoasDto>> Criar(MotoCreateDto dto)
        {
            var entity = new Moto
            {
                Cor = dto.Cor,
                Placa = dto.Placa,
                DataFabricacao = dto.DataFabricacao
            };

            var created = await _service.CreateAsync(entity);
            return CreatedAtRoute("GetMotoById", new { id = created.Id }, CriarLinks(created));
        }

        [HttpPut("{id}", Name = "UpdateMoto")]
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

        [HttpDelete("{id}", Name = "DeleteMoto")]
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
