namespace SupplierProductCalculator.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = [];
    public virtual ICollection<OrderRow> OrderRows { get; set; } = [];
}