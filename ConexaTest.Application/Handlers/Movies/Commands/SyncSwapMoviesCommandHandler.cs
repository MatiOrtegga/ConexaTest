using ConexaTest.Application.Builders;
using ConexaTest.Application.Commands.Movies;
using ConexaTest.Application.Services;
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

            foreach (var swapiMovie in swapiMovies)
            {
                var exists = await _dbContext.Movies
                    .AnyAsync(m => m.ExternalId == swapiMovie.Id && m.Source == "SWAPI", cancellationToken: cancellationToken);

                if (!exists)
                {
                    var movie = new MovieBuilder(swapiMovie.Properties.Title, swapiMovie.Properties.Director, swapiMovie.Properties.Producer)
                        .SetExternalId(swapiMovie.Id)
                        .SetSource("SWAPI")
                        .SetReleaseDate(DateTime.Parse(swapiMovie.Properties.ReleaseDate))
                        .SetDescription(swapiMovie.Description)
                        .SetEpisodeId(swapiMovie.Properties.EpisodeId)
                        .Build();

                    _dbContext.Movies.Add(movie);
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
