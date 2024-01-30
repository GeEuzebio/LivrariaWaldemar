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
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using LibraryApp.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace LibraryApp.Controllers
{
    public class EmployersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EmployersController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        // GET: Employers
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user!.Email;
            Employer? e = await _context.Employer.FirstOrDefaultAsync(m => m.Email == email);
            bool isAdmin = e!.IsAdmin;
            return _signInManager.IsSignedIn(User) && isAdmin ? View(await _context.Employer.ToListAsync()) : Redirect("/Home");
        }

        // GET: Employers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user!.Email;
            Employer? e = await _context.Employer.FirstOrDefaultAsync(m => m.Email == email);
            bool isAdmin = e!.IsAdmin;
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employer == null)
            {
                return NotFound();
            }

            return _signInManager.IsSignedIn(User) && isAdmin ? View(employer) : Redirect("/Home");
        }

        // GET: Employers/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user!.Email;
            Employer? e = await _context.Employer.FirstOrDefaultAsync(m => m.Email == email);
            bool isAdmin = e!.IsAdmin;
            return _signInManager.IsSignedIn(User) && isAdmin ? View() : Redirect("/Home");            
        }

        // POST: Employers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber,IsAdmin")] Employer employer)
        {
            var u = await _userManager.GetUserAsync(User);
            var email = u!.Email;
            Employer? e = await _context.Employer.FirstOrDefaultAsync(m => m.Email == email);
            bool isAdmin = e!.IsAdmin;
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { Email = employer.Email };
                await _context.SaveChangesAsync();
                var code = await _userManager.GetUserIdAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code!));
                employer.Token = code.ToString();
                _context.Update(employer);
                var callbackUrl = Url.Page(
                            "/Account/Register",
                            pageHandler: null,
                            values: new { area = "Identity", code = code },
                            protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(employer.Email!, "Create your account",
                    $"Please create your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return _signInManager.IsSignedIn(User) && isAdmin ? View(employer) : Redirect("/Home");
        }

        // GET: Employers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user!.Email;
            Employer? e = await _context.Employer.FirstOrDefaultAsync(m => m.Email == email);
            bool isAdmin = e!.IsAdmin;
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employer.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }
            return _signInManager.IsSignedIn(User) && isAdmin ? View(employer) : Redirect("/Home");
        }

        // POST: Employers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Email,PhoneNumber,Role")] Employer employer)
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user!.Email;
            Employer? e = await _context.Employer.FirstOrDefaultAsync(m => m.Email == email);
            bool isAdmin = e!.IsAdmin;
            if (id != employer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployerExists(employer.Id))
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
            return _signInManager.IsSignedIn(User) && isAdmin ? View(employer) : Redirect("/Home");
        }

        // GET: Employers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user!.Email;
            Employer? e = await _context.Employer.FirstOrDefaultAsync(m => m.Email == email);
            bool isAdmin = e!.IsAdmin;
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employer == null)
            {
                return NotFound();
            }

            return _signInManager.IsSignedIn(User) && isAdmin ? View(employer) : Redirect("/Home");
        }

        // POST: Employers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user!.Email;
            Employer? e = await _context.Employer.FirstOrDefaultAsync(m => m.Email == email);
            bool isAdmin = e!.IsAdmin;
            var employer = await _context.Employer.FindAsync(id);
            if (employer != null)
            {
                _context.Employer.Remove(employer);
                await _context.SaveChangesAsync();
            }
            
            return _signInManager.IsSignedIn(User) && isAdmin ? RedirectToAction(nameof(Index)) : Redirect("/Home");
        }

        private bool EmployerExists(long id)
        {
            return _context.Employer.Any(e => e.Id == id);
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
