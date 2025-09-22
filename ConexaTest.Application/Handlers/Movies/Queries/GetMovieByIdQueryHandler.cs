using ConexaTest.Application.Queries.Movies;
using ConexaTest.Domain.Errors.Movies;
using ConexaTest.Domain.Models;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConexaTest.Application.Handlers.Movies.Queries
{
    public class GetMovieByIdQueryHandler(AppDbContext dbContext) : IRequestHandler<GetMovieByIdQuery, ErrorOr<Movie>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<ErrorOr<Movie>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _dbContext.Movies
                .Where(m => m.Id == request.Id)                
                .FirstOrDefaultAsync(cancellationToken);
            
            if(movie is null)
            {
                return MoviesError.NoMovieFound;
            }
            return movie;
        }
    }
}
