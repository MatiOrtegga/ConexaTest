using ConexaTest.Application.Builders;
using ConexaTest.Application.Commands.Movies;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Handlers.Movies.Commands
{
    public class AddMovieCommandHandler(AppDbContext dbContext) : IRequestHandler<AddMovieCommand, ErrorOr<bool>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<ErrorOr<bool>> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = new MovieBuilder(request.Title, request.Director, request.Producer)
                .SetSource("Local")
                .SetReleaseDate(request.ReleaseDate.Value)
                .SetDescription(request.Description ?? "")
                .Build();
                ;

            await _dbContext.Movies.AddAsync(movie,cancellationToken);
            var response = await _dbContext.SaveChangesAsync(cancellationToken);

            if(response == 0)
            {
                return Error.Failure(description: "Failed to add movie");
            }

            return true;
        }
    }
}
