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
    public class PatioControllerV2 : ControllerBase
    {
        private readonly IPatioService _service;

        public PatioControllerV2(IPatioService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetPatiosV2")]
        [MapToApiVersion("2.0")] 
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PatioHateoasDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatioHateoasDto>>>> GetTodos([FromQuery] QueryParameters parameters)
        {
            var (items, totalCount) = await _service.GetAllAsync(parameters);
            var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            var hateoas = items.Select(p =>
            {
                var dto = CriarLinks(p);
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

            var response = new ApiResponse<IEnumerable<PatioHateoasDto>>
            {
                Data = hateoas,
                Message = "Pátios recuperados com sucesso (v2)"
            };

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetPatioByIdV2")]
        [MapToApiVersion("2.0")] 
        [ProducesResponseType(typeof(ApiResponse<PatioHateoasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PatioHateoasDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<PatioHateoasDto>>> GetPorId(long id)
        {
            var patio = await _service.GetByIdAsync(id);
            if (patio == null)
                return NotFound(new ApiResponse<PatioHateoasDto> { Success = false, Message = "Pátio não encontrado" });

            var dto = CriarLinks(patio);
            dto.Versao = "2.0";

            var response = new ApiResponse<PatioHateoasDto>
            {
                Data = dto,
                Message = "Pátio recuperado com sucesso (v2)"
            };
            return Ok(response);
        }

        private PatioHateoasDto CriarLinks(Patio patio)
        {
            var dto = new PatioHateoasDto
            {
                Id = patio.Id,
                Localizacao = patio.Localizacao
            };

            dto.Links.Add(new LinkPatio { Href = Url.Link("GetPatioByIdV2", new { version = "2.0", id = patio.Id }), Rel = "self", Method = "GET" });
            dto.Links.Add(new LinkPatio { Href = Url.Link("GetPatiosV2", new { version = "2.0" }), Rel = "all", Method = "GET" });

            return dto;
        }
    }
}
