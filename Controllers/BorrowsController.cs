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
using LibraryApp.Services;

namespace LibraryApp.Controllers
{
    public class BorrowsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly WhatsAppSender _whatsAppSender;
        private readonly EmailSender _emailSender;

        public BorrowsController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            _context = context;
            _signInManager = signInManager;
            _whatsAppSender = new WhatsAppSender(config);
            _emailSender = new EmailSender(config);
        }

        [HttpPost]
        [Route("emprestimos/busca")]
        public async Task<IActionResult> Search(long bookId, string userId)
        {
            var book = await _context.Book.FirstOrDefaultAsync(b => b.BookId == bookId);
            book!.UserId = userId;
            _context.Update(book);
            return RedirectToAction(nameof(Create), book);
        }

        [HttpPost]
        [Route("emprestimos/busca_livro")]
        public async Task<IActionResult> SearchBook(string bookId)
        {
            var book = await _context.Book.FirstOrDefaultAsync(b => b.Register == bookId);
            return RedirectToAction(nameof(Create), book);
        }

        // GET: Borrows
        [Route("emprestimos")]
        public async Task<IActionResult> Index()
        {
            return _signInManager.IsSignedIn(User) ? View(await _context.Borrow.ToListAsync()) : Redirect("/Home");
        }

        // GET: Borrows/Details/5
        [Route("emprestimos/detalhes")]
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
        [Route("emprestimos/cadastrar")]
        public async Task<IActionResult> Create(Book b)
        {
            User? user = await _context.User.FirstOrDefaultAsync(u => u.SIGE == b.UserId);
            ViewBag.UserData = user;
            return _signInManager.IsSignedIn(User) ? View(b) : Redirect("/Home");
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("emprestimos/cadastrar")]
        public async Task<IActionResult> CreateBorrow(string UserId, string BookId, string BookTitle, DateTime InitialDate, DateTime LastDate)
        {
            if (ModelState.IsValid)
            {
                var keys = Signature.GenerateKeyPair();
                string publicKey = keys.publicKey;
                string privateKey = keys.privateKey;
                Borrow borrow = new Borrow
                {
                    UserId = UserId,
                    BookId = BookId,
                    InitialDate = InitialDate,
                    LastDate = LastDate,
                    PublicKey = publicKey,
                    PrivateKey = privateKey
                };
                _context.Borrow.Add(borrow);
                Book? book = await _context.Book.FirstOrDefaultAsync(b => b.Register == BookId);
                book!.Status = Status.Borrowed;
                if(book.Reserved == Status.Reserved)
                {
                    book!.Reserved = Status.Available;
                    Reservation? reservation = await _context.Reservation.FirstOrDefaultAsync(r => r.BookId == BookId);
                    _context.Reservation.Remove(reservation!);
                }
                _context.Book.Update(book);
                User? user = await _context.User.FirstOrDefaultAsync(u => u.SIGE == UserId);
                user!.HasBorrow = true;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                string message_email = $"<strong>Biblioteca Waldemar Falcão</strong>" +
                    $"<br />Este é um e-mail automático contendo informações sobre o seu empréstimo realizado na escola EEMTI Waldemar Falcão." +
                    $"<br/><br/><strong>Informações do Empréstimo</strong>:" +
                    $"<br/><strong>Livro</strong>: {book.Title}" +
                    $"<br/><strong>Autor</strong>: {book.Author}" +
                    $"<br/><strong>Data de Realização do Empréstimo</strong>: {borrow.InitialDate.Value.Day}/{borrow.InitialDate.Value.Month}/{borrow.InitialDate.Value.Year}" +
                    $"<br/><strong>Data Prevista de Devolução</strong>: {borrow.LastDate.Value.Day}/{borrow.LastDate.Value.Month}/{borrow.LastDate.Value.Year}";

                var signed = Signature.SignData(message_email, privateKey);
                message_email = message_email + $"<br /><br />Assinatura Digital: {signed}";
                await _emailSender.SendEmail(user.Email!, $"Empréstimo Realizado - {book.Title}", message_email);
                return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
            }
            return _signInManager.IsSignedIn(User) ? RedirectToAction("Index") : Redirect("/Home");
        }

        // GET: Borrows/Edit/5
        // Altered with objectiv of renew a borrow
        [Route("emprestimos/editar")]
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
        [Route("emprestimos/editar")]
        public async Task<IActionResult> Edit(long id, [Bind("Id,BookId,UserId,InitialDate,LastDate")] Borrow borrow)
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
        [Route("emprestimos/deletar")]
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
        [Route("emprestimos/deletar")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var borrow = await _context.Borrow.FindAsync(id);
            if (borrow != null)
            {
                var keys = Signature.GenerateKeyPair();
                string publicKey = keys.publicKey;
                string privateKey = keys.privateKey;
                borrow!.IsDevolved = true;
                borrow!.PrivateKey = privateKey;
                borrow!.PublicKey = publicKey;
                Book? book = await _context.Book.FirstOrDefaultAsync(b => b.Register == borrow.BookId);
                book!.Status = Status.Available;
                User? user = await _context.User.FirstOrDefaultAsync(u => u.SIGE == borrow.UserId);
                user!.HasBorrow = false;
                _context.Book.Update(book);
                string message_email = $"<strong>Biblioteca Waldemar Falcão</strong>" +
                    $"<br />Este é um e-mail automático contendo informações sobre o seu empréstimo realizado na escola EEMTI Waldemar Falcão." +
                    $"<br/><br/><strong>Informações da Devolução</strong>:" +
                    $"<br/><strong>Livro</strong>: {book.Title}" +
                    $"<br/><strong>Autor</strong>: {book.Author}" +
                    $"<br/><strong>Data de Realização do Empréstimo</strong>: {borrow.InitialDate!.Value.Day}/{borrow.InitialDate!.Value.Month}/{borrow.InitialDate!.Value.Year}" +
                    $"<br/><strong>Data de Devolução</strong>: {DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}";

                var signed = Signature.SignData(message_email, privateKey);
                message_email = message_email + $"<br /><br />Assinatura Digital: {signed}";
                await _emailSender.SendEmail(user.Email!, $"Devolução Realizada - {book.Title}", message_email);
            }

            await _context.SaveChangesAsync();
            return _signInManager.IsSignedIn(User) ? RedirectToAction(nameof(Index)) : Redirect("/Home");
        }

        [Route("emprestimos/reservar")]
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
