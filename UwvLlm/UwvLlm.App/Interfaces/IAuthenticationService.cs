namespace UwvLlm.App.Interfaces;

public interface IAuthenticationService
{
    Task<bool> IsAuthenticatedAsync();
    Task LoginAsync(string? Email, string? Password);
    Task RegisterAsync(string? username, string? email, string? password, string? passwordRepeat);
}