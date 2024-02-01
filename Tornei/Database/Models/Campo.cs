using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    //Lista delle possibili tipologie
    // [LUCA] What purpose does this statement serve here? or rather if it is done here because it is redone again in "ListaDeCampo.razor", couldn't we refer to this? I want to avoid inserting string values ​​in multiple places because we could make mistakes in writing them or if we had to change we would have to do it in multiple places.
    // campo.cs
    // [NotMapped]
    // public List<string> ListDeTipologia { get; set; } = new List<string>() { "Playing field", "Training field" };
    // in ListaDeCampo.Razor
    // public List<string> ListDeTipologia { get; set; } = new List<string>() { "Playing field", "Training field" };
    // in my opinion in Listadecampo.razor we should refer to the statement made here
    //Development
    [NotMapped]
    public List<string> ListDeTipologia { get; set; } = new List<string>() { "Campo da gioco", "Campo di Allenamento"};

}
