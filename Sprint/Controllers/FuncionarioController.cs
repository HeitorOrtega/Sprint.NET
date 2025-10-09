using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sprint.DTOs;
using Sprint.Helpers;
using Sprint.Models;

namespace Sprint.Controllers
{
    [ApiController]
    [Route("v1/funcionarios")]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _service;

        public FuncionarioController(IFuncionarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todos os funcionários com paginação e HATEOAS.
        /// </summary>
        /// <param name="parameters">Parâmetros de paginação (pageNumber, pageSize)</param>
        /// <response code="200">Lista de funcionários com links HATEOAS e metadados de paginação</response>
        [HttpGet(Name = "GetFuncionarios")]
        [ProducesResponseType(typeof(ApiResponse<FuncionarioHateoasDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<FuncionarioHateoasDto>>>> GetTodos([FromQuery] QueryParameters parameters)
        {
            var (items, totalCount) = await _service.GetAllAsync(parameters);
            var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            var hateoas = items.Select(f => CriarLinks(f)).ToList();

            var response = new ApiResponse<IEnumerable<FuncionarioHateoasDto>>
            {
                Data = hateoas,
                Message = "Funcionários recuperados com sucesso"
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
        /// Retorna um funcionário pelo ID.
        /// </summary>
        /// <param name="id">ID do funcionário</param>
        /// <response code="200">Funcionário encontrado com links HATEOAS</response>
        /// <response code="404">Funcionário não encontrado</response>
        [HttpGet("{id}", Name = "GetFuncionarioById")]
        [ProducesResponseType(typeof(ApiResponse<FuncionarioHateoasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<FuncionarioHateoasDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<FuncionarioHateoasDto>>> GetPorId(long id)
        {
            var funcionario = await _service.GetByIdAsync(id);
            if (funcionario == null)
                return NotFound(new ApiResponse<FuncionarioHateoasDto> { Success = false, Message = "Funcionário não encontrado" });

            var response = new ApiResponse<FuncionarioHateoasDto>
            {
                Data = CriarLinks(funcionario),
                Message = "Funcionário recuperado com sucesso"
            };
            return Ok(response);
        }

        /// <summary>
        /// Cria um novo funcionário.
        /// </summary>
        /// <param name="dto">Dados do funcionário</param>
        /// <response code="201">Funcionário criado</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost(Name = "CreateFuncionario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<FuncionarioHateoasDto>>> Criar(FuncionarioCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                ModelState.AddModelError(nameof(dto.Nome), "O campo Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Cpf))
                ModelState.AddModelError(nameof(dto.Cpf), "O campo CPF é obrigatório.");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<FuncionarioHateoasDto>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var entity = new Funcionario
            {
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Email = dto.Email,
                Rg = dto.Rg,
                Telefone = dto.Telefone,
                PatioId = dto.PatioId
            };

            var created = await _service.CreateAsync(entity);

            var response = new ApiResponse<FuncionarioHateoasDto>
            {
                Data = CriarLinks(created),
                Message = "Funcionário criado com sucesso"
            };

            return CreatedAtRoute("GetFuncionarioById", new { id = created.Id }, response);
        }

        /// <summary>
        /// Atualiza um funcionário existente.
        /// </summary>
        /// <param name="id">ID do funcionário</param>
        /// <param name="dto">Novos dados</param>
        /// <response code="200">Funcionário atualizado</response>
        /// <response code="404">Funcionário não encontrado</response>
        [HttpPut("{id}", Name = "UpdateFuncionario")]
        [ProducesResponseType(typeof(ApiResponse<FuncionarioHateoasDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<FuncionarioHateoasDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<FuncionarioHateoasDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<FuncionarioHateoasDto>>> Atualizar(long id, FuncionarioUpdateDto dto)
        {
            // Validação igual ao POST
            if (string.IsNullOrWhiteSpace(dto.Nome))
                ModelState.AddModelError(nameof(dto.Nome), "O campo Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Cpf))
                ModelState.AddModelError(nameof(dto.Cpf), "O campo CPF é obrigatório.");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<FuncionarioHateoasDto>
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var entity = new Funcionario
            {
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Email = dto.Email,
                Rg = dto.Rg,
                Telefone = dto.Telefone,
                PatioId = dto.PatioId
            };

            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null)
                return NotFound(new ApiResponse<FuncionarioHateoasDto> { Success = false, Message = "Funcionário não encontrado" });

            var response = new ApiResponse<FuncionarioHateoasDto>
            {
                Data = CriarLinks(updated),
                Message = "Funcionário atualizado com sucesso"
            };

            return Ok(response);
        }

        /// <summary>
        /// Deleta um funcionário pelo ID.
        /// </summary>
        /// <param name="id">ID do funcionário</param>
        /// <response code="204">Funcionário deletado</response>
        /// <response code="404">Funcionário não encontrado</response>
        [HttpDelete("{id}", Name = "DeleteFuncionario")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletar(long id)
        {
            var removed = await _service.DeleteAsync(id);
            if (!removed)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Funcionário não encontrado" });

            return NoContent();
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

            dto.Links.Add(new LinkFuncionario { Href = Url.Link("GetFuncionarioById", new { id = f.Id }), Rel = "self", Method = "GET" });
            dto.Links.Add(new LinkFuncionario { Href = Url.Link("UpdateFuncionario", new { id = f.Id }), Rel = "update", Method = "PUT" });
            dto.Links.Add(new LinkFuncionario { Href = Url.Link("DeleteFuncionario", new { id = f.Id }), Rel = "delete", Method = "DELETE" });
            dto.Links.Add(new LinkFuncionario { Href = Url.Link("GetFuncionarios", null), Rel = "all", Method = "GET" });

            return dto;
        }
    }
}
