import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { WorkorderCreateResponse } from '../interfaces/response/workordercreateresponse';
import { WorkorderReadResponse } from '../interfaces/response/workorderreadresponse';
import { WorkorderUpdateResponse } from '../interfaces/response/workorderupdateresponse';
import { WorkorderDeleteResponse } from '../interfaces/response/workorderdeleteresponse';
import { WorkorderListResponse } from '../interfaces/response/workorderlistresponse';
import { WorkorderCreateRequest } from '../interfaces/request/workordercreaterequest';
import { WorkorderReadRequest } from '../interfaces/request/workorderreadrequest';
import { WorkorderUpdateRequest } from '../interfaces/request/workorderupdaterequest';
import { WorkorderDeleteRequest } from '../interfaces/request/workorderdeleterequest';
import { WorkorderListRequest } from '../interfaces/request/workorderlistrequest';

@Injectable({
  providedIn: 'root'
})
export class WorkordersService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  create(request: WorkorderCreateRequest): Observable<WorkorderCreateResponse> {
    return this.http.post<WorkorderCreateResponse>(this.constants.apiUrl + '/workorders/create', request);
  }
  read(request: WorkorderReadRequest): Observable<WorkorderReadResponse> {
    return this.http.post<WorkorderReadResponse>(this.constants.apiUrl + '/workorders/read', request);
  }
  update(request: WorkorderUpdateRequest): Observable<WorkorderUpdateResponse> {
    return this.http.post<WorkorderUpdateResponse>(this.constants.apiUrl + '/workorders/update', request);
  }
  delete(request: WorkorderDeleteRequest): Observable<WorkorderDeleteResponse> {
    return this.http.post<WorkorderDeleteResponse>(this.constants.apiUrl + '/workorders/delete', request);
  }
  list(request: WorkorderListRequest): Observable<WorkorderListResponse> {
    return this.http.post<WorkorderListResponse>(this.constants.apiUrl + '/workorders/list', request);
  }
}
