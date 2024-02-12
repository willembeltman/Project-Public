import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConstantsService } from '../services/constants.service';
import { HttpClient } from '@angular/common/http';
import { LoginResponse } from '../interfaces/response/loginresponse';
import { RegisterResponse } from '../interfaces/response/registerresponse';
import { LoginRequest } from '../interfaces/request/loginrequest';
import { RegisterRequest } from '../interfaces/request/registerrequest';

@Injectable({
  providedIn: 'root'
})
export class AuthService
{
  constructor(private constants:ConstantsService, private http:HttpClient) { }
  
  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.constants.apiUrl + '/auth/login', request);
  }
  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(this.constants.apiUrl + '/auth/register', request);
  }
}
