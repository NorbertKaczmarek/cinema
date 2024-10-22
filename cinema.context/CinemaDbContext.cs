using cinema.context.Entities;
using Microsoft.EntityFrameworkCore;

namespace cinema.context;

public class CinemaDbContext(DbContextOptions<CinemaDbContext> options) : DbContext(options)
{
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Screening> Screenings { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderedSeat> OrderedSeats { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.HasOne(m => m.MovieCategory)
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
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

    public void SeedDatabase()
    {
        if (!Database.CanConnect() || !Database.IsRelational()) return;
        var categories = new[]
        {
            new Category { Id = Guid.NewGuid(), Name = "Action" },
            new Category { Id = Guid.NewGuid(), Name = "Comedy" },
            new Category { Id = Guid.NewGuid(), Name = "Drama" }
        };
        if (!Categories.Any())
        {
            Categories.AddRange(categories);
            SaveChanges();
        }

        var movies = new[]
        {
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "The Action Movie",
                Duration = TimeSpan.FromMinutes(120),
                PosterUrl = "http://example.com/poster1.jpg",
                Director = "John Doe",
                Cast = "Actor 1, Actor 2",
                Description = "An action-packed adventure.",
                Rating = 8.5,
                MovieCategory = Categories.FirstOrDefault(x => x.Name == "Action")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "The Comedy Movie",
                Duration = TimeSpan.FromMinutes(90),
                PosterUrl = "http://example.com/poster2.jpg",
                Director = "Jane Smith",
                Cast = "Actor 3, Actor 4",
                Description = "A hilarious comedy.",
                Rating = 7.0,
                MovieCategory = Categories.FirstOrDefault(x => x.Name == "Comedy")
            }
        };
        if (!Movies.Any())
        {
            Movies.AddRange(movies);
            SaveChanges();
        }

        var users = new[]
        {
            new User
            {
                Id = Guid.NewGuid(),
                IsAdmin = true,
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                PasswordHash = "admin-password-hash"
            },
            new User
            {
                Id = Guid.NewGuid(),
                IsAdmin = false,
                Email = "user@example.com",
                FirstName = "Regular",
                LastName = "User",
                PasswordHash = "user-password-hash"
            }
        };
        if (!Users.Any())
        {
            Users.AddRange(users);
            SaveChanges();
        }

        var seats = new List<Seat>();
        for (var row = 'A'; row <= 'E'; row++)
        {
            for (var number = 1; number <= 10; number++)
            {
                seats.Add(new Seat
                {
                    Id = Guid.NewGuid(),
                    Row = row,
                    Number = number
                });
            }
        }
        if (!Seats.Any())
        {
            Seats.AddRange(seats);
            SaveChanges();
        }

        var screenings = new List<Screening>();
        foreach (var movie in movies)
        {
            var startTime = new DateTimeOffset(2024, 7, 1, 13, 30, 0, TimeSpan.Zero);
            screenings.Add(new Screening
            {
                Id = Guid.NewGuid(),
                MovieId = movie.Id,
                StartDateTime = startTime,
                EndDateTime = startTime.Add(movie.Duration)
            });
        }
        if (!Screenings.Any())
        {
            Screenings.AddRange(screenings);
            SaveChanges();
        }

        var orders = new List<Order>();
        foreach (var screening in screenings)
        {
            orders.Add(new Order
            {
                Id = Guid.NewGuid(),
                Email = "customer@example.com",
                PhoneNumber = "123-456-7890",
                Status = OrderStatus.Ready,
                ScreeningId = screening.Id
            });
        }
        if (!Orders.Any())
        {
            Orders.AddRange(orders);
            SaveChanges();
        }

        var orderedSeats = new List<OrderedSeat>();
        foreach (var order in orders)
        {
            orderedSeats.Add(new OrderedSeat
            {
                Id = Guid.NewGuid(),
                SeatId = Seats.FirstOrDefault(x => x.Number == 4 && x.Row == 'C')!.Id,
                OrderId = order.Id
            });
            orderedSeats.Add(new OrderedSeat
            {
                Id = Guid.NewGuid(),
                SeatId = Seats.FirstOrDefault(x => x.Number == 7 && x.Row == 'B')!.Id,
                OrderId = order.Id
            });
        }
        if (!OrderedSeats.Any())
        {
            OrderedSeats.AddRange(orderedSeats);
            SaveChanges();
        }
    }
}
