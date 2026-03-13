using System.Text.Json.Serialization;

namespace SomeFilmAPI.Models.API
{
    public class MovieDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Title { get; set; }

        [JsonPropertyName("typeNumber")]
        public int MovieType { get; set; }

        [JsonPropertyName("type")]
        public string? MovieTypeName { get; set; }

        [JsonPropertyName("year")]
        public int DateMovie { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("slogan")]
        public string? Slogan { get; set; }

        [JsonPropertyName("poster")]
        public PosterDto? Poster { get; set; }

        [JsonPropertyName("ratingMpaa")]
        public string? Mpaa { get; set; }

        [JsonPropertyName("countries")]
        public List<CountryDto>? Countries { get; set; }

        [JsonPropertyName("genres")]
        public List<GenreDto>? Genres { get; set; }

        [JsonPropertyName("rating")]
        public Dictionary<string, double>? Rating { get; set; }

        [JsonPropertyName("persons")]
        public List<PersonDto>? Persons { get; set; }
    }

    public class GenreDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class PosterDto
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("previewUrl")]
        public string? PreviewUrl { get; set; }

    }
    public class CountryDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

}
