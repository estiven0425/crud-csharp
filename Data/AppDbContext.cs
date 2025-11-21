using crud_csharp.Models;
using Microsoft.EntityFrameworkCore;

namespace crud_csharp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasOne(book => book.Author).WithMany(author => author.Books).HasForeignKey(book => book.AuthorId);
        modelBuilder.Entity<Book>().HasOne(book => book.Genre).WithMany(genre => genre.Books).HasForeignKey(book => book.GenreId);

        modelBuilder.Entity<Author>().HasData(new Author(
            "Unknown",
            18,
            "Unknown",
            "Default author",
            "defaultauthor@defaultauthor.com",
            "0000000000",
            false
            )
            {Id = 1}
            );
        modelBuilder.Entity<Genre>().HasData(new Genre(
            "Unknown",
            "Default genre"
            )
            {Id = 1}
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=./db/crud_csharp.db");
    }
}
