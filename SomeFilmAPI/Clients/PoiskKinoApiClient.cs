using SomeFilmAPI.Models.API;
using System.Text.Json;

namespace SomeFilmAPI.Clients
{
    public class PoiskKinoApiClient
    {
        private string _apiKey;
        private HttpClient _httpClient;
        public PoiskKinoApiClient(HttpClient httpClient)
        {
            _apiKey = "861C8MZ-NQHMRP3-HDGTG33-AB7QDKV";
            _httpClient = httpClient;
        }

        public async Task<MovieDto> GetMovieById(int id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.poiskkino.dev/v1.4/movie/{id}");
                request.Headers.TryAddWithoutValidation("X-API-KEY", _apiKey);

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<MovieDto>(json);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
    }
}
