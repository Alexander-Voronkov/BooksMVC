using BooksMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksMVC.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options):base(options) 
        {
            Database.EnsureCreated();
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(c => c.Authors)
                .WithMany(s => s.Books);
        }
    }
}
