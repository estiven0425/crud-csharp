using crud_csharp.Models;

namespace crud_csharp.ViewModels.DTOs;

public class StockBookRequest
{
    public Book Book { get; set; }
    public int Amount { get; set; }
}