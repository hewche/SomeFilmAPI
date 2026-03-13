using SomeFilmAPI.Models.DB;
using System.Text.Json.Serialization;

namespace SomeFilmAPI.Models.API
{
    public class MovieResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CountryName { get; set; }
        public string MovieTypeName { get; set; }
        public string MpaaRating { get; set; }
        public string? Description { get; set; }
        public int DateMovie { get; set; }
        public string? Slogan { get; set; }
        public string Poster { get; set; }

        public List<GenreResponseDto> Genres { get; set; }
        public List<CountryResponseDto> Countries { get; set; }
        public List<AwardResponseDto> Awards { get; set; }
        public List<RatingResponseDto> Ratings { get; set; } 
    }
    public class GenreResponseDto
    {
        public string? Name { get; set; }
    }

    public class AwardResponseDto
    {
        public string? Title { get; set; }
    }
    public class PosterResponseDto
    {
        public string? Url { get; set; }
        public string? PreviewUrl { get; set; }

    }
    public class CountryResponseDto
    {
        public string? Name { get; set; }
    }

    public class RatingResponseDto
    {
        public string? Name { get; set; }
        public decimal? Rating { get; set; }
    }
}
