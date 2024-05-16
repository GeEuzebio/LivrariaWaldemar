using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using LibraryApp.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace LibraryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
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
        public DbSet<LibraryApp.Models.Card> Card { get; set; } = default!;
        public DbSet<LibraryApp.Models.Schedule> Schedule { get; set; } = default!;
        public DbSet<LibraryApp.Models.Scheduling> Scheduling { get; set; } = default!;
    }
}
