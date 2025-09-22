using ConexaTest.Application.Commands.Movies;
using ConexaTest.Domain.Errors.Movies;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Handlers.Movies.Commands
{
    public class DeleteMovieCommandHandler(AppDbContext dbContext) : IRequestHandler<DeleteMovieCommand, ErrorOr<bool>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<ErrorOr<bool>> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _dbContext.Movies.FindAsync(request.Id, cancellationToken);

            if (movie is null)
            {
                return MoviesError.NoMovieFound;
            }

            _dbContext.Movies.Remove(movie);
            var resonse = await _dbContext.SaveChangesAsync(cancellationToken);

            if (resonse == 0)
            {
                return MoviesError.CantDeleteMovie;
            }
            return true;
        }
    }
}
