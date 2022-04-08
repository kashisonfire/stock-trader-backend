namespace StockTrader.Domain.Models
{
    public enum RegistrationResult
    {
        Fail,
        Success,
        PasswordsDoNotMatch,
        EmailAlreadyExists,
        UsernameAlreadyExists
    }
}
