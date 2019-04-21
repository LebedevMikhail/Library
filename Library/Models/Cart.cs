using System.Collections.Generic;
using System.Linq;

namespace Library.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Book book)
        {
            CartLine line = lineCollection
                .Where(b => b.Book.BookId == book.BookId)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Book = book,
                });
            }
        }

        public virtual void RemoveLine(Book book) =>
            lineCollection.RemoveAll(line => line.Book.BookId == book.BookId);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;
        public virtual IEnumerable<Book> books => Lines.Select(lines => lines.Book);
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Book Book { get; set; }
    }
}
