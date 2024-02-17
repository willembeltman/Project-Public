import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { InvoiceCreateResponse } from '../interfaces/response/invoicecreateresponse';
import { InvoiceReadResponse } from '../interfaces/response/invoicereadresponse';
import { InvoiceUpdateResponse } from '../interfaces/response/invoiceupdateresponse';
import { InvoiceDeleteResponse } from '../interfaces/response/invoicedeleteresponse';
import { InvoiceListResponse } from '../interfaces/response/invoicelistresponse';
import { InvoiceCreateRequest } from '../interfaces/request/invoicecreaterequest';
import { InvoiceReadRequest } from '../interfaces/request/invoicereadrequest';
import { InvoiceUpdateRequest } from '../interfaces/request/invoiceupdaterequest';
import { InvoiceDeleteRequest } from '../interfaces/request/invoicedeleterequest';
import { InvoiceListRequest } from '../interfaces/request/invoicelistrequest';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  create(request: InvoiceCreateRequest): Observable<InvoiceCreateResponse> {
    return this.http.post<InvoiceCreateResponse>(this.constants.apiUrl + '/invoice/create', request);
  }
  read(request: InvoiceReadRequest): Observable<InvoiceReadResponse> {
    return this.http.post<InvoiceReadResponse>(this.constants.apiUrl + '/invoice/read', request);
  }
  update(request: InvoiceUpdateRequest): Observable<InvoiceUpdateResponse> {
    return this.http.post<InvoiceUpdateResponse>(this.constants.apiUrl + '/invoice/update', request);
  }
  delete(request: InvoiceDeleteRequest): Observable<InvoiceDeleteResponse> {
    return this.http.post<InvoiceDeleteResponse>(this.constants.apiUrl + '/invoice/delete', request);
  }
  list(request: InvoiceListRequest): Observable<InvoiceListResponse> {
    return this.http.post<InvoiceListResponse>(this.constants.apiUrl + '/invoice/list', request);
  }
}
