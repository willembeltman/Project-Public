using BeltmanSoftwareDesign.Business.Helpers;
using CodeGenerator.Controllers;
using CodeGenerator.Helpers;
using CodeGenerator.Models;
using CodeGenerator.Services;
using static System.Net.Mime.MediaTypeNames;

namespace CodeGenerator
{


    public class GenerateFromServices
    {
        static string angular_app_directory = @"D:\Project-Public\BeltmanSoftwareDesign.Angular\BeltmanSoftwareDesign.Angular\src\app";

        static string angular_apiservices_directory = @"D:\Project-Public\BeltmanSoftwareDesign.Angular\BeltmanSoftwareDesign.Angular\src\app\apiservices";

        static string dotnet_controllers_directory = @"D:\Project-Public\BeltmanSoftwareDesign.Angular\BeltmanSoftwareDesign.Api\Controllers";

        public void Run(string[] args)
        {
            var modelNamespacesList = new Models.ModelNamespacesList();
            ExportModels(modelNamespacesList);

            //var list = new ControllerMethodList(group);
            //ExportServices(list);

            var servicesNamespacesList = new Services.ServiceNamespacesList(modelNamespacesList);
            ExportTsServices(servicesNamespacesList);
            ExportControllers(servicesNamespacesList);
        }

        private void ExportModels(Models.ModelNamespacesList namespaceList)
        {
            foreach (var namespaceItem in namespaceList.List)
            {
                var huidigeTsFolder = namespaceItem.TsFolder;

                foreach (var model in namespaceItem.Models)
                {
                    //import { InvoiceWorkorder } from "./invoiceworkorder";

                    //export interface Invoice {
                    //    id: number;
                    //    invoiceTypeId: number | null;
                    //    invoiceTypeName: string | null;
                    //    projectId: number | null;
                    //    projectName: string | null;
                    //    customerId: number | null;
                    //    customerName: string | null;
                    //    taxRateId: number | null;
                    //    taxRateName: string | null;
                    //    taxRatePercentage: number | null;
                    //    date: string;
                    //    invoiceNumber: string | null;
                    //    description: string | null;
                    //    isPayedInCash: boolean;
                    //    isPayed: boolean;
                    //    datePayed: string | null;
                    //    paymentCode: string | null;
                    //    invoiceWorkorders: InvoiceWorkorder[];
                    //}

                    var text = "";
                    var imports = model.Properties
                        .Select(a => a.Type)
                        .Where(a => a.Import)
                        .GroupBy(a => a.TsName)
                        .Select(a => a.First())
                        .ToArray();

                    foreach (var type in imports)
                    {
                        var targetTsFolder = type.Model?.ModelsNamespace?.TsFolder;
                        if (targetTsFolder == null) { continue; }
                        string folder = GetFolderPath(huidigeTsFolder, targetTsFolder);

                        text += "import { " + type.TsName + @" } from """ + folder + type.TsName.ToLower() + @""";" + Environment.NewLine;
                    }

                    if (imports.Any())
                        text += Environment.NewLine;

                    text += @"export interface " + model.Name + " {";
                    foreach (var property in model.Properties)
                    {
                        text += Environment.NewLine + "    " + property.NameLower + @": " + property.Type.TsFullName + @";";
                    }
                    text += Environment.NewLine + "}";

                    #region Saven

                    var filename = angular_app_directory + "\\" + namespaceItem.TsFolder.Replace("/", "\\") + "\\" + model.Name.ToLower() + ".ts";
                    WriteToFile(filename, text);
                    Console.WriteLine(filename + Environment.NewLine + text + Environment.NewLine);

                    #endregion
                }
            }
        }
        private void ExportTsServices(Services.ServiceNamespacesList servicesNamespacesList)
        {
            var huidigeTsFolder = "apiservices";

            var allmethods = servicesNamespacesList.ServiceNamespaces
                .SelectMany(a => a.Services)
                .SelectMany(a => a.Methods);

            var tsServices = allmethods.GroupBy(a => a.TsServiceName);
            foreach (var group2 in tsServices)
            {
                var tsServiceMethods = group2.ToArray();
                var tsServiceName = group2.Key;

                var namespaces1 = tsServiceMethods
                    .GroupBy(a => a.ResponseType)
                    .Select(a => a.Key)
                    .Where(a => a.Import)
                    .ToArray();
                var namespaces2 = tsServiceMethods
                    .GroupBy(a => a.RequestParameterType)
                    .Select(a => a.Key)
                    .Where(a => a.Import)
                    .ToArray();
                var namespaces = namespaces1.Concat(namespaces2)
                    .GroupBy(a => a)
                    .Select(a => a.Key)
                    .ToArray();

                //import { Injectable } from '@angular/core';
                //import { Observable } from 'rxjs';
                //import { ConstantsService } from '../services/constants.service';
                //import { HttpClient } from '@angular/common/http';
                //import { LoginResponse } from '../interfaces/response/loginresponse';
                //import { RegisterResponse } from '../interfaces/response/registerresponse';
                //import { LoginRequest } from '../interfaces/request/loginrequest';
                //import { RegisterRequest } from '../interfaces/request/registerrequest';

                //@Injectable({
                //  providedIn: 'root'
                //})
                //export class AuthService
                //{
                //  constructor(private constants:ConstantsService, private http:HttpClient) { }

                //  login(request: LoginRequest): Observable<LoginResponse> {
                //    return this.http.post<LoginResponse>(this.constants.apiUrl + '/auth/login', request);
                //  }
                //  register(request: RegisterRequest): Observable<RegisterResponse> {
                //    return this.http.post<RegisterResponse>(this.constants.apiUrl + '/auth/register', request);
                //  }
                //}

                var text = "";
                text += @"import { Injectable } from '@angular/core';" + Environment.NewLine;
                text += @"import { Observable } from 'rxjs';" + Environment.NewLine;
                text += @"import { ConstantsService } from '../services/constants.service';" + Environment.NewLine;
                text += @"import { HttpClient } from '@angular/common/http';" + Environment.NewLine;

                foreach (var nnamespace in namespaces)
                {
                    var targetTsFolder = nnamespace.Model.ModelsNamespace.TsFolder;
                    var folder = GetFolderPath(huidigeTsFolder, targetTsFolder);

                    text += $"import {{ {nnamespace.TsName} }} from '{folder}{nnamespace.TsName.ToLower()}';" + Environment.NewLine;
                }

                text += Environment.NewLine;

                text += @"@Injectable({" + Environment.NewLine;
                text += @"  providedIn: 'root'" + Environment.NewLine;
                text += @"})" + Environment.NewLine;
                text += $"export class " + tsServiceName + "Service" + Environment.NewLine;
                text += @"{" + Environment.NewLine;
                text += @"  constructor(private constants:ConstantsService, private http:HttpClient) { }" + Environment.NewLine;
                text += @"  " + Environment.NewLine;

                foreach (var method in tsServiceMethods)
                {
                    text += $"  {method.TsMethodName.ToLower()}({method.RequestParameterName}: {method.RequestParameterType.TsFullNameNotNull}): Observable<{method.ResponseType.TsFullNameNotNull}> {{" + Environment.NewLine;
                    text += $"    return this.http.post<{method.ResponseType.TsFullNameNotNull}>(this.constants.apiUrl + '/{method.TsServiceName.ToLower()}/{method.TsMethodName.ToLower()}', {method.RequestParameterName});" + Environment.NewLine;
                    text += $"  }}" + Environment.NewLine;
                }

                text += @"}" + Environment.NewLine;

                #region Saven

                var filename = angular_apiservices_directory + "\\" + NameHelper.LowerCaseFirstLetter(tsServiceName) + ".service.ts";
                WriteToFile(filename, text);

                Console.WriteLine(filename + Environment.NewLine + text + Environment.NewLine);

                #endregion
            }
        }
        private void ExportControllers(Services.ServiceNamespacesList servicesNamespacesList)
        {
            var list = servicesNamespacesList.ServiceNamespaces
                .SelectMany(a => a.Services)
                .SelectMany(a => a.Methods);

            var tsServices = list.GroupBy(a => a.TsServiceName);
            foreach (var tsService in tsServices)
            {
                var imports = new List<string>();
                var namespaces1 = tsService.GroupBy(a => a.RequestParameterType.Model.ModelsNamespace.Name).Select(a => a.Key).ToArray();
                var namespaces2 = tsService.GroupBy(a => a.ResponseType.Model.ModelsNamespace.Name).Select(a => a.Key).ToArray();
                var namespaces = namespaces1.Concat(namespaces2).GroupBy(a => a).Select(a => a.Key).ToArray();
                var servicenames = tsService.GroupBy(a => a.Service.NameWithService).Select(a => a.Key).ToArray();


                //using Microsoft.AspNetCore.Mvc;
                //using BeltmanSoftwareDesign.Business.Interfaces;

                //namespace BeltmanSoftwareDesign.Api.Controllers
                //{
                //    [ApiController]
                //    [Route("[controller]/[action]")]
                //    public class AuthController : BaseControllerBase
                //    {
                //        public AuthController(IAuthenticationService authenticationService) 
                //        {
                //            AuthenticationService = authenticationService;
                //        }

                //        public IAuthenticationService AuthenticationService { get; }

                //        [HttpPost]
                //        public LoginResponse? Login(LoginRequest request) 
                //            => AuthenticationService.Login(request, IpAddress, Headers);

                //        [HttpPost]
                //        public RegisterResponse? Register(RegisterRequest request) 
                //            => AuthenticationService.Register(request, IpAddress, Headers);
                //    }
                //}

                var text = "";
                text += @"using Microsoft.AspNetCore.Mvc;" + Environment.NewLine;
                text += @"using BeltmanSoftwareDesign.Business.Interfaces;" + Environment.NewLine;

                foreach (var name in namespaces)
                {
                    text += $"using {name};" + Environment.NewLine;
                }

                text += @"namespace BeltmanSoftwareDesign.Api.Controllers" + Environment.NewLine;
                text += @"{" + Environment.NewLine;
                text += @"    [ApiController]" + Environment.NewLine;
                text += @"    [Route(""[controller]/[action]"")]" + Environment.NewLine;
                text += $"    public class {tsService.Key}Controller : BaseControllerBase" + Environment.NewLine;
                text += @"    {" + Environment.NewLine;
                text += $"        public {tsService.Key}Controller(";

                var first = true;
                foreach (var servicename in servicenames)
                {
                    if (first)
                        first = false;
                    else
                        text += ", ";

                    text += $"I{servicename} {NameHelper.LowerCaseFirstLetter(servicename)}";
                }

                text += @") " + Environment.NewLine;
                text += @"        {" + Environment.NewLine;

                foreach (var servicename in servicenames)
                {
                    text += $"            {servicename} = {NameHelper.LowerCaseFirstLetter(servicename)};" + Environment.NewLine;
                }

                text += @"        }" + Environment.NewLine;
                text += Environment.NewLine;

                foreach (var servicename in servicenames)
                {
                    text += $"        public I{servicename} {servicename} {{ get; }}" + Environment.NewLine;
                }

                if (servicenames.Any())
                {
                    text += Environment.NewLine;
                }

                first = true;
                foreach (var method in tsService)
                {
                    //        [HttpPost]
                    //        public LoginResponse? Login(LoginRequest request) 
                    //            => AuthenticationService.Login(request, IpAddress, Headers);

                    if (first)  
                        first = false;
                    else
                        text += Environment.NewLine;

                    text += $"        [HttpPost]" + Environment.NewLine;
                    text += $"        public {method.ResponseType.Name} {method.Name}({method.RequestParameterType.Name} {method.RequestParameterName}) " + Environment.NewLine;
                    text += $"            => {method.Service.NameWithService}.{method.Name}({method.RequestParameterName}, IpAddress, Headers);" + Environment.NewLine;
                }

                text += @"    }" + Environment.NewLine;
                text += @"}" + Environment.NewLine;

                #region Saven

                var filename = dotnet_controllers_directory + "\\" + tsService.Key + "Controller.cs";
                WriteToFile(filename, text);

                Console.WriteLine(filename + Environment.NewLine + text + Environment.NewLine);

                #endregion
            }
        }


        private static string GetFolderPath(string huidige, string target)
        {
            var folder = "";
            if (target.StartsWith(huidige))
            {
                // dieper
                folder = "./" + target.Substring(huidige.Length);
            }
            else
            {
                // via root
                var split1 = huidige.Split('/');
                var split2 = target.Split('/');

                var i = 0;
                for (i = split1.Length; i > 0; i--)
                {
                    if (split2.Length >= i)
                    {
                        var path1 = string.Join("/", split1.Take(i));
                        var path2 = string.Join("/", split2.Take(i));
                        if (path1 == path2)
                        {
                            break;
                        }
                    }
                    folder += "../";
                }

                foreach (var item in split2.Skip(i))
                {
                    folder += item + "/";
                }
            }

            return folder;
        }

        //private void ExportServices(ControllerMethodList list)
        //{
        //    var huidigeTsNamespace = "apiservices";

        //    var controllerMethodGroups = list.ControllerMethods.GroupBy(a => a.ServiceName);
        //    foreach (var controllerMethodGroup in controllerMethodGroups)
        //    {
        //        var header = "";
        //        header += @"import { Injectable } from '@angular/core';" + Environment.NewLine;
        //        header += @"import { Observable } from 'rxjs';" + Environment.NewLine;
        //        header += @"import { ConstantsService } from '../services/constants.service';" + Environment.NewLine;
        //        header += @"import { HttpClient } from '@angular/common/http';" + Environment.NewLine;

        //        var body = "";
        //        body += @"@Injectable({" + Environment.NewLine;
        //        body += @"  providedIn: 'root'" + Environment.NewLine;
        //        body += @"})" + Environment.NewLine;
        //        body += $"export class " + controllerMethodGroup.Key + "Service" + Environment.NewLine;
        //        body += @"{" + Environment.NewLine;
        //        body += @"  constructor(private constants:ConstantsService, private http:HttpClient) { }" + Environment.NewLine;
        //        body += @"  " + Environment.NewLine;

        //        var imports = new List<string>();

        //        foreach (var controllerMethod in controllerMethodGroup)
        //        {
        //            if (controllerMethod.ReturnTypeImport)
        //            {
        //                if (!imports.Contains(controllerMethod.ReturnTsType))
        //                {
        //                    imports.Add(controllerMethod.ReturnTsType);

        //                    var target = controllerMethod.ReturnTypeImportNamespace.TsFolder;
        //                    var folder = GetFolderPath(huidigeTsNamespace, target);

        //                    header +=
        //                        "import { " +
        //                        controllerMethod.ReturnTsType +
        //                        " } from '" +
        //                        folder + controllerMethod.ReturnTsType.ToLower() +
        //                        "';" + Environment.NewLine;
        //                }
        //            }

        //            var parameter = controllerMethod.Parameters.First();
        //            if (parameter.Import)
        //            {
        //                if (!imports.Contains(parameter.TsType))
        //                {
        //                    imports.Add(parameter.TsType);

        //                    var targetTsNamespace = parameter.ImportNamespace.TsFolder;
        //                    var folder = GetFolderPath(huidigeTsNamespace, targetTsNamespace);

        //                    header +=
        //                        "import { " +
        //                        parameter.TsType +
        //                        " } from '" +
        //                        folder + parameter.TsType.ToLower() +
        //                        "';" + Environment.NewLine;
        //                }
        //            }

        //            body += $"  " + controllerMethod.ServiceMethod.ToLower() + "("+
        //                $"{parameter.Name}: {parameter.TsType}" + (parameter.IsList ? "[]" : "") +
        //                @"): Observable<" +
        //                controllerMethod.ReturnTsType + (controllerMethod.ReturnTypeIsList ? "[]" : "") +
        //                "> {" + Environment.NewLine;

        //            body += @"    return this.http.post<" +
        //                controllerMethod.ReturnTsType + (controllerMethod.ReturnTypeIsList ? "[]" : "") +
        //                ">(this.constants.apiUrl + '/" +
        //                controllerMethod.ControllerName + "', " +
        //                parameter.Name + ");" + Environment.NewLine;

        //            body += @"  }" + Environment.NewLine;

        //        }

        //        body += @"}" + Environment.NewLine;

        //        var filename = angular_services + "\\" + NameHelper.LowerCaseFirstLetter(controllerMethodGroup.Key) + ".service.ts";
        //        var filecontents = header + Environment.NewLine + Environment.NewLine + body;

        //        WriteToFile(filename, filecontents);

        //        Console.WriteLine(filename + Environment.NewLine + filecontents + Environment.NewLine);
        //    }
        //}

        private static void WriteToFile(string filename, string filecontents)
        {
            var info = new FileInfo(filename);
            if (info.Exists)
            {
                info.Delete();
            }

            using (var stream = File.Create(filename))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(filecontents);
            }
        }

    }


}