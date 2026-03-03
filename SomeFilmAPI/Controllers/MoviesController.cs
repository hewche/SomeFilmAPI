using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SomeFilmAPI.Models.DB;

namespace SomeFilmAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly SomeFilmContext _context;

        public MoviesController(SomeFilmContext context)
        {
            _context = context;
        }

        // GET: Movies
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
                return NotFound($"Фильм с ID {id} не найден");
            }

            return Ok(movie);


        }
    }
}
