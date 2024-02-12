using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeltmanSoftwareDesign.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankStatementExpenses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankStatementId = table.Column<long>(type: "bigint", nullable: true),
                    ExpenseId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStatementExpenses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BankStatementInvoices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankStatementId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStatementInvoices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BankStatements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    VolgNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditType = table.Column<int>(type: "int", nullable: false),
                    Bank = table.Column<int>(type: "int", nullable: false),
                    EigenRekeningNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    TegenRekeningNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TegenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Saldo = table.Column<double>(type: "float", nullable: false),
                    KleineOndernemersRegeling = table.Column<double>(type: "float", nullable: false),
                    IsHuur = table.Column<bool>(type: "bit", nullable: false),
                    IsBelastingBTW = table.Column<bool>(type: "bit", nullable: false),
                    IsBelasting = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStatements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangePaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangePaths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Changes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangesetId = table.Column<int>(type: "int", nullable: true),
                    ChangeSetPathId = table.Column<int>(type: "int", nullable: true),
                    ChangeType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Changes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    RealChangesetId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientDevices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDevices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ClientLocations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IpAddress = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLocations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentAttachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileMimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileMD5 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAttachments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseAttachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailUniqueId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileMimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileMD5 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EmailDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailIndex = table.Column<int>(type: "int", nullable: false),
                    EmailSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailHtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailTextBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hidden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseAttachments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseProducts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseProducts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    ExpenseTypeId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateKapot = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPayedInCash = table.Column<bool>(type: "bit", nullable: false),
                    Restwaarde = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTaxRatePrices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseId = table.Column<long>(type: "bigint", nullable: true),
                    TaxRateId = table.Column<long>(type: "bigint", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTaxRatePrices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTypes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVolledigeKosten = table.Column<bool>(type: "bit", nullable: false),
                    IsRepresentatieKosten = table.Column<bool>(type: "bit", nullable: false),
                    IsHalfTellen = table.Column<bool>(type: "bit", nullable: false),
                    BedrijfsKostenType = table.Column<int>(type: "int", nullable: false),
                    AfschrijfKostenType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceAttachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExperienceId = table.Column<long>(type: "bigint", nullable: false),
                    FileMimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileMD5 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Spotlight = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceAttachments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Stop = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AmountUur = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceTechnologies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExperienceId = table.Column<long>(type: "bigint", nullable: true),
                    TechnologyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceTechnologies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceAttachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsInvoicePDF = table.Column<bool>(type: "bit", nullable: false),
                    IsWorkorderPDF = table.Column<bool>(type: "bit", nullable: false),
                    FileMimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileMD5 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceAttachments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceEmails",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    EmailFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateVerzonden = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceGezien = table.Column<bool>(type: "bit", nullable: false),
                    DateInvoiceGezien = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailGezien = table.Column<bool>(type: "bit", nullable: false),
                    DateEmailGezien = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceEmails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceProducts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceProducts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TaxRateId = table.Column<long>(type: "bigint", nullable: true),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Residences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WOZWaarde = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueDouble = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postalcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RekeningNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publiekelijk = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsProgrammeerTaal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TechnologyAttachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnologyId = table.Column<long>(type: "bigint", nullable: false),
                    FileMimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileMD5 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologyAttachments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TrafficRegistrations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KilometerStart = table.Column<double>(type: "float", nullable: false),
                    KilometerStop = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficRegistrations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionLogs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    TransactieId = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_Amount = table.Column<int>(type: "int", nullable: false),
                    PaymentDetails_Created = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_CurrencyAmount = table.Column<int>(type: "int", nullable: false),
                    PaymentDetails_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_Exchange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_IdentifierHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_IdentifierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_IdentifierPublic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_Modified = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaidAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaidAttemps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaidBase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaidCosts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaidCostsVat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaidCurrencyAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaidCurreny = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaidDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaymentMethodDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaymentMethodId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaymentMethodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_PaymentOptionId = table.Column<int>(type: "int", nullable: false),
                    PaymentDetails_PaymentOptionSubId = table.Column<int>(type: "int", nullable: false),
                    PaymentDetails_PaymentProfileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_ProcessTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_Secure = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDetails_SecureStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_ServiceDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_ServiceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_StateDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_StateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDetails_Storno = table.Column<bool>(type: "bit", nullable: false),
                    Connection_BrowserData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_LocationLat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_LocationLon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_MerchantCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_MerchantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_OrderIP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_OrderReturnUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connection_Trust = table.Column<int>(type: "int", nullable: true),
                    Request_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Request_Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Request_Result = table.Column<bool>(type: "bit", nullable: false),
                    StatsDetails_Extra1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatsDetails_Extra2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatsDetails_Extra3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatsDetails_Info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatsDetails_Object = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatsDetails_PaymentSessionId = table.Column<int>(type: "int", nullable: true),
                    StatsDetails_PromotorId = table.Column<int>(type: "int", nullable: true),
                    StatsDetails_Tool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StornoDetails_BankAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StornoDetails_bic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StornoDetails_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StornoDetails_Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StornoDetails_EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StornoDetails_IBAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StornoDetails_Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StornoDetails_StornoAmount = table.Column<int>(type: "int", nullable: true),
                    StornoDetails_StornoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLogs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ConsumentenPrice = table.Column<double>(type: "float", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionPaymentReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionPaymentURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionPopupAllowed = table.Column<bool>(type: "bit", nullable: true),
                    RequestCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestResult = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPayed = table.Column<bool>(type: "bit", nullable: false),
                    DatePayed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BetaalAnnuleringsDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ClientDeviceProperties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientDeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDeviceProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientDeviceProperties_ClientDevices_ClientDeviceId",
                        column: x => x.ClientDeviceId,
                        principalTable: "ClientDevices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Postalcode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Place = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BtwNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    KvkNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PayNL_ApiToken = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PayNL_ServiceId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DateInsert = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDelete = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postalcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publiekelijk = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Customers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceTypes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTypes", x => x.id);
                    table.ForeignKey(
                        name: "FK_InvoiceTypes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaxRates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRates", x => x.id);
                    table.ForeignKey(
                        name: "FK_TaxRates_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaxRates_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CurrentCompanyId = table.Column<long>(type: "bigint", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LockedOut = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CurrentCompanyId",
                        column: x => x.CurrentCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publiekelijk = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.id);
                    table.ForeignKey(
                        name: "FK_Projects_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "WorkRates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TaxRateId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Tax = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRates", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkRates_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkRates_TaxRates_TaxRateId",
                        column: x => x.TaxRateId,
                        principalTable: "TaxRates",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ClientBearers",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ClientDeviceId = table.Column<long>(type: "bigint", nullable: true),
                    ClientIpAddressId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientBearers", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClientBearers_ClientDevices_ClientDeviceId",
                        column: x => x.ClientDeviceId,
                        principalTable: "ClientDevices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientBearers_ClientLocations_ClientIpAddressId",
                        column: x => x.ClientIpAddressId,
                        principalTable: "ClientLocations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientBearers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CurrentProjectId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentCustomerId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentSupplierId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentExpenseTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentInvoiceTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentRateId = table.Column<long>(type: "bigint", nullable: true),
                    Eigenaar = table.Column<bool>(type: "bit", nullable: false),
                    Admin = table.Column<bool>(type: "bit", nullable: false),
                    Actief = table.Column<bool>(type: "bit", nullable: false),
                    DateInsert = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDelete = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workorders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    RateId = table.Column<long>(type: "bigint", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Stop = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountUur = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workorders", x => x.id);
                    table.ForeignKey(
                        name: "FK_Workorders_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Workorders_Customers_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Customers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Workorders_Projects_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Projects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Workorders_WorkRates_RateId",
                        column: x => x.RateId,
                        principalTable: "WorkRates",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceTypeId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    TaxRateId = table.Column<long>(type: "bigint", nullable: true),
                    SetToPayed_By_CompanyUserId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatePrice = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPayedInCash = table.Column<bool>(type: "bit", nullable: false),
                    IsPayed = table.Column<bool>(type: "bit", nullable: false),
                    DatePayed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.id);
                    table.ForeignKey(
                        name: "FK_Invoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_CompanyUsers_SetToPayed_By_CompanyUserId",
                        column: x => x.SetToPayed_By_CompanyUserId,
                        principalTable: "CompanyUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Invoices_InvoiceTypes_InvoiceTypeId",
                        column: x => x.InvoiceTypeId,
                        principalTable: "InvoiceTypes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Invoices_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Invoices_TaxRates_TaxRateId",
                        column: x => x.TaxRateId,
                        principalTable: "TaxRates",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "WorkorderAttachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkorderId = table.Column<long>(type: "bigint", nullable: false),
                    FileMimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileMD5 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkorderAttachments", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkorderAttachments_Workorders_WorkorderId",
                        column: x => x.WorkorderId,
                        principalTable: "Workorders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceRows",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerPiece = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IsDiscountRow = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceRows", x => x.id);
                    table.ForeignKey(
                        name: "FK_InvoiceRows_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceWorkorders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    WorkorderId = table.Column<long>(type: "bigint", nullable: true),
                    RateId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceWorkorders", x => x.id);
                    table.ForeignKey(
                        name: "FK_InvoiceWorkorders_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceWorkorders_WorkRates_RateId",
                        column: x => x.RateId,
                        principalTable: "WorkRates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceWorkorders_Workorders_WorkorderId",
                        column: x => x.WorkorderId,
                        principalTable: "Workorders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientBearers_ClientDeviceId",
                table: "ClientBearers",
                column: "ClientDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientBearers_ClientIpAddressId",
                table: "ClientBearers",
                column: "ClientIpAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientBearers_UserId",
                table: "ClientBearers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientDeviceProperties_ClientDeviceId",
                table: "ClientDeviceProperties",
                column: "ClientDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CountryId",
                table: "Companies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_CompanyId",
                table: "CompanyUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_UserId",
                table: "CompanyUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CountryId",
                table: "Customers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRows_InvoiceId",
                table: "InvoiceRows",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CompanyId",
                table: "Invoices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceTypeId",
                table: "Invoices",
                column: "InvoiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ProjectId",
                table: "Invoices",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SetToPayed_By_CompanyUserId",
                table: "Invoices",
                column: "SetToPayed_By_CompanyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TaxRateId",
                table: "Invoices",
                column: "TaxRateId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTypes_CompanyId",
                table: "InvoiceTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWorkorders_InvoiceId",
                table: "InvoiceWorkorders",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWorkorders_RateId",
                table: "InvoiceWorkorders",
                column: "RateId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceWorkorders_WorkorderId",
                table: "InvoiceWorkorders",
                column: "WorkorderId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CompanyId",
                table: "Projects",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CustomerId",
                table: "Projects",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_CompanyId",
                table: "TaxRates",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_CountryId",
                table: "TaxRates",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentCompanyId",
                table: "Users",
                column: "CurrentCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkorderAttachments_WorkorderId",
                table: "WorkorderAttachments",
                column: "WorkorderId");

            migrationBuilder.CreateIndex(
                name: "IX_Workorders_CompanyId",
                table: "Workorders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Workorders_RateId",
                table: "Workorders",
                column: "RateId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRates_CompanyId",
                table: "WorkRates",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRates_TaxRateId",
                table: "WorkRates",
                column: "TaxRateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankStatementExpenses");

            migrationBuilder.DropTable(
                name: "BankStatementInvoices");

            migrationBuilder.DropTable(
                name: "BankStatements");

            migrationBuilder.DropTable(
                name: "ChangePaths");

            migrationBuilder.DropTable(
                name: "Changes");

            migrationBuilder.DropTable(
                name: "ChangeSets");

            migrationBuilder.DropTable(
                name: "ClientBearers");

            migrationBuilder.DropTable(
                name: "ClientDeviceProperties");

            migrationBuilder.DropTable(
                name: "DocumentAttachments");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "ExpenseAttachments");

            migrationBuilder.DropTable(
                name: "ExpenseProducts");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseTaxRatePrices");

            migrationBuilder.DropTable(
                name: "ExpenseTypes");

            migrationBuilder.DropTable(
                name: "ExperienceAttachments");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "ExperienceTechnologies");

            migrationBuilder.DropTable(
                name: "InvoiceAttachments");

            migrationBuilder.DropTable(
                name: "InvoiceEmails");

            migrationBuilder.DropTable(
                name: "InvoiceProducts");

            migrationBuilder.DropTable(
                name: "InvoiceRows");

            migrationBuilder.DropTable(
                name: "InvoiceWorkorders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Residences");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Technologies");

            migrationBuilder.DropTable(
                name: "TechnologyAttachments");

            migrationBuilder.DropTable(
                name: "TrafficRegistrations");

            migrationBuilder.DropTable(
                name: "TransactionLogs");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "WorkorderAttachments");

            migrationBuilder.DropTable(
                name: "ClientLocations");

            migrationBuilder.DropTable(
                name: "ClientDevices");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Workorders");

            migrationBuilder.DropTable(
                name: "CompanyUsers");

            migrationBuilder.DropTable(
                name: "InvoiceTypes");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "WorkRates");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "TaxRates");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
