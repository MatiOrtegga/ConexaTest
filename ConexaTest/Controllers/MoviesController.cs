using Azure;
using ConexaTest.Application.Commands.Movies;
using ConexaTest.Application.Queries.Movies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ConexaTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;        
        [AllowAnonymous]
        [HttpGet] 
        public async Task<IResult> GetMoviesAsync()
        {
            var query = await _mediator.Send(new GetMoviesQuery());

            if(query.IsError)
            {
                return Results.BadRequest(query.FirstError);
            }

            if (!query.Value.Any())
            {
                return Results.NoContent();
            }

            return Results.Ok(query.Value);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<IResult> GetMovieByIdAsync(int id) 
        {
            var query = await _mediator.Send(new GetMovieByIdQuery
            {
                Id = id
            });

            if (query.IsError)
            {
                return Results.BadRequest(query.FirstError);
            }

            return Results.Ok(query.Value);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost] 
        public async Task<IResult> AddMovieAsync([FromBody] AddMovieCommand command)
        {
            var response = await _mediator.Send(command);
            if(response.IsError)
            {
                return Results.BadRequest(response.FirstError);
            }

            return Results.Ok("Movie added succesfully.");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("sync-swapi")] 
        public async Task<IResult> SyncSwapiMoviesAsync()
        {
            var response = await _mediator.Send(new SyncSwapMoviesCommand());
            if (response.IsError)
            {
                return Results.BadRequest(response.FirstError);
            }

            return Results.Ok("Movies Sincronized succesfully.");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IResult> UpdateMovieAsync([FromBody] UpdateMovieCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsError)
            {
                return Results.BadRequest(response.FirstError);
            }
            return Results.Ok("Movie updated succesfully.");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IResult> DeleteMovieAsync(int id)
        {
            var response = await _mediator.Send(new DeleteMovieCommand
            {
                Id = id
            });
            if (response.IsError)
            {
                return Results.BadRequest(response.FirstError);
            }
            return Results.Ok("Movie deleted succesfully.");
        }
    }
}
