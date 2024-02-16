using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities.Templates
{
    internal class CsListRequestJsonTemplate : ITemplate
    {
        private EntityInfo? entity;
        private string csharp_shared_directory;
        private string csharp_shared_namespace;
        private ConstrainedProperty[] stateList;
        private EntityInfo?[] uitgebreideKoppelList;
        private EntityInfo?[] simpeleKoppelList;
        private EntityInfo?[] attachmentEntitiesList;

        public CsListRequestJsonTemplate(EntityInfo? entity, string csharp_shared_directory, string csharp_shared_namespace, ConstrainedProperty[] stateList, EntityInfo?[] uitgebreideKoppelList, EntityInfo?[] simpeleKoppelList, EntityInfo?[] attachmentEntitiesList)
        {
            this.entity = entity;
            this.csharp_shared_directory = csharp_shared_directory;
            this.csharp_shared_namespace = csharp_shared_namespace;
            this.stateList = stateList;
            this.uitgebreideKoppelList = uitgebreideKoppelList;
            this.simpeleKoppelList = simpeleKoppelList;
            this.attachmentEntitiesList = attachmentEntitiesList;
        }

        public string GetContent()
        {
            //namespace BeltmanSoftwareDesign.Shared.RequestJsons
            //{
            //    public class CompanyListRequest : Request
            //    {
            //    }
            //}

            string res = "";

            res += $"namespace {csharp_shared_namespace}.RequestJsons" + Environment.NewLine;
            res += $"{{" + Environment.NewLine;
            res += $"    public class {entity.Name}ListRequest: Request" + Environment.NewLine;
            res += $"    {{" + Environment.NewLine;
            res += $"    }}" + Environment.NewLine;
            res += $"}}";

            return res;
        }

        public string GetFullName()
        {
            return "";
        }
    }
}