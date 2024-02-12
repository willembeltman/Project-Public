import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../apiservices/auth.service';
import { StateService } from '../../services/state.service';
import { RegisterResponse } from '../../interfaces/response/registerresponse';
import ValidateForm from './validateform';

@Component({
  selector: 'app-register',
  standalone: true,
  imports:
    [
      FormsModule,
      NgIf,
      ReactiveFormsModule,
    ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  registerForm: FormGroup = this.fb.group({
    username: ['', Validators.required],
    email: ['', Validators.required],
    phoneNumber: ['', Validators.required],
    password: ['', Validators.required],
    password2: ['', Validators.required],
  });

  passwordNotEqual: boolean = false;

  apiErrorMessage: string | null = null;
  apiError: boolean = false;

  apiResponse: RegisterResponse | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private auth: AuthService,
    private state: StateService) {
  }

  getErrorEmailNotValid() {
    return this.apiResponse?.errorEmailNotValid ?? false;
  }
  getErrorUsernameInUse() {
    return this.apiResponse?.errorUsernameInUse ?? false;
  }
  getErrorUsernameEmpty() {
    return this.apiResponse?.errorUsernameEmpty ?? false;
  }
  getErrorPasswordEmpty() {
    return this.apiResponse?.errorPasswordEmpty ?? false;
  }
  getErrorPhoneNumberEmpty() {
    return this.apiResponse?.errorPhoneNumberEmpty ?? false;
  }
  getErrorEmailInUse() {
    return this.apiResponse?.errorEmailInUse ?? false;
  }

  onRegister() {
    // Validate form
    if (!this.registerForm.valid) {
      ValidateForm.validateAllFormFields(this.registerForm);
      return;
    }

    // Validate password equal
    this.passwordNotEqual =
      this.registerForm.controls['password'].value !=
      this.registerForm.controls['password2'].value;
    if (this.passwordNotEqual) {
      return;
    }

    // Call your authentication service
    this.auth
      .register(this.registerForm.value)
      .subscribe(
        (response) => {
          this.apiResponse = response;
          this.apiError = !response.success;

          if (response.success) {
            this.state.setState(response.state);

            // Authentication successful, navigate to a different page
            this.router.navigate(['/registersucces']);
          }
        },
        (error) => {
          console.error('Register failed:', error);
          // Handle login error, show error message, etc.
        });
  }
}
