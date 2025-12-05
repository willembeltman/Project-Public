using SupplierProductCalculator.Entities;

namespace SupplierProductCalculator;

public static class TestDataFactory
{
    public static Order CreateTestOrderInDatabase(TestDbContext db)
    {
        var supplierA = new Supplier() { Name = "Supplier A", DeliveryPrice = 5 };
        var supplierB = new Supplier() { Name = "Supplier B", DeliveryPrice = 35 };

        var product1 = new Product()
        {
            Name = "Laptop",
            SupplierProducts =
            [
                new() { Supplier = supplierA, Price = 500 },
                new() { Supplier = supplierB, Price = 450 }
            ]
        };
        var product2 = new Product()
        {
            Name = "Mouse",
            SupplierProducts =
            [
                new() { Supplier = supplierA, Price = 20 },
                new() { Supplier = supplierB, Price = 25 }
            ]
        };
        var product3 = new Product()
        {
            Name = "Keyboard",
            SupplierProducts =
            [
                new() { Supplier = supplierA, Price = 15 },
                new() { Supplier = supplierB, Price = 20 }
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
