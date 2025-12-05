namespace SupplierProductCalculator.Entities;

public class SupplierProduct
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; } = null!;
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public decimal Price { get; set; }
}


// Calculator output
