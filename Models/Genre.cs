using crud_csharp.Helpers;

namespace crud_csharp.Models;

public class Genre
{
    public int Id { get; set; }

    private  string _name =  string.Empty;
    public string Name
    {
        get => _name;
        set => _name = ValidationHelper.IsInvalidText(value)
            ? throw new ArgumentOutOfRangeException(nameof(Name), "Name cannot be empty")
            : _name = value;
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => _description = ValidationHelper.IsInvalidText(value)
            ? throw new ArgumentOutOfRangeException(nameof(Description), "Description cannot be empty")
            : _description = value;
    }

    public ICollection<Book> Books { get; private set; } = new List<Book>();

    public Genre()
    {}
    public Genre(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public int CountBooks()
    {
        return Books.Count;
    }
}