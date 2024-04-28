using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using LibraryApp.Models;

namespace LibraryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CultureInfo culture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        public DbSet<LibraryApp.Models.Book> Book { get; set; } = default!;
        public DbSet<LibraryApp.Models.Employer> Employer { get; set; } = default!;
        public DbSet<LibraryApp.Models.User> User { get; set; } = default!;
        public DbSet<LibraryApp.Models.Borrow> Borrow { get; set; } = default!;
        public DbSet<LibraryApp.Models.Reservation> Reservation { get; set; } = default!;
    }
}
