using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Models;

namespace LibraryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LibraryApp.Models.Book> Book { get; set; } = default!;
        public DbSet<LibraryApp.Models.Employer> Employer { get; set; } = default!;
        public DbSet<LibraryApp.Models.User> User { get; set; } = default!;
        public DbSet<LibraryApp.Models.Borrow> Borrow { get; set; } = default!;
        public DbSet<LibraryApp.Models.Reservation> Reservation { get; set; } = default!;
    }
}
