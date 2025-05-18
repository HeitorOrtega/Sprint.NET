using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Sprint_1.DTOs;
using Sprint_1.Models;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioDTO>>> GetTodos()
        {
            var lista = await _context.Funcionarios
                .Select(f => new FuncionarioDTO
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Cpf = f.Cpf,
                    Email = f.Email,
                    Rg = f.Rg,
                    Telefone = f.Telefone,
                    PatioId = f.PatioId
                })
                .ToListAsync();

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioDTO>> GetPorId(long id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null)
                return NotFound();

            return Ok(new FuncionarioDTO
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Cpf = funcionario.Cpf,
                Email = funcionario.Email,
                Rg = funcionario.Rg,
                Telefone = funcionario.Telefone,
                PatioId = funcionario.PatioId
            });
        }

        [HttpGet("buscarPorNome")]
        public async Task<ActionResult<IEnumerable<FuncionarioDTO>>> BuscarPorNome([FromQuery] string nome)
        {
            var lista = await _context.Funcionarios
                .Where(f => f.Nome.ToLower().Contains(nome.ToLower()))
                .Select(f => new FuncionarioDTO
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Cpf = f.Cpf,
                    Email = f.Email,
                    Rg = f.Rg,
                    Telefone = f.Telefone,
                    PatioId = f.PatioId
                })
                .ToListAsync();

            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<FuncionarioDTO>> Criar(FuncionarioCreateDTO dto)
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

            var retorno = new FuncionarioDTO
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Cpf = funcionario.Cpf,
                Email = funcionario.Email,
                Rg = funcionario.Rg,
                Telefone = funcionario.Telefone,
                PatioId = funcionario.PatioId
            };

            return CreatedAtAction(nameof(GetPorId), new { id = funcionario.Id }, retorno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(long id, FuncionarioUpdateDTO dto)
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
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(long id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
                return NotFound();

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
