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
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Country)
                .Include(m => m.MovieTypeNavigation)
                .Include(m => m.MpaaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                movie = await FetchAndSaveMovieAsync(id);
                return Ok(movie);
            }

            return Ok(movie);
        }


        private async Task<Movie> FetchAndSaveMovieAsync(int id)
        {
            MovieDto movieDto = await _pkApiClient.GetMovieByIdAsync(id);
            _logger.LogInformation(movieDto.Title);
            return DtoConverter.ToMovie(movieDto);

        }
        private async Task EnsureRelatedEntityExistAsync(Movie movie)
        {
            //Movietype movietype = await _context.Movietypes.FindAsync(movie.MovieType);
            if ((await _context.Movietypes.FindAsync(movie.MovieType) == null))
            {
                _context.Movietypes.Add(movie.MovieTypeNavigation);
            }
            if((await _context.Countries.FirstOrDefaultAsync(c=> c.Name ==  movie.Country.Name) == null))
            {
                _context.Countries.Add(movie.Country);
            }
            if((await _context.Ratingmpaas.FirstOrDefaultAsync(r => r.Title == movie.MpaaNavigation.Title) == null)){
                _context.Ratingmpaas.Add(movie.MpaaNavigation);
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
