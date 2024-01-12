using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Database.Models;

[Index("DesSocieta", Name = "IX_Societa_DesSocieta", IsUnique = true)]
public partial class Societa
{
   [Key]
   public int CodSocieta { get; set; }


   [Required]
   [StringLength(50)]
   public string DesSocieta { get; set; } = null!;

   [StringLength(16)]


   //[LUCA]: it does not have to check the typing after typing, but on the client side while typing it must deactivate everything except the numbers with the javascript code I sent you or something equivalent in C#
   [RegularExpression("^[0-9]*$", ErrorMessage = "Partita Iva deve contenere solo numeri [0 a 9]")]
   public string PartitaIva { get; set; } = null!;

   public int Matricola { get; set; }


   [Required]
   [StringLength(5)]
   public string QualificaClub { get; set; } = null!;

   [StringLength(5)]
   public string CodComune { get; set; } = null!;

   [StringLength(100)]
   public string Indirizzo { get; set; } = null!;

   [StringLength(15)]
   //[LUCA]: it does not have to check the typing after typing, but on the client side while typing it must deactivate everything except the numbers with the javascript code I sent you or something equivalent in C#
   [RegularExpression("^[0-9]*$", ErrorMessage = "Telefono deve contenere solo numeri[0 a 9]")]
   public string Telefono { get; set; } = null!;

   [StringLength(15)]
   //[LUCA]: it does not have to check the typing after typing, but on the client side while typing it must deactivate everything except the numbers with the javascript code I sent you or something equivalent in C#
   [RegularExpression("^[0-9]*$", ErrorMessage = "Cellulare deve contenere solo numeri[0 a 9]")]
   public string Cellulare { get; set; } = null!;

   [Required(ErrorMessage = "Email is required.")]
   [StringLength(50)]
   [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Formato mail non valido")]
   public string Mail { get; set; } = null!;

   public string SitoInternet { get; set; } = null!;

   public string Nota { get; set; } = null!;



   [StringLength(7)]
   public string CodiceDestinatario { get; set; } = null!;

   [StringLength(30)]
   public string Iban { get; set; } = null!;

   [StringLength(50)]

   public string Pec { get; set; } = null!;


   public string Logo { get; set; } = null!;

   [InverseProperty("CodSocietaNavigation")]
   public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();

   [InverseProperty("CodSocietaNavigation")]
   public virtual ICollection<Campo> Campos { get; set; } = new List<Campo>();

   [ForeignKey("CodComune")]
   [InverseProperty("Societa")]
   public virtual Comune CodComuneNavigation { get; set; } = null!;



    [NotMapped]
    public List<MenuRuolo>? RoleList { get; set; }
    [NotMapped]
    public List<AspNetUserRoles>? UserRoleList { get; set; }
    [NotMapped]
    public List<AspNetRole>? AspRoleList { get; set; }
}
