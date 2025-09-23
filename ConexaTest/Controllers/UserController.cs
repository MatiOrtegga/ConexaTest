using ConexaTest.Application.Commands.Users;
using ConexaTest.Application.Queries.Users;
using ConexaTest.Application.Utils;
using ConexaTest.Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConexaTest.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator,JwtUtils jwtUtils) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly JwtUtils _manager = jwtUtils;

        [HttpPost("register")] 
        public async Task<IResult> Register([FromBody] AddUserDto dto)
        {
            var response = await _mediator.Send(new AddUserCommand() { 
            Email = dto.Email,
            Name = dto.Name,
            Password = dto.Password,
            RoleId = dto.RoleId
            });

            if (response.IsError)
            {
                return Results.BadRequest(response.Errors);
            }

            return Results.Ok("User Added succesfully.");
        }

        [HttpPost("Login")]
        public async Task<IResult> Login([FromBody] ValidateUserQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsError)
            {
                return Results.BadRequest(result.FirstError);
            }

            var token = _manager.GenerateToken(result.Value);

            return Results.Ok(token);
        }
    }
}
