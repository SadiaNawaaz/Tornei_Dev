using Blazored.Toast; // Aggiungo i popup bellini TOAST
using Database.Models; // Aggiungo i modelli del progetto database
using Database.Services;  // Aggiungo i servizi del modello database
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Tornei.Components; // Aggiungo i componenti
using Tornei.Components.Account; // Aggiungo i componendi di login forse non serve
using Tornei.Data; // Aggiungo la connessione al database
using Syncfusion.Blazor;  // Aggiungo syncfusion
using Blazored.LocalStorage;  // Aggiungo local Storage 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddBlazoredLocalStorage(); /*Aggiungo local Storage*/
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
   options.DefaultScheme = IdentityConstants.ApplicationScheme;
   options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// INIZIO MODIFICHE

// Registro la licenza di syncfusion
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF5cWWJCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWX5eeHRTQmVfV0N3X0E=");

//builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>(); // Servizio di gestione posta
builder.Services.AddSyncfusionBlazor(); // Suncfusion
builder.Services.AddBlazoredToast(); // Aggiungo i popup bellini TOAST




// Dichiarare qui tutte le interfacce da usare con il relativo servizio
builder.Services.AddScoped<ComuneService>(); // Comune
builder.Services.AddScoped<UserService>(); // Gestione Utenti
builder.Services.AddScoped<SocietaService>(); // Societa Profilo
builder.Services.AddScoped<AnagraficaService>(); // Anagrafica Profilo
builder.Services.AddScoped<MenuService>(); // Menu
builder.Services.AddScoped<RoleService>(); // Ruolo
builder.Services.AddScoped<CampoServices>(); // Campo
builder.Services.AddScoped<ValidationServices>(); // Nuovo Servizio di validazione
builder.Services.AddDbContext<TorneiContext>(options => options.UseSqlServer(connectionString)); // Connessione dal database Tornei
//[LUCA]: ServiceLifetime.Scoped  what is this addition in the line above for
// FINE  MODIFICHE

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseMigrationsEndPoint();
}
else
{
   app.UseExceptionHandler("/Error", createScopeForErrors: true);
// Il valore HSTS predefinito è 30 giorni.Potrebbe essere necessario modificarlo per scenari di produzione, vedere  https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Aggiungere ulteriori endpoint richiesti dai componenti Identity/Account Razor.
app.MapAdditionalIdentityEndpoints();

app.Run();
