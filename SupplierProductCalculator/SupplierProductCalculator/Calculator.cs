namespace SupplierProductCalculator;

// Database
public record Order(IEnumerable<OrderRow> Rows);
public record OrderRow(int Quantity, Product Product);
public record Product(IEnumerable<SupplierProduct> SupplierProducts);
public record Supplier(decimal DeliveryPrice, IEnumerable<SupplierProduct> SupplierProducts);
public record SupplierProduct(Supplier Supplier, Product Product, decimal Price);

// Calculator output
public record Quote(SupplierOrder[] SupplierOrders, decimal TotalPrice);
public record SupplierOrder(Supplier Supplier, SupplierOrderRow[] SupplierOrderRows, decimal SubTotalPrice);
public record SupplierOrderRow(OrderRow OrderRow, SupplierProduct SupplierProduct, decimal SubTotalPrice);

// Calculator
public static class OrderExtention
{
    public static IEnumerable<Quote> CalculateQuotes(this Order order)
    {
        // Create OrderRowWithSupplierProducts "Bits"
        var orderRowsWithSupplierProducts = order.Rows
            .Where(row => row.Quantity > 0)
            .Select(row => new OrderRowWithSupplierProducts(row, [.. row.Product.SupplierProducts]))
            .ToArray();

        if (orderRowsWithSupplierProducts.Length == 0)
            yield break;

        // Calculate number of quote combinations
        var quotesCount = 1;
        try
        {
            foreach (var orderRowWithSupplierProducts in orderRowsWithSupplierProducts)
                quotesCount *= orderRowWithSupplierProducts.SupplierProducts.Length;
        }
        catch (OverflowException ex)
        {
            throw new Exception(
                "Internal exception: " +
                "The number of combinations exceeds maximum int32 value. " +
                "Your order is too big to use the algorithm.", ex);
        }

        // Yield each combination
        for (var i = 0; i < quotesCount; i++)
        {
            // Create quote with current supplier-product combination
            var supplierOrderRows = orderRowsWithSupplierProducts
                    .Select(a => new SupplierOrderRow(
                        a.OrderRow, 
                        a.SelectedSupplierProduct,
                        a.OrderRow.Quantity * a.SelectedSupplierProduct.Price))
                    .ToArray();
            var supplierOrders = supplierOrderRows
                    .GroupBy(a => a.SupplierProduct.Supplier)
                    .Select(supplierGroup => new SupplierOrder(
                        supplierGroup.Key, 
                        [.. supplierGroup],
                        supplierGroup.Key.DeliveryPrice + supplierGroup.Sum(a => a.SubTotalPrice)))
                    .ToArray();
            var quote = new Quote(
                supplierOrders,
                supplierOrders.Sum(a => a.SubTotalPrice));

            yield return quote;

            // Select next quote combination
            foreach (var row in orderRowsWithSupplierProducts)
            {
                var rollover = row.SelectNextProduct();
                if (rollover == false)
                    break;
            }
        }
    }
    private record OrderRowWithSupplierProducts(OrderRow OrderRow, SupplierProduct[] SupplierProducts)
    {
        public int SelectedSupplierProductIndex { get; private set; }
        public SupplierProduct SelectedSupplierProduct => SupplierProducts[SelectedSupplierProductIndex];
        public bool SelectNextProduct()
        {
            if (SupplierProducts.Length == 0)
            {
                throw new Exception(
                    "Internal exception: " +
                    "This order contains product with no suppliers to choose from.");
            }
            SelectedSupplierProductIndex++;
            if (SelectedSupplierProductIndex >= SupplierProducts.Length)
            {
                SelectedSupplierProductIndex = 0;
                return true;
            }
            return false;
        }
    }
}