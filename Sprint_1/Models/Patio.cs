using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint_1.Models
{
    [Table("PATIO")]
    public class Patio
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        public List<Funcionario> Funcionarios { get; set; } = new();
        public List<Moto> Motos { get; set; } = new();
    }
}