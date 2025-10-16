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
    public class FuncionarioControllerV2 : ControllerBase
    {
        private readonly IFuncionarioService _service;

        public FuncionarioControllerV2(IFuncionarioService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetFuncionariosV2")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(ApiResponse<FuncionarioHateoasDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<FuncionarioHateoasDto>>>> GetTodos([FromQuery] QueryParameters parameters)
        {
            var (items, totalCount) = await _service.GetAllAsync(parameters);
            var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            var hateoas = items.Select(f =>
            {
                var dto = CriarLinks(f);
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

            var response = new ApiResponse<IEnumerable<FuncionarioHateoasDto>>
            {
                Data = hateoas,
                Message = "Funcionários recuperados com sucesso (v2)"
            };

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetFuncionarioByIdV2")]
        [MapToApiVersion("2.0")] 
        [ProducesResponseType(typeof(ApiResponse<FuncionarioHateoasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<FuncionarioHateoasDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<FuncionarioHateoasDto>>> GetPorId(long id)
        {
            var funcionario = await _service.GetByIdAsync(id);
            if (funcionario == null)
                return NotFound(new ApiResponse<FuncionarioHateoasDto> { Success = false, Message = "Funcionário não encontrado" });

            var dto = CriarLinks(funcionario);
            dto.Versao = "2.0";

            var response = new ApiResponse<FuncionarioHateoasDto>
            {
                Data = dto,
                Message = "Funcionário recuperado com sucesso (v2)"
            };
            return Ok(response);
        }

        private FuncionarioHateoasDto CriarLinks(Funcionario f)
        {
            var dto = new FuncionarioHateoasDto
            {
                Id = f.Id,
                Nome = f.Nome,
                Cpf = f.Cpf,
                Email = f.Email,
                Rg = f.Rg,
                Telefone = f.Telefone,
                PatioId = f.PatioId
            };

            dto.Links.Add(new LinkFuncionario { Href = Url.Link("GetFuncionarioByIdV2", new { version = "2.0", id = f.Id }), Rel = "self", Method = "GET" });
            dto.Links.Add(new LinkFuncionario { Href = Url.Link("GetFuncionariosV2", new { version = "2.0" }), Rel = "all", Method = "GET" });

            return dto;
        }
    }
}
