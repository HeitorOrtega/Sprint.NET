using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sprint.DTOs;
using Sprint.Helpers;
using Sprint.Models;

namespace Sprint.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MotoControllerV2 : ControllerBase
    {
        private readonly IMotoService _service;

        public MotoControllerV2(IMotoService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetMotosV2")]
        [MapToApiVersion("2.0")] // 🚨 Adicionado para mapear o método à V2
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MotoHateoasDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MotoHateoasDto>>>> GetTodos([FromQuery] QueryParameters parameters)
        {
            var (items, totalCount) = await _service.GetAllAsync(parameters);
            var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            var hateoas = items.Select(m =>
            {
                var dto = CriarLinks(m);
                dto.Versao = "2.0";
                return dto;
            }).ToList();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(new
            {
                totalCount,
                pageSize = parameters.PageSize,
                currentPage = parameters.PageNumber,
                totalPages
            }));

            var response = new ApiResponse<IEnumerable<MotoHateoasDto>>
            {
                Data = hateoas,
                Message = "Motos recuperadas com sucesso (v2)"
            };

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetMotoByIdV2")]
        [MapToApiVersion("2.0")] // 🚨 Adicionado para mapear o método à V2
        [ProducesResponseType(typeof(ApiResponse<MotoHateoasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<MotoHateoasDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<MotoHateoasDto>>> GetPorId(long id)
        {
            var moto = await _service.GetByIdAsync(id);
            if (moto == null)
                return NotFound(new ApiResponse<MotoHateoasDto> { Success = false, Message = "Moto não encontrada" });

            var dto = CriarLinks(moto);
            dto.Versao = "2.0";

            var response = new ApiResponse<MotoHateoasDto>
            {
                Data = dto,
                Message = "Moto recuperada com sucesso (v2)"
            };
            return Ok(response);
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

            // 🚨 CORREÇÃO HATEOAS: Incluindo "version = 2.0" nos links
            dto.Links.Add(new LinkMoto { Href = Url.Link("GetMotoByIdV2", new { version = "2.0", id = moto.Id }), Rel = "self", Method = "GET" });
            dto.Links.Add(new LinkMoto { Href = Url.Link("GetMotosV2", new { version = "2.0" }), Rel = "all", Method = "GET" });

            return dto;
        }
    }
}
