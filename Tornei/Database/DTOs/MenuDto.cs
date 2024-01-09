using System.ComponentModel.DataAnnotations.Schema;

namespace Database.DTOs
{
   public class MenuDto
   {
      public int CodMenu { get; set; } 
      public int CodMenuPadre { get; set; } 
      public string Route { get; set; }
      public string DesMenu { get; set; } 

      // Navigation properties
      [NotMapped]
      public MenuDto Padre { get; set; }  
      public List<MenuDto> Figli { get; set; }
   }
}
