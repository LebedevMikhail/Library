using Library.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Library.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreRepository _context;

        public GenreController(IGenreRepository context)
        {
            _context = context;
        }
        public ViewResult Index() => View(_context.Genres);


    }
}
