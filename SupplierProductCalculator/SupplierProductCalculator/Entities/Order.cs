namespace SupplierProductCalculator.Entities;

public class Order
{
    public int Id { get; set; }
    public virtual ICollection<OrderRow> OrderRows { get; set; } = [];
}
