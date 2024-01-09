using Database.Models;

using Microsoft.EntityFrameworkCore;

namespace Database.Services
{
   public class UserService
    {
        private readonly TorneiContext _context;

        public UserService(TorneiContext context)
        {
            _context = context;
        }

        public async Task<AspNetUser?> GetUserByEmailAsync(string email)
        {
            // Ensure the email comparison is case-insensitive
            using (var context = new TorneiContext()) // Dichiaro la variabile database
            {
                return await context.AspNetUsers
                .FirstOrDefaultAsync(u => u.Email == email);
            }
        }
    }
}
