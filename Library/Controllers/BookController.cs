using Library.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Library.Models;
using Library.Models.Utils;

namespace Library.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IOrderRepository _orderRepository;
        private IEnumerable<Book> FilteredBooks { get; set; } = new List<Book>();

        public BookController(IBookRepository bookRepository, IGenreRepository genreRepository, IOrderRepository orderRepository)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _orderRepository = orderRepository;
        }
        public ViewResult Index(int page = 1, SortState sortOrder = SortState.NameAsc, string query = "")
        {
            var books = GetBookRepository(query);
            ViewBag.GenreRepository = _genreRepository;
            ViewBag.OrderRepository = _orderRepository;
            ViewBag.NumberPage = (int)(Math.Ceiling((decimal)books.Count() / Constants.PageSize));
            if (page > ViewBag.NumberPage)
            {
                page = 1;
            }
            ViewBag.CurrentPage = page;

            books = GetSortState(sortOrder, books);
            books = books.Skip((page - 1) * Constants.PageSize)
                 .Take(Constants.PageSize).ToList();
            return View(books);
        }

        private IEnumerable<Book> GetSortState(SortState sortOrder, IEnumerable<Book> books)
        {
            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["DatePublicationSort"] = sortOrder == SortState.DatePublicationAsc ? SortState.DatePublicationDesc : SortState.DatePublicationAsc;
            ViewData["GenreSort"] = sortOrder == SortState.GenreAsc ? SortState.GenreDesc : SortState.GenreAsc;
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    books = books.OrderBy(b => b.Name);
                    break;
                case SortState.NameDesc:
                    books = books.OrderByDescending(b => b.Name);
                    break;
                case SortState.GenreAsc:
                    books = books.OrderBy(b => _genreRepository.GetGenreById(b.GenreId).Name);
                    break;
                case SortState.GenreDesc:
                    books = books.OrderByDescending(b => _genreRepository.GetGenreById(b.GenreId).Name);
                    break;
                case SortState.DatePublicationAsc:
                    books = books.OrderBy(b => b.DatePublication.ToString());
                    break;
                case SortState.DatePublicationDesc:
                    books = books.OrderByDescending(b => b.DatePublication.ToString());
                    break;
                default: break;
            }

            return books;
        }

        public ViewResult Edit(int bookId)
        {
            ViewBag.Genres = _genreRepository.Genres;
            return View(_bookRepository.Books.FirstOrDefault(book => book.BookId == bookId));
        }

        public ActionResult Create()
        {
            ViewBag.Genres = _genreRepository.Genres;

            return View("Edit", new Book());
        }

        [HttpPost]
        public IActionResult UpdateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.SaveBook(book);
                TempData["message"] = $"Книга {book.Name} была обновлена";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = $"Книга {book.Name} не была обновлена";
                return View(book);
            }

        }

        public IActionResult SortedBookByField(string field)
        {
            var result = _bookRepository.Books.OrderBy(b => field).ToList();
            return View(result);
        }
        [HttpPost]
        public IActionResult DeleteBook(int BookId)
        {
            Book DeletedBook = _bookRepository.DeleteBook(BookId);
            if (DeletedBook != null)
            {
                TempData["message"] = $" Книга{ DeletedBook.Name} была удалена";
            }
            return RedirectToAction("Index");
        }
        public IEnumerable<Book> GetBookRepository(string query)
        {
            List<Book> books = new List<Book>();
            if (query != "" && query != "favicon.ico")
            {
                List<Book> FilteredBooksByName = new List<Book>();
                List<Book> filteredBooksByGenreName = new List<Book>();
                FilteredBooksByName = _bookRepository.GetFilteredBooks(query);
                var GenresIndex = _genreRepository.Genres.Where(g => g.Name.ToLower().Equals(query.ToLower())).Select(g => g.GenreId);
                foreach (var index in GenresIndex)
                {
                    foreach (var book in _bookRepository.GetBooksByGenreId(index))
                    {
                        filteredBooksByGenreName.Add(book);
                    }

                }
                foreach (var book in FilteredBooksByName)
                {
                    books.Add(book);
                }
                foreach (var book in filteredBooksByGenreName)
                {
                    books.Add(book);
                }
            }
            else
            {
                books = _bookRepository.Books.Where(b => b.CountAvailableBooks > 0).ToList();
            }

            return books;
        }

        public void ClearInputSeacrh() => ViewBag.query = "";
    }
}
