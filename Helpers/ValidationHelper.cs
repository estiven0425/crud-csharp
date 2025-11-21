namespace crud_csharp.Helpers;

public static class ValidationHelper
{
    public static bool IsInvalidText(string text) => string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
    public static bool IsValidEmail(string email) => email.Contains('@');
    public static bool IsInvalidNumber(int number) => number <= 0;
    public static bool IsInvalidPrice(decimal price) => price < 0;
    public static bool IsInvalidAmount(int amount) => amount < 0;
    public static bool IsInvalidPhone(string phone) => string.IsNullOrEmpty(phone) || string.IsNullOrWhiteSpace(phone) || phone.Length < 10;
}