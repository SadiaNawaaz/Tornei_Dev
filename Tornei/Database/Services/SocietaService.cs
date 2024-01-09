using Database.Models;

using Microsoft.EntityFrameworkCore;

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
            return await _dbContext.Societa.FindAsync(soc); // Effettuo la ricerca sulla tabella delle societa
         }
         return default;
      }

      // Cerca la società per descrizione
      public async Task<Societa?> GetSocietaByDescriptionAsync(string description)
      {
         // Ricerco nella tabella delle società, e restituosco la società o null se non la trovo
         Societa societa = await _dbContext.Societa.FirstOrDefaultAsync(s => s.DesSocieta == description);
         return societa; // societa can be null
      }

      // Trova tutte le società presenti in archivio
      public async Task<List<Societa>> GetAllSocietaAsync()
      {
         return await _dbContext.Societa.ToListAsync();
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
   }
}
