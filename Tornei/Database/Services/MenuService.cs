using Database.DTOs;
using Database.Models;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;



namespace Database.Services
{
    public class MenuService
    {
        int flag = 0;

        private readonly TorneiContext _dbContext; // Replace YourDbContext with the actual name of your DbContext

        public MenuService(TorneiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<MenuHierarchyDto> GetMenuItems()
        {
            try
            {

                // Mi carico il menù dal database
                using (var context = new TorneiContext())
                {
                    List<MenuHierarchyDto> hierarchicalMenu = context.MenuHierarchyDtos
                .FromSqlRaw("EXEC GetMenu")
                .AsNoTracking()
                .ToList();

                    // Ritorno il menù letto precedentemente e formattato dalla procedura di seguito.
                    return OrganizeMenuItems(hierarchicalMenu, 0);
                }
            }
            catch (Exception ex)
            {
                // visualizzo l'errore a video, poi andrà gestito l'errore
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<MenuHierarchyDto>();
            }
        }

        // Trovo i menù di primo livello che servono per caricare la drop down list
        public async Task<List<MenuVistaDto>> GetFirstLevelMenuItems(string UserId)
        {
            try
            {
                using (var context = new TorneiContext()) // Dichiaro la variabile database
                {
                    // Dichiaro una lista di parametri SQL per la stored procedure
                    var param = new SqlParameter[] {
                  new SqlParameter() {
                     ParameterName = "@UserId",
                     SqlDbType =  System.Data.SqlDbType.VarChar,
                     Direction = System.Data.ParameterDirection.Input,
                     Value =UserId
                  },
               };
                    // Eseguo la store procedure passandogli i parametri
                    var menus = await context.MenuVistaDtos.FromSqlRaw("EXEC GetFirstLevelMenuItems @UserId", param).AsNoTracking().ToListAsync();
                    return menus; // Restituisco il risultato (menù)
                }
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        // Trovo i menù di livello inferiore
        public async Task<List<MenuHierarchyDto>> GetNextLevelMenuItems(string UserId, int CodMenuPadre)
        {
            try
            {
                // Dichiaro una lista di parametri SQL per la stored procedure
                var param = new SqlParameter[] {
               // Parametro UserID
               new SqlParameter() {
                  ParameterName = "@UserId",
                  SqlDbType =  System.Data.SqlDbType.VarChar,
                  Direction = System.Data.ParameterDirection.Input,
                  Value =UserId
               },
               // Parametro CodMenuPadre
               new SqlParameter() {
                  ParameterName = "@CodMenuPadre",
                  SqlDbType =  System.Data.SqlDbType.Int,
                  Direction = System.Data.ParameterDirection.Input,
                  Value =CodMenuPadre
               },
            };

                using (var context = new TorneiContext()) // Replace YourDbContext with the actual name of your DbContext
                {
                    // Dichiaro la variabile che conterrà il menu e gli dico di prendersi i valori che tornano dall'esecuzione della stored procedure SQL
                    List<MenuHierarchyDto> hierarchicalMenu = context.MenuHierarchyDtos
                       .FromSqlRaw("EXEC GetNextMenuItemsHer @UserId,@CodMenuPadre", param)
                       .AsNoTracking()
                       .ToList();
                    return OrganizeMenuItems(hierarchicalMenu, CodMenuPadre); // Resituitsco il menù rielaborato passo come parametro il padre principale
                }
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        // Trasforma il menù da tabella di database ad albero
        // In pratica costruisco l'albero dei menù
        private List<MenuHierarchyDto> OrganizeMenuItems(List<MenuHierarchyDto> flatMenu, int parentID)
        {
            List<MenuHierarchyDto> organizedMenu = new List<MenuHierarchyDto>();

            // Per ogni menù che hanno lo stesso padre
            foreach (var menuItem in flatMenu.Where(m => m.CodMenuPadre == parentID))
            {
                menuItem.Figli = OrganizeMenuItems(flatMenu, menuItem.CodMenu); // ripeto l'operazione per ogni figlio
                organizedMenu.Add(menuItem); // Aggiungo il menù in questo livello
            }

            return organizedMenu;
        }

        // Aggiungo un menù
        public async Task<string> AddMenuAsync(Menu menu)
        {
            try
            {
                using (var context = new TorneiContext())
                {
                    // trovo il menù più grande (per codice)
                    int maxcode = await context.Menus.MaxAsync(s => (int?)s.CodMenu) ?? 0;

                    int nextCode = maxcode + 1; // Incremento di 1 il codice massimo trovato
                    menu.CodMenu = nextCode; // Lo assegno alla tabella
                    context.Menus.Add(menu); // Aggiungo il menù al database
                    await context.SaveChangesAsync(); // effettuo la sincronizzazione, salvo realmente su DB
                    foreach (var i in menu.RoleList)
                    {
                        int maxid = await context.MenuRuolos.MaxAsync(x => (int?)x.Id) ?? 0;
                        int nextid = maxid + 1;
                        i.Id = nextid;
                        i.CodMenu = menu.CodMenu;
                        context.MenuRuolos.Add(i);
                        await context.SaveChangesAsync();
                    }
                    return "Saved Successfully"; // Ritorno messaggio di aggiunta riuscita con successo
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Aggiorno il menù
        public async Task UpdateMenuAsync(Menu menu)
        {

            //removal of existing  MenuRole with menu.codMenu
            var result = await _dbContext.MenuRuolos
        .Where(x => x.CodMenu == menu.CodMenu)
        .ToListAsync();

            if (result.Any()&& result.Count>1)
            {
                _dbContext.MenuRuolos.RemoveRange(result);
                await _dbContext.SaveChangesAsync();
            }
         
            //update record with changes 
            
            _dbContext.Entry(menu).State = EntityState.Modified; // dichiaro che il menù è stato modificato
            await _dbContext.SaveChangesAsync(); // gli dico di aggiornarlo sul database
            foreach (var i in menu.RoleList)
            {
                int maxid = await _dbContext.MenuRuolos.MaxAsync(x => (int?)x.Id) ?? 0;
                int nextid = maxid + 1;
                i.Id = nextid;
                i.CodMenu = menu.CodMenu;
                _dbContext.MenuRuolos.Add(i);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Cancello il menù
        public async Task DeleteMenuAsync(int id)
        {
            var menu = await _dbContext.Menus.FindAsync(id); // Ricerco il menù tramite codice         
            if (menu != null) // Se lo trovo
            {
                _dbContext.Menus.Remove(menu); // Lo cancello
                await _dbContext.SaveChangesAsync(); // salvo la richiesta sul Database
            }
        }

        // [Luca]: This should perhaps go not in the service menu but in a new service called PaginaHTMLService, because then it will also be used to crud the PaginaHTML table.If I try to do it, okay?
        public async Task<List<PaginaHTML>> GetPaginaHTMLListAsync()
        {
            using (var context = new TorneiContext()) // Replace YourDbContext with the actual name of your DbContext
            {
                return await context.PaginasHTML.AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<MenuRuolo>> GetRolsByMenu(int MenuId)
        {
            return await _dbContext.MenuRuolos.Where(x => x.CodMenu == MenuId).ToListAsync();

        }

        // Trovo il menù tramite codice
        public async Task<Menu> GetMenuByID(int id)
        {
            Menu tempObj = new();
            using (var context = new TorneiContext()) // Dichiaro che uso il Database (context)
            {
                var risultato = await context.Menus.FirstOrDefaultAsync(x => x.CodMenu == id); // Ricerco per codice
                var menurole = await context.MenuRuolos.Where(x => x.CodMenu == id).ToListAsync();
                tempObj = risultato;
                tempObj.RoleList = menurole;
                return tempObj; // Ritorno il codice trovato
            }
        }




    }
}
