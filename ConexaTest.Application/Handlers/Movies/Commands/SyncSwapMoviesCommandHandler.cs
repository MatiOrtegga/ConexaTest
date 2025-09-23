using AutoMapper;
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

            if (swapiMovies.IsError)
            {
                return swapiMovies.FirstError;
            }
            foreach (var swapiMovie in swapiMovies.Value)
            {
                var existingMovie = await _dbContext.Movies
                    .FirstOrDefaultAsync(m => m.ExternalId == swapiMovie.Id && m.Source == "SWAPI", cancellationToken);

                if (existingMovie is null)
                {
                    var movie = new Movie
                    {
                        ExternalId = swapiMovie.Id,
                        Title = swapiMovie.Properties.Title,
                        Director = swapiMovie.Properties.Director,
                        Producer = swapiMovie.Properties.Producer,
                        ReleaseDate = DateTime.SpecifyKind(DateTime.Parse(swapiMovie.Properties.ReleaseDate), DateTimeKind.Utc),
                        Description = swapiMovie.Description ?? string.Empty,
                        EpisodeId = swapiMovie.Properties.EpisodeId,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _dbContext.Movies.Add(movie);
                }
                else
                {
                    existingMovie.ExternalId = swapiMovie.Id;
                    existingMovie.Title = swapiMovie.Properties.Title;
                    existingMovie.Director = swapiMovie.Properties.Director;
                    existingMovie.Producer = swapiMovie.Properties.Producer;
                    existingMovie.Source = "SWAPI";
                    existingMovie.ReleaseDate = DateTime.SpecifyKind(DateTime.Parse(swapiMovie.Properties.ReleaseDate), DateTimeKind.Utc);
                    existingMovie.Description = swapiMovie.Description ?? string.Empty;
                    existingMovie.EpisodeId = swapiMovie.Properties.EpisodeId;
                    existingMovie.UpdatedAt = DateTime.UtcNow;
                    _dbContext.Movies.Update(existingMovie);
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
