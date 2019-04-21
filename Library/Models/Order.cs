using Library.Models.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Library.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        [Column("status")]
        public int Status { get; set; } = (int)StatusOrder.Decorated;

        [Required(ErrorMessage = "Пожалуйства введите корректное имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = " Пожалуйства введите корректную дату")]
        [DataType(DataType.Date)]
        public DateTime TermUse { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateConfirm { get; set; }

        public IList<string> getBookNames()
        {
            IList<string> result = Lines?.Select(line => line.Book.Name).ToList();
            return result;
        }
        public IList<Book> GetBooksFromOrder()
        {
            return Lines?.Select(line => line.Book).ToList();
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
