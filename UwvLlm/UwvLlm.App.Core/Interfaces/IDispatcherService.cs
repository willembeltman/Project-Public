namespace UwvLlm.App.Core.Interfaces;

public interface IDispatcherService
{
    void Invoke(Action action);
}
