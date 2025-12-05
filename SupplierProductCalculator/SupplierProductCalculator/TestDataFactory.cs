using SupplierProductCalculator.Entities;

namespace SupplierProductCalculator;

public static class TestDataFactory
{
    public static Order CreateOrderInDatabase(ApplicationDbContext db)
    {
        var supplierA = new Supplier() { Name = "Supplier A", DeliveryPrice = 5m };
        var supplierB = new Supplier() { Name = "Supplier B", DeliveryPrice = 10m };

        var product1 = new Product()
        {
            Name = "Laptop",
            SupplierProducts =
            [
                new() { Supplier = supplierA, Price = 500m },
                new() { Supplier = supplierB, Price = 450m }
            ]
        };
        var product2 = new Product()
        {
            Name = "Mouse",
            SupplierProducts =
            [
                new() { Supplier = supplierA, Price = 20m },
                new() { Supplier = supplierB, Price = 25 }
            ]
        };
        var product3 = new Product()
        {
            Name = "Keyboard",
            SupplierProducts =
            [
                new() { Supplier = supplierA, Price = 15m },
                new() { Supplier = supplierB, Price = 20m }
            ]
        };

        var order = new Order()
        {
            OrderRows =
            [
                new() { Product = product1, Quantity = 1 },
                new() { Product = product2, Quantity = 2 },
                new() { Product = product3, Quantity = 2 }
            ]
        };

        db.Suppliers.Add(supplierA);
        db.Suppliers.Add(supplierB);
        db.Products.Add(product1);
        db.Products.Add(product2);
        db.Products.Add(product3);
        db.Orders.Add(order);
        db.SaveChanges();

        return order;
    }
}
