using Library.Extensions;
using Library.Models;
using Library.Models.Interfaces;
using Library.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Library.Controllers
{
    [Authorize(Roles = "Users, Admins")]
    public class CartController : Controller
    {
        private IBookRepository repository;

        public CartController(IBookRepository repo)
        {
            repository = repo;
            ViewBag.CartQuantity = 10;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int bookId)
        {
            Book book = repository.Books
                .FirstOrDefault(p => p.BookId == bookId);

            if (book != null)
            {
                Cart cart = GetCart();
                cart.AddItem(book);
                SaveCart(cart);

            }
            TempData["message"] = $"Книга {book.Name} была добавлена в корзину";

            return RedirectToAction("Index", "Book");
        }

        public RedirectToActionResult RemoveFromCart(int bookId,
                string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(p => p.BookId == bookId);

            if (book != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(book);
                SaveCart(cart);
            }
            TempData["message"] = $"Книга {book.Name} была удалена из корзины";

            return RedirectToAction("Index", "Book");
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        public ActionResult RedirectToCart()
        {
            return View("Index", "Cart");
        }
    }
}
