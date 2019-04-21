using Library.Models;
using Library.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private IBookRepository bookRepository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, IBookRepository bookRepo, Cart cartService)
        {
            repository = repoService;
            bookRepository = bookRepo;
            cart = cartService;
        }

        [Authorize]
        public ViewResult Index(int userId = 1, bool isLibrarianPage = false)
        {
            ViewBag.isLibrarianPage = isLibrarianPage;
            if ((User.IsInRole("Admins") || User.IsInRole("Librarian")) && isLibrarianPage)
            {
                return View("Index", repository.Orders);
            }
            else
            {
                return View("Index", repository.GetOrdersByUserName(User.Identity.Name));
            }
        }

        [Authorize(Roles = "Users")]
        public ActionResult Create()
        {
            ViewBag.Orders = repository.GetLines();
            ViewBag.DateToday = DateTime.Now;
            ViewBag.DateTodayString = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var temp = new Order
            {
                DateCreate = DateTime.Now
            };
            TempData["message"] = $"Заявка {temp.Name} была добавлена в корзину";
            return View("Checkout", new Order());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                ViewBag.Lines = order.Lines;
                TempData["message"] = $"Заявка {order.Name} была подтверждена пользователем";
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                TempData["message"] = $"Заявка {order.Name} не была подтверждена пользователем";
                return View(order);
            }
        }

        [Authorize(Roles = "Admins, Librarian")]
        public ActionResult Confirm(int orderId)
        {
            Order order = repository.GetOrderById(orderId);
            if (order != null)
            {
                order.Status++;
                if (order.Status == 1)
                {
                    foreach (var book in order.GetBooksFromOrder())
                    {
                        book.CountAvailableBooks--;
                    }
                }
                if (order.Status == 3)
                {
                    foreach (var book in order.GetBooksFromOrder())
                    {
                        book.CountAvailableBooks++;
                    }
                }
                order.DateConfirm = DateTime.Now;
                repository.SaveOrder(order);
            }
            TempData["message"] = $"Заявка {order.Name} была подтверждена библиотекарем";
            return RedirectToAction("Index", "Book");
        }

        public ViewResult Completed()
        {
            IEnumerable<Book> books = cart.books;
            foreach (Book book in books)
            {
                bookRepository.SaveBook(book);
            }
            cart.Clear();
            return View();
        }

        public ActionResult Canceled(int orderId)
        {
            Order temp = repository.GetOrderById(orderId);
            if (temp != null)
            {
                temp.Status += 2;
            }
            repository.SaveOrder(temp);
            TempData["message"] = $"Заявка {temp.Name} была отменена библиотекарем";
            return RedirectToAction("Index", "Book");
        }

        public ViewResult UpStatus(int orderId)
        {
            Order temp = repository.GetOrderById(orderId);
            if (temp != null)
            {
                temp.Status++;
            }
            repository.SaveOrder(temp);
            TempData["message"] = "Статус заявки был обновлен";
            return View("Index", repository.Orders);
        }
    }
}
