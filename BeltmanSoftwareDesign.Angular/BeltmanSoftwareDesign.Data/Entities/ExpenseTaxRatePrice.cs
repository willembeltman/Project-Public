using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class ExpenseTaxRatePrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long? ExpenseId { get; set; }
        public long? TaxRateId { get; set; }

        public double Price { get; set; }
        //public virtual Expense Expense { get; set; }
        //public virtual TaxRate TaxRate { get; set; }
        //[NotMapped]
        //public double Tax => Price * TaxRate.Percentage / 100;
        //[NotMapped]
        //public double ConsumentenRatePrice => Price + Tax;

        //public override string ToString()
        //    => $"{Expense.Date.ToShortDateString()} €{ConsumentenRatePrice.ToString("F2")} {Expense.Supplier}: {Expense.Description}";
    
    }
}
