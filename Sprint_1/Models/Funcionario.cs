using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint_1.Models
{
    [Table("FUNCIONARIO")]
    public class Funcionario
    {
        [Key]
        public long Id { get; set; }

        [Column("NOME")]
        public string Nome { get; set; } = string.Empty;

        [Column("CPF")]
        public string Cpf { get; set; } = string.Empty;

        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        [Column("RG")]
        public string Rg { get; set; }

        [Column("TELEFONE")]
        public string Telefone { get; set; }

        [NotMapped]
        public List<Patio> Patios { get; set; }
    }
}