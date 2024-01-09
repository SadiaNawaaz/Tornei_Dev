using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
  [Table("PaginaHTML")]
    public class PaginaHTML
    {
        [Key]
        public short CodPaginaHtml { get; set; }

        [Required]
        [MaxLength(100)]
        public string Titolo { get; set; }

        [Required]
        public string Testo { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }
    }
}
