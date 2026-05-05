namespace UwvLlm.App.Core.Interfaces;

public interface IAuthenticationService
{
    Task<bool> IsAuthenticatedAsync();
    Task LoginAsync(string? email, string? password);
    Task RegisterAsync(string? username, string? email, string? password, string? passwordRepeat);
}