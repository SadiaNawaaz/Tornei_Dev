using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Models;

[Keyless]
[Table("MenuRuolo")]
public partial class MenuRuolo
{
    public int Id { get; set; }

    [StringLength(450)]
    public string RoleId { get; set; } = null!;

    public int CodMenu { get; set; }

    [ForeignKey("CodMenu")]
    public virtual Menu CodMenuNavigation { get; set; } = null!;

    [ForeignKey("RoleId")]
    public virtual AspNetRole Role { get; set; } = null!;
}
