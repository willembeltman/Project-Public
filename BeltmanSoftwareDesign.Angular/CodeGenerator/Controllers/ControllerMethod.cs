using CodeGenerator.Models;

namespace CodeGenerator.Controllers
{
    public class ControllerMethod
    {
        public ControllerMethod(
            string serviceName,
            string serviceMethod,
            string actionMethod,
            string controllerName,
            string controllerNameFullName,
            string methodName,
            string methodReturnType,
            string methodReturnTsType,
            bool methodReturnTypeNulleble,
            bool methodReturnTypeIsList,
            bool methodReturnTypeCreateImports,
            Models.ModelNamespace? methodReturnTypeImportNamespace)
        {
            ServiceName = serviceName;
            ServiceMethod = serviceMethod;
            ActionMethod = actionMethod;
            ControllerName = controllerName;
            ControllerNameFullName = controllerNameFullName;
            MethodName = methodName;
            ReturnType = methodReturnType;
            ReturnTsType = methodReturnTsType;
            ReturnTypeNulleble = methodReturnTypeNulleble;
            ReturnTypeIsList = methodReturnTypeIsList;
            ReturnTypeImport = methodReturnTypeCreateImports;
            ReturnTypeImportNamespace = methodReturnTypeImportNamespace;
        }

        public string ServiceName { get; }
        public string ServiceMethod { get; }
        public string ActionMethod { get; }
        public string ControllerName { get; }
        public string ControllerNameFullName { get; }
        public string MethodName { get; }

        public string ReturnType { get; }
        public bool ReturnTypeNulleble { get; }
        public bool ReturnTypeIsList { get; }
        public bool ReturnTypeImport { get; }
        public string ReturnTsType { get; }

        public ModelNamespace? ReturnTypeImportNamespace { get; }
        public List<ControllerParameter> Parameters { get; } = new List<ControllerParameter>();
    }
}