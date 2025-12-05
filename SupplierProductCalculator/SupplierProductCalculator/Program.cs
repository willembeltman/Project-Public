namespace SupplierProductCalculator;

class Program
{
    static void Main(string[] args)
    {
        // Create test database
        var db = new ApplicationDbContext();

        var order = TestDataFactory.CreateOrderInDatabase(db);

        var quotes = order.CalculateQuotes()
            .OrderBy(a => a.TotalPrice)
            .ToArray();

        ConsoleService.ShowQuotes(quotes);
    }   
}