using CodeGenerator.Helpers;
using CodeGenerator.Models;
using System.Reflection;

namespace CodeGenerator.Services
{
    public class ServiceMethod
    {
        public ServiceMethod(Service service, MethodInfo method)
        {
            Service = service;

            Name = method.Name;
            _ResponseType = method.ReturnType;

            var serviceandmethod = method.CustomAttributes
                .FirstOrDefault(b =>
                    b.AttributeType.Name == "TsServiceMethodAttribute");
            if (serviceandmethod == null)
                throw new Exception("No TsServiceMethodAttribute found");

            TsServiceName = NameHelper.UpperCaseFirstLetter(serviceandmethod.ConstructorArguments.FirstOrDefault().ToString().Replace("\"", ""));
            TsMethodName = serviceandmethod.ConstructorArguments.LastOrDefault().ToString().Replace("\"", "");

            var parameters = method.GetParameters();
            if (parameters.Length != 3)
                throw new Exception("Too many parameters");
            if (parameters[1].ParameterType != typeof(string))
                throw new Exception("Too many parameters");
            if (parameters[2].ParameterType != typeof(KeyValuePair<string, string?>[]))
                throw new Exception("Too many parameters");

            RequestParameterName = parameters[0].Name;
            _RequestParameterType = parameters[0].ParameterType;
        }

        public Service Service { get; }

        public string Name { get; }
        public string TsServiceName { get; }
        public string TsMethodName { get; }
        public string RequestParameterName { get; }

        Type _RequestParameterType { get; }
        TypeRapport _RequestParameterTypeRapport { get; set; }
        public TypeRapport RequestParameterType
        {
            get
            {
                if (_RequestParameterTypeRapport == null)
                    _RequestParameterTypeRapport = new TypeRapport(_RequestParameterType, Service.ServicesNamespace.ServicesNamespacesList.ModelNamespacesList);
                return _RequestParameterTypeRapport;
            }
        }

        Type _ResponseType { get; }
        TypeRapport _ResponseTypeRapport { get; set; }
        public TypeRapport ResponseType
        {
            get
            {
                if (_ResponseTypeRapport == null)
                    _ResponseTypeRapport = new TypeRapport(_ResponseType, Service.ServicesNamespace.ServicesNamespacesList.ModelNamespacesList);
                return _ResponseTypeRapport;
            }
        }

        //public string RequestParameterTypeFullName => RequestParameterTypeRapport.FullName;
        //public string RequestParameterTypeName => RequestParameterTypeRapport.Name;
        //public string RequestParameterTypeTsName => RequestParameterTypeRapport.TsName;
        //public bool RequestParameterTypeNulleble => RequestParameterTypeRapport.Nulleble;
        //public bool RequestParameterTypeList => RequestParameterTypeRapport.List;
        //public bool RequestParameterTypeImport => RequestParameterTypeRapport.Import;
        //public Model? RequestParameterTypeImportModel => RequestParameterTypeRapport.ImportModel;

        //public string ResponseTypeFullName => ResponseTypeRapport.FullName;
        //public string ResponseTypeName => ResponseTypeRapport.Name;
        //public string ResponseTypeTsName => ResponseTypeRapport.TsName;
        //public bool ResponseTypeNulleble => ResponseTypeRapport.Nulleble;
        //public bool ResponseTypeList => ResponseTypeRapport.List;
        //public bool ResponseTypeImport => ResponseTypeRapport.Import;
        //public Model? ResponseTypeImportModel => ResponseTypeRapport.ImportModel;


        public override string ToString()
        {
            return Name;
        }
    }
}
