using ConexaTest.Application.Builders;
using ConexaTest.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ConexaTest.Tests.ApplicationTests
{
    public class SwapiServiceTests
    {
        [Fact]
        public async Task AddMovies_Should_Insert_New_Movies()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = DbContextFactory.CreateInMemoryDbContext(dbName);

            var swapiMovies = new List<SwapiResult>
    {
        new() { Id = "1", Properties = new SwapiProperties { Title = "A New Hope", Director = "Lucas", Producer = "Kurtz", ReleaseDate = "1977-05-25", EpisodeId = 4 }, Description = "First film" },
        new() { Id = "2", Properties = new SwapiProperties { Title = "The Empire Strikes Back", Director = "Kershner", Producer = "Kurtz", ReleaseDate = "1980-05-21", EpisodeId = 5 }, Description = "Second film" }
    };

            foreach (var swapiMovie in swapiMovies)
            {
                var exists = await context.Movies.AnyAsync(m => m.ExternalId == swapiMovie.Id && m.Source == "SWAPI");
                if (!exists)
                {
                    var movie = new MovieBuilder(swapiMovie.Properties.Title, swapiMovie.Properties.Director, swapiMovie.Properties.Producer)
                        .SetExternalId(swapiMovie.Id)
                        .SetSource("SWAPI")
                        .SetReleaseDate(DateTime.SpecifyKind(DateTime.Parse(swapiMovie.Properties.ReleaseDate), DateTimeKind.Utc))
                        .SetDescription(swapiMovie.Description)
                        .SetEpisodeId(swapiMovie.Properties.EpisodeId)
                        .Build();
                    context.Movies.Add(movie);
                }
            }

            await context.SaveChangesAsync();
            var movies = await context.Movies.ToListAsync();

            Assert.Equal(2, movies.Count);
        }
     
    }
}
