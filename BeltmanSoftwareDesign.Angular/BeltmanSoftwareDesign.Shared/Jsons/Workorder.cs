namespace BeltmanSoftwareDesign.Shared.Jsons
{
    public class Workorder
    {
        public long id { get; set; }

        public long CompanyId { get; set; }

        public DateTime Start { get; set; }
        public DateTime? Stop { get; set; }
        public string Description { get; set; }

        public long? ProjectId { get; set; }
        public string ProjectName { get; set; }

        public long? CustomerId { get; set; }
        public string CustomerName { get; set; }

        public List<InvoiceWorkorder> InvoiceWorkorders { get; set; }
        public List<WorkorderAttachment> WorkorderAttachments { get; set; }

        public double? AmountUur
        {
            get
            {
                if (Stop == null) return null;
                return (Stop.Value - Start).TotalHours;
            }
            set
            {
                if (value == null)
                    Stop = null;
                else
                    Stop = Start.AddHours(value.Value);
            }
        }
    }
}
