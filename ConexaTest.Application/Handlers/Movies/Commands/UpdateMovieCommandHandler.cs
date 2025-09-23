using ConexaTest.Application.Builders;
using ConexaTest.Application.Commands.Movies;
using ConexaTest.Domain.Errors.Movies;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConexaTest.Application.Handlers.Movies.Commands
{
    public class UpdateMovieCommandHandler(AppDbContext dbContext) : IRequestHandler<UpdateMovieCommand, ErrorOr<bool>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<ErrorOr<bool>> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var movieInDb = _dbContext.Movies
                .AsNoTracking()
                .FirstOrDefault(m => m.Id == request.Id);

            if(movieInDb is null)
            {
                return MoviesError.NoMovieFound;
            }

            var movieToUpdate = new MovieBuilder(request.Title, request.Director, request.Producer)
                .SetId(request.Id)
                .SetSource("Local")
                .SetReleaseDate(request.ReleaseDate.Value)
                .SetDescription(request.Description ?? "")
                .Build();            

            _dbContext.Movies.Update(movieToUpdate);
            var response = await _dbContext.SaveChangesAsync(cancellationToken);

            if (response == 0)
            {
                return MoviesError.CantUpdateMovie;
            }

            return true;
        }
    }
}
