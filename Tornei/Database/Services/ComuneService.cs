using Database.Models;

using Microsoft.EntityFrameworkCore;

namespace Database.Services
{
   public class ComuneService
   {
      private readonly TorneiContext _dbContext;

      public ComuneService(TorneiContext dbContext)
      {
         _dbContext = dbContext;
      }

      public async Task<List<Comune>> GetComunesAsync()
      {
         try
         {
            // Quando un codice viene restituito semplicemente senza modifiche o aggiunte, possiamo evitare i costi di tracciamento.
            using (var context = new TorneiContext()) 
                {
                    var comunes = await context.Comunes.AsNoTracking().ToListAsync();
                    return comunes;
                }
            // Normalmente sarebbe così
            // var comunes = await _dbContext.Comunes.ToListAsync();
            // using (var context = new TorneiContext())


         }
         catch (Exception)
         {

            throw;
         }
      }

      public async Task<Comune> GetComuneByIdAsync(string codComune)
      {
         return await _dbContext.Comunes.FirstOrDefaultAsync(c => c.CodComune == codComune);
      }

      public async Task AddUpdateComuneAsync(Comune comune)
      {
         try
         {
            if (comune.CodComune == "0")
            {
               _dbContext.Comunes.Add(comune);
               await _dbContext.SaveChangesAsync();
            }
            else
            {
               _dbContext.Comunes.Update(comune);
               await _dbContext.SaveChangesAsync();
            }
         }
         catch (Exception ex)
         {

            throw;
         }
      }



      public async Task DeleteComuneAsync(string codComune)
      {
         var comune = await _dbContext.Comunes.FirstOrDefaultAsync(c => c.CodComune == codComune);
         if (comune != null)
         {
            _dbContext.Comunes.Remove(comune);
            await _dbContext.SaveChangesAsync();
         }
      }
   }
}
