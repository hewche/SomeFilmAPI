using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SomeFilmAPI.Clients;
using SomeFilmAPI.Models.API;
using SomeFilmAPI.Models.DB;

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
                .Include(m => m.Country)
                .Include(m => m.MovieTypeNavigation)
                .Include(m => m.MpaaNavigation)
                .ToListAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponseDto>> GetMovie(int id)
        {
            //var movie = new Movie() { Id = 12 };
            var movie = await LoadMovieFromDbByIdAsync(id);
            if (movie == null)
            {
                movie = await FetchAndSaveMovieAsync(id);
            }

            return Ok(movie);
        }


        private async Task<MovieResponseDto> LoadMovieFromDbByIdAsync(int id)
        {

            var movie = await _context.Movies
                .Include(m => m.Country)
                .Include(m => m.MovieTypeNavigation)
                .Include(m => m.MpaaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            return DtoConverter.ToMovieResponseDto(movie);
        }

        private async Task<MovieResponseDto> FetchAndSaveMovieAsync(int id)
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
            return DtoConverter.ToMovieResponseDto(movie);

        }
        private async Task EnsureRelatedEntityExistAsync(Movie movie)
        {
            Movietype movietype = await _context.Movietypes.FirstOrDefaultAsync(t => t.Title == movie.MovieTypeNavigation.Title);
            if (movietype == null)
            {
                _context.Movietypes.Add(movie.MovieTypeNavigation);
            }
            else
            {
                movie.MovieTypeNavigation = movietype;
            }
            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Name == movie.Country.Name);
            if(country == null)
            {
                _context.Countries.Add(movie.Country);
            }
            else
            {
                movie.Country = country;
            }
            Ratingmpaa ratingmpaa = await _context.Ratingmpaas.FirstOrDefaultAsync(r => r.Title == movie.MpaaNavigation.Title);
            if (ratingmpaa == null){
                _context.Ratingmpaas.Add(movie.MpaaNavigation);
            }
            else
            {
                movie.MpaaNavigation = ratingmpaa;
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
