using ConexaTest.Domain.Models;


namespace ConexaTest.Application.Builders
{
    public class MovieBuilder
    {
        private readonly Movie Movie;

        public MovieBuilder(string Title,string Director,string Producer)
        {
            Movie = new Movie()
            {
                Title = Title,
                Director = Director,
                Producer = Producer
            };
        }
        public MovieBuilder SetId(int id)
        {
            Movie.Id = id;
            return this;
        }
        public MovieBuilder SetExternalId(string externalId)
        {
            Movie.ExternalId = externalId;
            return this;
        }
        public MovieBuilder SetSource(string source)
        {
            Movie.Source = source;
            return this;
        }
        public MovieBuilder SetReleaseDate(DateTime releaseDate)
        {
            Movie.ReleaseDate = releaseDate;
            return this;
        }
        public MovieBuilder SetDescription(string description)
        {
            Movie.Description = description;
            return this;
        }
        public MovieBuilder SetEpisodeId(int episodeId)
        {
            Movie.EpisodeId = episodeId;
            return this;
        }
        public Movie Build()
        {
            return Movie;
        }
    }
}
