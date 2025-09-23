using System.Diagnostics.CodeAnalysis;

namespace ConexaTest.Domain.Dto
{
    
    public class AddMovieDto
    {
        public string ExternalId { get; set; } = "";
        public string Source { get; set; } = "Local";
        public string Title { get; set; } = "";
        public string Director { get; set; } = "";
        public string Producer { get; set; } = "";
        public DateTime? ReleaseDate { get; set; }
        public string Description { get; set; } = "";
        public int? EpisodeId { get; set; }
    }
}
