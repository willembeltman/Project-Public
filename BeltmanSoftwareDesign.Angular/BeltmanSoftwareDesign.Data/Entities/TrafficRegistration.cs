using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class TrafficRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public long CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public string Description { get; set; }
        public DateTime Date { get; set; }
        public double KilometerStart { get; set; }
        public double KilometerStop { get; set; }

        public double AmountKm
        {
            get
            {
                return KilometerStop - KilometerStart;
            }
        }

        public byte GetQuarter()
        {
            return Convert.ToByte(Math.Ceiling(Convert.ToDouble(Date.Month) / 3));
        }
    }
}
