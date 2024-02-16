import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { ProjectCreateResponse } from '../interfaces/response/projectcreateresponse';
import { ProjectReadResponse } from '../interfaces/response/projectreadresponse';
import { ProjectUpdateResponse } from '../interfaces/response/projectupdateresponse';
import { ProjectDeleteResponse } from '../interfaces/response/projectdeleteresponse';
import { ProjectListResponse } from '../interfaces/response/projectlistresponse';
import { ProjectCreateRequest } from '../interfaces/request/projectcreaterequest';
import { ProjectReadRequest } from '../interfaces/request/projectreadrequest';
import { ProjectUpdateRequest } from '../interfaces/request/projectupdaterequest';
import { ProjectDeleteRequest } from '../interfaces/request/projectdeleterequest';
import { ProjectListRequest } from '../interfaces/request/projectlistrequest';

@Injectable({
  providedIn: 'root'
})
export class ProjectService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  create(request: ProjectCreateRequest): Observable<ProjectCreateResponse> {
    return this.http.post<ProjectCreateResponse>(this.constants.apiUrl + '/project/create', request);
  }
  read(request: ProjectReadRequest): Observable<ProjectReadResponse> {
    return this.http.post<ProjectReadResponse>(this.constants.apiUrl + '/project/read', request);
  }
  update(request: ProjectUpdateRequest): Observable<ProjectUpdateResponse> {
    return this.http.post<ProjectUpdateResponse>(this.constants.apiUrl + '/project/update', request);
  }
  delete(request: ProjectDeleteRequest): Observable<ProjectDeleteResponse> {
    return this.http.post<ProjectDeleteResponse>(this.constants.apiUrl + '/project/delete', request);
  }
  list(request: ProjectListRequest): Observable<ProjectListResponse> {
    return this.http.post<ProjectListResponse>(this.constants.apiUrl + '/project/list', request);
  }
}
