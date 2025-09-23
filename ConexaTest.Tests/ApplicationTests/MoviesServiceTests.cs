using AutoMapper;
using ConexaTest.Application.Commands.Movies;
using ConexaTest.Application.Commands.Users;
using ConexaTest.Application.Handlers.Movies.Commands;
using ConexaTest.Application.Handlers.Movies.Queries;
using ConexaTest.Application.Queries.Movies;
using ConexaTest.Domain.Dto;
using ConexaTest.Domain.Errors.Movies;
using ConexaTest.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ConexaTest.Tests.ApplicationTests
{
    public class MoviesServiceTests
    {
        private IMapper _mapper;
        private MapperConfiguration _config;

        public MoviesServiceTests()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Movie, AddMovieDto>().ReverseMap();
                cfg.CreateMap<Movie, UpdateMovieDto>().ReverseMap();
                cfg.CreateMap<AddUserCommand, User>().ReverseMap();
            }, new NullLoggerFactory());
            _mapper = _config.CreateMapper();
        }
        [Fact]
        public async Task GetAllMovies_WhenMovieExists_ReturnAllMovies()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new GetMoviesQueryHandler(fakeDbContext);

            fakeDbContext.Movies.AddRange(
                new Movie { Title = "Inception", Director = "Christopher Nolan", Producer = "Emma Thomas" },
                new Movie { Title = "The Matrix", Director = "Wachowskis", Producer = "Joel Silver" }
            );
            await fakeDbContext.SaveChangesAsync();

            var query = new GetMoviesQuery();
            var result = await handler.Handle(query, CancellationToken.None);
            var movies = result.Value.ToList();

            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
            Assert.Equal(2, movies.Count);
            Assert.Contains(movies, m => m.Title == "Inception");
            Assert.Contains(movies, m => m.Title == "The Matrix");
        }
        [Fact]
        public async Task GetAllMovies_WhenDontHaveMovies_ReturnNothing()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new GetMoviesQueryHandler(fakeDbContext);

            var query = new GetMoviesQuery();
            var result = await handler.Handle(query, CancellationToken.None);
            var movies = result.Value.ToList();

            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
            Assert.Empty(movies);
        }
        [Fact]
        public async Task GetById_WhenMovieExists_ReturnsMovie()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new GetMovieByIdQueryHandler(fakeDbContext);

            var movie = new Movie
            {
                Title = "Inception",
                Director = "Christopher Nolan",
                Producer = "Emma Thomas",
            };
            fakeDbContext.Movies.Add(movie);
            await fakeDbContext.SaveChangesAsync();

            var query = new GetMovieByIdQuery()
            {
                Id = movie.Id
            };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
            Assert.Equal(movie.Id, result.Value.Id);
            Assert.Equal("Inception", result.Value.Title);
            Assert.Equal("Christopher Nolan", result.Value.Director);
            Assert.Equal("Emma Thomas", result.Value.Producer);
        }
        [Fact]
        public async Task GetById_WhenMovieDoesNotExist_ReturnsError()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new GetMovieByIdQueryHandler(fakeDbContext);
            var query = new GetMovieByIdQuery()
            {
                Id = 123123123 // Non-existent ID
            };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(MoviesError.NoMovieFound, result.FirstError);
        }
        [Fact]
        public async Task AddMovie_WhenMovieTitleAlreadyExits_ReturnsError()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new AddMovieCommandHandler(fakeDbContext, _mapper);

            var movie = new Movie
            {
                Title = "The Empire Strikes Back",
                Director = "Irvin Kershner",
                Producer = "Gary Kurtz",
                ReleaseDate = new DateTime(1980, 5, 21),
                Description = "Second film in the Star Wars saga.",
                EpisodeId = 5
            };
            fakeDbContext.Movies.Add(movie);
            await fakeDbContext.SaveChangesAsync();

            var command = new AddMovieCommand
            {
                ExternalId = Guid.NewGuid().ToString(),
                Source = "UnitTest",
                Title = "The Empire Strikes Back",
                Director = "Irvin Kershner",
                Producer = "Gary Kurtz",
                ReleaseDate = new DateTime(1980, 5, 21),
                Description = "Second film in the Star Wars saga.",
                EpisodeId = 5
            };
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(MoviesError.MovieAlreadyExists, result.FirstError);
        }
        [Fact]
        public async Task AddMovie_NewMovie_ReturnsTrue()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new AddMovieCommandHandler(fakeDbContext, _mapper);

            var command = new AddMovieCommand
            {
                ExternalId = Guid.NewGuid().ToString(),
                Source = "UnitTest",
                Title = "The Empire Strikes Back",
                Director = "Irvin Kershner",
                Producer = "Gary Kurtz",
                ReleaseDate = new DateTime(1980, 5, 21),
                Description = "Second film in the Star Wars saga.",
                EpisodeId = 5
            };
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsError);
            Assert.True(result.Value);
        }
        [Fact]
        public async Task UpdateMovie_WithMovieIdInexistent_ReturnsError()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new UpdateMovieCommandHandler(fakeDbContext, _mapper);

            var command = new UpdateMovieCommand
            {
                Id = 1,
                Source = "UnitTest",
                Title = "The Empire Strikes Back",
                Director = "Irvin Kershner",
                Producer = "Gary Kurtz",
                ReleaseDate = new DateTime(1980, 5, 21),
                Description = "Second film in the Star Wars saga.",
                EpisodeId = 5
            };
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(MoviesError.NoMovieFound, result.FirstError);
        }
        [Fact]
        public async Task UpdateMovie_UpdateMovie_ReturnsTrue()
        {
            var dbName = Guid.NewGuid().ToString();
            var dbContext = DbContextFactory.CreateInMemoryDbContext(dbName);
            var movie = new Movie
            {
                Title = "The Empire Strikes Back",
                Director = "Irvin Kershner",
                Producer = "Gary Kurtz",
                ReleaseDate = new DateTime(1980, 5, 21),
                Description = "Second film in the Star Wars saga.",
                EpisodeId = 5
            };

            dbContext.Movies.Add(movie);
            await dbContext.SaveChangesAsync();
            var handler = new UpdateMovieCommandHandler(dbContext, _mapper);
            var command = new UpdateMovieCommand
            {
                Id = movie.Id,
                Title = "The Empire Strikes Back 2",
                Director = "Irvin Kershner",
                Producer = "Gary Kurtz",
                ReleaseDate = new DateTime(1981, 5, 21),
                Description = "Third film in the Star Wars saga.",
                EpisodeId = 3
            };

            var result = await handler.Handle(command, CancellationToken.None);
            var movieUpdated = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == movie.Id);

            Assert.False(result.IsError);
            Assert.True(result.Value);
            Assert.NotNull(movieUpdated);
            Assert.Equal(movieUpdated.Title, command.Title);
            Assert.Equal(movieUpdated.ReleaseDate, command.ReleaseDate);
            Assert.Equal(movieUpdated.EpisodeId, command.EpisodeId);
        }
        [Fact]
        public async Task DeleteMovie_DeleteMovieWithInexistentId_ReturnsError()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new DeleteMovieCommandHandler(fakeDbContext);

            var command = new DeleteMovieCommand
            {
                Id = 1
            };
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(MoviesError.NoMovieFound, result.FirstError);
        }
        [Fact]
        public async Task DeleteMovie_DeleteExistentMovie_ReturnsTrue()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new DeleteMovieCommandHandler(fakeDbContext);

            var movie = new Movie
            {
                Title = "The Empire Strikes Back",
                Director = "Irvin Kershner",
                Producer = "Gary Kurtz",
                ReleaseDate = new DateTime(1980, 5, 21),
                Description = "Second film in the Star Wars saga.",
                EpisodeId = 5
            };
            fakeDbContext.Movies.Add(movie);
            await fakeDbContext.SaveChangesAsync();

            var command = new DeleteMovieCommand
            {
                Id = movie.Id
            };
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsError);
            Assert.True(result.Value);
        }
    }
}
