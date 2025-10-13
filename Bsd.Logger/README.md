# Bsd.Logger

A minimal logger I made to avoid cross-referencing between my own projects.  
Use it, don’t use it — I don’t care. Just don’t expect stability.

## ⚠️ Disclaimer

**This is not a stable API.**  
Method names, behavior, or the whole thing might change at any time.  
Use at your own risk.

## 🔧 Installation

Install via NuGet:

```
dotnet add package Bsd.Logger
```

Or via the .csproj file:

```xml
<PackageReference Include="Bsd.Logger" Version="x.y.z" />
```

## ✅ Basic Usage

## 🧪 Example

```csharp
using Bsd.Logger;

using (var logger = new ConsoleLogger())
{
    // Start the logging thread, or the logger will just sit there judging you silently.
    logger.StartThread();

    // Classic "Hello, World!" — because why not.
    logger.WriteLine("Hello, World! 1");

    // Rewriting the same line a few times.
    // Useful if you're pretending to show progress or just indecisive.
    logger.ReWriteLine("Hello, World! 2");
    logger.ReWriteLine("Hello, World! 3");
    logger.ReWriteLine("Hello, World! 4");

    // A final message to commit the line to the console.
    logger.WriteLine("Hello, World! 5");

    // Throwing in an exception for good measure.
    logger.WriteException(new Exception("Test exception"));

    // And just in case someone logs an empty string... sure, why not.
    logger.WriteException("");
}
```

> Note: `StartThread()` is required if you actually want output.  
> Without it, you're just writing to the void. Like shouting into a pillow.  


## ❓ Why this exists

I got tired of pulling in logging dependencies just to print stuff to the console.  
This is a dead simple solution to make my life easier.

## ❌ What it doesn't do

- No fancy configuration
- No structured logging
- No sinks or targets
- No async buffering
- No dependency injection
- No guarantees

## 💡 Looking for a "real" logger?

Check out:

- [Serilog](https://serilog.net/)
- [NLog](https://nlog-project.org/)
- [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging)

## 📜 License

MIT. Do whatever you want.
