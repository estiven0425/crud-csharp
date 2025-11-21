using crud_csharp.Data;
using crud_csharp.Models;
using Microsoft.EntityFrameworkCore;

namespace crud_csharp.Services;

public class ServiceBook
{
    private AppDbContext _dbContext;

    public ServiceBook(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Book> GetAllBooks()
    {
        return _dbContext.Books
            .Include(book => book.Author)
            .Include(book => book.Genre)
            .ToList();
    }

    public Book? GetBookById(int id)
    {
        return _dbContext.Books
            .Include(book => book.Author)
            .Include(book => book.Genre)
            .FirstOrDefault(book => book.Id == id);
    }

    public Book? GetBookByTitle(string title)
    {
        return _dbContext.Books
            .Include(book => book.Author)
            .Include(book => book.Genre)
            .FirstOrDefault(book => book.Title == title);
    }

    public Book AddBook(Book book)
    {
        Book newBook = _dbContext.Books.Add(book).Entity;

        _dbContext.SaveChanges();

        return newBook;
    }

    public Book? UpdateBook(Book book)
    {
        Book? bookToUpdate = GetBookById(book.Id);
        if (bookToUpdate != null)
        {
            bookToUpdate.Title = book.Title;
            bookToUpdate.Country = book.Country;
            bookToUpdate.Price = book.Price;
            bookToUpdate.Stock = book.Stock;

            bookToUpdate.ChangeAuthor(book.Author);
            bookToUpdate.ChangeGenre(book.Genre);

            _dbContext.SaveChanges();
        }

        return bookToUpdate;
    }

    public Book? DeleteBook(Book book)
    {
        Book? bookToRemove = GetBookById(book.Id);
        Book? deletedBook = bookToRemove != null
            ? _dbContext.Books.Remove(bookToRemove).Entity
            : null;

        _dbContext.SaveChanges();

        return deletedBook;
    }

    public Book? RestockBook(Book book, int amount)
    {
        Book? bookToOperate = GetBookById(book.Id);
        if (bookToOperate != null)
        {
            bookToOperate.Restock(amount);
            _dbContext.SaveChanges();
        }

        return bookToOperate;
    }

    public Book? SellBook(Book book, int amount)
    {
        Book? bookToOperate = GetBookById(book.Id);
        if (bookToOperate != null)
        {
            bookToOperate.Sell(amount);
            _dbContext.SaveChanges();
        }

        return bookToOperate;
    }

    public Book? ChangeAuthorBook(Book book, Author author)
    {
        Book? bookToOperate = GetBookById(book.Id);
        if (bookToOperate != null)
        {
            bookToOperate.ChangeAuthor(author);
            _dbContext.SaveChanges();
        }

        return bookToOperate;
    }

    public Book? ChangeGenreBook(Book book, Genre genre)
    {
        Book? bookToOperate = GetBookById(book.Id);
        if (bookToOperate != null)
        {
            bookToOperate.ChangeGenre(genre);
            _dbContext.SaveChanges();
        }

        return bookToOperate;
    }
}