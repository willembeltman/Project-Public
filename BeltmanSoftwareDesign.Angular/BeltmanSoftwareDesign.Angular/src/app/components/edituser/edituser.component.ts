import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Country } from '../../interfaces/country';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { StateService } from '../../services/state.service';
import ValidateForm from '../register/validateform';
import { User } from '../../interfaces/user';
import { NgForOf, NgIf } from '@angular/common';
import { UserService } from '../../apiservices/user.service';

@Component({
  selector: 'app-edituser',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf, ReactiveFormsModule, RouterLink],
  templateUrl: './edituser.component.html',
  styleUrl: './edituser.component.css'
})
export class EditUserComponent {
  userId: string | null = null;
  countries: Country[] | null = null;
  email: string | null = "";
  userForm: FormGroup = this.fb.group({
    id: [''],
    userName: [''],
    phoneNumber: [''],
  });

  loaded: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private stateService: StateService,
    private userService: UserService,
    private route: ActivatedRoute) {
    this.userId = this.route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {

    this.userService
      .readknownuser({
        bearerId: this.stateService.getState()?.bearerId ?? null,
        currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null,
        userId: this.userId
      })
      .subscribe({
        next: (response) => {
          if (response.success) {
            let user = response.user;
            if (user != null) {

              this.email = user.email;

              this.userForm = this.fb.group({
                id: [user.id, Validators.required],
                userName: [user.userName, Validators.required],
                phoneNumber: [user.phoneNumber]
              });

              this.loaded = true;
            }
          }
        }
      });
  }

  onSubmit(): void {
    if (!this.userForm.valid) {
      ValidateForm.validateAllFormFields(this.userForm);
      return;
    }

    const formData = this.userForm.value;
    const user: User = {
      currentCompanyId: this.stateService.getState()?.currentCompany?.id,
      email: this.email,
      ...formData
    };
    this.userService
      .updatemyself({
        bearerId: this.stateService.getState()?.bearerId ?? null,
        currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null,
        user: user
      })
      .subscribe({
        next: (response) => {
          this.stateService.setState(response.state);
          if (response.success) {
            this.router.navigate(['/users']);
          }
        }
      });
  }
}
