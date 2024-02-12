using CodeGenerator.Models;

namespace CodeGenerator.Models
{
    public class ModelNamespacesList
    {
        public ModelNamespacesList()
        {
            List = new ModelNamespace[]
            {
                new ModelNamespace(
                    this,
                    typeof(BeltmanSoftwareDesign.Shared.Jsons.Workorder).Assembly,
                    "BeltmanSoftwareDesign.Shared.Jsons",
                    "interfaces"),
                new ModelNamespace(
                    this,
                    typeof(BeltmanSoftwareDesign.Shared.RequestJsons.LoginRequest).Assembly,
                    "BeltmanSoftwareDesign.Shared.RequestJsons",
                    "interfaces/request"),
                new ModelNamespace(
                    this,
                    typeof(BeltmanSoftwareDesign.Shared.ResponseJsons.LoginResponse).Assembly,
                    "BeltmanSoftwareDesign.Shared.ResponseJsons",
                    "interfaces/response"),
            };
        }

        public ModelNamespace[] List { get; }
    }
}
