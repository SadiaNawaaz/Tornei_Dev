using Microsoft.AspNetCore.Components.Web;

// questo modello viene utilizzato quando si fa clic su qualsiasi nodo dell'albero,
// dobbiamo inviare informazioni su quale clic è stato fatto clic e quale è CodMenu è stato scelto
// quindi usiamo questo modello
namespace Tornei.Models
{
   public class NodeClickEventArgs
   {
      public MouseEventArgs MouseEventArgs { get; set; }
      public int CodMenu { get; set; }
   }
}
