using ConexaTest.Application.Builders;
using ConexaTest.Application.Commands.Movies;
using ConexaTest.Application.Services;
using ConexaTest.Domain.Models;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConexaTest.Application.Handlers.Movies.Commands
{
    public class SyncSwapMoviesCommandHandler(AppDbContext dbContext, SwapiServices swapiServices) : IRequestHandler<SyncSwapMoviesCommand, ErrorOr<bool>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly SwapiServices _swapiServices = swapiServices;
        public async Task<ErrorOr<bool>> Handle(SyncSwapMoviesCommand request, CancellationToken cancellationToken)
        {
            var swapiMovies = await _swapiServices.GetStarWarsMoviesAsync();

            if(swapiMovies.IsError)
            {
                return swapiMovies.FirstError;
            }
            foreach (var swapiMovie in swapiMovies.Value)
            {
                var movieExists = await _dbContext.Movies
                    .FirstOrDefaultAsync(m => m.ExternalId == swapiMovie.Id && m.Source == "SWAPI", cancellationToken);

                if (movieExists is null)
                {
                    var movie = new MovieBuilder(swapiMovie.Properties.Title, swapiMovie.Properties.Director, swapiMovie.Properties.Producer)
                        .SetExternalId(swapiMovie.Id)
                        .SetSource("SWAPI")
                        .SetReleaseDate(DateTime.SpecifyKind(DateTime.Parse(swapiMovie.Properties.ReleaseDate), DateTimeKind.Utc))
                        .SetDescription(swapiMovie.Description ?? string.Empty)
                        .SetEpisodeId(swapiMovie.Properties.EpisodeId)
                        .Build();
                    _dbContext.Movies.Add(movie);
                }
                else
                {
                    movieExists.Title = swapiMovie.Properties.Title;
                    movieExists.Director = swapiMovie.Properties.Director;
                    movieExists.Producer = swapiMovie.Properties.Producer;
                    movieExists.ReleaseDate = DateTime.SpecifyKind(DateTime.Parse(swapiMovie.Properties.ReleaseDate), DateTimeKind.Utc);
                    movieExists.Description = swapiMovie.Description ?? string.Empty;
                    movieExists.EpisodeId = swapiMovie.Properties.EpisodeId;
                    movieExists.UpdatedAt = DateTime.UtcNow;
                    _dbContext.Movies.Update(movieExists);
                }
            }
            var response = await _dbContext.SaveChangesAsync(cancellationToken);
            if (response == 0)
            {
                return Error.Failure(description: "Failed to sync SWAPI movies.");
            }
            return true;
        }
    }
}
