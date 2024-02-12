using CodeGenerator.Models;

namespace CodeGenerator.Services
{
    public class ServiceNamespacesList
    {
        public ServiceNamespacesList(Models.ModelNamespacesList modelNamespaces)
        {
            ModelNamespacesList = modelNamespaces;
            ServiceNamespaces = new ServiceNamespace[]
            {
                new ServiceNamespace(this,
                    typeof(BeltmanSoftwareDesign.Business.Services.AuthenticationService).Assembly,
                    "BeltmanSoftwareDesign.Business.Services"),
            };
        }

        public Models.ModelNamespacesList ModelNamespacesList { get; }
        public ServiceNamespace[] ServiceNamespaces { get; }
    }
}
