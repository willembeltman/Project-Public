namespace SupplierProductCalculator.Entities;

public class OrderRow
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}


// Calculator output
