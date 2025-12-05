namespace SupplierProductCalculator;

class Program
{
    static void Main(string[] args)
    {
        // Create test database
        var db = new TestDbContext();

        // Create order
        var testOrder = TestDataFactory.CreateTestOrderInDatabase(db);

        // Calculate quotes
        var quotes = testOrder.CalculateQuotes()
            .OrderBy(a => a.TotalPrice)
            .ToArray();

        // Show quotes
        ConsoleOutput.ShowQuotes(quotes);
    }   
}