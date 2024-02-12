using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class TransactionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public long? InvoiceId { get; set; }
        public long? TransactieId { get; set; }
        //public virtual Company Company { get; set; }
        //public virtual Invoice Invoice { get; set; }
        //public virtual Transaction Transactie { get; set; }

        public DateTime DateCreated { get; set; }

        public string Url { get; set; } = string.Empty;
        public string Request { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;

        #region PayNL properties


        public int PaymentDetails_Amount { get; set; } 
        public string PaymentDetails_Created { get; set; } = string.Empty;
        public int PaymentDetails_CurrencyAmount { get; set; } 
        public string PaymentDetails_Description { get; set; } = string.Empty;
        public string PaymentDetails_Exchange { get; set; } = string.Empty;
        public string PaymentDetails_IdentifierHash { get; set; } = string.Empty;
        public string PaymentDetails_IdentifierName { get; set; } = string.Empty;
        public string PaymentDetails_IdentifierPublic { get; set; } = string.Empty;
        public string PaymentDetails_Modified { get; set; } = string.Empty;
        public string PaymentDetails_PaidAmount { get; set; } = string.Empty;
        public string PaymentDetails_PaidAttemps { get; set; } = string.Empty;
        public string PaymentDetails_PaidBase { get; set; } = string.Empty;
        public string PaymentDetails_PaidCosts { get; set; } = string.Empty;
        public string PaymentDetails_PaidCostsVat { get; set; } = string.Empty;
        public string PaymentDetails_PaidCurrencyAmount { get; set; } = string.Empty;
        public string PaymentDetails_PaidCurreny { get; set; } = string.Empty;
        public string PaymentDetails_PaidDuration { get; set; } = string.Empty;
        public string PaymentDetails_PaymentMethodDescription { get; set; } = string.Empty;
        public string PaymentDetails_PaymentMethodId { get; set; } = string.Empty;
        public string PaymentDetails_PaymentMethodName { get; set; } = string.Empty;
        public int PaymentDetails_PaymentOptionId { get; set; }
        public int PaymentDetails_PaymentOptionSubId { get; set; }
        public string PaymentDetails_PaymentProfileName { get; set; } = string.Empty;
        public string PaymentDetails_ProcessTime { get; set; } = string.Empty;
        public bool PaymentDetails_Secure { get; set; }
        public string PaymentDetails_SecureStatus { get; set; } = string.Empty;
        public string PaymentDetails_ServiceDescription { get; set; } = string.Empty;
        public string PaymentDetails_ServiceId { get; set; } = string.Empty;
        public string PaymentDetails_ServiceName { get; set; } = string.Empty;
        public string PaymentDetails_State { get; set; } = string.Empty;
        public string PaymentDetails_StateDescription { get; set; } = string.Empty;
        public string PaymentDetails_StateName { get; set; } = string.Empty;
        public bool PaymentDetails_Storno { get; set; }
        public string Connection_BrowserData { get; set; } = string.Empty;
        public string Connection_City { get; set; } = string.Empty;
        public string Connection_Country { get; set; } = string.Empty;
        public string Connection_Host { get; set; } = string.Empty;
        public string Connection_IP { get; set; } = string.Empty;
        public string Connection_LocationLat { get; set; } = string.Empty;
        public string Connection_LocationLon { get; set; } = string.Empty;
        public string Connection_MerchantCode { get; set; } = string.Empty;
        public string Connection_MerchantName { get; set; } = string.Empty;
        public string Connection_OrderIP { get; set; } = string.Empty;
        public string Connection_OrderReturnUrl { get; set; } = string.Empty;
        public int? Connection_Trust { get; set; }
        public string Request_Code { get; set; } = string.Empty;
        public string Request_Message { get; set; } = string.Empty;
        public bool Request_Result { get; set; }
        public string StatsDetails_Extra1 { get; set; } = string.Empty;
        public string StatsDetails_Extra2 { get; set; } = string.Empty;
        public string StatsDetails_Extra3 { get; set; } = string.Empty;
        public string StatsDetails_Info { get; set; } = string.Empty;
        public string StatsDetails_Object { get; set; } = string.Empty;
        public int? StatsDetails_PaymentSessionId { get; set; }
        public int? StatsDetails_PromotorId { get; set; }
        public string StatsDetails_Tool { get; set; } = string.Empty;
        public string StornoDetails_BankAccount { get; set; } = string.Empty;
        public string StornoDetails_bic { get; set; } = string.Empty;
        public string StornoDetails_City { get; set; } = string.Empty;
        public string StornoDetails_Date { get; set; } = string.Empty;
        public string StornoDetails_EmailAddress { get; set; } = string.Empty;
        public string StornoDetails_IBAN { get; set; } = string.Empty;
        public string StornoDetails_Reason { get; set; } = string.Empty;
        public int? StornoDetails_StornoAmount { get; set; }
        public int? StornoDetails_StornoId { get; set; }

        #endregion
    }
}
