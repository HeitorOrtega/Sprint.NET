using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint_1.Models
{
    [Table("CHAVEIRO")]
    public class Chaveiro
    {
        [Key]
        public long Id { get; set; }

        public string Dispositivo { get; set; }

        [ForeignKey("Moto")]
        public long MotoId { get; set; }

        public Moto Moto { get; set; }
    }
}