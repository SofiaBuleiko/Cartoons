using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CartoonsWebApp
{
    public partial class CartoonsContext : DbContext
    {
        public CartoonsContext()
        {
        }

        public CartoonsContext(DbContextOptions<CartoonsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Awards> Awards { get; set; }
        public virtual DbSet<CartoonAwards> CartoonAwards { get; set; }
        public virtual DbSet<CartoonHeroes> CartoonHeroes { get; set; }
        public virtual DbSet<Cartoons> Cartoons { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<FilmStudios> FilmStudios { get; set; }
        public virtual DbSet<GenreCartoons> GenreCartoons { get; set; }
        public virtual DbSet<Genres> Genres { get; set; }
        public virtual DbSet<Peoples> Peoples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=WIN-M3V7UJRCC4A\\SQLEXPRESS; Database=Cartoons; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Awards>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CartoonAwards>(entity =>
            {
                entity.HasOne(d => d.Awards)
                    .WithMany(p => p.CartoonAwards)
                    .HasForeignKey(d => d.AwardsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartoonAwards_Awards");

                entity.HasOne(d => d.Cartoons)
                    .WithMany(p => p.CartoonAwards)
                    .HasForeignKey(d => d.CartoonsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartoonAwards_Cartoons");
            });

            modelBuilder.Entity<CartoonHeroes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Cartoons)
                    .WithMany(p => p.CartoonHeroes)
                    .HasForeignKey(d => d.CartoonsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartoonHeroes_Cartoons");
            });

            modelBuilder.Entity<Cartoons>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.FilmStudios)
                    .WithMany(p => p.Cartoons)
                    .HasForeignKey(d => d.FilmStudiosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cartoons_FilmStudios");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FilmStudios>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Countries)
                    .WithMany(p => p.FilmStudios)
                    .HasForeignKey(d => d.CountriesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FilmStudios_Countries");
            });
            modelBuilder.Entity<GenreCartoons>(entity =>
            {
                entity.HasOne(d => d.Cartoons)
                    .WithMany(p => p.GenreCartoons)
                    .HasForeignKey(d => d.CartoonsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenreCartoons_Cartoons");

                entity.HasOne(d => d.Genres)
                    .WithMany(p => p.GenreCartoons)
                    .HasForeignKey(d => d.GenresId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenreCartoons_Genres");
            });

            modelBuilder.Entity<Genres>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Peoples>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Peoples)
                    .HasForeignKey<Peoples>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Peoples_CartoonHeroes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}