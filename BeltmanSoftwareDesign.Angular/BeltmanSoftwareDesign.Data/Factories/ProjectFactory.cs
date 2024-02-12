using BeltmanSoftwareDesign.Data.Entities;

namespace BeltmanSoftwareDesign.Data.Factories
{
    public class ProjectFactory
    {
        public Shared.Jsons.Project Convert(Project a)
        {
            return new Shared.Jsons.Project()
            {
                id = a.id,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer?.Name,
                Name = a.Name,
                Publiekelijk = a.Publiekelijk,
            };
        }
    }
}