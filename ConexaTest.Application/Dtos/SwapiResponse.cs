using Newtonsoft.Json;
namespace ConexaTest.Application.Dtos
{
    public class SwapiResponse
    {
        [JsonProperty("result")]
        public List<SwapiResult> Result { get; set; }
    }
    public class SwapiResult
    {
        [JsonProperty("properties")]
        public SwapiProperties Properties { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
    public class SwapiProperties
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("episode_id")]
        public int EpisodeId { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("producer")]
        public string Producer { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

    }

}
