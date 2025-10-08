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
        /// Retorna todos os funcionários com suporte a paginação.
        /// </summary>
        /// <param name="parameters">Parâmetros de paginação (pageNumber, pageSize).</param>
        /// <response code="200">Retorna a lista paginada de funcionários.</response>
        [HttpGet(Name = "GetFuncionarios")]
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

            var hateoas = items.Select(f => CriarLinks(f)).ToList();

            var paginationLinks = new List<LinkFuncionario>
            {
                new LinkFuncionario { Href = Url.Link("GetFuncionarios", new { pageNumber = 1, pageSize = parameters.PageSize }), Rel = "first", Method = "GET" },
                new LinkFuncionario { Href = Url.Link("GetFuncionarios", new { pageNumber = Math.Max(1, parameters.PageNumber - 1), pageSize = parameters.PageSize }), Rel = "prev", Method = "GET" },
                new LinkFuncionario { Href = Url.Link("GetFuncionarios", new { pageNumber = Math.Min(totalPages, parameters.PageNumber + 1), pageSize = parameters.PageSize }), Rel = "next", Method = "GET" },
                new LinkFuncionario { Href = Url.Link("GetFuncionarios", new { pageNumber = totalPages, pageSize = parameters.PageSize }), Rel = "last", Method = "GET" }
            };

            return Ok(hateoas); 
        }

        /// <summary>
        /// Retorna um funcionário pelo ID.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <response code="200">Retorna o funcionário.</response>
        /// <response code="404">Funcionário não encontrado.</response>
        [HttpGet("{id}", Name = "GetFuncionarioById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FuncionarioHateoasDto>> GetPorId(long id)
        {
            var funcionario = await _service.GetByIdAsync(id);
            if (funcionario == null) return NotFound();
            return Ok(CriarLinks(funcionario));
        }

        /// <summary>
        /// Cria um novo funcionário.
        /// </summary>
        /// <param name="dto">Dados do funcionário a ser criado.</param>
        /// <response code="201">Funcionário criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost(Name = "CreateFuncionario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FuncionarioHateoasDto>> Criar(FuncionarioCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                ModelState.AddModelError(nameof(dto.Nome), "O campo Nome é obrigatório.");
        
            if (string.IsNullOrWhiteSpace(dto.Cpf))
                ModelState.AddModelError(nameof(dto.Cpf), "O campo CPF é obrigatório.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
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
            return CreatedAtRoute("GetFuncionarioById", new { id = created.Id }, CriarLinks(created));
        }

        /// <summary>
        /// Atualiza um funcionário existente.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <param name="dto">Novos dados do funcionário.</param>
        /// <response code="200">Funcionário atualizado.</response>
        /// <response code="404">Funcionário não encontrado.</response>
        [HttpPut("{id}", Name = "UpdateFuncionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FuncionarioHateoasDto>> Atualizar(long id, FuncionarioUpdateDto dto)
        {
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
            if (updated == null) return NotFound();
            return Ok(CriarLinks(updated));
        }

        /// <summary>
        /// Deleta um funcionário pelo ID.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <response code="204">Funcionário deletado.</response>
        /// <response code="404">Funcionário não encontrado.</response>
        [HttpDelete("{id}", Name = "DeleteFuncionario")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletar(long id)
        {
            var removed = await _service.DeleteAsync(id);
            if (!removed) return NotFound();
            return NoContent();
        }

        private FuncionarioHateoasDto CriarLinks(Funcionario funcionario)
        {
            var dto = new FuncionarioHateoasDto
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Cpf = funcionario.Cpf,
                Email = funcionario.Email,
                Rg = funcionario.Rg,
                Telefone = funcionario.Telefone,
                PatioId = funcionario.PatioId
            };

            dto.Links.Add(new LinkFuncionario { Href = Url.Link("GetFuncionarioById", new { id = funcionario.Id }), Rel = "self", Method = "GET" });
            dto.Links.Add(new LinkFuncionario { Href = Url.Link("UpdateFuncionario", new { id = funcionario.Id }), Rel = "update", Method = "PUT" });
            dto.Links.Add(new LinkFuncionario { Href = Url.Link("DeleteFuncionario", new { id = funcionario.Id }), Rel = "delete", Method = "DELETE" });
            dto.Links.Add(new LinkFuncionario { Href = Url.Link("GetFuncionarios", null), Rel = "all", Method = "GET" });

            return dto;
        }
    }
}
