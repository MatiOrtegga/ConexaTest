namespace ConexaTest.Domain.Models
{   
    public class Movie
    {
        public int Id { get; set; }
        public string ExternalId { get; set; } = "";
        public string Source { get; set; } = "Local";
        public required string Title { get; set; }
        public required string Director { get; set; }
        public required string Producer { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Description { get; set; } = "";
        public int? EpisodeId { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
