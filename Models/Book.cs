using crud_csharp.Helpers;

namespace crud_csharp.Models;

public class Book
{
    public int Id { get; set; }

    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => _title = ValidationHelper.IsInvalidText(value)
            ? throw new ArgumentException("Title cannot be empty")
            : _title = value;
    }

    private string _country = string.Empty;
    public string Country
    {
        get => _country;
        set => _country = ValidationHelper.IsInvalidText(value)
            ? throw new ArgumentException("Country cannot be empty")
            : _country = value;
    }

    private int _genreId;
    public int GenreId
    {
        get => _genreId;
        protected set => _genreId = ValidationHelper.IsInvalidNumber(value)
            ? throw new ArgumentOutOfRangeException(nameof(GenreId), "0 is not permitted")
            : _genreId = value;
    }
    public Genre Genre { get; private set; }

    private int _authorId;
    public int AuthorId
    {
        get => _authorId;
        protected set => _authorId = ValidationHelper.IsInvalidNumber(value)
            ? throw new ArgumentOutOfRangeException(nameof(AuthorId), "0 is not permitted")
            : _authorId = value;
    }
    public Author Author { get; private set; }

    private decimal _price;
    public decimal Price
    {
        get => _price;
        set => _price = ValidationHelper.IsInvalidPrice(value)
            ? throw new ArgumentOutOfRangeException(nameof(Price), "The price cannot be less than 0")
            : _price = value;

    }

    private int _stock;
    public int Stock
    {
        get => _stock;
        set => _stock = ValidationHelper.IsInvalidAmount(value)
            ? throw new ArgumentOutOfRangeException(nameof(Stock), "The stock cannot be less than 0")
            : _stock = value;

    }

    public string Status => IsAvailable();

    public Book()
    {}
    public Book(string title, string country, Genre genre, Author author, decimal price, int stock)
    {
        Title = title;
        Country = country;
        GenreId = genre.Id;
        Genre = genre;
        AuthorId = author.Id;
        Author = author;
        Price = price;
        Stock = stock;
    }

    private string IsAvailable()
    {
        string status = Stock > 0
            ? "Available"
            : "Not available";

        return $"{status}: ({Stock})";
    }

    public void Restock(int amount)
    {
        if (amount > 0)
        {
            Stock += amount;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "It can only increase the stock");
        }
    }

    public void Sell(int amount)
    {
        if (amount <= Stock && amount >= 0)
        {
            Stock -= amount;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Insufficient stock");
        }
    }

    public void ChangeAuthor(Author? author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author),  "Author cannot be null");
        }
        else
        {
            AuthorId = author.Id;
            Author = author;
        }
    }

    public void ChangeGenre(Genre? genre)
    {
        if (genre == null)
        {
            throw new ArgumentNullException(nameof(genre), "Genre cannot be null");
        }
        else
        {
            GenreId = genre.Id;
            Genre = genre;
        }
    }
}