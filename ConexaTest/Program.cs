using ConexaTest.Application.Commands.Users;
using ConexaTest.Application.Handlers.Movies.Commands;
using ConexaTest.Application.Services;
using ConexaTest.Application.Utils;
using ConexaTest.Application.Validators.Movies;
using ConexaTest.Domain.Dto;
using ConexaTest.Domain.Models;
using ConexaTest.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<SwapiServices>();
builder.Services.AddScoped<JwtUtils>();

builder.Services.AddValidatorsFromAssemblyContaining<AddMovieDtoValidator>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<AddMovieCommandHandler>();
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<Movie, AddMovieDto>().ReverseMap();
    cfg.CreateMap<Movie, UpdateMovieDto>().ReverseMap();
    cfg.CreateMap<AddUserCommand, User>().ReverseMap();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ConexaDb")
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
