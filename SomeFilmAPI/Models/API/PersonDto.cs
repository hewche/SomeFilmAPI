using System.Text.Json.Serialization;

namespace SomeFilmAPI.Models.API
{
    public class PersonDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public DateOnly Birthday { get; set; }

        [JsonPropertyName("photo")]
        public string Photo { get; set; }

        public string Country { get; set; }

    }
}
