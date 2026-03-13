
using SomeFilmAPI.Models.DB;

namespace SomeFilmAPI.Models.API
{
    public class DtoConverter
    {
        public static Movie ToMovie(MovieDto movieDto)
        {
            return new Movie
            {
                Id = movieDto.Id,
                Title = movieDto.Title,
                MovieType = movieDto.MovieType,
                MovieTypeNavigation = new Movietype() { Id = movieDto.MovieType, Title = movieDto.MovieTypeName },
                DateMovie = DateOnly.Parse($"01.01.{movieDto.DateMovie}"),
                Description = movieDto.Description,
                Slogan = movieDto.Slogan,
                Poster = movieDto.Poster.Url,
                Country = new Country() { Name = movieDto.Countries[0].Name },
                MpaaNavigation= new Ratingmpaa() { Title=movieDto.Mpaa}
            };
        }

        public static MovieResponseDto ToMovieResponseDto(Movie movie)
        {
            return new MovieResponseDto
            {
                Id = movie.Id,
                Title = movie.Title,
                MovieTypeName = movie.MovieTypeNavigation.Title,
                DateMovie = movie.DateMovie.Year,
                Description = movie.Description,
                Slogan = movie.Slogan,
                Poster = movie.Poster,
                Countries = new List<CountryResponseDto>() { new CountryResponseDto() {Name= movie.Country.Name } },
                Genres = new List<GenreResponseDto>() { new GenreResponseDto() { Name = movie.Country.Name } }
            };
        }
        private static List<CountryResponseDto> ToCountryResponseDto(List<Country> countriesDto)
        {
            return (List<CountryResponseDto>)countriesDto.Select(c => new CountryResponseDto() { Name = c.Name});
        }
        private static List<GenreResponseDto> ToGenreResponseDto(List<Genre> genresDto)
        {
            return (List<GenreResponseDto>)genresDto.Select(g => new GenreResponseDto() { Name = g.Title});
        }
    }
}
