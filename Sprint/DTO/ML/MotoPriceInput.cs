using Microsoft.ML.Data;

namespace Sprint.DTOs.ML
{

    public class MotoPriceInput
    {
        [LoadColumn(0)]
        public string Cor { get; set; }

        [LoadColumn(1)]
        public float DiasUso { get; set; }

        [LoadColumn(2)]
        [ColumnName("Label")]
        public float PrecoAlvo { get; set; }
    }
}