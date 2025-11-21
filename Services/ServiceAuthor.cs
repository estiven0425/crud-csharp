using crud_csharp.Data;
using crud_csharp.Models;

namespace crud_csharp.Services;

public class ServiceAuthor
{
    private AppDbContext _dbContext;

    public ServiceAuthor(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Author> GetAllAuthors()
    {
        return _dbContext.Authors.ToList();
    }

    public Author? GetAuthorById(int id)
    {
        return _dbContext.Authors.FirstOrDefault(author => author.Id == id);
    }

    public Author AddAuthor(Author author)
    {
        Author newAuthor = _dbContext.Authors.Add(author).Entity;

        _dbContext.SaveChanges();

        return newAuthor;
    }

    public Author? UpdateAuthor(Author author)
    {
        Author? authorToUpdate = GetAuthorById(author.Id);
        if (authorToUpdate != null)
        {
            authorToUpdate.Name = author.Name;
            authorToUpdate.Age = author.Age;
            authorToUpdate.Country = author.Country;
            authorToUpdate.Description = author.Description;
            authorToUpdate.Email = author.Email;
            authorToUpdate.Phone = author.Phone;
            authorToUpdate.Status = author.Status;

            _dbContext.SaveChanges();
        }

        return authorToUpdate;
    }

    public Author? DeleteAuthor(Author author)
    {
        Author? authorToRemove = GetAuthorById(author.Id);
        Author? authorUnknown = _dbContext.Authors.Find(1);
        Author? deletedAuthor;

        if (authorToRemove != null && authorUnknown != null)
        {
            var booksToReassign = _dbContext.Books.Where(book => book.AuthorId == authorToRemove.Id).ToList();
            foreach (var book in booksToReassign )
            {
                book.ChangeAuthor(authorUnknown);
            }

            deletedAuthor = _dbContext.Remove(authorToRemove).Entity;

            _dbContext.SaveChanges();
        }
        else
        {
            deletedAuthor = null;
        }

        return deletedAuthor;
    }

    public string? GetInfoAuthor(Author author)
    {
        Author? authorToOperate = GetAuthorById(author.Id);

        return authorToOperate?.GetInfo();
    }
}