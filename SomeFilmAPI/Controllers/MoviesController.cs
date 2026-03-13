using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SomeFilmAPI.Clients;
using SomeFilmAPI.Models.API;
using SomeFilmAPI.Models.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;

namespace SomeFilmAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly SomeFilmContext _context;
        private PoiskKinoApiClient _pkApiClient;
        private ILogger<MoviesController> _logger;

        public MoviesController(SomeFilmContext context, PoiskKinoApiClient poiskKinoApiClient, ILogger<MoviesController> logger)
        {
            _context = context;
            _pkApiClient = poiskKinoApiClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.MovieTypeNavigation)
                .Include(m => m.MpaaNavigation)
                .ToListAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await LoadMovieFromDbByIdAsync(id);
            if (movie == null)
            {
                movie = await FetchAndSaveMovieAsync(id);
            }


            return Ok(DtoConverter.ToMovieResponseDto(movie));
        }


        private async Task<Movie> LoadMovieFromDbByIdAsync(int id)
        {

            var movie = await _context.Movies
                .Include(m => m.MovieTypeNavigation)
                .Include(m => m.MpaaNavigation)
                .Include(m=>m.Countries)
                .Include(m=>m.Genres)
                .Include(m=>m.Awards)
                .Include(m=>m.Movieratings)
                .Include(m=>m.Movieratings)
                .ThenInclude(r=>r.RatingNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            return movie;
        }

        private async Task<Movie> FetchAndSaveMovieAsync(int id)
        {
            MovieDto movieDto = await _pkApiClient.GetMovieByIdAsync(id);
            _logger.LogInformation("=== ДИАГНОСТИКА API ===");
            _logger.LogInformation("ID: {Id}", movieDto?.Id);
            _logger.LogInformation("Title: {Title}", movieDto?.Title);
            _logger.LogInformation("MovieType: {MovieType}", movieDto?.MovieType);
            _logger.LogInformation("MovieTypeName: {MovieTypeName}", movieDto?.MovieTypeName);
            _logger.LogInformation("Countries count: {Count}", movieDto?.Countries?.Count ?? 0);

            _logger.LogInformation(movieDto.Title);
            var movie = DtoConverter.ToMovie(movieDto);
            _logger.LogInformation(movie.Title);
            await ImportMovieFromApi(movie);
            return movie;

        }
        private async Task EnsureRelatedEntityExistAsync(Movie movie)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                movie.MovieTypeNavigation = await FindOrCreateMovieTypeAsync(movie.MovieTypeNavigation);
                movie.MpaaNavigation = await FindOrCreateMovieMpaaAsync(movie.MpaaNavigation);

                await _context.SaveChangesAsync();

                movie.Countries = await FindOrCreateCountriesAsync(movie.Countries.ToList());

                movie.Genres = await FindOrCreateGenresAsync(movie.Genres.ToList());

                movie.Awards = await FindOrCreateAwardsAsync(movie.Awards.ToList());

                await _context.SaveChangesAsync();

                movie.Movieratings = await FindOrCreateRatingsAsync(movie.Movieratings.ToList());

                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}");
                transaction.Rollback();
            }
        }

        private async Task<Movietype> FindOrCreateMovieTypeAsync (Movietype movieType)
        {
            var existing = await _context.Movietypes.FindAsync(movieType);
            if (existing != null)
            {
                return existing;
            }
            _context.Movietypes.Add(movieType);
            return movieType;
        }

        private async Task<Ratingmpaa> FindOrCreateMovieMpaaAsync(Ratingmpaa ratingmpaa)
        {
            var existing = await _context.Ratingmpaas.FindAsync(ratingmpaa);
            if (existing != null) return existing;

            _context.Ratingmpaas.Add(ratingmpaa);
            return ratingmpaa;
        }

        private async Task<List<Country>> FindOrCreateCountriesAsync(List<Country> countries)
        {
            List<Country> newCountries = new List<Country>();
            List<Country> result = new List<Country>();
            foreach (Country country in countries)
            {
                var existing = await _context.Countries.FirstOrDefaultAsync(c => c.Name == country.Name);
                if (existing == null)
                {
                    newCountries.Add(country);
                }
                else
                {
                    result.Add(existing);
                }


            }
            if (newCountries.Any())
            {
                _context.Countries.AddRange(newCountries);
                result.AddRange(newCountries);
            }
            return result;
        }

        private async Task<List<Genre>> FindOrCreateGenresAsync(List<Genre> genres)
        {
            List<Genre> newGenres = new List<Genre>();
            List<Genre> result = new List<Genre>();
            foreach (Genre genre in genres)
            {
                var existing = await _context.Genres.FirstOrDefaultAsync(g=> g.Title == genre.Title);
                if (existing == null)
                {
                    newGenres.Add(genre);
                }
                else
                {
                    result.Add(existing);
                }


            }
            if (newGenres.Any())
            {
                _context.Genres.AddRange(newGenres);
                result.AddRange(result);
            }
            return result;
        }

        private async Task<List<Award>> FindOrCreateAwardsAsync(List<Award> awards)
        {
            List<Award> newAwards = new List<Award>();
            List<Award> result = new List<Award>();
            foreach (Award award in awards)
            {
                var existing = await _context.Awards.FirstOrDefaultAsync(a=> a.Title == award.Title);
                if (existing == null)
                {
                    newAwards.Add(award);
                }
                else
                {
                    result.Add(existing);
                }


            }
            if (newAwards.Any())
            {
                _context.Awards.AddRange(awards);
                result.AddRange(result);
            }
            return result;
        }

        private async Task<List<Movierating>> FindOrCreateRatingsAsync(List<Movierating> ratings)
        {
            List<Movierating> result = new List<Movierating>();
            foreach (Movierating rating in ratings)
            {
                Rating ratingType = await FindOrCreateRatingType(rating.RatingNavigation);
                result.Add(new Movierating() { RatingId=ratingType.Id, Rating = rating.Rating, MovieId = rating.MovieId, RatingNavigation = ratingType});
            }
            return result;
        }

        private async Task<Rating> FindOrCreateRatingType(Rating rating)
        {
            var existing = await _context.Ratings.FirstOrDefaultAsync(r => r.Title == rating.Title);
            if(existing == null)
            {
                _context.Ratings.Add(rating);
                return rating;
            }
            else
            {
                return existing;
            }
        }

        private async Task ImportMovieFromApi(Movie movie)
        {
            try
            {
                await EnsureRelatedEntityExistAsync(movie);
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка: "+ex.Message);
            }
        }

    }
}
