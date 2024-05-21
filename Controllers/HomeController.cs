using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using LibraryApp.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace LibraryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInResult;
        private readonly WhatsAppSender _whatsAppSender;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _signInResult = signInManager;
            _whatsAppSender = new WhatsAppSender(config);
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
        [Route("pesquisa")]
        public async Task<IActionResult> Search(string title, string filter)
        {
            if(filter == "Titulo")
            {
                List<Book> list = await _context.Book
                                                .Where(m => m.Title!.ToUpper().Contains(title.ToUpper()))
                                                .OrderBy(b => b.Register)
                                                .ToListAsync();
                return View(nameof(Index), list);
            }
            else if(filter == "Autor")
            {
                List<Book> list = await _context.Book
                                                .Where(m => m.Author!.ToUpper().Contains(title.ToUpper()))
                                                .OrderBy(a => a.Author)
                                                .ToListAsync();
                return View(nameof(Index), list);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(string id)
        {
            Debug.WriteLine(id);
            Book? book = await _context.Book.FirstOrDefaultAsync(b => b.Register == id);
            return _signInResult.IsSignedIn(User) ? RedirectToAction("Create", "Reservations", book) : Redirect("/Home");
        }

        [HttpPost]
        public async Task<IActionResult> ViewReservation(string id)
        {
            Reservation? reservation = await _context.Reservation.FirstOrDefaultAsync(r => r.BookId == id);
            long? reservationId = reservation!.Id;
            return _signInResult.IsSignedIn(User) ? Redirect($"/Reservations/Details/{reservationId}") : Redirect("/Home");
        }

        [HttpPost]
        public async Task<IActionResult> Devolve(string id)
        {
            Borrow? borrow = await _context.Borrow.FirstOrDefaultAsync(b => b.BookId == id);
            return _signInResult.IsSignedIn(User) ? Redirect($"/Borrows/Delete/{borrow!.Id}") : Redirect("/Home");
        }

        [Route("consulta_agendamentos")]
        public async Task<IActionResult> Schedules()
        {
            DateTime toDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Scheduling? schedule = await _context.Scheduling.FirstOrDefaultAsync(d => d.Day == toDay.ToUniversalTime());
            ViewBag.Schedule = schedule;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("consulta_agendamentos/consulta_por_data")]
        public async Task<IActionResult> SearchSchedules(DateTime date)
        {
            return View(nameof(Schedules), await _context.Scheduling.FirstOrDefaultAsync(d => d.Day == date.ToUniversalTime()));
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
