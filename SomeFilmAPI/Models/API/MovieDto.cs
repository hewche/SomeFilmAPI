using Newtonsoft.Json;
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
        public Dictionary<string, decimal>? Rating { get; set; }

        [JsonPropertyName("persons")]
        public List<PersonDto>? Persons { get; set; }

        [JsonPropertyName("awards")]
        public List<AwardDto> Awards { get; set; }

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

    public class AwardDto
    {
        [JsonPropertyName("nomination")]
        public NominationDto? Nomination { get; set; }

        [JsonPropertyName("winning")]
        public bool IsWinning { get; set; }
        public bool ShouldSerializeNomination()
        {
            return IsWinning;
        }
    }

    public class NominationDto
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("year")]
        public int? year { get; set; }
    }



}
