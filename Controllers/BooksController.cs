﻿using System;
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
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public BooksController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Books
        [Route("livros")]
        public async Task<IActionResult> Index()
        {
            return _signInManager.IsSignedIn(User) ? View(await _context.Book
                .OrderByDescending(b => b.BookId)
                .Take(5)
                .ToListAsync()) : Redirect("/Home");
        }
        [Route("livros/livros_cadastrados")]
        public async Task<IActionResult> RegistredBooks()
        {
            return _signInManager.IsSignedIn(User) ? View(await _context.Book.ToListAsync()) : Redirect("/Home");
        }

        // GET: Books/Details/5
        [Route("livros/detalhes")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Register == id);
            if (book == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            return _signInManager.IsSignedIn(User) ? View(book) : Redirect("/Home");
        }

        // GET: Books/Create
        [Route("livros/cadastrar")]
        public IActionResult Create()
        {
            return _signInManager.IsSignedIn(User) ? View() : Redirect("/Home");
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("livros/cadastrar")]
        public async Task<IActionResult> Create([Bind("BookId,Title,Genre,Author,Register")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
            }
            return _signInManager.IsSignedIn(User) ? View(book) : Redirect("/Home");
        }

        // GET: Books/Edit/5
        [Route("livros/editar")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }
            return _signInManager.IsSignedIn(User) ? View(book) : Redirect("/Home");
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("livros/editar")]
        public async Task<IActionResult> Edit(long id, [Bind("BookId,Title,Genre,Author,Register")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            return _signInManager.IsSignedIn(User) ? View(book) : Redirect("/Home");
        }

        // GET: Books/Delete/5
        [Route("livros/deletar")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return _signInManager.IsSignedIn(User) ? NotFound() : Redirect("/Home");
            }

            return _signInManager.IsSignedIn(User) ? View(book) : Redirect("/Home");
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("livros/deletar")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
        }

        private bool BookExists(long id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
