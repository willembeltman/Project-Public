using UwvLlm.App.Core.Interfaces;

namespace UwvLlm.App.Services;

public class DispatcherService : IDispatcherService
{
    public void Invoke(Action action)
    {
        MainThread.BeginInvokeOnMainThread(action);
    }
}