using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Models;

[Table("Comune")]
public partial class Comune
{
    [Key]
    [StringLength(5)]
    public string CodComune { get; set; } = null!;

    [StringLength(50)]
    public string DesComune { get; set; } = null!;

    [StringLength(50)]
    public string Provincia { get; set; } = null!;

    [StringLength(50)]
    public string Regione { get; set; } = null!;

    [StringLength(50)]
    public string Nazione { get; set; } = null!;

    [Column("CAP")]
    [StringLength(5)]
    public string Cap { get; set; } = null!;

    public bool Obsoleto { get; set; }

    [StringLength(5)]
    public string CodComuneNuovo { get; set; } = null!;

    [StringLength(5)]
    public string Prefisso { get; set; } = null!;

    [StringLength(122)]
    public string DesComuneEstesa { get; set; } = null!;

    [InverseProperty("CodComuneDiNascitaNavigation")]
    public virtual ICollection<Anagrafica> AnagraficaCodComuneDiNascitaNavigations { get; set; } = new List<Anagrafica>();

    [InverseProperty("CodComuneResidenzaNavigation")]
    public virtual ICollection<Anagrafica> AnagraficaCodComuneResidenzaNavigations { get; set; } = new List<Anagrafica>();

    [InverseProperty("CodComuneNavigation")]
    public virtual ICollection<Societa> Societa { get; set; } = new List<Societa>();
}
