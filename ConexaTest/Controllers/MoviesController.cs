using ConexaTest.Application.Commands.Movies;
using ConexaTest.Application.Queries.Movies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace ConexaTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet] 
        public async Task<IResult> GetMoviesAsync()
        {
            var query = await _mediator.Send(new GetMoviesQuery());

            if(query.IsError)
            {
                return Results.Problem(query.FirstError.Description);
            }

            if(!query.Value.Any())
            {
                return Results.NoContent();
            }

            return Results.Ok(query.Value);
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetMovieByIdAsync(int id) 
        {
            var query = await _mediator.Send(new GetMovieByIdQuery
            {
                Id = id
            });

            if (query.IsError)
            {
                return Results.Problem(query.FirstError.Description);
            }

            return Results.Ok(query.Value);
        }

        [HttpPost] 
        public async Task<IResult> AddMovieAsync([FromBody] AddMovieCommand command)
        {
            var response = await _mediator.Send(command);
            if(response.IsError)
            {
                return Results.Problem(response.FirstError.Description);
            }

            return Results.Ok("Movie added succesfully.");
        }

        [HttpPost("sync-swapi")] 
        public async Task<IResult> SyncSwapiMoviesAsync()
        {
            var response = await _mediator.Send(new SyncSwapMoviesCommand());
            if (response.IsError)
            {
                return Results.Problem(response.FirstError.Description);
            }

            return Results.Ok("Movies Sincronized succesfully.");
        }

        [HttpPut]
        public async Task<IResult> UpdateMovieAsync([FromBody] UpdateMovieCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsError)
            {
                return Results.Problem(response.FirstError.Description);
            }
            return Results.Ok("Movie updated succesfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteMovieAsync(int id)
        {
            var response = await _mediator.Send(new DeleteMovieCommand
            {
                Id = id
            });
            if (response.IsError)
            {
                return Results.Problem(response.FirstError.Description);
            }
            return Results.Ok("Movie deleted succesfully.");
        }
    }
}
