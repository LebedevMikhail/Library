using System.Collections.Generic;

namespace Library.Models.Interfaces
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> Genres { get; }
        Genre GetGenreById(int genreId);
        string GetGenreNameById(int genreId);
    }
}
