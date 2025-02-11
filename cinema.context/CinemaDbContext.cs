﻿using cinema.context.Entities;
using Microsoft.EntityFrameworkCore;

namespace cinema.context;

public class CinemaDbContext(DbContextOptions<CinemaDbContext> options) : DbContext(options)
{
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Screening> Screenings { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var polandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        modelBuilder.Entity<Category>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.Property(eb => eb.Id).HasColumnType("char(36)");
            eb.Property(eb => eb.Name).HasColumnType("varchar(100)");

            eb.Property(eb => eb.CreatedOnUtc)
                .HasConversion(
                    v => v.UtcDateTime,
                    v => TimeZoneInfo.ConvertTimeFromUtc(v, polandTimeZone)
                )
                .HasColumnType("datetime(6)");
            eb.Property(eb => eb.ModifiedOnUtc)
                .HasConversion(
                    v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                    v => v.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(v.Value, polandTimeZone) : (DateTimeOffset?)null
                )
                .HasColumnType("datetime(6)");
        });

        modelBuilder.Entity<Movie>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.Property(eb => eb.Id).HasColumnType("char(36)");
            eb.Property(eb => eb.Title).HasColumnType("varchar(100)");
            eb.Property(eb => eb.DurationMinutes).HasColumnType("int");
            eb.Property(eb => eb.PosterUrl).HasColumnType("varchar(100)");
            eb.Property(eb => eb.TrailerUrl).HasColumnType("varchar(100)");
            eb.Property(eb => eb.BackgroundUrl).HasColumnType("varchar(100)");
            eb.Property(eb => eb.Director).HasColumnType("varchar(100)");
            eb.Property(eb => eb.Cast).HasColumnType("varchar(100)");
            eb.Property(eb => eb.Description).HasColumnType("varchar(1000)");
            eb.Property(eb => eb.Rating).HasColumnType("double");
            eb.Property(eb => eb.CategoryId).HasColumnType("char(36)");

            eb.Property(eb => eb.CreatedOnUtc)
                .HasConversion(
                    v => v.UtcDateTime,
                    v => TimeZoneInfo.ConvertTimeFromUtc(v, polandTimeZone)
                )
                .HasColumnType("datetime(6)");
            eb.Property(eb => eb.ModifiedOnUtc)
                .HasConversion(
                    v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                    v => v.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(v.Value, polandTimeZone) : (DateTimeOffset?)null
                )
                .HasColumnType("datetime(6)");

            eb.HasOne(m => m.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            eb.HasMany(m => m.Screenings)
                .WithOne(s => s.Movie)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Screening>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.Property(eb => eb.Id).HasColumnType("char(36)");
            eb.Property(eb => eb.StartDateTime)
                .HasConversion(
                    v => v.UtcDateTime,
                    v => TimeZoneInfo.ConvertTimeFromUtc(v, polandTimeZone)
                )
                .HasColumnType("datetime(6)");
            eb.Property(eb => eb.EndDateTime)
                .HasConversion(
                    v => v.UtcDateTime,
                    v => TimeZoneInfo.ConvertTimeFromUtc(v, polandTimeZone)
                )
                .HasColumnType("datetime(6)");
            eb.Property(eb => eb.MovieId).HasColumnType("char(36)");

            eb.Property(eb => eb.CreatedOnUtc)
                .HasConversion(
                    v => v.UtcDateTime,
                    v => TimeZoneInfo.ConvertTimeFromUtc(v, polandTimeZone)
                )
                .HasColumnType("datetime(6)");
            eb.Property(eb => eb.ModifiedOnUtc)
                .HasConversion(
                    v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                    v => v.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(v.Value, polandTimeZone) : (DateTimeOffset?)null
                )
                .HasColumnType("datetime(6)");

            eb.HasOne(m => m.Movie)
                .WithMany(m => m.Screenings)
                .HasForeignKey(c => c.MovieId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Seat>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.Property(eb => eb.Id).HasColumnType("char(36)");
            eb.Property(eb => eb.Row).HasColumnType("varchar(1)");
            eb.Property(eb => eb.Number).HasColumnType("int");
        });

        modelBuilder.Entity<Order>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.Property(eb => eb.Id).HasColumnType("char(36)");
            eb.Property(eb => eb.Email).HasColumnType("varchar(100)");
            eb.Property(eb => eb.PhoneNumber).HasColumnType("varchar(100)");
            eb.Property(eb => eb.FourDigitCode).HasColumnType("varchar(4)");
            eb.Property(eb => eb.Status).HasColumnType("varchar(100)");
            eb.Property(eb => eb.ScreeningId).HasColumnType("char(36)");

            eb.Property(eb => eb.CreatedOnUtc)
                .HasConversion(
                    v => v.UtcDateTime,
                    v => TimeZoneInfo.ConvertTimeFromUtc(v, polandTimeZone)
                )
                .HasColumnType("datetime(6)");
            eb.Property(eb => eb.ModifiedOnUtc)
                .HasConversion(
                    v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                    v => v.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(v.Value, polandTimeZone) : (DateTimeOffset?)null
                )
                .HasColumnType("datetime(6)");

            eb.HasMany(o => o.Seats)
                .WithMany();

            eb.HasOne(m => m.Screening)
                .WithMany()
                .HasForeignKey(c => c.ScreeningId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<User>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.Property(eb => eb.Id).HasColumnType("char(36)");
            eb.Property(eb => eb.IsAdmin).HasColumnType("tinyint(1)");
            eb.Property(eb => eb.Email).HasColumnType("varchar(100)");
            eb.Property(eb => eb.FirstName).HasColumnType("varchar(100)");
            eb.Property(eb => eb.LastName).HasColumnType("varchar(100)");
            eb.Property(eb => eb.Salt).HasColumnType("varchar(100)");
            eb.Property(eb => eb.SaltedHashedPassword).HasColumnType("varchar(100)");

            eb.Property(eb => eb.CreatedOnUtc)
                .HasConversion(
                    v => v.UtcDateTime,
                    v => TimeZoneInfo.ConvertTimeFromUtc(v, polandTimeZone)
                )
                .HasColumnType("datetime(6)");
            eb.Property(eb => eb.ModifiedOnUtc)
                .HasConversion(
                    v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                    v => v.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(v.Value, polandTimeZone) : (DateTimeOffset?)null
                )
                .HasColumnType("datetime(6)");
        });
    }

    public void UpdateDatabase()
    {
        if (Database.CanConnect() && Database.IsRelational())
        {
            var pendingMigrations = Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                Database.Migrate();
            }
        }

        SaveChanges();
    }
}
