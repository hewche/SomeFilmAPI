using SomeFilmAPI.Models.API;
using System.Text.Json;

namespace SomeFilmAPI.Clients
{
    public class PoiskKinoApiClient
    {
        private string _apiKey;
        private HttpClient _httpClient;
        private ILogger<PoiskKinoApiClient> _logger;
        public PoiskKinoApiClient(HttpClient httpClient, ILogger<PoiskKinoApiClient> logger)
        {
            _apiKey = "861C8MZ-NQHMRP3-HDGTG33-AB7QDKV";
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<MovieDto> GetMovieByIdAsync(int id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.poiskkino.dev/v1.4/movie/id={id}");
                request.Headers.TryAddWithoutValidation("X-API-KEY", _apiKey);

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(json);
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
