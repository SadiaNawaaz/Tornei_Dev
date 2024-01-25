using Microsoft.AspNetCore.Identity.UI.Services;

using System.Net.Mail;

using Microsoft.Extensions.Configuration;

namespace Tornei.Helper
{
   public class EmailSender : IEmailSender
   {

      public Task SendEmailAsync(string email, string subject, string htmlMessage)
      {
         try
         {
            using (MailMessage Messaggio = new MailMessage())
            {
               var Configurazione = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
               var Mittente = Configurazione.GetValue<string>("Mail:Mittente");
               var SMTP = Configurazione.GetValue<string>("Mail:SMTP");
               var Porta = Configurazione.GetValue<int>("Mail:Porta");
               var UtenteCredenziali = Configurazione.GetValue<string>("Mail:UtenteCredenziali");
               var PasswordCredenziali = Configurazione.GetValue<string>("Mail:PasswordCredenziali");

               // Read From appsettings.json configuration 
               Messaggio.From = new MailAddress(Mittente); 
               Messaggio.To.Add(email);
               Messaggio.Subject = subject;
               Messaggio.Body = htmlMessage;
               Messaggio.IsBodyHtml = true;

               // Server
               using (SmtpClient Server = new SmtpClient(SMTP, Porta)) 
               {
                  Server.Credentials = new System.Net.NetworkCredential(UtenteCredenziali, PasswordCredenziali);
                  Server.EnableSsl = true;
                  Server.Send(Messaggio);
               }
            }
         }
         catch (Exception)
         {
         }

         return Task.CompletedTask;
      }
   }
}



