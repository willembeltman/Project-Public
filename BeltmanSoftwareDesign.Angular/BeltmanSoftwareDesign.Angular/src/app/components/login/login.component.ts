import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../apiservices/auth.service';
import { LoginRequest } from '../../interfaces/request/loginrequest';
import { StateService } from '../../services/state.service';
import { LoginResponse } from '../../interfaces/response/loginresponse';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  email: string = '';
  password: string = '';

  apiErrorMessage: string | null = null;
  apiError: boolean = false;

  apiResponse: LoginResponse | null = null;

  constructor(
    private router:Router, 
    private auth: AuthService,
    private state: StateService) { }

    
  getErrorEmailNotValid() {
    return this.apiResponse?.errorEmailNotValid ?? false;
  }
  getErrorPasswordEmpty() {
    return this.apiResponse?.authenticationError ?? false;
  }
  getErrorPauthentication() {
    return this.apiResponse?.authenticationError ?? false;
  }

  login() {
    // Call your authentication service

    const request: LoginRequest =
    {
      email: this.email,
      password: this.password
    };

    this.auth.login(request)
      .subscribe(
        (response) => {
          this.apiResponse = response;
          this.apiError = !response.success;

          if (response.success) {
            this.state.setState(response.state);

            // Authentication successful, navigate to a different page
            this.router.navigate(['/loginsucces']);
          }
        },
        (error) => {
          console.error('Login failed:', error);
          // Handle login error, show error message, etc.
        }
      );
  }
}
