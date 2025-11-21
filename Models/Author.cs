using crud_csharp.Helpers;

namespace crud_csharp.Models;

public class Author
{
    public int Id { get; set; }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => _name = ValidationHelper.IsInvalidText(value)
            ? throw new ArgumentException("Name cannot be empty")
            : _name = value;
    }

    private int _age;
    public int Age
    {
        get => _age;
        set => _age = ValidationHelper.IsInvalidNumber(value)
            ? throw new ArgumentOutOfRangeException(nameof(Age), "Age cannot be negative or 0")
            : _age = value;
    }

    private string _country = string.Empty;
    public string Country
    {
        get => _country;
        set => _country = ValidationHelper.IsInvalidText(value)
            ? throw new ArgumentException("Country cannot be empty")
            : _country = value;
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => _description = ValidationHelper.IsInvalidText(value)
            ? _description = "Not provided"
            : _description = value;
    }

    public ICollection<Book> Books { get; private set; }  = new List<Book>();

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set => _email = ValidationHelper.IsInvalidText(value)
            ? throw new ArgumentException("Email cannot be empty")
            : ValidationHelper.IsValidEmail(value)
                ? _email = value
                : throw new ArgumentException("The email must be in a valid format");
    }

    private string _phone = string.Empty;
    public string Phone
    {
        get => _phone;
        set => _phone = ValidationHelper.IsInvalidPhone(value)
            ? throw new ArgumentOutOfRangeException(nameof(Phone), "Phone cannot be empty or less than 10")
            : value.All(char.IsDigit)
                ? _phone = value
                : throw new ArgumentException("The phone must be in a valid format");
    }

    public bool Status { get; set; }

    public Author()
    {}
    public Author(string name, int age, string country, string description, string email, string phone, bool status)
    {
        Name = name;
        Age = age;
        Country = country;
        Description = description;
        Email = email;
        Phone = phone;
        Status = status;
    }

    public string GetInfo()
    {
        return $"Author: {Name}," +
               $"Age: {Age}," +
               $"Country: {Country}," +
               $"Description: {Description}," +
               $"Email: {Email}," +
               $"Phone: {Phone}," +
               $"Status: {Status}";
    }
}