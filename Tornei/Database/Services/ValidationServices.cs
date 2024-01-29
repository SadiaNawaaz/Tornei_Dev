using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services
{
    public class ValidationServices
    {
        private string errorMessage = "";
        public string ValidateNumber(string inputValue)
        {
            // Use InvokeAsync to run the asynchronous logic

            if (double.TryParse(inputValue, out _))
            {
                // It's a valid number
                return errorMessage = "";

            }
            else
            {
                // It's not a valid number
                if (string.IsNullOrEmpty(inputValue))
                {
                    return errorMessage = "";

                }
                return errorMessage = "Deve contenere solo numeri[0 a 9]";
            }
        }
    }
}
