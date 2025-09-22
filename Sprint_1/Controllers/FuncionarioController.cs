using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sprint_1.DTOs;
using Sprint_1.Helpers;
using Sprint_1.Models;

namespace Sprint_1.Controllers
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

        [HttpGet(Name = "GetFuncionarios")]
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

            return Ok(new { Data = hateoas, Pagination = paginationLinks });
        }

        [HttpGet("{id}", Name = "GetFuncionarioById")]
        public async Task<ActionResult<FuncionarioHateoasDto>> GetPorId(long id)
        {
            var funcionario = await _service.GetByIdAsync(id);
            if (funcionario == null) return NotFound();
            return Ok(CriarLinks(funcionario));
        }

        [HttpPost(Name = "CreateFuncionario")]
        public async Task<ActionResult<FuncionarioHateoasDto>> Criar(FuncionarioCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Cpf))
                return BadRequest("Nome e CPF são obrigatórios.");

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

        [HttpPut("{id}", Name = "UpdateFuncionario")]
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

        [HttpDelete("{id}", Name = "DeleteFuncionario")]
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
