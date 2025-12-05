namespace SupplierProductCalculator;

class Program
{
    static void Main(string[] args)
    {
        // Create test database
        var db = new ApplicationDbContext();

        // Create order
        var order = TestDataFactory.CreateTestOrderInDatabase(db);

        // Calculate quotes
        var quotes = order.CalculateQuotes()
            .OrderBy(a => a.TotalPrice)
            .ToArray();

        // Show quotes
        ConsoleService.ShowQuotes(quotes);
    }   
}