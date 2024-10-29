using cinema.context.Entities;
using Microsoft.EntityFrameworkCore;
using System;

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
        modelBuilder.Entity<Category>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.Property(eb => eb.Id).HasColumnType("char(36)");
            eb.Property(eb => eb.Name).HasColumnType("varchar(100)");
        });

        modelBuilder.Entity<Movie>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.Property(eb => eb.Id).HasColumnType("char(36)");
            eb.Property(eb => eb.Title).HasColumnType("varchar(100)");
            eb.Property(eb => eb.Duration).HasColumnType("time(6)");
            eb.Property(eb => eb.PosterUrl).HasColumnType("varchar(100)");
            eb.Property(eb => eb.Director).HasColumnType("varchar(100)");
            eb.Property(eb => eb.Cast).HasColumnType("varchar(100)");
            eb.Property(eb => eb.Description).HasColumnType("varchar(1000)");
            eb.Property(eb => eb.Rating).HasColumnType("double");
            eb.Property(eb => eb.CategoryId).HasColumnType("char(36)");

            eb.HasOne(m => m.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Screening>(eb =>
        {
            eb.HasKey(eb => eb.Id);
            eb.Property(eb => eb.Id).HasColumnType("char(36)");
            eb.Property(eb => eb.StartDateTime).HasColumnType("datetime(6)");
            eb.Property(eb => eb.EndDateTime).HasColumnType("datetime(6)");
            eb.Property(eb => eb.MovieId).HasColumnType("char(36)");

            eb.HasOne(m => m.Movie)
                .WithMany()
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
            eb.Property(eb => eb.Status).HasColumnType("varchar(100)");
            eb.Property(eb => eb.ScreeningId).HasColumnType("char(36)");

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
            eb.Property(eb => eb.PasswordHash).HasColumnType("varchar(100)");
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

        if (!Categories.Any())
        {
            Categories.AddRange(SeedCategories());
            SaveChanges();
        }
        
        if (!Movies.Any())
        {
            Movies.AddRange(SeedMovies());
            SaveChanges();
        }

        if (!Users.Any())
        {
            Users.AddRange(SeedUsers());
            SaveChanges();
        }

        if (!Seats.Any())
        {
            Seats.AddRange(SeedSeats());
            SaveChanges();
        }


        if (!Screenings.Any())
        {
            Screenings.AddRange(SeedScreenings(Movies.ToList()));
            SaveChanges();
        }

        if (!Orders.Any())
        {
            Orders.AddRange(SeedOrders(Screenings.ToList()));
            SaveChanges();
        }
    }

    private List<Category> SeedCategories()
    {
        var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Dramat" },
            new Category { Id = Guid.NewGuid(), Name = "Animacja" },
            new Category { Id = Guid.NewGuid(), Name = "Sci-Fi" },
            new Category { Id = Guid.NewGuid(), Name = "Horror" },
        };

        return categories;
    }

    private List<Movie> SeedMovies()
    {
        var movies = new List<Movie>
        {
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Joker: Folie à Deux",
                Duration = TimeSpan.FromMinutes(138),
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/rc4oXn6mQ1KSwXJo190bFulvHtB.jpg",
                Director = "Todd Phillips",
                Cast = "Joaquin Phoenix, Lady Gaga, Brendan Gleeson",
                Description = "Arthur Fleck (Joaquin Phoenix) przebywa w więzieniu Arkham, oczekując na proces za swoje zbrodnie, które popełnił jako Joker. Zmagając się ze swoją podwójną tożsamością, odnajduje prawdziwą miłość, a także muzykę, która zawsze w nim tkwiła.",
                Rating = 5.8,
                Category = Categories.FirstOrDefault(x => x.Name == "Dramat")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Dziki robot",
                Duration = TimeSpan.FromMinutes(101),
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/wTnV3PCVW5O92JMrFvvrRcV39RU.jpg",
                Director = "Chris Sanders",
                Cast = "Lupita Nyong'o, Pedro Pascal, Kit Connor, Bill Nighy, Stephanie Hsu, Matt Berry",
                Description = "ROZZUM jest robotem, jednostką 7134. Rozbija się na niezamieszkanej wyspie i musi nauczyć dostosowywać do surowego otoczenia. Stopniowo zaczyna budować relacje z tamtejszymi zwierzętami i staje się również przybranym rodzicem osieroconej gęsi. Animacja oparta jest na bestsellerowej książce dla dzieci o tym samym tytule i opowiada o odkrywaniu siebie oraz bada, co to znaczy być żywym i połączonym ze wszystkimi istotami.",
                Rating = 8.6,
                Category = Categories.FirstOrDefault(x => x.Name == "Animacja")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Venom 3: Ostatni taniec",
                Duration = TimeSpan.FromMinutes(109),
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/k42Owka8v91trK1qMYwCQCNwJKr.jpg",
                Director = "Kelly Marcel",
                Cast = "Tom Hardy, Chiwetel Ejiofor, Juno Temple, Clark Backo, Rhys Ifans",
                Description = "Eddie and Venom are on the run. Hunted by both of their worlds and with the net closing in, the duo are forced into a devastating decision that will bring the curtains down on Venom and Eddie's last dance.",
                Rating = 6.6,
                Category = Categories.FirstOrDefault(x => x.Name == "Sci-Fi")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Terrifier 3",
                Duration = TimeSpan.FromMinutes(125),
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/63xYQj1BwRFielxsBDXvHIJyXVm.jpg",
                Director = "Damien Leonel",
                Cast = "Lauren LaVera, David Howard Thornton, Antonella Rose",
                Description = "Pięć lat po przeżyciu halloweenowej masakry klauna Arta, Sienna i jej brat wciąż walczą o odbudowanie swojego zniszczonego życia. Gdy zbliża się okres świąteczny, starają się przyjąć ducha Bożego Narodzenia i zostawić za sobą okropności z przeszłości. Ale właśnie wtedy, gdy myślą, że są bezpieczni, Art powraca, zdeterminowany, by zmienić ich świąteczną radość w nowy koszmar. Okres świąteczny szybko się rozpada, gdy Art uwalnia swoją pokręconą markę terroru, udowadniając, że żadne święta nie są bezpieczne.",
                Rating = 7.3,
                Category = Categories.FirstOrDefault(x => x.Name == "Horror")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Obcy: Romulus",
                Duration = TimeSpan.FromMinutes(119),
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/6FsZvai0QZDdtOf2ideSRUZZyg.jpg",
                Director = "Fede Álvarez",
                Cast = "Cailee Spaeny, David Jonsson, Archie Renaux, Isabela Merced",
                Description = "Podczas przeszukiwania głębin opuszczonej stacji kosmicznej grupa młodych kolonizatorów kosmosu staje twarzą w twarz z najbardziej przerażającą formą życia we wszechświecie.",
                Rating = 7.3,
                Category = Categories.FirstOrDefault(x => x.Name == "Sci-Fi")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Transformers: Początek",
                Duration = TimeSpan.FromMinutes(104),
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/4GEtIdL6WzLKwLrj7DY4kHnma2v.jpg",
                Director = "Josh Cooley",
                Cast = "Chris Hemsworth, Brian Tyree Henry, Scarlett Johansson, Keegan-Michael Key, Jon Hamm",
                Description = "Poznaj historię Optimusa Prime'a i Megatrona, lepiej znanych jako zaprzysięgli wrogowie. Kiedyś byli jednak przyjaciółmi związanymi jak bracia, którzy na zawsze zmienili los Cybertronu.",
                Rating = 8.1,
                Category = Categories.FirstOrDefault(x => x.Name == "Animacja")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Uśmiechnij się 2",
                Duration = TimeSpan.FromMinutes(132),
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/aE85MnPIsSoSs3978Noo16BRsKN.jpg",
                Director = "Parker Finn",
                Cast = "Naomi Scott, Rosemarie DeWitt, Dylan Gelula, Lukas Gage, Peter Jacobson",
                Description = "Przed wyruszeniem w nową trasę koncertową, globalna sensacja popu Skye Riley zaczyna doświadczać coraz bardziej przerażających i niewytłumaczalnych wydarzeń. Przytłoczona narastającymi horrorami i presją sławy, Skye jest zmuszona stawić czoła swojej mrocznej przeszłości, aby odzyskać kontrolę nad swoim życiem, zanim wymknie się spod kontroli.",
                Rating = 6.9,
                Category = Categories.FirstOrDefault(x => x.Name == "Horror")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Reagan",
                Duration = TimeSpan.FromMinutes(135),
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/o21NB4f5fNk1dtrRlyAmA0C0cb3.jpg",
                Director = "Sean McNamara",
                Cast = "Dennis Quaid, Jon Voight, Penelope Ann Miller, Mena Suvari",
                Description = "Historia opowiedziana z perspektywy byłego agenta KGB, którego życie nierozerwalnie łączy się z osobą Ronalda Reagana od czasu, gdy ten po raz pierwszy zwrócił na siebie uwagę Sowietów jako aktor w Hollywood. Reagan pokonuje przeciwności losu i zostaje 40. prezydentem Stanów Zjednoczonych, jednym z największych w historii.",
                Rating = 6.3,
                Category = Categories.FirstOrDefault(x => x.Name == "Dramat")
            }
        };

        return movies;
    }

    private List<User> SeedUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                IsAdmin = true,
                Email = "admin@cinema.com",
                FirstName = "Admin",
                LastName = "User",
                PasswordHash = "AQAAAAIAAYagAAAAED4cFxkJ39lBJJcndsDW7zdH6C3qd6BJ7oZiyg5U9Z5oBEiEPy4hFeLbSVZzS7rG6A=="
            },
            new User
            {
                Id = Guid.NewGuid(),
                IsAdmin = false,
                Email = "user@cinema.com",
                FirstName = "Regular",
                LastName = "User",
                PasswordHash = "AQAAAAIAAYagAAAAELOJS+TqdtQUup3mVJMOqOWGNrYvwn48x4U6G7AB7ocRDWlXagMLK5gLM2BFi0G39g=="
            }
        };

        return users;
    }

    private List<Seat> SeedSeats()
    {
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

        return seats;
    }

    private List<Screening> SeedScreenings(List<Movie> movies)
    {
        var _random = new Random();
        long startTicks = new DateTimeOffset(2024, 6, 10, 8, 30, 0, TimeSpan.Zero).UtcTicks;
        long endTicks = new DateTimeOffset(2024, 12, 15, 16, 30, 0, TimeSpan.Zero).UtcTicks;
        var screenings = new List<Screening>();
        foreach (var movie in movies)
        {
            var screeningsAmount = _random.Next(4, 15);
            for (var i = 0; i < screeningsAmount; i++)
            {
                long randomTicks = (long)(_random.NextDouble() * (endTicks - startTicks)) + startTicks;

                var randomDate = new DateTimeOffset(randomTicks, TimeSpan.Zero);
                var randomHour = _random.Next(8, 20);
                var randomMinute = _random.Next(0, 59);

                var startTime = new DateTimeOffset(
                    randomDate.Year, randomDate.Month, randomDate.Day, randomHour, randomMinute, 0, TimeSpan.Zero);

                screenings.Add(new Screening
                {
                    Id = Guid.NewGuid(),
                    StartDateTime = startTime,
                    EndDateTime = startTime.Add(movie.Duration),
                    Movie = movie
                });
            }
        }

        return screenings;
    }

    private List<Order> SeedOrders(List<Screening> screenings)
    {
        var _random = new Random();
        var rows = Seats.GroupBy(s => s.Row).Select(g => g.Key).ToList();
        var orders = new List<Order>();
        foreach (var screening in screenings)
        {
            foreach (var selectedRow in rows)
            {
                if (_random.Next(1, 11) > 4)
                {
                    var seatCount = _random.Next(1, 6);
                    var rowSeats = Seats.Where(s => s.Row == selectedRow).OrderBy(s => s.Number).ToList();
                    var firstSeatNumber = _random.Next(1, rowSeats.Count - seatCount);
                    List<Seat> selectedSeats = new List<Seat>();
                    for (int j = firstSeatNumber; j <= firstSeatNumber + seatCount; j++)
                    {
                        selectedSeats.Add(Seats.First(x => x.Number == j && x.Row == selectedRow));
                    }

                    orders.Add(new Order
                    {
                        Id = Guid.NewGuid(),
                        Email = "customer@example.com",
                        PhoneNumber = "123-456-7890",
                        Status = OrderStatus.Ready,
                        Screening = screening,
                        Seats = selectedSeats
                    });
                }
            }
        }

        return orders;
    }
}
