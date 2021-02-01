using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Lab3Databas
{
    public partial class Lab2BokhandelContext : DbContext
    {
        public Lab2BokhandelContext()
        {
        }

        public Lab2BokhandelContext(DbContextOptions<Lab2BokhandelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Butiker> Butikers { get; set; }
        public virtual DbSet<Böcker> Böckers { get; set; }
        public virtual DbSet<Författare> Författares { get; set; }
        public virtual DbSet<Kunder> Kunders { get; set; }
        public virtual DbSet<Lagersaldo> Lagersaldos { get; set; }
        public virtual DbSet<OrderHuvud> OrderHuvuds { get; set; }
        public virtual DbSet<OrderRader> OrderRaders { get; set; }
        public virtual DbSet<TitlarPerFörfattare> TitlarPerFörfattares { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=Lab2 - Bokhandel;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Finnish_Swedish_CI_AS");

            modelBuilder.Entity<Butiker>(entity =>
            {
                entity.HasKey(e => e.ButiksId);

                entity.ToTable("Butiker");

                entity.Property(e => e.ButiksId)
                    .ValueGeneratedNever()
                    .HasColumnName("ButiksID");

                entity.Property(e => e.Butiksnamn).IsUnicode(false);

                entity.Property(e => e.Stad).IsUnicode(false);

                entity.Property(e => e.TelefonNr)
                    .HasMaxLength(15)
                    .HasColumnName("Telefon nr")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Böcker>(entity =>
            {
                entity.HasKey(e => e.Isbn);

                entity.ToTable("Böcker");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("ISBN");

                entity.Property(e => e.FörfattareId).HasColumnName("FörfattareID");

                entity.Property(e => e.Pris).HasColumnType("money");

                entity.Property(e => e.Språk).IsUnicode(false);

                entity.Property(e => e.Titel).IsUnicode(false);

                entity.Property(e => e.Utgivningsdatum).HasColumnType("date");

                entity.HasOne(d => d.Författare)
                    .WithMany(p => p.Böckers)
                    .HasForeignKey(d => d.FörfattareId)
                    .HasConstraintName("FK_Böcker_Författare");
            });

            modelBuilder.Entity<Författare>(entity =>
            {
                entity.ToTable("Författare");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Efternamn).IsUnicode(false);

                entity.Property(e => e.Födelsedatum).HasColumnType("date");

                entity.Property(e => e.Förnamn).IsUnicode(false);
            });

            modelBuilder.Entity<Kunder>(entity =>
            {
                entity.HasKey(e => e.KundId)
                    .HasName("PK__Kunder__F2B5DEAC870FA300");

                entity.ToTable("Kunder");

                entity.Property(e => e.KundId).HasColumnName("KundID");

                entity.Property(e => e.Efternamn).IsUnicode(false);

                entity.Property(e => e.Förnamn).IsUnicode(false);

                entity.Property(e => e.TelefonNr)
                    .HasMaxLength(10)
                    .HasColumnName("Telefon nr")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Lagersaldo>(entity =>
            {
                entity.HasKey(e => new { e.ButiksId, e.Isbn })
                    .HasName("PK_Lagersaldo_1");

                entity.ToTable("Lagersaldo");

                entity.Property(e => e.ButiksId).HasColumnName("ButiksID");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("ISBN");

                entity.HasOne(d => d.Butiks)
                    .WithMany(p => p.Lagersaldos)
                    .HasForeignKey(d => d.ButiksId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lagersaldo_Butiker1");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.Lagersaldos)
                    .HasForeignKey(d => d.Isbn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lagersaldo_Böcker1");
            });

            modelBuilder.Entity<OrderHuvud>(entity =>
            {
                entity.HasKey(e => e.Ordernr);

                entity.ToTable("OrderHuvud");

                entity.Property(e => e.Ordernr).ValueGeneratedNever();

                entity.Property(e => e.Beställningsdatum).HasColumnType("date");

                entity.Property(e => e.KundId).HasColumnName("KundID");

                entity.Property(e => e.Leveranstatus)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Kund)
                    .WithMany(p => p.OrderHuvuds)
                    .HasForeignKey(d => d.KundId)
                    .HasConstraintName("FK_OrderHuvud_Kunder");
            });

            modelBuilder.Entity<OrderRader>(entity =>
            {
                entity.HasKey(e => new { e.Ordernr, e.Radnummer });

                entity.ToTable("OrderRader");

                entity.Property(e => e.Ordernr).ValueGeneratedOnAdd();

                entity.Property(e => e.Isbn)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Pris).HasColumnType("money");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.OrderRaders)
                    .HasForeignKey(d => d.Isbn)
                    .HasConstraintName("FK_OrderRader_Böcker");

                entity.HasOne(d => d.OrdernrNavigation)
                    .WithMany(p => p.OrderRaders)
                    .HasForeignKey(d => d.Ordernr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderRader_OrderHuvud");
            });

            modelBuilder.Entity<TitlarPerFörfattare>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("TitlarPerFörfattare");

                entity.Property(e => e.Lagervärde).HasMaxLength(4000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
