using Microsoft.AspNetCore.Mvc;
using Sprint.DTOs;
using Sprint.DTOs.ML;
using Sprint.Services;
using Microsoft.AspNetCore.Authorization; 

namespace Sprint.Controllers
{
    [ApiController]
    [ApiVersion("1.0")] 
    [Route("v1/[controller]")]
    public class PrevisaoController : ControllerBase
    {
        private readonly IMotoPricePredictionService _predictionService;

        public PrevisaoController(IMotoPricePredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        /// <summary>
        /// Realiza a previsão do preço de venda de uma moto com base em seus atributos de entrada.
        /// </summary>
        /// <param name="input">Dados da moto a serem usados na previsão (Cor, DiasUso).</param>
        /// <returns>O preço de venda previsto pelo modelo de Machine Learning.</returns>
        [HttpPost("preco-moto", Name = "PreverPrecoMoto")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(ApiResponse<MotoPricePrediction>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ApiResponse<MotoPricePrediction>> PreverPreco([FromBody] MotoPriceInput input)
        {
            if (input == null)
            {
                return BadRequest(new ApiResponse<MotoPricePrediction> { Success = false, Message = "Dados de entrada nulos ou inválidos." });
            }

            var prediction = _predictionService.PredictPrice(input);

            var response = new ApiResponse<MotoPricePrediction>
            {
                Data = prediction,
                Message = $"Preço de venda previsto com sucesso."
            };

            return Ok(response);
        }
    }
}