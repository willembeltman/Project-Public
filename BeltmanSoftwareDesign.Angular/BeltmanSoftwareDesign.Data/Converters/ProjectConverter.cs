namespace BeltmanSoftwareDesign.Data.Converters
{
    public class ProjectConverter
    {
        public Shared.Jsons.Project Create(Entities.Project a)
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
        public Entities.Project Create(Shared.Jsons.Project a)
        {
            return new Entities.Project()
            {
                id = a.id,
                CustomerId = a.CustomerId,
                Name = a.Name,
                Publiekelijk = a.Publiekelijk,
            };
        }

        public bool Copy(Shared.Jsons.Project? source, Entities.Project dest)
        {
            var changed = false;
            if (dest.CustomerId != source.CustomerId) { dest.CustomerId = source.CustomerId; changed = true; }
            if (dest.Name != source.Name) { dest.Name = source.Name; changed = true; }
            if (dest.Publiekelijk != source.Publiekelijk) { dest.Publiekelijk = source.Publiekelijk; changed = true; }
            return changed;
        }
    }
}