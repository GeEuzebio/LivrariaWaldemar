using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace LibraryApp.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ReservationsController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservation.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Search(long bookId, string userId)
        {
            var book = await _context.Book.FirstOrDefaultAsync(b => b.BookId == bookId);
            book!.UserId = userId;
            _context.Update(book);
            return RedirectToAction(nameof(Create), book);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> Create(Book book)
        {
            if(book == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }
            Borrow? borrow = await _context.Borrow.FirstOrDefaultAsync(b => b.BookId == book.Register);
            DateTime? initialDate = borrow!.LastDate;
            ViewBag.InitialDate = initialDate;
            User? user = await _context.User.FirstOrDefaultAsync(u => u.SIGE == book.UserId);
            ViewBag.UserData = user;
            return _signInManager.IsSignedIn(User) ? View(book): Redirect("/Home");
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation(string UserId, string BookId, string BookTitle, DateTime InitialDate, DateTime LastDate)
        {
            if (ModelState.IsValid)
            {
                Reservation reservation = new Reservation
            {
                UserId = UserId,
                BookId = BookId,
                InitialDate = InitialDate,
                LastDate = LastDate
            };
            _context.Reservation.Add(reservation);
            Book? book = await _context.Book.FirstOrDefaultAsync(b => b.Register == BookId);
            book!.Reserved = Status.Reserved;
            _context.Book.Update(book);
            User? user = await _context.User.FirstOrDefaultAsync(u => u.SIGE == UserId);
            user!.HasReservation = true;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
        }
            return _signInManager.IsSignedIn(User)? RedirectToAction("Index") : Redirect("/Home");
    }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,BookId,InitialDate,LastDate")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                User? user = await _context.User.FirstOrDefaultAsync(b => b.SIGE == reservation.UserId);
                user!.HasReservation = false;
                _context.User.Update(user);
                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(long id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
