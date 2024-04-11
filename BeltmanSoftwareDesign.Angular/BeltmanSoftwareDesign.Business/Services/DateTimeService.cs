using BeltmanSoftwareDesign.Business.Interfaces;

namespace BeltmanSoftwareDesign.Business.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
