using BeltmanSoftwareDesign.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeltmanSoftwareDesign.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<BankStatement> BankStatements { get; set; }
        public DbSet<BankStatementExpense> BankStatementExpenses { get; set; }
        public DbSet<BankStatementInvoice> BankStatementInvoices { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<ClientBearer> ClientBearers { get; set; }
        public DbSet<ClientDevice> ClientDevices { get; set; }
        public DbSet<ClientDeviceProperty> ClientDeviceProperties { get; set; }
        public DbSet<ClientIpAddress> ClientLocations { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseAttachment> ExpenseAttachments { get; set; }
        public DbSet<ExpenseProduct> ExpenseProducts { get; set; }
        public DbSet<ExpensePrice> ExpensePrices { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceAttachment> InvoiceAttachments { get; set; }
        public DbSet<InvoiceEmail> InvoiceEmails { get; set; }
        public DbSet<InvoiceProduct> InvoiceProducts { get; set; }
        public DbSet<InvoiceRow> InvoiceRows { get; set; }
        public DbSet<InvoiceType> InvoiceTypes { get; set; }
        public DbSet<InvoiceTransaction> InvoiceTransactions { get; set; }
        public DbSet<InvoiceWorkorder> InvoiceWorkorders { get; set; }
        public DbSet<InvoicePrice> InvoicePrices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Residence> Residences { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<TrafficRegistration> TrafficRegistrations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Workorder> Workorders { get; set; }
        public DbSet<WorkorderAttachment> WorkorderAttachments { get; set; }



        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionParameter> TransactionParameters { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<TransactionLogParameter> TransactionLogParameters { get; set; }


        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentAttachment> DocumentAttachments { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Technology> Technologies { get; set; }
        public DbSet<TechnologyAttachment> TechnologyAttachments { get; set; }

        public DbSet<Experience> Experiences { get; set; }
        public DbSet<ExperienceAttachment> ExperienceAttachments { get; set; }
        public DbSet<ExperienceTechnology> ExperienceTechnologies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientBearer>()
                .HasOne(cb => cb.ClientDevice)
                .WithMany(cd => cd.ClientBearers)
                .HasForeignKey(cb => cb.ClientDeviceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClientBearer>()
                .HasOne(cb => cb.ClientIpAddress)
                .WithMany(cd => cd.ClientBearers)
                .HasForeignKey(cb => cb.ClientIpAddressId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClientBearer>()
                .HasOne(cb => cb.User)
                .WithMany(u => u.ClientBearers)
                .HasForeignKey(cb => cb.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClientDeviceProperty>()
                .HasOne(p => p.ClientDevice)
                .WithMany(d => d.ClientDeviceProperties)
                .HasForeignKey(p => p.ClientDeviceId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<User>()
                .HasOne(u => u.CurrentCompany)
                .WithMany(c => c.CurrentUsers)
                .HasForeignKey(p => p.CurrentCompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CompanyUser>()
                .HasOne(u => u.Company)
                .WithMany(c => c.CompanyUsers)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CompanyUser>()
                .HasOne(u => u.User)
                .WithMany(c => c.CompanyUsers)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Workorder>()
                .HasOne(u => u.Rate)
                .WithMany(c => c.Workorders)
                .HasForeignKey(p => p.RateId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Workorder>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Workorders)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Workorder>()
                .HasOne(u => u.Project)
                .WithMany(c => c.Workorders)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Workorder>()
                .HasOne(u => u.Customer)
                .WithMany(c => c.Workorders)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InvoiceWorkorder>()
                .HasOne(u => u.Workorder)
                .WithMany(c => c.InvoiceWorkorders)
                .HasForeignKey(p => p.WorkorderId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<InvoiceWorkorder>()
                .HasOne(u => u.Invoice)
                .WithMany(c => c.InvoiceWorkorders)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkorderAttachment>()
                .HasOne(u => u.Workorder)
                .WithMany(c => c.WorkorderAttachments)
                .HasForeignKey(p => p.WorkorderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasOne(u => u.Country)
                .WithMany(c => c.Customers)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Customers)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasOne(u => u.Customer)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Project>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
                .HasOne(u => u.Country)
                .WithMany(c => c.Companies)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Invoice>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Invoices)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invoice>()
                .HasOne(u => u.InvoiceType)
                .WithMany(c => c.Invoices)
                .HasForeignKey(p => p.InvoiceTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Invoice>()
                .HasOne(u => u.Project)
                .WithMany(c => c.Invoices)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Invoice>()
                .HasOne(u => u.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Invoice>()
                .HasOne(u => u.IsPayedInCash_By_CompanyUser)
                .WithMany(c => c.Invoices_SetToPayed)
                .HasForeignKey(p => p.IsPayedInCash_By_CompanyUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InvoiceType>()
                .HasOne(u => u.Company)
                .WithMany(c => c.InvoiceTypes)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaxRate>()
                .HasOne(u => u.Company)
                .WithMany(c => c.TaxRates)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaxRate>()
                .HasOne(u => u.Country)
                .WithMany(c => c.TaxRates)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rate>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Rates)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rate>()
                .HasOne(u => u.TaxRate)
                .WithMany(c => c.Rates)
                .HasForeignKey(p => p.TaxRateId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TransactionLogParameter>()
                .HasOne(u => u.TransactionLog)
                .WithMany(c => c.TransactionLogParameters)
                .HasForeignKey(p => p.TransactionLogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionParameter>()
                .HasOne(u => u.Transaction)
                .WithMany(c => c.TransactionParameters)
                .HasForeignKey(p => p.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionLog>()
                .HasOne(u => u.Transaction)
                .WithMany(c => c.TransactionLogs)
                .HasForeignKey(p => p.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Transactions)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BankStatement>()
                .HasOne(u => u.Company)
                .WithMany(c => c.BankStatements)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpenseType>()
                .HasOne(u => u.Company)
                .WithMany(c => c.ExpenseTypes)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Expense>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Expenses)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Expense>()
                .HasOne(u => u.ExpenseType)
                .WithMany(c => c.Expenses)
                .HasForeignKey(p => p.ExpenseTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Expense>()
                .HasOne(u => u.Project)
                .WithMany(c => c.Expenses)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Project>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Project>()
                .HasOne(u => u.Customer)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Expense>()
                .HasOne(u => u.Supplier)
                .WithMany(c => c.Expenses)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExpenseAttachment>()
                .HasOne(u => u.Expense)
                .WithMany(c => c.ExpenseAttachments)
                .HasForeignKey(p => p.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Suppliers)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Supplier>()
                .HasOne(u => u.Country)
                .WithMany(c => c.Suppliers)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Expense>()
                .HasOne(u => u.Customer)
                .WithMany(c => c.Expenses)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Product>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(u => u.Supplier)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InvoiceProduct>()
                .HasOne(u => u.Product)
                .WithMany(c => c.InvoiceProducts)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<InvoiceProduct>()
                .HasOne(u => u.Invoice)
                .WithMany(c => c.InvoiceProducts)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InvoiceAttachment>()
                .HasOne(u => u.Invoice)
                .WithMany(c => c.InvoiceAttachments)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpensePrice>()
                .HasOne(u => u.Expense)
                .WithMany(c => c.ExpensePrices)
                .HasForeignKey(p => p.ExpenseId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ExpensePrice>()
                .HasOne(u => u.TaxRate)
                .WithMany(c => c.ExpensePrices)
                .HasForeignKey(p => p.TaxRateId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<InvoiceRow>()
                .HasOne(u => u.TaxRate)
                .WithMany(c => c.InvoiceRows)
                .HasForeignKey(p => p.TaxRateId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<InvoiceRow>()
                .HasOne(u => u.Invoice)
                .WithMany(c => c.InvoiceRows)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductPrice>()
                .HasOne(u => u.Product)
                .WithMany(c => c.ProductPrices)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductPrice>()
                .HasOne(u => u.TaxRate)
                .WithMany(c => c.ProductPrices)
                .HasForeignKey(p => p.TaxRateId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Setting>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Settings)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpenseProduct>()
                .HasOne(u => u.Expense)
                .WithMany(c => c.ExpenseProducts)
                .HasForeignKey(p => p.ExpenseId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ExpenseProduct>()
                .HasOne(u => u.Product)
                .WithMany(c => c.ExpenseProducts)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InvoiceEmail>()
                .HasOne(u => u.Invoice)
                .WithMany(c => c.InvoiceEmails)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceTransaction>()
                .HasOne(u => u.Invoice)
                .WithMany(c => c.InvoiceTransactions)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<InvoiceTransaction>()
                .HasOne(u => u.Transaction)
                .WithMany(c => c.InvoiceTransactions)
                .HasForeignKey(p => p.TransactionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BankStatementExpense>()
                .HasOne(u => u.BankStatement)
                .WithMany(c => c.BankStatementExpenses)
                .HasForeignKey(p => p.BankStatementId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BankStatementExpense>()
                .HasOne(u => u.Expense)
                .WithMany(c => c.BankStatementExpenses)
                .HasForeignKey(p => p.ExpenseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BankStatementInvoice>()
                .HasOne(u => u.BankStatement)
                .WithMany(c => c.BankStatementInvoices)
                .HasForeignKey(p => p.BankStatementId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BankStatementInvoice>()
                .HasOne(u => u.Invoice)
                .WithMany(c => c.BankStatementInvoices)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<InvoicePrice>()
                .HasOne(u => u.TaxRate)
                .WithMany(c => c.InvoicePrices)
                .HasForeignKey(p => p.TaxRateId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<InvoicePrice>()
                .HasOne(u => u.Invoice)
                .WithMany(c => c.InvoicePrices)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Residence>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Residences)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TrafficRegistration>()
                .HasOne(u => u.Company)
                .WithMany(c => c.TrafficRegistrations)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Document>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Documents)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Document>()
                .HasOne(u => u.DocumentType)
                .WithMany(c => c.Documents)
                .HasForeignKey(p => p.DocumentTypeId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Document>()
                .HasOne(u => u.Project)
                .WithMany(c => c.Documents)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Document>()
                .HasOne(u => u.Supplier)
                .WithMany(c => c.Documents)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Document>()
                .HasOne(u => u.Customer)
                .WithMany(c => c.Documents)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DocumentType>()
                .HasOne(u => u.Company)
                .WithMany(c => c.DocumentTypes)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DocumentAttachment>()
                .HasOne(u => u.Document)
                .WithMany(c => c.DocumentAttachments)
                .HasForeignKey(p => p.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
