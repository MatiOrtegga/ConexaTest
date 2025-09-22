using ConexaTest.Application.Dtos;

namespace ConexaTest.Application.Services
{
    public class SwapiServices
    {
        public async Task<List<SwapiResult>> GetStarWarsMoviesAsync()
        {
            using HttpClient httpClient = new();
            using var response = await httpClient.GetAsync("https://www.swapi.tech/api/films");
            
            string apiResponse = await response.Content.ReadAsStringAsync();
            var formatedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<SwapiResponse>(apiResponse);

            return formatedResponse.Result;
        }
    }
}
