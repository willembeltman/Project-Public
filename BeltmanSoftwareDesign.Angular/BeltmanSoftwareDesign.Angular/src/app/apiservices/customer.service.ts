import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { CustomerCreateResponse } from '../interfaces/response/customercreateresponse';
import { CustomerReadResponse } from '../interfaces/response/customerreadresponse';
import { CustomerUpdateResponse } from '../interfaces/response/customerupdateresponse';
import { CustomerDeleteResponse } from '../interfaces/response/customerdeleteresponse';
import { CustomerListResponse } from '../interfaces/response/customerlistresponse';
import { CustomerCreateRequest } from '../interfaces/request/customercreaterequest';
import { CustomerReadRequest } from '../interfaces/request/customerreadrequest';
import { CustomerUpdateRequest } from '../interfaces/request/customerupdaterequest';
import { CustomerDeleteRequest } from '../interfaces/request/customerdeleterequest';
import { CustomerListRequest } from '../interfaces/request/customerlistrequest';

@Injectable({
  providedIn: 'root'
})
export class CustomerService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  create(request: CustomerCreateRequest): Observable<CustomerCreateResponse> {
    return this.http.post<CustomerCreateResponse>(this.constants.apiUrl + '/customer/create', request);
  }
  read(request: CustomerReadRequest): Observable<CustomerReadResponse> {
    return this.http.post<CustomerReadResponse>(this.constants.apiUrl + '/customer/read', request);
  }
  update(request: CustomerUpdateRequest): Observable<CustomerUpdateResponse> {
    return this.http.post<CustomerUpdateResponse>(this.constants.apiUrl + '/customer/update', request);
  }
  delete(request: CustomerDeleteRequest): Observable<CustomerDeleteResponse> {
    return this.http.post<CustomerDeleteResponse>(this.constants.apiUrl + '/customer/delete', request);
  }
  list(request: CustomerListRequest): Observable<CustomerListResponse> {
    return this.http.post<CustomerListResponse>(this.constants.apiUrl + '/customer/list', request);
  }
}
