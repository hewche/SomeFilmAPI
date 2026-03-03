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

        public MoviesController(SomeFilmContext context)
        {
            _context = context;
            _pkApiClient = new PoiskKinoApiClient(new HttpClient());
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
                MovieDto movieDto = _pkApiClient.GetMovieById(id).Result;
                return Ok(DtoConverter.ToMovie(movieDto));

            }

            return Ok(movie);


        }

    }
}
