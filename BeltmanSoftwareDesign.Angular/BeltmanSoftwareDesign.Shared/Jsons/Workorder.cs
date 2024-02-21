namespace BeltmanSoftwareDesign.Shared.Jsons
{
    public class Workorder
    {
        public long id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }

        public long? ProjectId { get; set; }
        public string? ProjectName { get; set; }

        public long? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public List<InvoiceWorkorder>? InvoiceWorkorders { get; set; }
        public List<WorkorderAttachment>? WorkorderAttachments { get; set; }

        public double AmountUur
        {
            get
            {
                return (Stop - Start).TotalHours;
            }
            set
            {
                Stop = Start.AddHours(value);
            }
        }
    }
}
