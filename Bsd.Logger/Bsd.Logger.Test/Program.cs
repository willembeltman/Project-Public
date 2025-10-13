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