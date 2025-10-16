using Microsoft.ML.Data;
using System.ComponentModel.DataAnnotations;

namespace Sprint.DTOs.ML
{
    public class MotoPriceInput
    {
        public string Cor { get; set; }
        public float DiasUso { get; set; }


        [ColumnName("Label")] 
        public float PrecoAlvo { get; set; }
    }
}