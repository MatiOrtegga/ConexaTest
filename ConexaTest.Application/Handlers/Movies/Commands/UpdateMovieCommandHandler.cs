using AutoMapper;
using ConexaTest.Application.Commands.Movies;
using ConexaTest.Domain.Errors.Movies;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Handlers.Movies.Commands
{
    public class UpdateMovieCommandHandler(AppDbContext dbContext, IMapper mapper) : IRequestHandler<UpdateMovieCommand, ErrorOr<bool>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper mapper = mapper;
        public async Task<ErrorOr<bool>> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var movieInDb = _dbContext.Movies
                .FirstOrDefault(m => m.Id == request.Id);

            if(movieInDb is null)
            {
                return MoviesError.NoMovieFound;
            }

            mapper.Map(request, movieInDb);
            movieInDb.UpdatedAt = DateTime.UtcNow;

            _dbContext.Movies.Update(movieInDb);
            var response = await _dbContext.SaveChangesAsync(cancellationToken);

            if (response == 0)
            {
                return MoviesError.CantUpdateMovie;
            }

            return true;
        }
    }
}
