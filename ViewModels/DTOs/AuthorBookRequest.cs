using crud_csharp.Models;

namespace crud_csharp.ViewModels.DTOs;

public class AuthorBookRequest
{
    public Book Book { get; set; }
    public Author Author { get; set; }
}