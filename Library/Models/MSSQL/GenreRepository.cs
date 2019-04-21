using Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Models.MSSQL
{
    public class GenreRepository : IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private ApplicationDbContext _context;
        public IEnumerable<Genre> Genres => _context.Genres;
        public Genre GetGenreById(int genreId) => Genres.FirstOrDefault(genre => genre.GenreId == genreId);

        public string GetGenreNameById(int genreId)
        {
            return GetGenreById(genreId).Name;
        }
    }
}
