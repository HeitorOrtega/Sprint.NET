using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sprint_1.DTOs;
using Sprint_1.Helpers;
using Sprint_1.Models;

namespace Sprint_1.Controllers
{
    [ApiController]
    [Route("v1/motos")]
    public class MotoController : ControllerBase
    {
        private readonly IMotoService _service;

        public MotoController(IMotoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todas as motos com suporte a paginação.
        /// </summary>
        /// <param name="parameters">Parâmetros de paginação (pageNumber, pageSize)</param>
        /// <response code="200">Retorna a lista paginada de motos no corpo e os metadados no header X-Pagination.</response>
        [HttpGet(Name = "GetMotos")]
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

            var hateoas = items.Select(m => CriarLinks(m)).ToList();
            
          
            return Ok(hateoas); 
        }

        /// <summary>
        /// Retorna uma moto pelo ID.
        /// </summary>
        /// <param name="id">ID da moto.</param>
        /// <response code="200">Retorna a moto com links HATEOAS.</response>
        /// <response code="404">Moto não encontrada.</response>
        [HttpGet("{id}", Name = "GetMotoById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MotoHateoasDto>> GetPorId(long id)
        {
            var moto = await _service.GetByIdAsync(id);
            if (moto == null) return NotFound();
            return Ok(CriarLinks(moto));
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        /// <param name="dto">Dados da moto a ser criada.</param>
        /// <response code="201">Moto criada com sucesso.</response>
        /// <response code="400">Dados inválidos (erros detalhados pelo ModelState).</response>
        [HttpPost(Name = "CreateMoto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MotoHateoasDto>> Criar(MotoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Placa))
                ModelState.AddModelError(nameof(dto.Placa), "O campo Placa é obrigatório.");

            if (string.IsNullOrWhiteSpace(dto.Cor))
                ModelState.AddModelError(nameof(dto.Cor), "O campo Cor é obrigatório.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            
            var entity = new Moto
            {
                Cor = dto.Cor,
                Placa = dto.Placa,
                DataFabricacao = dto.DataFabricacao
            };

            var created = await _service.CreateAsync(entity);
            return CreatedAtRoute("GetMotoById", new { id = created.Id }, CriarLinks(created));
        }

        /// <summary>
        /// Atualiza uma moto existente.
        /// </summary>
        /// <param name="id">ID da moto.</param>
        /// <param name="dto">Novos dados da moto.</param>
        /// <response code="200">Moto atualizada e retornada.</response>
        /// <response code="404">Moto não encontrada.</response>
        [HttpPut("{id}", Name = "UpdateMoto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MotoHateoasDto>> Atualizar(long id, MotoUpdateDto dto)
        {
            var entity = new Moto
            {
                Cor = dto.Cor,
                Placa = dto.Placa,
                DataFabricacao = dto.DataFabricacao
            };

            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null) return NotFound();
            return Ok(CriarLinks(updated)); 
        }

        /// <summary>
        /// Deleta uma moto pelo ID.
        /// </summary>
        /// <param name="id">ID da moto.</param>
        /// <response code="204">Moto deletada com sucesso (sem conteúdo de retorno).</response>
        /// <response code="404">Moto não encontrada.</response>
        [HttpDelete("{id}", Name = "DeleteMoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletar(long id)
        {
            var removed = await _service.DeleteAsync(id);
            if (!removed) return NotFound();
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