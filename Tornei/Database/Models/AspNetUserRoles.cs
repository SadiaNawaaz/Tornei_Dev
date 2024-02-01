using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
   
   // Modello per l'associazione tra AspnetUser - AspNetRoles legame n->n
   public class AspNetUserRoles
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }
        [Key, Column(Order = 1)]
        public string RoleId { get; set; }
    }
}
