using ConexaTest.Application.Commands.Movies;
using ConexaTest.Application.Queries.Movies;
using FluentValidation;
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

            if (query.IsError)
            {
                return Results.BadRequest(new
                {
                    Errors = query.Errors
                        .Select(e => new
                        {
                            e.Code,
                            e.Description
                        })
                });
            }
            if (!query.Value.Any())
            {
                return Results.NoContent();
            }

            return Results.Ok(query.Value);
        }
        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IResult> GetMovieByIdAsync(int id, [FromServices] IValidator<GetMovieByIdQuery> validator)
        {
            var query = await _mediator.Send(new GetMovieByIdQuery
            {
                Id = id
            });
            var validationResult = await validator.ValidateAsync(new GetMovieByIdQuery
            {
                Id = id
            });

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new
                {
                    Errors = validationResult.Errors
                        .Select(e => new
                        {
                            e.PropertyName,
                            e.ErrorMessage
                        })
                });
            }
            if (query.IsError)
            {
                return Results.BadRequest(new
                {
                    Errors = query.Errors
                        .Select(e => new
                        {
                            e.Code,
                            e.Description
                        })
                });
            }

            return Results.Ok(query.Value);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IResult> AddMovieAsync([FromBody] AddMovieCommand command, [FromServices] IValidator<AddMovieCommand> validator)
        {
            var response = await _mediator.Send(command);
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new
                {
                    Errors = validationResult.Errors
                        .Select(e => new
                        {
                            e.PropertyName,
                            e.ErrorMessage
                        })
                });
            }
            if (response.IsError)
            {
                return Results.BadRequest(new
                {
                    Errors = response.Errors
                        .Select(e => new
                        {
                            e.Code,
                            e.Description
                        })
                });
            }

            return Results.NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IResult> UpdateMovieAsync([FromBody] UpdateMovieCommand command, [FromServices] IValidator<UpdateMovieCommand> validator)
        {
            var response = await _mediator.Send(command);
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new
                {
                    Errors = validationResult.Errors
                        .Select(e => new
                        {
                            e.PropertyName,
                            e.ErrorMessage
                        })
                });
            }
            if (response.IsError)
            {
                return Results.BadRequest(new
                {
                    Errors = response.Errors
                        .Select(e => new
                        {
                            e.Code,
                            e.Description
                        })
                });
            }


            return Results.NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("sync-swapi")]
        public async Task<IResult> SyncSwapiMoviesAsync()
        {
            var response = await _mediator.Send(new SyncSwapMoviesCommand());
            if (response.IsError)
            {
                return Results.BadRequest(new
                {
                    Errors = response.Errors
                        .Select(e => new
                        {
                            e.Code,
                            e.Description
                        })
                });
            }


            return Results.NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IResult> DeleteMovieAsync(int id, [FromServices] IValidator<DeleteMovieCommand> validator)
        {
            var response = await _mediator.Send(new DeleteMovieCommand
            {
                Id = id
            });
            var validationResult = await validator.ValidateAsync(new DeleteMovieCommand
            {
                Id = id
            });
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new
                {
                    Errors = validationResult.Errors
                        .Select(e => new
                        {
                            e.PropertyName,
                            e.ErrorMessage
                        })
                });
            }
            if (response.IsError)
            {
                return Results.BadRequest(new
                {
                    Errors = response.Errors
                        .Select(e => new
                        {
                            e.Code,
                            e.Description
                        })
                });
            }
            return Results.NoContent();
        }
    }
}
