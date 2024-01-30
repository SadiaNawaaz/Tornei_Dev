using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

[Table("Anagrafica")]
public partial class Anagrafica
{
   [Key]
   public int CodAnagrafica { get; set; }

   [StringLength(50)]
   [Required]
   public string Cognome { get; set; } = null!;

   [StringLength(50)]
   [Required]
   public string Nome { get; set; } = null!;

   [StringLength(50)]

   public string? CodFiscale { get; set; }

   [StringLength(100)]

   public string Indirizzo { get; set; } = null!;

   [StringLength(50)]
   //[LUCA]: it does not have to check the typing after typing, but on the client side while typing it must deactivate everything except the numbers with the javascript code I sent you or something equivalent in C#
   
   //[XXXX] this is blazor form validation and recomended as this is more secure , this is recomended but if you say we can use java script validation.
   [RegularExpression("^[0-9]*$", ErrorMessage = "Telefono deve contenere solo numeri [0 a 9]")]
   public string Telefono { get; set; } = null!;

   [StringLength(50)]
    //[LUCA]: it does not have to check the typing after typing, but on the client side while typing it must deactivate everything except the numbers with the javascript code I sent you or something equivalent in C#
    //[XXXX] this is blazor form validation and recomended as this is more secure , this is recomended but if you say we can use java script validation.
    
    [RegularExpression("^[0-9]*$", ErrorMessage = "Cellulare deve contenere solo numeri [0 a 9]")]
   public string Cellulare { get; set; } = null!;

   [StringLength(50)]

    /* [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Formato Mail non valido.")]*/
    [RegularExpression(@"^\s*$|^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Formato Mail non valido.")]

    public string Mail { get; set; } = null!;

   public bool NoMail { get; set; }

   [StringLength(5)]
   public string CodComuneDiNascita { get; set; } = null!;

   public DateOnly? DataDiNascita { get; set; }

   [StringLength(50)]
   public string Sesso { get; set; } = null!;

   [StringLength(5)]
   public string CodComuneResidenza { get; set; } = null!;

   [StringLength(100)]
   public string IndirizzoResidenza { get; set; } = null!;

   public string Immagine { get; set; } = null!;

   [StringLength(101)]
   public string? NomeCompleto { get; set; }

   [ForeignKey("CodComuneDiNascita")]
   [InverseProperty("AnagraficaCodComuneDiNascitaNavigations")]
   public virtual Comune CodComuneDiNascitaNavigation { get; set; } = null!;

   [ForeignKey("CodComuneResidenza")]
   [InverseProperty("AnagraficaCodComuneResidenzaNavigations")]
   public virtual Comune CodComuneResidenzaNavigation { get; set; } = null!;


    [NotMapped]
    public List<MenuRuolo>? RoleList { get; set; }
    [NotMapped]
    public List<AspNetUserRoles>? UserRoleList { get; set; }
    [NotMapped]
    public List<AspNetRole>? AspRoleList { get; set; }
}
