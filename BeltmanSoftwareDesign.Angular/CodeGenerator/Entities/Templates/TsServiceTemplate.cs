using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities.Templates
{
    internal class TsServiceTemplate : ITemplate
    {
        private DbSetInfo DbSet { get; }
        public EntityInfo Entity { get; }
        public string NamespaceName { get; }

        public TsServiceTemplate(DbSetInfo dbSet, string namespaceName = "BeltmanSoftwareDesign.Shared.Jsons")
        {
            DbSet = dbSet;
            Entity = dbSet.Entity;
            NamespaceName = namespaceName;
        }

        public string GetContent()
        {
            throw new NotImplementedException();
            var text = "";

            //var imports = Entity.Properties
            //    .Select(a => a.Type)
            //    .Where(a => a.Import)
            //    .GroupBy(a => a.TsName)
            //    .Select(a => a.First())
            //    .ToArray();

            ////import { Injectable } from '@angular/core';
            ////import { Observable } from 'rxjs';
            ////import { ConstantsService } from '../services/constants.service';
            ////import { HttpClient } from '@angular/common/http';
            ////import { LoginResponse } from '../interfaces/response/loginresponse';
            ////import { RegisterResponse } from '../interfaces/response/registerresponse';
            ////import { LoginRequest } from '../interfaces/request/loginrequest';
            ////import { RegisterRequest } from '../interfaces/request/registerrequest';

            ////@Injectable({
            ////  providedIn: 'root'
            ////})
            ////export class AuthService
            ////{
            ////  constructor(private constants:ConstantsService, private http:HttpClient) { }

            ////  login(request: LoginRequest): Observable<LoginResponse> {
            ////    return this.http.post<LoginResponse>(this.constants.apiUrl + '/auth/login', request);
            ////  }
            ////  register(request: RegisterRequest): Observable<RegisterResponse> {
            ////    return this.http.post<RegisterResponse>(this.constants.apiUrl + '/auth/register', request);
            ////  }
            ////}

            //foreach (var type in imports)
            //{
            //    var targetTsFolder = type.Model?.ModelsNamespace?.TsFolder;
            //    if (targetTsFolder == null) { continue; }
            //    string folder = GetFolderPath(huidigeTsFolder, targetTsFolder);

            //    text += "import { " + type.TsName + @" } from """ + folder + type.TsName.ToLower() + @""";" + Environment.NewLine;
            //}

            //if (imports.Any())
            //    text += Environment.NewLine;

            //text += @"export interface " + Entity.Name + " {";
            //foreach (var property in Entity.Properties)
            //{
            //    text += Environment.NewLine + "    " + property.NameLower + @": " + property.Type.TsFullName + @";";
            //}
            //text += Environment.NewLine + "}";

            return text;
        }

        public string GetFullName()
        {
            throw new NotImplementedException();
        }
    }
}