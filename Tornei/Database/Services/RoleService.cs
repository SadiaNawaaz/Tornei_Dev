using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services
{
    public class RoleService
    {
        private readonly TorneiContext _dbContext; // Replace YourDbContext with the actual name of your DbContext
        public RoleService(TorneiContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task<List<AspNetRole>> GetRoles()
        {
            using (var context = new TorneiContext()) // Dichiaro la variabile database
            {
                return await context.AspNetRoles.AsNoTracking().ToListAsync();
            }
        }

    }
}
