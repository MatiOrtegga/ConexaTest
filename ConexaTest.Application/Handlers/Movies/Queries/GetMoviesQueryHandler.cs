using ConexaTest.Application.Queries.Movies;
using ConexaTest.Domain.Models;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConexaTest.Application.Handlers.Movies.Queries
{
    public class GetMoviesQueryHandler(AppDbContext dbContext) : IRequestHandler<GetMoviesQuery, ErrorOr<IEnumerable<Movie>>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<ErrorOr<IEnumerable<Movie>>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _dbContext.Movies.ToListAsync(cancellationToken);
            return movies;
        }
    }
}
