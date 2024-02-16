import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { CountryListResponse } from '../interfaces/response/countrylistresponse';
import { CountryListRequest } from '../interfaces/request/countrylistrequest';

@Injectable({
  providedIn: 'root'
})
export class CountryService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  list(request: CountryListRequest): Observable<CountryListResponse> {
    return this.http.post<CountryListResponse>(this.constants.apiUrl + '/country/list', request);
  }
}
