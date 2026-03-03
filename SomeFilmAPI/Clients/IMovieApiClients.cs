using SomeFilmAPI.Models.API;

namespace SomeFilmAPI.Clients
{
    public interface IMovieApiClients
    {
        Task<MovieDto> GetMovieById(int id);
        Task<MovieDto> GetMovieByTitle(string title);
        Task<List<MovieDto>> GetMovies();

        Task<PersonDto> GetPersonById(int id);
        Task<PersonDto> GetPersonByName(string name);
        Task<PersonDto> GetPersons();
    }
}
