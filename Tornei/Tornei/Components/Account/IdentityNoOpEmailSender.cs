using Tornei.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Tornei.Components.Account
{
   // Rimuove il blocco "else if (EmailSender is IdentityNoOpEmailSender)" da RegisterConfirmation.razor dopo l'aggiornamento con un'implementazione reale.
   internal sealed class IdentityEmailSender : IEmailSender<ApplicationUser>
   {



      private readonly IEmailSender emailSender = new Helper.EmailSender();

      public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
          emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

      public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
          emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

      public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
          emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
   }
}
