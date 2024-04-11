using CodeGenerator.Helpers;
using CodeGenerator.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGenerator.Controllers
{
    public class ControllerMethodList
    {
        static string namespaceControllers = "BeltmanSoftwareDesign.Api.Controllers";
        static string controllerpattern = "Controller";

        static string nulleblestring = "System.Nullable`1[[";
        static string nulleblestringend = "]]";

        static string isliststring = "System.Collections.Generic.List`1[[";
        static string isliststringend = "]]";

        public ControllerMethodList(ModelNamespacesList namespaceList)
        {
            NamespaceList = namespaceList;

            Assembly assemblyControllers = typeof(BeltmanSoftwareDesign.Api.Controllers.BaseControllerBase).Assembly;

            var types = assemblyControllers.GetTypes()
                .Where(type => type.Namespace == namespaceControllers)
                .ToArray();

            var controllerMethods = new List<ControllerMethod>();

            // Door alle gevonden types lopen
            foreach (var type in types)
            {
                // Controller name
                var controllernamefullname = type.FullName;
                if (controllernamefullname == null) continue;
                var controllernamefull = controllernamefullname
                    .Substring(namespaceControllers.Length + 1, controllernamefullname.Length - namespaceControllers.Length - 1);
                var controllername = controllernamefull
                    .Substring(0, controllernamefull.Length - controllerpattern.Length);

                // Methods
                var methods = type
                    .GetMethods()
                    .Where(a =>
                        a.CustomAttributes.Any(b =>
                            b.AttributeType.Name == "TsServiceAttribute"))
                    .ToArray();
                foreach (var method in methods)
                {
                    var methodname = method.Name;

                    var returnTypeName = method.ReturnType.FullName;
                    var returnTsType =
                        CsToTs(
                            returnTypeName, 
                            out bool returnTypeNulleble, 
                            out bool returnTypeIsList, 
                            out bool returnTypeImport,
                            out ModelNamespace? returnTypeImportNamespace);

                    var isGet = method.CustomAttributes
                        .Any(a => a.AttributeType.Name == "HttpGetAttribute");
                    var isPost = method.CustomAttributes
                        .Any(a => a.AttributeType.Name == "HttpPostAttribute");
                    var isDelete = method.CustomAttributes
                        .Any(a => a.AttributeType.Name == "HttpDeleteAttribute");

                    var actionmethod = "get";
                    if (isPost) actionmethod = "post";
                    if (isDelete) actionmethod = "delete";

                    var serviceandmethod = method.CustomAttributes
                        .FirstOrDefault(b =>
                            b.AttributeType.Name == "TsControllerMethodAttribute");
                    if (serviceandmethod == null) continue;
                    var service = NameHelper.UpperCaseFirstLetter(serviceandmethod.ConstructorArguments.FirstOrDefault().ToString().Replace("\"", ""));
                    var servicemethode = serviceandmethod.ConstructorArguments.LastOrDefault().ToString().Replace("\"", "");

                    var controller =
                        new ControllerMethod(
                            service,
                            servicemethode,
                            actionmethod.ToUpper(),
                            controllername,
                            controllernamefull,
                            methodname,
                            returnTypeName,
                            returnTsType,
                            returnTypeNulleble,
                            returnTypeIsList,
                            returnTypeImport,
                            returnTypeImportNamespace);
                    controllerMethods.Add(controller);

                    var parameters = method.GetParameters();
                    foreach (var parameter in parameters)
                    {
                        var parameterName = parameter.Name;

                        var parameterType = parameter.ParameterType.FullName;
                        var parameterTsType =
                            CsToTs(
                                parameterType,
                                out bool parameterNulleble, 
                                out bool parameterIsList, 
                                out bool parameterImport,
                                out ModelNamespace? parameterImportNamespace);

                        var controllerparameter = 
                            new ControllerParameter(
                                parameterName, 
                                parameterType, 
                                parameterTsType, 
                                parameterNulleble, 
                                parameterIsList, 
                                parameterImport,
                                parameterImportNamespace);
                        controller.Parameters.Add(controllerparameter);
                    }
                }
            }

            ControllerMethods = controllerMethods.ToArray();
        }

        public ModelNamespacesList NamespaceList { get; }
        public ControllerMethod[] ControllerMethods { get; }

        string? CsToTs(string typeName, out bool isNulleble, out bool isList, out bool import, out ModelNamespace? importNamespace)
        {
            isNulleble = false;
            isList = false;
            import = false;
            importNamespace = null;
            var filteredtype = typeName;

            if (typeName.StartsWith(nulleblestring))
            {
                isNulleble = true;
                var end = filteredtype.IndexOf(nulleblestringend);
                var csv = filteredtype.Substring(nulleblestring.Length, end - nulleblestring.Length);
                var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                filteredtype = csvlines.First();
            }

            if (typeName.StartsWith(isliststring))
            {
                isList = true;
                var end = typeName.IndexOf(isliststringend);
                var csv = typeName.Substring(isliststring.Length, end - isliststring.Length);
                var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                typeName = csvlines.First();
            }

            //var tstype = NameHelper.GetTypeScriptType(filteredtype);
            //if (tstype == null)
            //{
            //    if (propertytype.StartsWith(namespaceModels))
            //    {
            //        tstype = propertytype.Substring(namespaceModels.Length + 1, propertytype.Length - namespaceModels.Length - 1);
            //        if (tstype.EndsWith("[]"))
            //        {
            //            tstype = tstype.Substring(0, tstype.Length - 2);
            //            islist = true;
            //        }

            //        createimports = true;
            //    }
            //}


            var tstype = NameHelper.GetTsType(filteredtype);
            if (tstype == null)
            {
                foreach (var namespaceItem in NamespaceList.List)
                {
                    var namespaceName = namespaceItem.Name;
                    if (typeName.StartsWith(namespaceName))
                    {
                        import = true;

                        tstype = typeName.Substring(
                            namespaceName.Length + 1,
                            typeName.Length - namespaceName.Length - 1);

                        importNamespace = namespaceItem;
                        break;
                    }
                }
            }

            if (tstype.EndsWith("[]"))
            {
                tstype = tstype.Substring(0, tstype.Length - 2);
                isList = true;
            }

            return tstype;
        }

    }
}
