using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public long? ExpenseTypeId { get; set; }
        public virtual ExpenseType? ExpenseType { get; set; }
        public long? ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public long? SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public long? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DateKapot { get; set; }
        public bool IsPayedInCash { get; set; }
        public double Restwaarde { get; set; }

        public virtual ICollection<ExpenseAttachment>? ExpenseAttachments { get; set; } = new List<ExpenseAttachment>();
        public virtual ICollection<ExpensePrice>? ExpensePrices { get; set; } = new List<ExpensePrice>();
        public virtual ICollection<ExpenseProduct>? ExpenseProducts { get; set; } = new List<ExpenseProduct>();
        public virtual ICollection<BankStatementExpense>? BankStatementExpenses { get; set; } = new List<BankStatementExpense>();


        //[NotMapped]
        //public byte Quarter => Convert.ToByte(Math.Ceiling(Convert.ToDouble(Date.Month) / 3));
        //[NotMapped]
        //public double Price => ExpenseRatePriceen.Sum(a => a.Price);
        //[NotMapped]
        //public double Tax => ExpenseRatePriceen.Sum(a => a.Tax);
        //[NotMapped]
        //public double ConsumentenRatePrice => ExpenseRatePriceen.Sum(a => a.ConsumentenRatePrice);
        //[NotMapped]
        //public double IsPayed => BankStatementExpenses.Sum(a => a.BankStatement.DebitRatePrice);

        //public override string ToString()
        //    => $"{Date.ToShortDateString()} €{ConsumentenRatePrice.ToString("F2")} {Supplier}: {Description}";

    }
}
