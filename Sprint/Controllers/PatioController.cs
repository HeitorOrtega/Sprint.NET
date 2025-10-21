using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sprint.DTOs;
using Sprint.Helpers;
using Sprint.Models;

namespace Sprint.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PatioController : ControllerBase
    {
        private readonly IPatioService _service;

        public PatioController(IPatioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todos os pátios com paginação e HATEOAS.
        /// </summary>
        /// <param name="parameters">Parâmetros de paginação (pageNumber, pageSize)</param>
        /// <response code="200">Lista de pátios com links HATEOAS e metadados de paginação</response>
        [HttpGet(Name = "GetPatios")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PatioHateoasDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatioHateoasDto>>>> GetTodos([FromQuery] QueryParameters parameters)
        {
            var (items, totalCount) = await _service.GetAllAsync(parameters);
            var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            var hateoas = items.Select(p => CriarLinks(p)).ToList();

            var response = new ApiResponse<IEnumerable<PatioHateoasDto>>
            {
                Data = hateoas,
                Message = "Pátios recuperados com sucesso"
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(new
            {
                totalCount,
                pageSize = parameters.PageSize,
                currentPage = parameters.PageNumber,
                totalPages
            }));

            return Ok(response);
        }

        /// <summary>
        /// Retorna um pátio pelo ID.
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <response code="200">Pátio encontrado com links HATEOAS</response>
        /// <response code="404">Pátio não encontrado</response>
        [HttpGet("{id}", Name = "GetPatioById")]
        [ProducesResponseType(typeof(ApiResponse<PatioHateoasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PatioHateoasDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<PatioHateoasDto>>> GetPorId(long id)
        {
            var patio = await _service.GetByIdAsync(id);
            if (patio == null)
                return NotFound(new ApiResponse<PatioHateoasDto> { Success = false, Message = "Pátio não encontrado" });

            var response = new ApiResponse<PatioHateoasDto>
            {
                Data = CriarLinks(patio),
                Message = "Pátio recuperado com sucesso"
            };
            return Ok(response);
        }

        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        /// <param name="dto">Dados do pátio</param>
        /// <response code="201">Pátio criado</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost(Name = "CreatePatio")]
        [ProducesResponseType(typeof(ApiResponse<PatioHateoasDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<PatioHateoasDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<PatioHateoasDto>>> Criar(PatioCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Localizacao))
                ModelState.AddModelError(nameof(dto.Localizacao), "A Localização é obrigatória para criar um pátio.");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<PatioHateoasDto>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var entity = new Patio { Localizacao = dto.Localizacao };
            var created = await _service.CreateAsync(entity);

            var response = new ApiResponse<PatioHateoasDto>
            {
                Data = CriarLinks(created),
                Message = "Pátio criado com sucesso"
            };

            return CreatedAtRoute("GetPatioById", new { id = created.Id }, response);
        }

        /// <summary>
        /// Atualiza um pátio existente.
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <param name="dto">Novos dados</param>
        /// <response code="200">Pátio atualizado</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Pátio não encontrado</response>
        [HttpPut("{id}", Name = "UpdatePatio")]
        [ProducesResponseType(typeof(ApiResponse<PatioHateoasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PatioHateoasDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PatioHateoasDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<PatioHateoasDto>>> Atualizar(long id, PatioUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Localizacao))
                ModelState.AddModelError(nameof(dto.Localizacao), "A Localização é obrigatória para atualizar um pátio.");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<PatioHateoasDto>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var entity = new Patio { Localizacao = dto.Localizacao };
            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null)
                return NotFound(new ApiResponse<PatioHateoasDto> { Success = false, Message = "Pátio não encontrado" });

            var response = new ApiResponse<PatioHateoasDto>
            {
                Data = CriarLinks(updated),
                Message = "Pátio atualizado com sucesso"
            };

            return Ok(response);
        }

        /// <summary>
        /// Deleta um pátio pelo ID.
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <response code="204">Pátio deletado</response>
        /// <response code="404">Pátio não encontrado</response>
        [HttpDelete("{id}", Name = "DeletePatio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletar(long id)
        {
            var removed = await _service.DeleteAsync(id);
            if (!removed)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Pátio não encontrado" });

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
