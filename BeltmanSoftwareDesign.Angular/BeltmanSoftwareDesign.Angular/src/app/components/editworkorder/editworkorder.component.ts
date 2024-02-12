import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Country } from '../../interfaces/country';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { WorkordersService } from '../../apiservices/workorders.service';
import { StateService } from '../../services/state.service';
import ValidateForm from '../register/validateform';
import { Workorder } from '../../interfaces/workorder';
import { NgForOf, NgIf } from '@angular/common';
import { CountriesService } from '../../apiservices/countries.service';

@Component({
  selector: 'app-editworkorder',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf, ReactiveFormsModule, RouterLink],
  templateUrl: './editworkorder.component.html',
  styleUrl: './editworkorder.component.css'
})
export class EditWorkorderComponent {
  workorderId: number | null = 0;
  countries: Country[] | null = null;
  workorderForm: FormGroup = this.fb.group({
    address: [''],
    btwNumber: [''],
    countryId: [''],
    email: ['', Validators.email],
    iban: [''],
    id: [''],
    kvkNumber: [''],
    name: ['', Validators.required],
    phoneNumber: [''],
    place: [''],
    postalcode: [''],
    website: [''],
  });

  loaded: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private stateService: StateService,
    private countriesService: CountriesService,
    private workordersService: WorkordersService,
    private route: ActivatedRoute) {
    this.workorderId = parseInt(this.route.snapshot.paramMap.get('id') ?? "0");
  }

  ngOnInit(): void {
    let request = this.stateService.createStandardRequest();
    this.countriesService
      .list(request)
      .subscribe({
        next: (response) => {
          this.stateService.setState(response.state);
          if (response.success) {
            this.countries = response.countries;

            this.workordersService
              .read({
                ...request,
                workorderId: this.workorderId ?? 0
              })
              .subscribe({
                next: (response) => {
                  if (response.success) {
                    let workorder = response.workorder;
                    if (workorder != null) {                      
                      this.workorderForm = this.fb.group({
                        id: [workorder.id, Validators.required],
                        customerId: [workorder.customerId, Validators.required],
                        projectId: [workorder.projectId, Validators.required],
                        description: [workorder.description, Validators.required],
                        start: [workorder.start, Validators.required],
                        stop: [workorder.stop, Validators.required],
                      });
                      this.loaded = true;
                    }
                  }
                }
              });
          }
        }
      });
  }
  
  getCountries() {
    return this.countries;
  }

  onSubmit(): void {
    if (!this.workorderForm.valid) {
      ValidateForm.validateAllFormFields(this.workorderForm);
      return;
    }
    //return;

    const formData = this.workorderForm.value;
    const workorder: Workorder = {
      ...formData
    };
    this.workordersService
      .update({
        bearerId: this.stateService.getState()?.bearerId ?? null,
        currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null,
        workorder: workorder
      })
      .subscribe({
        next: (response) => {
          this.stateService.setState(response.state);
          if (response.success) {
            //this.cacheService.resetWorkorders();
            this.router.navigate(['/workorders']);
          }
        }
      });
  }
}
