import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { SetCurrentCompanyResponse } from '../interfaces/response/setcurrentcompanyresponse';
import { UserReadResponse } from '../interfaces/response/userreadresponse';
import { UserUpdateResponse } from '../interfaces/response/userupdateresponse';
import { UserDeleteResponse } from '../interfaces/response/userdeleteresponse';
import { UserListResponse } from '../interfaces/response/userlistresponse';
import { SetCurrentCompanyRequest } from '../interfaces/request/setcurrentcompanyrequest';
import { UserReadRequest } from '../interfaces/request/userreadrequest';
import { UserUpdateRequest } from '../interfaces/request/userupdaterequest';
import { UserDeleteRequest } from '../interfaces/request/userdeleterequest';
import { UserListRequest } from '../interfaces/request/userlistrequest';

@Injectable({
  providedIn: 'root'
})
export class UserService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  setcurrentcompany(request: SetCurrentCompanyRequest): Observable<SetCurrentCompanyResponse> {
    return this.http.post<SetCurrentCompanyResponse>(this.constants.apiUrl + '/user/setcurrentcompany', request);
  }
  readknownuser(request: UserReadRequest): Observable<UserReadResponse> {
    return this.http.post<UserReadResponse>(this.constants.apiUrl + '/user/readknownuser', request);
  }
  updatemyself(request: UserUpdateRequest): Observable<UserUpdateResponse> {
    return this.http.post<UserUpdateResponse>(this.constants.apiUrl + '/user/updatemyself', request);
  }
  deletemyself(request: UserDeleteRequest): Observable<UserDeleteResponse> {
    return this.http.post<UserDeleteResponse>(this.constants.apiUrl + '/user/deletemyself', request);
  }
  listknownusers(request: UserListRequest): Observable<UserListResponse> {
    return this.http.post<UserListResponse>(this.constants.apiUrl + '/user/listknownusers', request);
  }
}
