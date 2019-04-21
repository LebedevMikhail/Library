using Library.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Library.Models.MSSQL
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Order> Orders => context.Orders
                            .Include(o => o.Lines)
                            .ThenInclude(l => l.Book);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Book));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }

        public IEnumerable<Order> GetOrdersByUserName(string userName)
        {

            return context.Orders
                .Include(o => o.Lines)
                .ThenInclude(o => o.Book)
                .Where(o => o.Name == userName
                );
        }

        public Order GetOrderById(int orderId)
        {
            return context.Orders
                .Include(o => o.Lines)
                .ThenInclude(o => o.Book)
                .FirstOrDefault(o => o.OrderID == orderId);
        }

        public IEnumerable<Book> GetBooksFromOrder(int orderId)
        {
            return GetOrderById(orderId).Lines.Select(l => l.Book);
        }

        public IEnumerable<ICollection<CartLine>> GetLines() => Orders.Select(o => o.Lines);

    }
}
