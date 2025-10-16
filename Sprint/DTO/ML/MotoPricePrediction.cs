using Microsoft.ML.Data;

namespace Sprint.DTOs.ML
{
    public class MotoPricePrediction
    {
        [ColumnName("Score")]
        public float PrecoPrevisto { get; set; }
    }
}