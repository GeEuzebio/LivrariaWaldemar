using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Diagnostics;

namespace LibraryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInResult;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _signInResult = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Borrow([Bind("BookId")] long BookId)
        {
            Book? book = await _context.Book.FirstOrDefaultAsync(m => m.BookId == BookId);
            return RedirectToAction("Create", "Borrows", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string title)
        {
            List<Book> list = await _context.Book.Where(m => m.Title == title).ToListAsync();
            return View(nameof(Index), list);
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(long id)
        {
            Debug.WriteLine(id);
            Book? book = await _context.Book.FirstOrDefaultAsync(b => b.BookId == id);
            return _signInResult.IsSignedIn(User) ? RedirectToAction("Create", "Reservations", book) : Redirect("/Home");
        }

        [HttpPost]
        public async Task<IActionResult> ViewReservation(long id)
        {
            Reservation? reservation = await _context.Reservation.FirstOrDefaultAsync(r => r.BookId == id);
            long? reservationId = reservation!.Id;
            return _signInResult.IsSignedIn(User) ? Redirect($"/Reservations/Details/{reservationId}") : Redirect("/Home");
        }

        [HttpPost]
        public async Task<IActionResult> Devolve(long id)
        {
            Borrow? borrow = await _context.Borrow.FirstOrDefaultAsync(b => b.BookId == id);
            return _signInResult.IsSignedIn(User) ? Redirect($"/Borrows/Delete/{borrow!.Id}") : Redirect("/Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
