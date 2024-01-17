using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Models;

[Table("Campo")]
public partial class Campo
{
    [Key]
    public int CodCampo { get; set; }

    public int CodSocieta { get; set; }

    [StringLength(20)]
    public string Tipologia { get; set; } = null!;

    [StringLength(50)]
    public string Nome { get; set; } = null!;

    [StringLength(30)]
    public string NomeCustode { get; set; } = null!;

    [StringLength(15)]
    public string TelefonoCustode { get; set; } = null!;

    public string GeoLocalizzazione { get; set; } = null!;

    [ForeignKey("CodSocieta")]
    [InverseProperty("Campos")]
    public virtual Societa CodSocietaNavigation { get; set; } = null!;

    //Development
    [NotMapped]
    public List<string> ListDeTipologia { get; set; } = new List<string>() { "Campo da gioco", "Campo di Allenamento"};

}
