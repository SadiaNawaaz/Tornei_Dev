namespace Database.Services
{

   //[Luca]: 
   public class ValidationServices
   {

      private string errorMessage = "";

      public string ValidateNumber(string inputValue)
      {
         // Utilizzare InvokeAsync per eseguire la logica asincrona

         if (double.TryParse(inputValue, out _))
         {
            // Se ho un numero valido
            return errorMessage = "";
         }
         else
         {
            // Se il numero non è valido
            if (string.IsNullOrEmpty(inputValue))
            {
               return errorMessage = "";
            }
            return errorMessage = "Deve contenere solo numeri[0 a 9]";
         }
      }
   }
}
