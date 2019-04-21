using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Library.Models;
using Library.Models.Interfaces;

namespace Library.AdditionalComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IBookRepository repository;
        private IGenreRepository genreRepository;

        public NavigationMenuViewComponent(IBookRepository _repository, IGenreRepository _genreRepository)
        {
            repository = _repository;
            genreRepository = _genreRepository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedGenre = RouteData?.Values["genre"];
            return View(genreRepository.Genres.Distinct().ToList());
        }
    }
}
