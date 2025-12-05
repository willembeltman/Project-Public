namespace SupplierProductCalculator.Entities;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal DeliveryPrice { get; set; }
    public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = [];
}