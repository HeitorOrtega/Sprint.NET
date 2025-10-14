using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sprint.DTOs;
using Sprint.Helpers;
using Sprint.Models;

namespace Sprint.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/[controller]")]
    public class MotoController : ControllerBase
    {
        private readonly IMotoService _service;

        public MotoController(IMotoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todas as motos com suporte a paginação e HATEOAS.
        /// </summary>
        /// <param name="parameters">Parâmetros de paginação (pageNumber, pageSize)</param>
        /// <response code="200">Lista de motos com links HATEOAS e metadados de paginação</response>
        [HttpGet(Name = "GetMotos")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MotoHateoasDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MotoHateoasDto>>>> GetTodos([FromQuery] QueryParameters parameters)
        {
            var (items, totalCount) = await _service.GetAllAsync(parameters);
            var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            var hateoas = items.Select(m => CriarLinks(m)).ToList();

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
                Message = "Motos recuperadas com sucesso"
            };

            return Ok(response);
        }

        /// <summary>
        /// Retorna uma moto pelo ID.
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <response code="200">Moto encontrada com links HATEOAS</response>
        /// <response code="404">Moto não encontrada</response>
        [HttpGet("{id}", Name = "GetMotoById")]
        [ProducesResponseType(typeof(ApiResponse<MotoHateoasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<MotoHateoasDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<MotoHateoasDto>>> GetPorId(long id)
        {
            var moto = await _service.GetByIdAsync(id);
            if (moto == null)
                return NotFound(new ApiResponse<MotoHateoasDto> { Success = false, Message = "Moto não encontrada" });

            var response = new ApiResponse<MotoHateoasDto>
            {
                Data = CriarLinks(moto),
                Message = "Moto recuperada com sucesso"
            };
            return Ok(response);
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        /// <param name="dto">Dados da moto</param>
        /// <response code="201">Moto criada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost(Name = "CreateMoto")]
        [ProducesResponseType(typeof(ApiResponse<MotoHateoasDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<MotoHateoasDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<MotoHateoasDto>>> Criar(MotoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Placa))
                ModelState.AddModelError(nameof(dto.Placa), "O campo Placa é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Cor))
                ModelState.AddModelError(nameof(dto.Cor), "O campo Cor é obrigatório.");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<MotoHateoasDto>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var entity = new Moto
            {
                Cor = dto.Cor,
                Placa = dto.Placa,
                DataFabricacao = dto.DataFabricacao
            };

            var created = await _service.CreateAsync(entity);

            var response = new ApiResponse<MotoHateoasDto>
            {
                Data = CriarLinks(created),
                Message = "Moto criada com sucesso"
            };

            return CreatedAtRoute("GetMotoById", new { id = created.Id }, response);
        }

        /// <summary>
        /// Atualiza uma moto existente.
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <param name="dto">Novos dados</param>
        /// <response code="200">Moto atualizada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Moto não encontrada</response>
        [HttpPut("{id}", Name = "UpdateMoto")]
        [ProducesResponseType(typeof(ApiResponse<MotoHateoasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<MotoHateoasDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<MotoHateoasDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<MotoHateoasDto>>> Atualizar(long id, MotoUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Placa))
                ModelState.AddModelError(nameof(dto.Placa), "O campo Placa é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Cor))
                ModelState.AddModelError(nameof(dto.Cor), "O campo Cor é obrigatório.");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<MotoHateoasDto>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var entity = new Moto
            {
                Cor = dto.Cor,
                Placa = dto.Placa,
                DataFabricacao = dto.DataFabricacao
            };

            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null)
                return NotFound(new ApiResponse<MotoHateoasDto> { Success = false, Message = "Moto não encontrada" });

            return Ok(new ApiResponse<MotoHateoasDto>
            {
                Data = CriarLinks(updated),
                Message = "Moto atualizada com sucesso"
            });
        }

        /// <summary>
        /// Deleta uma moto pelo ID.
        /// </summary>
        /// <param name="id">ID da moto</param>
        /// <response code="204">Moto deletada com sucesso</response>
        /// <response code="404">Moto não encontrada</response>
        [HttpDelete("{id}", Name = "DeleteMoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletar(long id)
        {
            var removed = await _service.DeleteAsync(id);
            if (!removed)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Moto não encontrada" });

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
