using Database.DTOs; // Gestione delle viste

using Microsoft.EntityFrameworkCore;

namespace Database.Models;

public partial class TorneiContext : DbContext
{
    public TorneiContext()
    {
    }

    public TorneiContext(DbContextOptions<TorneiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anagrafica> Anagraficas { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
    public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Campo> Campos { get; set; }

    public virtual DbSet<Comune> Comunes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuRuolo> MenuRuolos { get; set; }

    public virtual DbSet<Societa> Societa { get; set; }
    public virtual DbSet<MenuDto> MenuDtos { get; set; }
    public virtual DbSet<MenuHierarchyDto> MenuHierarchyDtos { get; set; }
    public virtual DbSet<MenuVistaDto> MenuVistaDtos { get; set; } //Menu Vista
    public virtual DbSet<PaginaHTML> PaginasHTML { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=sql6030.site4now.net;Initial Catalog=db_a8b7d6_tornei;Persist Security Info=True;User ID=db_a8b7d6_tornei_admin;Password=Tornei2023.#;Connection Lifetime=0;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuHierarchyDto>().HasKey(e => e.CodMenu);
        modelBuilder.Entity<MenuDto>().HasNoKey();
        modelBuilder.Entity<MenuVistaDto>().HasNoKey();
        //  modelBuilder.Entity<AspNetUserRoles>().HasNoKey();
        modelBuilder.Entity<MenuHierarchyDto>()
            .HasMany(m => m.Figli)
            .WithOne(m => m.Padre)
            .HasForeignKey(m => m.CodMenuPadre)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Anagrafica>(entity =>
        {
            entity.Property(e => e.Cellulare).HasDefaultValue("");
            entity.Property(e => e.CodComuneDiNascita).HasDefaultValueSql("((0))");
            entity.Property(e => e.CodComuneResidenza).HasDefaultValueSql("((0))");
            entity.Property(e => e.Immagine).HasDefaultValue("~/Dati/Immagini/Anagrafica/Vuota.webp");
            entity.Property(e => e.Indirizzo).HasDefaultValue("");
            entity.Property(e => e.IndirizzoResidenza).HasDefaultValue("");
            entity.Property(e => e.Mail).HasDefaultValue("");
            entity.Property(e => e.NoMail).HasDefaultValue(true);
            entity.Property(e => e.NomeCompleto).HasComputedColumnSql("(Trim(([Cognome]+' ')+[Nome]))", false);
            entity.Property(e => e.Sesso).HasDefaultValue("");
            entity.Property(e => e.Telefono).HasDefaultValue("");

            entity.HasOne(d => d.CodComuneDiNascitaNavigation).WithMany(p => p.AnagraficaCodComuneDiNascitaNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Anagrafica_Comune_Nascita");

            entity.HasOne(d => d.CodComuneResidenzaNavigation).WithMany(p => p.AnagraficaCodComuneResidenzaNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Anagrafica_Comune_Residenza");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
        });


        modelBuilder.Entity<AspNetUserRoles>()
          .HasKey(e => new { e.UserId, e.RoleId });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasOne(d => d.CodSocietaNavigation).WithMany(p => p.AspNetUsers).HasConstraintName("FK_AspNetUsers_Societa");

            //entity.HasMany(d => d.Roles).WithMany(p => p.Users)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "AspNetUserRole",
            //        r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
            //        l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
            //        j =>
            //        {
            //            j.HasKey("UserId", "RoleId");
            //            j.ToTable("AspNetUserRoles");
            //            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
            //        });

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
          .UsingEntity<AspNetUserRoles>(
              j =>
              {
                  j.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId");
                  j.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId");
                  j.HasKey("UserId", "RoleId");
                  j.ToTable("AspNetUserRoles");
                  j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
              });
        });

        modelBuilder.Entity<Campo>(entity =>
        {
            entity.Property(e => e.CodCampo).ValueGeneratedNever();
            entity.Property(e => e.CodSocieta).HasComment("");
            entity.Property(e => e.GeoLocalizzazione).HasDefaultValue("");
            entity.Property(e => e.Nome).HasComment("");
            entity.Property(e => e.NomeCustode)
                .HasDefaultValue("")
                .IsFixedLength();
            entity.Property(e => e.TelefonoCustode)
                .HasDefaultValue("")
                .IsFixedLength();
            entity.Property(e => e.Tipologia).IsFixedLength();

            entity.HasOne(d => d.CodSocietaNavigation).WithMany(p => p.Campos).HasConstraintName("FK_Campo_Societa");
        });

        modelBuilder.Entity<Comune>(entity =>
        {
            entity.Property(e => e.Cap).HasDefaultValue("");
            entity.Property(e => e.CodComuneNuovo).HasDefaultValueSql("((0))");
            entity.Property(e => e.DesComune).HasDefaultValue("");
            entity.Property(e => e.DesComuneEstesa).HasComputedColumnSql("(case when [nazione]='ITALIA' then case when [DesComune]=[Provincia] then ((' '+[Provincia])+' - ')+[CAP] else ((([Provincia]+' - ')+[DesComune])+' - ')+[CAP] end else [NAZIONE] end+case when [Obsoleto]=(1) then ' (Obsoleto)' else '' end)", false);
            entity.Property(e => e.Nazione).HasDefaultValue("");
            entity.Property(e => e.Prefisso).HasDefaultValue("");
            entity.Property(e => e.Provincia).HasDefaultValue("ITALIA");
            entity.Property(e => e.Regione).HasDefaultValue("");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.Property(e => e.DataFinePubblicazione).HasDefaultValue(new DateTime(2100, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));
            entity.Property(e => e.DataInizioPubblicazione).HasDefaultValue(new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            entity.Property(e => e.Parametro).HasDefaultValue("");
            entity.Property(e => e.Route).HasDefaultValue("");
            entity.Property(e => e.ToolTip).HasDefaultValue("");
        });

        modelBuilder.Entity<MenuRuolo>(entity =>
        {
            entity.HasOne(d => d.CodMenuNavigation).WithMany().HasConstraintName("FK_MenuRuolo_Menu");

            entity.HasOne(d => d.Role).WithMany().HasConstraintName("FK_MenuRuolo_AspNetRoles");
        });

        modelBuilder.Entity<Societa>(entity =>
        {
            entity.Property(e => e.CodSocieta).ValueGeneratedNever();
            entity.Property(e => e.CodComune).HasDefaultValueSql("((0))");
            entity.Property(e => e.Logo).HasDefaultValue("~/Dati/Immagini/Loghi_Societa_Sportive/Vuota.webp");
            entity.Property(e => e.Nota).HasDefaultValue("~/Dati/Immagini/Loghi_Societa_Sportive/Vuota.webp");
            entity.Property(e => e.QualificaClub).HasDefaultValue("A");
            entity.Property(e => e.SitoInternet).HasDefaultValue("~/Dati/Immagini/Loghi_Societa_Sportive/Vuota.webp");

            entity.HasOne(d => d.CodComuneNavigation).WithMany(p => p.Societa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Societa_Comune");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
