using BeltmanSoftwareDesign.Data.Attributes;
using BeltmanSoftwareDesign.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class BankStatement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public string VolgNr { get; set; } = string.Empty;
        public CreditTypeEnum CreditType { get; set; }
        public BankEnum? Bank { get; set; }
        public string EigenRekeningNumber { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string TegenRekeningNumber { get; set; } = string.Empty;
        public string TegenName { get; set; } = string.Empty;
        public string Description1 { get; set; } = string.Empty;
        public string Description2 { get; set; } = string.Empty;
        public string Description3 { get; set; } = string.Empty;
        public double Saldo { get; set; }
        public double KleineOndernemersRegeling { get; set; }
        public bool IsHuur { get; set; }
        public bool IsBelastingBTW { get; set; }
        public bool IsBelasting { get; set; }

        public virtual ICollection<BankStatementInvoice>? BankStatementInvoices { get; set; }
        public virtual ICollection<BankStatementExpense>? BankStatementExpenses { get; set; }

        [Name]
        [NotMapped]
        public string Description => Description1 + Environment.NewLine + Description2 + Environment.NewLine + Description3;
        [NotMapped]
        public double CreditRatePrice => CreditType == CreditTypeEnum.Credit ? Price : Price * -1;
        [NotMapped]
        public double DebitRatePrice => CreditType == CreditTypeEnum.Debit ? Price : Price * -1;

        public override string ToString()
        {
            return $"{VolgNr} {Date.ToShortDateString()} {CreditRatePrice.ToString("F2")}: {Description}";
        }
    }
}
