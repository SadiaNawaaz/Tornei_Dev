using Microsoft.AspNetCore.Identity;

namespace Tornei.Data
{
   // Add profile data for application users by adding properties to the ApplicationUser class
   public class ApplicationUser : IdentityUser
   {
      public int CodSocieta { get; set; }
      public int CodAnagrafica { get; set; }
   }

}
