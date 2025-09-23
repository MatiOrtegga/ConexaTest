using Azure;
using ConexaTest.Application.Commands.Movies;
using ConexaTest.Application.Commands.Users;
using ConexaTest.Application.Queries.Users;
using ConexaTest.Application.Utils;
using ConexaTest.Domain.Dto;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConexaTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator, JwtUtils jwtUtils) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly JwtUtils _manager = jwtUtils;

        [HttpPost("register")]
        public async Task<IResult> Register([FromBody] AddUserDto dto, [FromServices] IValidator<AddUserDto> validator)
        {
            var response = await _mediator.Send(new AddUserCommand()
            {
                Email = dto.Email,
                Name = dto.Name,
                Password = dto.Password,
                RoleId = dto.RoleId
            });
            var validationResult = await validator.ValidateAsync(dto);

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

            return Results.Ok("User Added succesfully.");
        }

        [HttpPost("Login")]
        public async Task<IResult> Login([FromBody] ValidateUserQuery query, [FromServices] IValidator<ValidateUserQuery> validator)
        {
            var response = await _mediator.Send(query);
            var validationResult = await validator.ValidateAsync(query);

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

            var token = _manager.GenerateToken(response.Value);

            return Results.Ok(token);
        }
    }
}
