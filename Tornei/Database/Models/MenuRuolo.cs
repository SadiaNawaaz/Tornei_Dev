using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

[Table("MenuRuolo")]
public partial class MenuRuolo
{
   [Key]
   public int Id { get; set; }

   [StringLength(450)]
   public string RoleId { get; set; } = null!;

   public int CodMenu { get; set; }

   // [LUCA]
   // between STARD and END you find that two links to different tables have been called "CodMenuNavigation"
   // and the other "Role"
   // We should use a common and always the same rule to assign these names.
   // When the system creates them as standard by calling them with the name of the field and adding "Navigation" we should always use this rule in my opinion.
   // Look at the example I gave you
   // [ForeignKey("CodMenu")]
   // [InverseProperty("MenuRuolo")]
   // public virtual AspNetRole AspNetRoleNavigation { get; set; } = null!;
   // START
   [ForeignKey("CodMenu")]
   public virtual Menu CodMenuNavigation { get; set; } = null!;

   [ForeignKey("RoleId")]
   public virtual AspNetRole Role { get; set; } = null!;
   // END
}
