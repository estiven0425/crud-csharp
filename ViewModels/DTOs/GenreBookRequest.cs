using crud_csharp.Models;

namespace crud_csharp.ViewModels.DTOs;

public class GenreBookRequest
{
    public Book Book { get; set; }
    public Genre Genre { get; set; }
}