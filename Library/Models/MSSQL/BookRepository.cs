using Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.MSSQL
{
    public class BookRepository : IBookRepository
    {
        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;
        public IEnumerable<Book> Books => _context.Books;

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
            {
                book.CountAvailableBooks = book.CountAllBooks;
                _context.Books.Add(book);

            }
            else
            {
                var dbEntry = Books.FirstOrDefault(b => b.BookId == book.BookId);
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.DatePublication = book.DatePublication;
                    dbEntry.GenreId = book.GenreId;

                    var delta = book.CountAllBooks - dbEntry.CountAllBooks;
                    if (delta <= dbEntry.CountAvailableBooks)
                    {
                        dbEntry.CountAvailableBooks = dbEntry.CountAvailableBooks + delta; ;
                        dbEntry.CountAllBooks = dbEntry.CountAllBooks + delta;
                    }
                }
            }
            _context.SaveChanges();
        }

        public Book DeleteBook(int bookId)
        {
            var dbEntry = Books.FirstOrDefault(b => b.BookId == bookId);
            if (dbEntry != null && dbEntry.CountAvailableBooks == dbEntry.CountAllBooks)
            {
                _context.Books.Remove(dbEntry);
            }
            _context.SaveChanges();
            return dbEntry;
        }

        public Book FindBookByName(string name) => Books.FirstOrDefault(b => b.Name == name);

        public List<Book> GetFilteredBooks(string query)
        {
            var result = new List<Book>();
            foreach (var book in Books)
            {
                if (book.Name.ToLower().Contains(query.ToLower()))
                {
                    result.Add(book);
                }
            }
            return result.ToList();
        }

        public Book GetBookById(int bookId) =>
         Books.FirstOrDefault(b => b.BookId == bookId);

        public IEnumerable<Book> GetBooksByGenreId(int genreId) => Books.Where(b => b.GenreId == genreId);
    }
}
