using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Models;

[Table("Menu")]
public partial class Menu
{
    [Key]
    public int CodMenu { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Please select Parent Menu")]
    public int CodMenuPadre { get; set; }

    [StringLength(30)]
    public string DesMenu { get; set; } = null!;

    [StringLength(255)]
    public string ToolTip { get; set; } = null!;

    public byte Ordine { get; set; }

    [StringLength(50)]
    public string Route { get; set; } = null!;

    public short CodPaginaHtml { get; set; }

    [StringLength(100)]
    public string Parametro { get; set; } = null!;

    public int Mostra { get; set; }

    [Precision(0)]
    public DateTime DataInizioPubblicazione { get; set; }

    [Precision(0)]
    public DateTime DataFinePubblicazione { get; set; }
    [NotMapped]
    public List<MenuRuolo>? MenuRuoloLista { get; set; }
}
