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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsersController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> FilterUser(string turma)
        {
            List<User> users = users = await _context.User.Where(u => u.Class == turma).ToListAsync();
            return View(nameof(Index), users);
        }

        // GET: Users
        public IActionResult Index()
        {
            return _signInManager.IsSignedIn(User) ? View() : Redirect("/Home");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.SIGE == id);
            if (user == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            return _signInManager.IsSignedIn(User) ? View(user) : Redirect("/Home");
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return _signInManager.IsSignedIn(User) ? View() : Redirect("/Home");
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Email,PhoneNumber,Class,SIGE")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
            }
            return _signInManager.IsSignedIn(User) ? View(user) : Redirect("/Home");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }
            return _signInManager.IsSignedIn(User) ? View(user) : Redirect("/Home");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,Name,Email,PhoneNumber,Class,SIGE")] User user)
        {
            if (id != user.UserId)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return _signInManager.IsSignedIn(User) ? View(user) : Redirect("/Home");
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            return _signInManager.IsSignedIn(User) ? View(user) : Redirect("/Home");
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
        }

        private bool UserExists(long id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
