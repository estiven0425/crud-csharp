using crud_csharp.Data;
using crud_csharp.Models;

namespace crud_csharp.Services;

public class ServiceGenre
{
    private AppDbContext _dbContext;

    public ServiceGenre(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Genre> GetAllGenres()
    {
        return _dbContext.Genres.ToList();
    }

    public Genre? GetGenreById(int id)
    {
        return _dbContext.Genres.FirstOrDefault(genre => genre.Id == id);
    }

    public Genre AddGenre(Genre genre)
    {
        Genre newGenre = _dbContext.Genres.Add(genre).Entity;

        _dbContext.SaveChanges();

        return newGenre;
    }

    public Genre? UpdateGenre(Genre genre)
    {
        Genre? genreToUpdate = GetGenreById(genre.Id);
        if (genreToUpdate != null)
        {
            genreToUpdate.Name = genre.Name;
            genreToUpdate.Description = genre.Description;

            _dbContext.SaveChanges();
        }

        return genreToUpdate;
    }

    public Genre? DeleteGenre(Genre genre)
    {
        Genre? genreToRemove = GetGenreById(genre.Id);
        Genre? genreUnknown = _dbContext.Genres.Find(1);
        Genre? deletedGenre;

        if (genreToRemove != null && genreUnknown != null)
        {
            var booksToReassign = _dbContext.Books.Where(book => book.GenreId == genreToRemove.Id).ToList();
            foreach (var book in booksToReassign)
            {
                book.ChangeGenre(genreUnknown);
            }

            deletedGenre = _dbContext.Genres.Remove(genreToRemove).Entity;

            _dbContext.SaveChanges();
        }
        else
        {
            deletedGenre = null;
        }

        return deletedGenre;
    }

    public int? GetCountBooksGenre(Genre genre)
    {
        Genre? genreToOperate = GetGenreById(genre.Id);

        return genreToOperate?.CountBooks();
    }
}