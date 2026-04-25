    using var cts = new CancellationTokenSource();
    var consoleTask = StartConsole(cts.Token);
    var serverTask = StartServer(cts.Token);
    await Task.WhenAny(consoleTask, serverTask);
    cts.Cancel();
    // Wait till all tasks finish and throw exception if any task throws
    await Task.WhenAll(consoleTask, serverTask); 

    async Task StartConsole(CancellationToken ct)
    {
        var userWantsToQuit = false;
        while (userWantsToQuit == false)
        {
            // ... do awaitable work
            ct.ThrowIfCancellationRequested();
        }
    }

    async Task StartServer(CancellationToken ct)
    {
        var serverWantsToQuit = false;
        while (serverWantsToQuit == false)
        {
            // ... do awaitable work
            ct.ThrowIfCancellationRequested();
        }
    }