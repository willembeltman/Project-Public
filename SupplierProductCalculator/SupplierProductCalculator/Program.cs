namespace SupplierProductCalculator;

class Program
{
    static void Main(string[] args)
    {
        var db = new ApplicationDbContext();
        db.Database.EnsureCreated();

        var order = TestDataFactory.CreateOrderInDatabase(db);

        var quotes = order.CalculateQuotes()
            .OrderBy(a => a.TotalPrice)
            .ToArray();

        ShowQuotes(quotes);
    }
    
    private static void ShowQuotes(Quote[] quotes)
    {
        int idx = 1;
        Console.WriteLine($"Generated {quotes.Length} quotes:\n");
        foreach (var quote in quotes)
        {
            Console.WriteLine($"--- QUOTE {idx++} ---");
            foreach (var so in quote.SupplierOrders)
            {
                Console.WriteLine($"Supplier: {so.Supplier.Name}");
                Console.WriteLine($"  Delivery: {so.Supplier.DeliveryPrice:F2}eur");
                foreach (var row in so.SupplierOrderRows)
                {
                    Console.WriteLine($"  {row.OrderRow.Product.Name} x{row.OrderRow.Quantity} @ {row.SupplierProduct.Price:F2}eur = {row.SubTotalPrice:F2}eur");
                }
                Console.WriteLine($"  Subtotal: {so.SubTotalPrice:F2}eur");
                Console.WriteLine();
            }
            Console.WriteLine($"Total Quote Price: {quote.TotalPrice:F2}eur");
            Console.WriteLine();
        }
    }
}