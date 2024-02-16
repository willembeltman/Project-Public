import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { CompanyCreateResponse } from '../interfaces/response/companycreateresponse';
import { CompanyReadResponse } from '../interfaces/response/companyreadresponse';
import { CompanyUpdateResponse } from '../interfaces/response/companyupdateresponse';
import { CompanyDeleteResponse } from '../interfaces/response/companydeleteresponse';
import { CompanyListResponse } from '../interfaces/response/companylistresponse';
import { CompanyCreateRequest } from '../interfaces/request/companycreaterequest';
import { CompanyReadRequest } from '../interfaces/request/companyreadrequest';
import { CompanyUpdateRequest } from '../interfaces/request/companyupdaterequest';
import { CompanyDeleteRequest } from '../interfaces/request/companydeleterequest';
import { CompanyListRequest } from '../interfaces/request/companylistrequest';

@Injectable({
  providedIn: 'root'
})
export class CompanyService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  create(request: CompanyCreateRequest): Observable<CompanyCreateResponse> {
    return this.http.post<CompanyCreateResponse>(this.constants.apiUrl + '/company/create', request);
  }
  read(request: CompanyReadRequest): Observable<CompanyReadResponse> {
    return this.http.post<CompanyReadResponse>(this.constants.apiUrl + '/company/read', request);
  }
  update(request: CompanyUpdateRequest): Observable<CompanyUpdateResponse> {
    return this.http.post<CompanyUpdateResponse>(this.constants.apiUrl + '/company/update', request);
  }
  delete(request: CompanyDeleteRequest): Observable<CompanyDeleteResponse> {
    return this.http.post<CompanyDeleteResponse>(this.constants.apiUrl + '/company/delete', request);
  }
  list(request: CompanyListRequest): Observable<CompanyListResponse> {
    return this.http.post<CompanyListResponse>(this.constants.apiUrl + '/company/list', request);
  }
}
