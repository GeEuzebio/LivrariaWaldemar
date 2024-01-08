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
using System.Diagnostics;

namespace LibraryApp.Controllers
{
    public class BorrowsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public BorrowsController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Search(long bookId, long userId)
        {
            var book = await _context.Book.FirstOrDefaultAsync(b => b.BookId == bookId);
            book!.UserId = userId;
            _context.Update(book);
            return RedirectToAction(nameof(Create), book);
        }

        // GET: Borrows
        public async Task<IActionResult> Index()
        {
            return _signInManager.IsSignedIn(User) ? View(await _context.Borrow.ToListAsync()) : Redirect("/Home");
        }

        // GET: Borrows/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return _signInManager.IsSignedIn(User) ? View(borrow) : Redirect("/Home");
        }

        // GET: Borrows/Create
        public async Task<IActionResult> Create(Book b)
        {
            User? user = await _context.User.FirstOrDefaultAsync(u => u.UserId == b.UserId);
            ViewBag.UserData = user;
            return _signInManager.IsSignedIn(User) ? View(b) : Redirect("/Home");
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBorrow(long UserId, long BookId, string BookTitle, DateTime InitialDate, DateTime LastDate)
        {
            if (ModelState.IsValid)
            {
                Borrow borrow = new Borrow
                {
                    UserId = UserId,
                    BookId = BookId,
                    InitialDate = InitialDate,
                    LastDate = LastDate
                };
                _context.Borrow.Add(borrow);
                Book? book = await _context.Book.FirstOrDefaultAsync(b => b.BookId == BookId);
                book!.Status = Status.Borrowed;
                _context.Book.Update(book);
                User? user = await _context.User.FirstOrDefaultAsync(u => u.UserId == UserId);
                user!.HasBorrow = true;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
            }
            return _signInManager.IsSignedIn(User) ? RedirectToAction("Index") : Redirect("/Home");
        }

        // GET: Borrows/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            var borrow = await _context.Borrow.FindAsync(id);
            if (borrow == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }
            return _signInManager.IsSignedIn(User) ? View(borrow) : Redirect("/Home");
        }

        // POST: Borrows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,InitialDate,LastDate")] Borrow borrow)
        {
            if (id != borrow.Id)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowExists(borrow.Id))
                    {
                        return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
            }
            return View(borrow);
        }

        // GET: Borrows/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            var borrow = await _context.Borrow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            return _signInManager.IsSignedIn(User) ? View(borrow) : NotFound();
        }

        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var borrow = await _context.Borrow.FindAsync(id);
            if (borrow != null)
            {
                _context.Borrow.Remove(borrow);
                Book? book = await _context.Book.FirstOrDefaultAsync(b => b.BookId == id);
                book!.Status = Status.Available;
                _context.Book.Update(book);
            }

            await _context.SaveChangesAsync();
            return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
        }

        public async Task<IActionResult> Reserve(long? Id)
        {
            if(Id == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            Book? book = await _context.Book.FirstOrDefaultAsync(b => b.BookId == Id);
            if(book == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }
            return _signInManager.IsSignedIn(User) ? RedirectToAction("Create", "Reservations", book) : Redirect("/Home");
        }

        private bool BorrowExists(long id)
        {
            return _context.Borrow.Any(e => e.Id == id);
        }
    }
}
