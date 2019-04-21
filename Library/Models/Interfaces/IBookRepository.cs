using System;
using System.Collections.Generic;

namespace Library.Models.Interfaces
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }
        void SaveBook(Book book);
        Book DeleteBook(int bookId);
        Book FindBookByName(string name);
        List<Book> GetFilteredBooks(string query);
        Book GetBookById(int bookid);
        IEnumerable<Book> GetBooksByGenreId(int genreId);
    }
}
