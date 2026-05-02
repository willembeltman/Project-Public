namespace UwvLlm.App.Interfaces;

public interface IAuthenticationService
{
    Task GotoRegister();
    Task Login(string? Email, string? Password);
    Task Register(string? email, string? password, string? passwordRepeat);
}