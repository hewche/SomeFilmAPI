
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
                MovieTypeNavigation = new Movietype() { Title = movieDto.MovieTypeName },
                MpaaNavigation = new Ratingmpaa() { Title = movieDto.Mpaa ?? "unkown"},
                DateMovie = DateOnly.Parse($"01.01.{movieDto.DateMovie}"),
                Description = movieDto.Description,
                Slogan = movieDto.Slogan ?? "unkown",
                Poster = movieDto.Poster.Url ?? "unkown",
                Countries = ToCountries(movieDto.Countries),
                Genres = ToGenres(movieDto.Genres),
                Awards = ToAwards(movieDto.Awards),
                Movieratings = ToMovieratings(movieDto.Rating, movieDto.Id),
            };
        }

        public static MovieResponseDto ToMovieResponseDto(Movie movie)
        {
            return new MovieResponseDto
            {
                Id = movie.Id,
                Title = movie.Title,
                MovieTypeName = movie.MovieTypeNavigation.Title,
                MpaaRating =  movie.MpaaNavigation.Title,
                DateMovie = movie.DateMovie.Year,
                Description = movie.Description,
                Slogan = movie.Slogan,
                Poster = movie.Poster,
                Countries = ToCountriesResponseDto(movie.Countries.ToList()),
                Genres = ToGenresResponseDto(movie.Genres.ToList()),
                Awards = movie.Awards != null? ToAwardsResponseDto(movie.Awards.ToList()): null,
                Ratings = ToMovieratingsResponseDto(movie.Movieratings.ToList()),
            };
        }
        private static List<CountryResponseDto> ToCountriesResponseDto(List<Country> countries)
        {
            return countries.Select(c => new CountryResponseDto() { Name = c.Name}).ToList();
        }
        private static List<GenreResponseDto> ToGenresResponseDto(List<Genre> genres)
        {
            return genres.Select(g => new GenreResponseDto() { Name = g.Title}).ToList();
        }

        private static List<AwardResponseDto> ToAwardsResponseDto(List<Award> awards)
        {
            return awards.Select(a => new AwardResponseDto() { Title = a.Title}).ToList();
        }
        private static List<RatingResponseDto> ToMovieratingsResponseDto(List<Movierating> ratings)
        {
            return ratings.Select(r => new RatingResponseDto() { Name= r.RatingNavigation.Title, Rating = r.Rating}).ToList();
        }

        private static List<Country> ToCountries(List<CountryDto> countriesDto)
        {

            return countriesDto.Select(c => new Country() { Name = c.Name }).ToList();
        }
        private static List<Genre> ToGenres(List<GenreDto> countriesDto)
        {
            return countriesDto.Select(c => new Genre() { Title = c.Name }).ToList();
        }
        private static List<Award> ToAwards(List<AwardDto> awardsDto)
        {
            if (awardsDto != null)
            {
                return awardsDto.Where(c => c.IsWinning).Select(c => new Award() { Title = c.Nomination.Title}).ToList();
            }
            return null;
        }

        private static List<Movierating> ToMovieratings(Dictionary<string, decimal> ratings, int Id)
        {
            return ratings.Select(r=> new Movierating() { RatingNavigation = new Rating() { Title = r.Key}, Rating=r.Value, MovieId=Id }).ToList();
        }
    }
}
