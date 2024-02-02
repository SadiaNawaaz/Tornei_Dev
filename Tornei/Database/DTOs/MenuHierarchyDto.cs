using System.ComponentModel.DataAnnotations.Schema;

namespace Database.DTOs
{
   public class MenuHierarchyDto
   {
      public int CodMenu { get; set; }
      public int CodMenuPadre { get; set; }
      public string? DesMenuPadre { get; set; }
      public string DesMenu { get; set; }
      public string ToolTip { get; set; }
      public byte Ordine { get; set; }
      public string Route { get; set; }
      public short CodPaginaHtml { get; set; }
      public string Parametro { get; set; }
      public int Mostra { get; set; }
      public DateTime DataInizioPubblicazione { get; set; }
      public DateTime DataFinePubblicazione { get; set; }
  


      // [LUCA]: This field not exist
      //[NotMapped]
      //public bool MostraFigi { get; set; } = true;

      [NotMapped]
      public MenuHierarchyDto Padre { get; set; }
      public List<MenuHierarchyDto> Figli { get; set; }
   }
}
