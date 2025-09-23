using ConexaTest.Application.Dtos;
using ConexaTest.Domain.Errors.Swapi;
using ErrorOr;

namespace ConexaTest.Application.Services
{
    public class SwapiServices
    {
        public  async Task<ErrorOr<List<SwapiResult>>> GetStarWarsMoviesAsync()
        {
            using HttpClient httpClient = new();
            HttpResponseMessage response;

            try
            {
                response = await httpClient.GetAsync("https://www.swapi.tech/api/films");
            }
            catch (HttpRequestException)
            {
                return SwapiErrors.SwapiServiceUnavailable;
            }

            if (!response.IsSuccessStatusCode)
            {
                return SwapiErrors.SwapiServiceUnavailable;
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            var formatedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<SwapiResponse>(apiResponse);

            if (formatedResponse == null || formatedResponse.Result == null)
            {
                return SwapiErrors.SwapiInvalidResponse;
            }

            if (formatedResponse.Result.Count == 0)
            {
                return SwapiErrors.SwapiMovieNotFound;
            }

            return formatedResponse.Result;
        }

    }
}
