using System.Collections.Generic;

namespace Library.Models.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Orders { get; }
        void SaveOrder(Order order);
        IEnumerable<Order> GetOrdersByUserName(string userName);
        Order GetOrderById(int orderId);
        IEnumerable<Book> GetBooksFromOrder(int orderId);
        IEnumerable<ICollection<CartLine>> GetLines();
    }
}
