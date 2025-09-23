using AutoMapper;
using ConexaTest.Application.Commands.Movies;
using ConexaTest.Domain.Errors.Movies;
using ConexaTest.Domain.Models;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Handlers.Movies.Commands
{
    public class AddMovieCommandHandler(AppDbContext dbContext,IMapper mapper) : IRequestHandler<AddMovieCommand, ErrorOr<bool>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper mapper = mapper;
        public async Task<ErrorOr<bool>> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = mapper.Map<Movie>(request);
            
            movie.CreatedAt = DateTime.UtcNow;
            movie.UpdatedAt = DateTime.UtcNow;
          
            var movieWithSameTitle = _dbContext.Movies
                .FirstOrDefault(m => m.Title == request.Title);

            if(movieWithSameTitle is not null)
            {
                return MoviesError.MovieAlreadyExists;
            }

                await _dbContext.Movies.AddAsync(movie,cancellationToken);
            var response = await _dbContext.SaveChangesAsync(cancellationToken);

            if(response == 0)
            {
                return MoviesError.CantAddMovie;
            }

            return true;
        }
    }
}
