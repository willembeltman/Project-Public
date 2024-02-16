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
        public Project Convert(Shared.Jsons.Project a)
        {
            return new Project()
            {
                id = a.id,
                CustomerId = a.CustomerId,
                Name = a.Name,
                Publiekelijk = a.Publiekelijk,
            };
        }

        public bool Copy(Shared.Jsons.Project? source, Project dest)
        {
            var changed = false;
            if (dest.CustomerId != source.CustomerId) { dest.CustomerId = source.CustomerId; changed = true; }
            if (dest.Name != source.Name) { dest.Name = source.Name; changed = true; }
            if (dest.Publiekelijk != source.Publiekelijk) { dest.Publiekelijk = source.Publiekelijk; changed = true; }
            return changed;
        }
    }
}