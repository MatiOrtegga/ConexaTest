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
    }
}
