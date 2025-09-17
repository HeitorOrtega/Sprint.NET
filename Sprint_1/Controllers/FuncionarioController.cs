using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Sprint_1.DTOs;
using Sprint_1.Models;
using Sprint_1.Helpers;
using System.Text.Json;

namespace Sprint_1.Controllers
{
    [ApiController]
    [Route("v1/funcionarios")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetFuncionarios")]
        public async Task<ActionResult<IEnumerable<FuncionarioHateoasDto>>> GetTodos([FromQuery] QueryParameters parameters)
        {
            var query = _context.Funcionarios.AsQueryable();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

            var paginationMetadata = new
            {
                totalCount,
                pageSize = parameters.PageSize,
                currentPage = parameters.PageNumber,
                totalPages
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var funcionarios = await query
                .OrderBy(f => f.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var hateoasFuncionarios = funcionarios.Select(f => CriarLinks(f)).ToList();

            var paginationLinks = new List<LinkFuncionario>
            {
                new LinkFuncionario { Href = Url.Link("GetFuncionarios", new { pageNumber = 1, pageSize = parameters.PageSize }), Rel = "first", Method = "GET" },
                new LinkFuncionario { Href = Url.Link("GetFuncionarios", new { pageNumber = Math.Max(1, parameters.PageNumber - 1), pageSize = parameters.PageSize }), Rel = "prev", Method = "GET" },
                new LinkFuncionario { Href = Url.Link("GetFuncionarios", new { pageNumber = Math.Min(totalPages, parameters.PageNumber + 1), pageSize = parameters.PageSize }), Rel = "next", Method = "GET" },
                new LinkFuncionario { Href = Url.Link("GetFuncionarios", new { pageNumber = totalPages, pageSize = parameters.PageSize }), Rel = "last", Method = "GET" }
            };

            return Ok(new { Data = hateoasFuncionarios, Pagination = paginationLinks });
        }

        [HttpGet("{id}", Name = "GetFuncionarioById")]
        public async Task<ActionResult<FuncionarioHateoasDto>> GetPorId(long id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
                return NotFound();

            return Ok(CriarLinks(funcionario));
        }

        [HttpPost(Name = "CreateFuncionario")]
        public async Task<ActionResult<FuncionarioHateoasDto>> Criar(FuncionarioCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Cpf))
                return BadRequest("Nome e CPF são obrigatórios.");

            var funcionario = new Funcionario
            {
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Email = dto.Email,
                Rg = dto.Rg,
                Telefone = dto.Telefone,
                PatioId = dto.PatioId
            };

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetFuncionarioById", new { id = funcionario.Id }, CriarLinks(funcionario));
        }

        [HttpPut("{id}", Name = "UpdateFuncionario")]
        public async Task<ActionResult<FuncionarioHateoasDto>> Atualizar(long id, FuncionarioUpdateDto dto)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
                return NotFound();

            funcionario.Nome = dto.Nome;
            funcionario.Cpf = dto.Cpf;
            funcionario.Email = dto.Email;
            funcionario.Rg = dto.Rg;
            funcionario.Telefone = dto.Telefone;
            funcionario.PatioId = dto.PatioId;

            await _context.SaveChangesAsync();

            return Ok(CriarLinks(funcionario));
        }

        [HttpDelete("{id}", Name = "DeleteFuncionario")]
        public async Task<IActionResult> Deletar(long id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
                return NotFound();

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

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
