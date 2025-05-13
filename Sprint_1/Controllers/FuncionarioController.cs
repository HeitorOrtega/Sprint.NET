using Microsoft.AspNetCore.Mvc;
using Sprint_1.DTOs;
using Sprint_1.Models;

namespace Sprint_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private static List<Funcionario> funcionarios = new();
        private static long nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<FuncionarioDTO>> GetTodos()
        {
            var lista = funcionarios.Select(f => new FuncionarioDTO
            {
                Id = f.Id,
                Nome = f.Nome,
                Cpf = f.Cpf,
                Email = f.Email,
                Rg = f.Rg,
                Telefone = f.Telefone
            });

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public ActionResult<FuncionarioDTO> GetPorId(long id)
        {
            var funcionario = funcionarios.FirstOrDefault(f => f.Id == id);
            if (funcionario == null)
                return NotFound();

            return Ok(new FuncionarioDTO
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Cpf = funcionario.Cpf,
                Email = funcionario.Email,
                Rg = funcionario.Rg,
                Telefone = funcionario.Telefone
            });
        }

        [HttpGet("buscarPorNome")]
        public ActionResult<IEnumerable<FuncionarioDTO>> BuscarPorNome([FromQuery] string nome)
        {
            var lista = funcionarios
                .Where(f => f.Nome.ToLower().Contains(nome.ToLower()))
                .Select(f => new FuncionarioDTO
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Cpf = f.Cpf,
                    Email = f.Email,
                    Rg = f.Rg,
                    Telefone = f.Telefone
                });

            return Ok(lista);
        }

        [HttpPost]
        public ActionResult<FuncionarioDTO> Criar(FuncionarioCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Cpf))
                return BadRequest("Nome e CPF são obrigatórios.");

            var funcionario = new Funcionario
            {
                Id = nextId++,
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Email = dto.Email,
                Rg = dto.Rg,
                Telefone = dto.Telefone,
                Patios = new List<Patio>()
            };

            funcionarios.Add(funcionario);

            return CreatedAtAction(nameof(GetPorId), new { id = funcionario.Id }, funcionario);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(long id, FuncionarioUpdateDTO dto)
        {
            var funcionario = funcionarios.FirstOrDefault(f => f.Id == id);
            if (funcionario == null)
                return NotFound();

            funcionario.Nome = dto.Nome;
            funcionario.Cpf = dto.Cpf;
            funcionario.Email = dto.Email;
            funcionario.Rg = dto.Rg;
            funcionario.Telefone = dto.Telefone;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(long id)
        {
            var funcionario = funcionarios.FirstOrDefault(f => f.Id == id);
            if (funcionario == null)
                return NotFound();

            funcionarios.Remove(funcionario);
            return NoContent();
        }
    }
}
