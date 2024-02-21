import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { RateCreateResponse } from '../interfaces/response/ratecreateresponse';
import { RateReadResponse } from '../interfaces/response/ratereadresponse';
import { RateUpdateResponse } from '../interfaces/response/rateupdateresponse';
import { RateDeleteResponse } from '../interfaces/response/ratedeleteresponse';
import { RateListResponse } from '../interfaces/response/ratelistresponse';
import { RateCreateRequest } from '../interfaces/request/ratecreaterequest';
import { RateReadRequest } from '../interfaces/request/ratereadrequest';
import { RateUpdateRequest } from '../interfaces/request/rateupdaterequest';
import { RateDeleteRequest } from '../interfaces/request/ratedeleterequest';
import { RateListRequest } from '../interfaces/request/ratelistrequest';

@Injectable({
  providedIn: 'root'
})
export class RateService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  create(request: RateCreateRequest): Observable<RateCreateResponse> {
    return this.http.post<RateCreateResponse>(this.constants.apiUrl + '/rate/create', request);
  }
  read(request: RateReadRequest): Observable<RateReadResponse> {
    return this.http.post<RateReadResponse>(this.constants.apiUrl + '/rate/read', request);
  }
  update(request: RateUpdateRequest): Observable<RateUpdateResponse> {
    return this.http.post<RateUpdateResponse>(this.constants.apiUrl + '/rate/update', request);
  }
  delete(request: RateDeleteRequest): Observable<RateDeleteResponse> {
    return this.http.post<RateDeleteResponse>(this.constants.apiUrl + '/rate/delete', request);
  }
  list(request: RateListRequest): Observable<RateListResponse> {
    return this.http.post<RateListResponse>(this.constants.apiUrl + '/rate/list', request);
  }
}
