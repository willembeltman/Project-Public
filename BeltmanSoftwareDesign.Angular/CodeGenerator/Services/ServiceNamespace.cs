using CodeGenerator.Models;
using System.Reflection;

namespace CodeGenerator.Services
{
    public class ServiceNamespace
    {
        public ServiceNamespace(ServiceNamespacesList servicesNamespacesList, Assembly assembly, string name)
        {
            ServicesNamespacesList = servicesNamespacesList;
            Name = name;

            Services = assembly
                .GetTypes()
                .Where(a =>
                    a.IsVisible &&
                    a.Namespace == name &&
                    !a.CustomAttributes.Any(b => b.AttributeType.Name == "TsHiddenAttribute"))
                .Select(a => new Service(this, a))
                .ToArray();
        }

        public ServiceNamespacesList ServicesNamespacesList { get; }
        public string Name { get; } = "BeltmanSoftwareDesign.Business.Services";
        public Service[] Services { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
