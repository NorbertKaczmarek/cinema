using cinema.context.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace cinema.context;

public class Seeder
{
    private readonly CinemaDbContext _context;

    public Seeder(CinemaDbContext context)
    {
        _context = context;
    }

    public void SeedDatabase()
    {
        if (!_context.Database.CanConnect() || !_context.Database.IsRelational()) return;

        if (!_context.Categories.Any())
        {
            _context.Categories.AddRange(GetCategories());
            _context.SaveChanges();
        }

        if (!_context.Movies.Any())
        {
            _context.Movies.AddRange(GetMovies());
            _context.SaveChanges();
        }

        if (!_context.Users.Any())
        {
            _context.Users.AddRange(GetUsers());
            _context.SaveChanges();
        }

        if (!_context.Seats.Any())
        {
            _context.Seats.AddRange(SeedSeats());
            _context.SaveChanges();
        }


        if (!_context.Screenings.Any())
        {
            _context.Screenings.AddRange(GetScreenings(_context.Movies.ToList()));
            _context.SaveChanges();
        }

        if (!_context.Orders.Any())
        {
            _context.Orders.AddRange(GetOrders(_context.Screenings.ToList()));
            _context.SaveChanges();
        }
    }

    private List<Category> GetCategories()
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

    private List<Movie> GetMovies()
    {
        var movies = new List<Movie>
        {
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Joker: Folie à Deux",
                DurationMinutes = 138,
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/rc4oXn6mQ1KSwXJo190bFulvHtB.jpg",
                TrailerUrl = "https://youtu.be/1ZqXN6LELgQ",
                BackgroundUrl = "https://media.themoviedb.org/t/p/w533_and_h300_bestv2/uGmYqxh8flqkudioyFtD7IJSHxK.jpg",
                Director = "Todd Phillips",
                Cast = "Joaquin Phoenix, Lady Gaga, Brendan Gleeson",
                Description = "Arthur Fleck (Joaquin Phoenix) przebywa w więzieniu Arkham, oczekując na proces za swoje zbrodnie, które popełnił jako Joker. Zmagając się ze swoją podwójną tożsamością, odnajduje prawdziwą miłość, a także muzykę, która zawsze w nim tkwiła.",
                Rating = 5.8,
                Category = _context.Categories.FirstOrDefault(x => x.Name == "Dramat")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Dziki robot",
                DurationMinutes = 101,
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/wTnV3PCVW5O92JMrFvvrRcV39RU.jpg",
                TrailerUrl = "https://youtu.be/vVVTWfWBAs8",
                BackgroundUrl = "https://image.tmdb.org/t/p/original/4zlOPT9CrtIX05bBIkYxNZsm5zN.jpg",
                Director = "Chris Sanders",
                Cast = "Lupita Nyong'o, Pedro Pascal, Kit Connor, Bill Nighy, Stephanie Hsu, Matt Berry",
                Description = "ROZZUM jest robotem, jednostką 7134. Rozbija się na niezamieszkanej wyspie i musi nauczyć dostosowywać do surowego otoczenia. Stopniowo zaczyna budować relacje z tamtejszymi zwierzętami i staje się również przybranym rodzicem osieroconej gęsi. Animacja oparta jest na bestsellerowej książce dla dzieci o tym samym tytule i opowiada o odkrywaniu siebie oraz bada, co to znaczy być żywym i połączonym ze wszystkimi istotami.",
                Rating = 8.6,
                Category = _context.Categories.FirstOrDefault(x => x.Name == "Animacja")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Venom 3: Ostatni taniec",
                DurationMinutes = 109,
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/k42Owka8v91trK1qMYwCQCNwJKr.jpg",
                TrailerUrl = "https://youtu.be/GXrD3RU3Zfk",
                BackgroundUrl = "https://media.themoviedb.org/t/p/w533_and_h300_bestv2/3V4kLQg0kSqPLctI5ziYWabAZYF.jpg",
                Director = "Kelly Marcel",
                Cast = "Tom Hardy, Chiwetel Ejiofor, Juno Temple, Clark Backo, Rhys Ifans",
                Description = "Eddie and Venom are on the run. Hunted by both of their worlds and with the net closing in, the duo are forced into a devastating decision that will bring the curtains down on Venom and Eddie's last dance.",
                Rating = 6.6,
                Category = _context.Categories.FirstOrDefault(x => x.Name == "Sci-Fi")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Terrifier 3",
                DurationMinutes = 125,
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/63xYQj1BwRFielxsBDXvHIJyXVm.jpg",
                TrailerUrl = "https://youtu.be/ZeXum1wGiUU",
                BackgroundUrl = "https://media.themoviedb.org/t/p/w533_and_h300_bestv2/18TSJF1WLA4CkymvVUcKDBwUJ9F.jpg",
                Director = "Damien Leonel",
                Cast = "Lauren LaVera, David Howard Thornton, Antonella Rose",
                Description = "Pięć lat po przeżyciu halloweenowej masakry klauna Arta, Sienna i jej brat wciąż walczą o odbudowanie swojego zniszczonego życia. Gdy zbliża się okres świąteczny, starają się przyjąć ducha Bożego Narodzenia i zostawić za sobą okropności z przeszłości. Ale właśnie wtedy, gdy myślą, że są bezpieczni, Art powraca, zdeterminowany, by zmienić ich świąteczną radość w nowy koszmar. Okres świąteczny szybko się rozpada, gdy Art uwalnia swoją pokręconą markę terroru, udowadniając, że żadne święta nie są bezpieczne.",
                Rating = 7.3,
                Category = _context.Categories.FirstOrDefault(x => x.Name == "Horror")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Obcy: Romulus",
                DurationMinutes = 119,
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/6FsZvai0QZDdtOf2ideSRUZZyg.jpg",
                TrailerUrl = "https://youtu.be/-5FlWjXKyYs",
                BackgroundUrl = "https://media.themoviedb.org/t/p/w533_and_h300_bestv2/6vn6K9oX82i6E86ZiHVxqVEMQqP.jpg",
                Director = "Fede Álvarez",
                Cast = "Cailee Spaeny, David Jonsson, Archie Renaux, Isabela Merced",
                Description = "Podczas przeszukiwania głębin opuszczonej stacji kosmicznej grupa młodych kolonizatorów kosmosu staje twarzą w twarz z najbardziej przerażającą formą życia we wszechświecie.",
                Rating = 7.3,
                Category = _context.Categories.FirstOrDefault(x => x.Name == "Sci-Fi")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Transformers: Początek",
                DurationMinutes = 104,
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/4GEtIdL6WzLKwLrj7DY4kHnma2v.jpg",
                TrailerUrl = "https://youtu.be/_NKmP44pVMg",
                BackgroundUrl = "https://media.themoviedb.org/t/p/w533_and_h300_bestv2/zViRwl3ySscZnbXZJ2Q9wq3SeUG.jpg",
                Director = "Josh Cooley",
                Cast = "Chris Hemsworth, Brian Tyree Henry, Scarlett Johansson, Keegan-Michael Key, Jon Hamm",
                Description = "Poznaj historię Optimusa Prime'a i Megatrona, lepiej znanych jako zaprzysięgli wrogowie. Kiedyś byli jednak przyjaciółmi związanymi jak bracia, którzy na zawsze zmienili los Cybertronu.",
                Rating = 8.1,
                Category = _context.Categories.FirstOrDefault(x => x.Name == "Animacja")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Uśmiechnij się 2",
                DurationMinutes = 132,
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/aE85MnPIsSoSs3978Noo16BRsKN.jpg",
                TrailerUrl = "https://youtu.be/hrMdS5ZcGTs",
                BackgroundUrl = "https://media.themoviedb.org/t/p/w500_and_h282_face/3rxoUVI74z7rTWYSAC2q3Uax2nC.jpg",
                Director = "Parker Finn",
                Cast = "Naomi Scott, Rosemarie DeWitt, Dylan Gelula, Lukas Gage, Peter Jacobson",
                Description = "Przed wyruszeniem w nową trasę koncertową, globalna sensacja popu Skye Riley zaczyna doświadczać coraz bardziej przerażających i niewytłumaczalnych wydarzeń. Przytłoczona narastającymi horrorami i presją sławy, Skye jest zmuszona stawić czoła swojej mrocznej przeszłości, aby odzyskać kontrolę nad swoim życiem, zanim wymknie się spod kontroli.",
                Rating = 6.9,
                Category = _context.Categories.FirstOrDefault(x => x.Name == "Horror")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Reagan",
                DurationMinutes = 135,
                PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/o21NB4f5fNk1dtrRlyAmA0C0cb3.jpg",
                TrailerUrl = "https://youtu.be/i_iI30fam6k",
                BackgroundUrl = "https://media.themoviedb.org/t/p/w533_and_h300_bestv2/3G7eBuF7OheKMUP2bMjPvSMDP7o.jpg",
                Director = "Sean McNamara",
                Cast = "Dennis Quaid, Jon Voight, Penelope Ann Miller, Mena Suvari",
                Description = "Historia opowiedziana z perspektywy byłego agenta KGB, którego życie nierozerwalnie łączy się z osobą Ronalda Reagana od czasu, gdy ten po raz pierwszy zwrócił na siebie uwagę Sowietów jako aktor w Hollywood. Reagan pokonuje przeciwności losu i zostaje 40. prezydentem Stanów Zjednoczonych, jednym z największych w historii.",
                Rating = 6.3,
                Category = _context.Categories.FirstOrDefault(x => x.Name == "Dramat")
            }
        };

        return movies;
    }

    private List<User> GetUsers()
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
                Salt = "sD3JrhsaKwbJSEIGtCroxg==",
                SaltedHashedPassword = "KebGc28ucmF35m9Mem88N4EmGn1kI/tkR7edZAxSuIs="
            },
            new User
            {
                Id = Guid.NewGuid(),
                IsAdmin = false,
                Email = "user@cinema.com",
                FirstName = "Regular",
                LastName = "User",
                Salt = "IzOkJvJcKGzVGolBAAIOEw==",
                SaltedHashedPassword = "wrLiIVdbY7IKGSEDUsqkzHd/JsjrGilRwPVSGZomGBs="
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

    private List<Screening> GetScreenings(List<Movie> movies)
    {
        var _random = new Random();
        long startTicks = new DateTimeOffset(2025, 1, 1, 8, 30, 0, TimeSpan.Zero).UtcTicks;
        long endTicks = new DateTimeOffset(2025, 6, 15, 16, 30, 0, TimeSpan.Zero).UtcTicks;
        var screenings = new List<Screening>();
        foreach (var movie in movies)
        {
            var screeningsAmount = _random.Next(10, 30);
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
                    EndDateTime = startTime.AddMinutes(movie.DurationMinutes), 
                    Movie = movie
                });
            }
        }

        return screenings;
    }

    private List<Order> GetOrders(List<Screening> screenings)
    {
        var _random = new Random();
        var rows = _context.Seats.GroupBy(s => s.Row).Select(g => g.Key).ToList();
        var orders = new List<Order>();
        foreach (var screening in screenings)
        {
            foreach (var selectedRow in rows)
            {
                if (_random.Next(1, 11) > 4)
                {
                    var seatCount = _random.Next(1, 6);
                    var rowSeats = _context.Seats.Where(s => s.Row == selectedRow).OrderBy(s => s.Number).ToList();
                    var firstSeatNumber = _random.Next(1, rowSeats.Count - seatCount);
                    List<Seat> selectedSeats = new List<Seat>();
                    for (int j = firstSeatNumber; j <= firstSeatNumber + seatCount; j++)
                    {
                        selectedSeats.Add(_context.Seats.First(x => x.Number == j && x.Row == selectedRow));
                    }

                    var customerId = _random.Next(1, 11);
                    var customerPhoneNumber = _random.Next(111111111, 999999999).ToString();
                    orders.Add(new Order
                    {
                        Id = Guid.NewGuid(),
                        Email = $"customer{customerId}@example.com",
                        PhoneNumber = customerPhoneNumber,
                        Status = (OrderStatus)_random.Next(Enum.GetValues(typeof(OrderStatus)).Length),
                        Screening = screening,
                        Seats = selectedSeats
                    });
                }
            }
        }

        return orders;
    }
}
