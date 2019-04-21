using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("genreId")]
        [Required]
        [ForeignKey("Genre")]
        public int GenreId { get; set; }

        [Column("date")]
        [Required]
        public DateTime DatePublication { get; set; }

        [Required]
        public int CountAvailableBooks { get; set; }

        public int CountAllBooks { get; set; }

        public Book()
        {
        }

        public Book(int bookId, string name, int genreId, DateTime datePublication, int count)
        {
            BookId = bookId;
            Name = name;
            GenreId = genreId;
            DatePublication = datePublication;
            CountAvailableBooks = count;
        }

        public override bool Equals(object obj)
        {
            return obj is Book book &&
                   BookId == book.BookId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BookId, Name, GenreId, DatePublication);
        }
    }
}
