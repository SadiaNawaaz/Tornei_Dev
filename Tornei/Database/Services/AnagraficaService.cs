using Database.Models;

using Microsoft.EntityFrameworkCore;

namespace Database.Services
{


    public class AnagraficaService
    {
        private readonly TorneiContext _dbContext;

        public AnagraficaService(TorneiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Anagrafica>> GetAllAnagraficheAsync()
        {
            using (var context = new TorneiContext()) // Dichiaro che uso il Database (context)
            {
                return await context.Anagraficas.ToListAsync();
            }
        }

        public async Task<Anagrafica> GetAnagraficaByIdAsync(int id)
        {
            using (var context = new TorneiContext()) // Dichiaro che uso il Database (context)
            {

                return await context.Anagraficas.FindAsync(id);
            }

        }

        //Trova un'anagrafica tramite il suo UserName il c
        public async Task<Anagrafica> GetAnagraficaRoleByIdAsync(int id)
        {

            Anagrafica tempObj = new();
            var res = await _dbContext.Anagraficas.FindAsync(id);
            if (res == null)
            {
                return null;
            }
            else
            {
                using (var context = new TorneiContext()) // Dichiaro che uso il Database (context)
                {
                    //Trova il codice anagrafica tramite user name
                    var risultato = await context.AspNetUsers.FirstOrDefaultAsync(x => x.CodAnagrafica == id); // Ricerco per codice
                    if (risultato != null)
                    {
                        var UserRole = await context.AspNetUserRoles.Where(x => x.UserId == risultato.Id).ToListAsync();
                        tempObj = res;
                        tempObj.UserRoleList = UserRole;
                        return tempObj; // Ritorno il codice trovato
                    }
                    /*   
                    */
                }
            }
            return tempObj;

        }

        //Trova un'anagrafica tramite il suo UserName il c
        public async Task<Anagrafica> GetAnagraficaByUserIdAsync(string id)
        {
            //Trova il codice anagrafica tramite user name
            var user = _dbContext.AspNetUsers.FirstOrDefault(x => x.Id == id);
            if (user != null) // Se lo trova
            {
                // Cerca i dati anagrafici tramite il codice Anagrafica
                var agid = user.CodAnagrafica;
                return await _dbContext.Anagraficas.FindAsync(agid);
            }
            return default;
        }


        public async Task<Anagrafica> AddAnagraficaAsync(Anagrafica anagrafica, string email)
        {
            try
            {
                // Se i seguenti valori son nulli li imposta a ''
                anagrafica.CodFiscale ??= "";
                anagrafica.Indirizzo ??= "";
                anagrafica.Telefono ??= "";
                anagrafica.Mail ??= "";

                // Assegna il codice progressimo (trova il max e somma 1)
                int maxCodAnagrafica = await _dbContext.Anagraficas.MaxAsync(a => (int?)a.CodAnagrafica) ?? 0;
                int nextCodAnagrafica = maxCodAnagrafica + 1;
                anagrafica.CodAnagrafica = nextCodAnagrafica;

                // Aggiunge all'archivio l'anagrafica corretta
                var entityEntry = await _dbContext.Anagraficas.AddAsync(anagrafica);
                await _dbContext.SaveChangesAsync(); // Aggiorna i dati sul DB

                // Aggiorna il codice Anagrafica nella tabella degli utenti
                var user = await _dbContext.AspNetUsers.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    user.CodAnagrafica = nextCodAnagrafica;
                }
                await _dbContext.SaveChangesAsync(); // Aggiorna i dati sul DB
                return entityEntry.Entity;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        // Aggiungi Anagrafica da Gestione
        public async Task<Anagrafica> AddAnagraficaMemberAsync(Anagrafica anagrafica, string email)
        {
            try
            {
                AspNetUserRoles tempObj = new();
                // Se i seguenti valori son nulli li imposta a ''
                anagrafica.CodFiscale ??= "";
                anagrafica.Indirizzo ??= "";
                anagrafica.Telefono ??= "";
                anagrafica.Mail ??= "";
                anagrafica.Sesso ??= "";
                // [LUCA]: why isn't it on it? Either it's wrong to put it here or it's missing above
                //[XXXX] : this may be null , we have to give default value here or in model to convert null to empty string.
                // Assegna il codice progressimo (trova il max e somma 1)

                // Aggiunge all'archivio l'anagrafica corretta
                _dbContext.Anagraficas.Update(anagrafica);
                await _dbContext.SaveChangesAsync(); // Aggiorna i dati sul DB

                //[LUCA]: what is the whole code below for? from start to end?
                //[XXXX] : this is to save roles, actually migration is not run update date and i a m also not changing any thing in migration
                // other wise this is save automatically if we pass a list to main object.
                var User = await _dbContext.AspNetUsers.FirstOrDefaultAsync(x => x.CodAnagrafica == anagrafica.CodAnagrafica);
                if (User != null)
                {

                    foreach (var i in anagrafica.RoleList)
                    {
                        tempObj.UserId = User.Id;
                        tempObj.RoleId = i.RoleId;
                        await _dbContext.AspNetUserRoles.AddAsync(tempObj);
                        await _dbContext.SaveChangesAsync();
                        tempObj = new();
                    }
                }

                return anagrafica;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // Aggiorna l'anagrafica
        public async Task<Anagrafica> UpdateAnagraficaAsync(Anagrafica anagrafica)
        {
            try
            {
                _dbContext.Anagraficas.Update(anagrafica); // dice all'archivio delle anagrafiche di aggiornare quella passata
                await _dbContext.SaveChangesAsync(); // Aggiorna i dati sul DB
                return anagrafica; // Restituisce l'anagrafica inserita
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<Anagrafica> UpdateAnagraficaMemberAsync(Anagrafica anagrafica)
        {
            try
            {
                AspNetUserRoles tempObj = new();
                var User = await _dbContext.AspNetUsers.FirstOrDefaultAsync(x => x.CodAnagrafica == anagrafica.CodAnagrafica);
                if (User != null)
                {
                    var result = await _dbContext.AspNetUserRoles
        .Where(x => x.UserId == User.Id)
        .ToListAsync();
                    if (result.Any() && result.Count > 0)
                    {
                        _dbContext.AspNetUserRoles.RemoveRange(result);
                        await _dbContext.SaveChangesAsync();
                    }

                }

                foreach (var i in anagrafica.RoleList)
                {

                    tempObj.UserId = User.Id;
                    tempObj.RoleId = i.RoleId;
                    await _dbContext.AspNetUserRoles.AddAsync(tempObj);
                    await _dbContext.SaveChangesAsync();
                    tempObj = new();
                }

                _dbContext.Anagraficas.Update(anagrafica); // dice all'archivio delle anagrafiche di aggiornare quella passata
                await _dbContext.SaveChangesAsync(); // Aggiorna i dati sul DB
                return anagrafica; // Restituisce l'anagrafica inserita
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // Cancella l'anagrafica passandogli il suo codice

        public async Task<bool> DeleteAnagraficaAsync(int id)
        {
            try
            {
                var anagrafica = await _dbContext.Anagraficas.FindAsync(id); // La cerca
                if (anagrafica == null) // Se non la trova
                {
                    return false; // Anagrafica non trovata
                }

                _dbContext.Anagraficas.Remove(anagrafica); // Se invece la trova la rimuove
                await _dbContext.SaveChangesAsync(); // Aggiorna i dati sul DB
                return true;
            }
            catch (Exception ex)
            {
                // Handle exception at later
                throw;
            }
        }
    }

}
