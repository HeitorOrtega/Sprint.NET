using Sprint.DTOs.ML;

namespace Sprint.Services
{
    public interface IMotoPricePredictionService
    {
        MotoPricePrediction PredictPrice(MotoPriceInput input);
    }
}