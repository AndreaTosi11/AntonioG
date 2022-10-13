using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectWorkDemo.Models
{
    public partial class ProjectWorkDatabaseContext : DbContext
    {
        public ProjectWorkDatabaseContext()
        {
        }

        public ProjectWorkDatabaseContext(DbContextOptions<ProjectWorkDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Dettaglio> Dettaglio { get; set; } = null!;
        public virtual DbSet<Fornitori> Fornitori { get; set; } = null!;
        public virtual DbSet<Lavorazione> Lavorazione { get; set; } = null!;
        public virtual DbSet<Mezzo> Mezzo { get; set; } = null!;
        public virtual DbSet<Produttore> Produttore { get; set; } = null!;
        public virtual DbSet<ServiziFornitore> ServiziFornitore { get; set; } = null!;
        public virtual DbSet<TipoForn> TipoForn { get; set; } = null!;
        public virtual DbSet<GetMezziById> GetMezziById { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ProjectWorkDatabase;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Cognome).IsRequired();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Nome).IsRequired();

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Telefono).IsRequired();

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Dettaglio>(entity =>
            {
                entity.HasKey(e => e.IdDettaglio);

                entity.ToTable("Dettaglio");

                entity.Property(e => e.CognomeLavoratore).HasMaxLength(30);

                entity.Property(e => e.CostoNetto).HasColumnType("money");

                entity.Property(e => e.CostoTotDet)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([CostoNetto]+([CostoNetto]*[Iva])/(100))", false);

                entity.Property(e => e.DataFineLavoro).HasColumnType("datetime");

                entity.Property(e => e.DataInizioLavoro).HasColumnType("datetime");

                entity.Property(e => e.NomeLavoratore).HasMaxLength(30);

                entity.Property(e => e.TipoLavorazione).HasMaxLength(50);

                entity.HasOne(d => d.IdLavorazioneNavigation)
                    .WithMany(p => p.Dettaglio)
                    .HasForeignKey(d => d.IdLavorazione)
                    .HasConstraintName("FK_Dettaglio_Lavorazione");
            });

            modelBuilder.Entity<Fornitori>(entity =>
            {
                entity.HasKey(e => e.IdFornitori);

                entity.ToTable("Fornitori");

                entity.Property(e => e.DataCreazione).HasColumnType("datetime");

                entity.Property(e => e.Indirizzo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Latitudine)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Longitudine)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.NomeDitta)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Piva)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(10);         
            });


            modelBuilder.Entity<Lavorazione>(entity =>
            {
                entity.HasKey(e => e.IdLavorazione);

                entity.ToTable("Lavorazione");

                entity.Property(e => e.CodiceIdentificativo).HasMaxLength(9);

                entity.Property(e => e.DataLavorazione).HasColumnType("datetime");

                entity.Property(e => e.NumGaranzia)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.CostoTotaleLavorazione)
                    .HasColumnType("money")
                    .HasComputedColumnSql("([dbo].[CostoTotaleLavorazione]([IdLavorazione]))", false);


                entity.HasOne(d => d.IdFornitoriNavigation)
                    .WithMany(p => p.Lavorazione)
                    .HasForeignKey(d => d.IdFornitori)
                    .HasConstraintName("FK_Lavorazione_Fornitori");

                entity.HasOne(d => d.IdMezzoNavigation)
                    .WithMany(p => p.Lavorazione)
                    .HasForeignKey(d => d.IdMezzo)
                    .HasConstraintName("FK_SchedaLavoro_Mezzo");
            });

            modelBuilder.Entity<Mezzo>(entity =>
            {
                entity.HasKey(e => e.IdMezzo);

                entity.ToTable("Mezzo");

                entity.Property(e => e.DataImmatricolazione).HasColumnType("datetime");

                entity.Property(e => e.Targa).HasMaxLength(7);

                entity.Property(e => e.TipoAuto).HasMaxLength(50);

                entity.HasOne(d => d.IdProduttoreNavigation)
                    .WithMany(p => p.Mezzo)
                    .HasForeignKey(d => d.IdProduttore)
                    .HasConstraintName("FK_Mezzo_Produttore");
            });

            modelBuilder.Entity<Produttore>(entity =>
            {
                entity.HasKey(e => e.IdProduttore);

                entity.ToTable("Produttore");

                entity.Property(e => e.IdProduttore).ValueGeneratedNever();

                entity.Property(e => e.Denominazione).HasMaxLength(50);
            });

            modelBuilder.Entity<ServiziFornitore>(entity =>
            {
                entity.HasKey(e => e.IdServizio);

                entity.ToTable("ServiziFornitore");

                entity.HasOne(d => d.IdFornitoriNavigation)
                    .WithMany(p => p.ServiziFornitore)
                    .HasForeignKey(d => d.IdFornitori)
                    .HasConstraintName("FK_ServiziFornitore_Fornitori");

                entity.HasOne(d => d.IdTipoFornNavigation)
                    .WithMany(p => p.ServiziFornitore)
                    .HasForeignKey(d => d.IdTipoForn)
                    .HasConstraintName("FK_ServiziFornitore_TipoForn");
            });

            modelBuilder.Entity<TipoForn>(entity =>
            {
                entity.HasKey(e => e.IdTipoForn);

                entity.ToTable("TipoForn");

                entity.Property(e => e.Descrizione).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
