using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services
{
    public class CampoServices
    {
        private readonly TorneiContext _dbContext;
        public CampoServices(TorneiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Campo>> GetCampoAsync()
        {
            try
            {
                // Quando un codice viene restituito semplicemente senza modifiche o aggiunte, possiamo evitare i costi di tracciamento.
                using (var context = new TorneiContext())
                {
                    var Campos = await context.Campos.AsNoTracking().ToListAsync(); // Elenca tutti i campi
                    return Campos;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
