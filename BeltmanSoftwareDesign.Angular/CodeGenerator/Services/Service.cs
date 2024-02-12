namespace CodeGenerator.Services
{
    public class Service
    {
        static string ServiceNameEnd = "Service";

        public Service(ServiceNamespace servicesNamespace, Type type)
        {
            ServicesNamespace = servicesNamespace;

            if (type == null) return;
            if (type.FullName == null) return;

            Fullname = type.FullName;
            NameWithService = type.Name;
            if (NameWithService.ToLower().EndsWith(ServiceNameEnd.ToLower()))
                Name = NameWithService
                    .Substring(0, NameWithService.Length - ServiceNameEnd.Length);

            var list2 = type
                .GetMethods();
            var list = list2
                .Where(a => a.CustomAttributes.Any(b => b.AttributeType.Name == "TsServiceMethodAttribute"))
                .ToArray();
            Methods = list
                .Select(a => new ServiceMethod(this, a))
                .ToArray();
        }

        public ServiceNamespace ServicesNamespace { get; }
        public string Fullname { get; } = "BeltmanSoftwareDesign.Business.Services.CompaniesService";
        public string NameWithService { get; } = "CompaniesService";
        public string Name { get; } = "Companies";
        public ServiceMethod[] Methods { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
