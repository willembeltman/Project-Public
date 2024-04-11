import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { SetCurrentCompanyResponse } from '../interfaces/response/setcurrentcompanyresponse';
import { ReadKnownUserResponse } from '../interfaces/response/readknownuserresponse';
import { UpdateMyselfResponse } from '../interfaces/response/updatemyselfresponse';
import { DeleteMyselfResponse } from '../interfaces/response/deletemyselfresponse';
import { ListKnownUsersResponse } from '../interfaces/response/listknownusersresponse';
import { SetCurrentCompanyRequest } from '../interfaces/request/setcurrentcompanyrequest';
import { ReadKnownUserRequest } from '../interfaces/request/readknownuserrequest';
import { UpdateMyselfRequest } from '../interfaces/request/updatemyselfrequest';
import { DeleteMyselfRequest } from '../interfaces/request/deletemyselfrequest';
import { ListKnownUsersRequest } from '../interfaces/request/listknownusersrequest';

@Injectable({
  providedIn: 'root'
})
export class UserService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  setcurrentcompany(request: SetCurrentCompanyRequest): Observable<SetCurrentCompanyResponse> {
    return this.http.post<SetCurrentCompanyResponse>(this.constants.apiUrl + '/user/setcurrentcompany', request);
  }
  readknownuser(request: ReadKnownUserRequest): Observable<ReadKnownUserResponse> {
    return this.http.post<ReadKnownUserResponse>(this.constants.apiUrl + '/user/readknownuser', request);
  }
  updatemyself(request: UpdateMyselfRequest): Observable<UpdateMyselfResponse> {
    return this.http.post<UpdateMyselfResponse>(this.constants.apiUrl + '/user/updatemyself', request);
  }
  deletemyself(request: DeleteMyselfRequest): Observable<DeleteMyselfResponse> {
    return this.http.post<DeleteMyselfResponse>(this.constants.apiUrl + '/user/deletemyself', request);
  }
  listknownusers(request: ListKnownUsersRequest): Observable<ListKnownUsersResponse> {
    return this.http.post<ListKnownUsersResponse>(this.constants.apiUrl + '/user/listknownusers', request);
  }
}
