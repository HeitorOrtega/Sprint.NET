using System.ComponentModel.DataAnnotations;

namespace Sprint_1.DTOs
{
    public class MotoDto
    {
        public long Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        
        [Required]
        public int Ano { get; set; }
        public string Placa { get; set; }
    }

    public class MotoCreateDto
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        
        [Required]
        public int Ano { get; set; }
        public string Placa { get; set; }
    }

    public class MotoUpdateDto
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        [Required]
        public int Ano { get; set; }
        public string Placa { get; set; }
    }
}