using Database.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Database.Services
{
    public class SocietaService
    {
        private readonly TorneiContext _dbContext;

        public SocietaService(TorneiContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Cerca la società tramite il suo Codice
        public async Task<Societa> GetSocietaByCodeAsync(int codSocieta)
        {
            return await _dbContext.Societa.FindAsync(codSocieta);
        }

        // Cerca la società tramite lo Userd ID
        public async Task<Societa> GetSocietaByUserIdAsync(string id)
        {
            // Ricerca il codice nella tabella degli utenti
            var user = _dbContext.AspNetUsers.FirstOrDefault(x => x.Id == id);
            if (user != null) // Se lo trova
            {
                var soc = user.CodSocieta; // Memorizzo il codice
                var societaObj = await _dbContext.Societa.FindAsync(soc); // Effettuo la ricerca sulla tabella delle societa              
                if (societaObj != null) // Se la trovo
                {
                    // Cerco i ruoli della società
                    var UserRole = await _dbContext.AspNetUserRoles.Where(x => x.UserId == user.Id).ToListAsync();
                    societaObj.UserRoleList = UserRole; // 
                    return societaObj; // Ritorno il codice trovato
                }
            }
            return default;
        }

        // Cerca la società per descrizione
        public async Task<Societa?> GetSocietaByDescriptionAsync(string description)
        {
            // [LUCA]: here too we don't use tournamentdbcontext but always applicationdbcontext
            // Ricerco nella tabella delle società, e restituosco la società o null se non la trovo
            using (var context = new TorneiContext())
            {
                Societa societa = await context.Societa.FirstOrDefaultAsync(s => s.DesSocieta == description);
                return societa; // societa can be null
            }
        }

        // [LUCA]: here too we don't use tournamentdbcontext but always applicationdbcontext
        // Trova tutte le società presenti in archivio
        public async Task<List<Societa>> GetAllSocietaAsync()
        {
            using (var context = new TorneiContext()) // Dichiaro che uso il Database (context)
            {
                return await context.Societa.ToListAsync();
            }
        }

        // Salva la società
        public async Task<Societa> SaveSocietaAsync(Societa societa, string email, ImageModel imageModel)
        {
            try
            {
                // Se i seguenti valori son nulli li imposta a ''
                societa.PartitaIva ??= "";
                societa.Pec ??= "";
                societa.CodiceDestinatario ??= "";
                societa.Telefono ??= "";

                // Assegna il codice progressimo (trova il max e somma 1)
                int maxCodSocieta = await _dbContext.Societa.MaxAsync(s => (int?)s.CodSocieta) ?? 0;
                int nextCodSocieta = maxCodSocieta + 1;
                societa.CodSocieta = nextCodSocieta;

                // Aggiunge all'archivio l'anagrafica corretta
                var entityEntry = await _dbContext.Societa.AddAsync(societa);

                // Aggiorna il codice della società nella tabella degli utenti
                var user = await _dbContext.AspNetUsers.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    user.CodSocieta = societa.CodSocieta;
                }

                await _dbContext.SaveChangesAsync(); // Aggiorna i dati sul DB
                return entityEntry.Entity; // Ritorna la società appena inserita
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // Aggiorna la società
        public async Task<Societa> UpdateSocietaAsync(Societa societa, string email, ImageModel imageModel)
        {
            try
            {
                // Cerco la società per codice 
                var entityUpdate = await _dbContext.Societa.FindAsync(societa.CodSocieta);
                if (entityUpdate != null) // Se la trovo
                {
                    // Aggiorno ogni campo con il valore passato 
                    entityUpdate.PartitaIva = societa.PartitaIva;
                    entityUpdate.Matricola = societa.Matricola;
                    entityUpdate.QualificaClub = societa.QualificaClub;
                    entityUpdate.CodComune = societa.CodComune;
                    entityUpdate.Indirizzo = societa.Indirizzo;
                    entityUpdate.Telefono = societa.Telefono;
                    entityUpdate.Cellulare = societa.Cellulare;
                    entityUpdate.Mail = societa.Mail;
                    entityUpdate.SitoInternet = societa.SitoInternet;
                    entityUpdate.Nota = societa.Nota;
                    entityUpdate.CodiceDestinatario = societa.CodiceDestinatario;
                    entityUpdate.Iban = societa.Iban;
                    entityUpdate.Pec = societa.Pec;
                    entityUpdate.Logo = societa.Logo;
                }
                // // Aggiorna i dati sul DB
                await _dbContext.SaveChangesAsync();
                return entityUpdate;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Aggiorna i ruoli per la società
        public async Task<Societa> UpdateSocietaRoleAsync(Societa societa)
        {
            try
            {
                AspNetUserRoles tempObj = new();
                // Cerco l'untente nella della società
                var User = await _dbContext.AspNetUsers.FirstOrDefaultAsync(x => x.CodSocieta == societa.CodSocieta);
                if (User != null) // Se l trovo
                {
                    // Cerco i ruoli dell'utente
                    var result = await _dbContext.AspNetUserRoles.Where(x => x.UserId == User.Id).ToListAsync();
                    if (result.Any() && result.Count > 0) // Se trovo almeno un ruolo
                    {
                        _dbContext.AspNetUserRoles.RemoveRange(result); // Li cancello
                        await _dbContext.SaveChangesAsync(); // Aggiorno il database
                    }
                }

                // Per ogni ruolo presente nella lista
                foreach (var i in societa.RoleList)
                {
                    // imposto i valori dell'oggetto temporaneo
                    tempObj.UserId = User.Id;
                    tempObj.RoleId = i.RoleId;
                    await _dbContext.AspNetUserRoles.AddAsync(tempObj); // Lo aggiungo all'archivio
                    await _dbContext.SaveChangesAsync(); // Aggiorno il database
                    tempObj = new(); // Creo un nuovo oggetto vuoto contenente i ruoli
                }

                // Cerco la società da aggiornare
                var entityUpdate = await _dbContext.Societa.FindAsync(societa.CodSocieta);
                if (entityUpdate != null) // Se la trovo
                {
                    // Aggiorno ogni campo con il valore passato 
                    entityUpdate.PartitaIva = societa.PartitaIva;
                    entityUpdate.Matricola = societa.Matricola;
                    entityUpdate.QualificaClub = societa.QualificaClub;
                    entityUpdate.CodComune = societa.CodComune;
                    entityUpdate.Indirizzo = societa.Indirizzo;
                    entityUpdate.Telefono = societa.Telefono;
                    entityUpdate.Cellulare = societa.Cellulare;
                    entityUpdate.Mail = societa.Mail;
                    entityUpdate.SitoInternet = societa.SitoInternet;
                    entityUpdate.Nota = societa.Nota;
                    entityUpdate.CodiceDestinatario = societa.CodiceDestinatario;
                    entityUpdate.Iban = societa.Iban;
                    entityUpdate.Pec = societa.Pec;
                    entityUpdate.Logo = societa.Logo;
                }
                // // Aggiorna i dati sul DB
                await _dbContext.SaveChangesAsync(); // Aggiorno il database
                return entityUpdate; // Ritorno l'entità aggiornata
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // CAncella la società
        public async Task<bool> DeleteSocietaAsync(int id)
        {
            try
            {
                var societa = await _dbContext.Societa.FindAsync(id); // La cerca
                _dbContext.Societa.Remove(societa); // Se invece la trova la rimuove
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
