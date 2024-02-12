import { NgForOf, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Workorder } from '../../interfaces/workorder';
import { WorkordersService } from '../../apiservices/workorders.service';
import { StateService } from '../../services/state.service';
import { Country } from '../../interfaces/country';
import { Router, RouterLink } from '@angular/router';
import ValidateForm from '../register/validateform';
import { CountriesService } from '../../apiservices/countries.service';

@Component({
  selector: 'app-createworkorder',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf, ReactiveFormsModule, RouterLink],
  templateUrl: './createworkorder.component.html',
  styleUrl: './createworkorder.component.css'
})
export class CreateWorkorderComponent implements OnInit {

  workorderForm: FormGroup = this.fb.group({
    countryId: [''],
    name: ['', Validators.required],
    address: [''],
    postalcode: [''],
    place: [''],
    phoneNumber: [''],
    email: ['', Validators.email],
    website: [''],
    btwNumber: [''],
    kvkNumber: [''],
    iban: [''],
  });

  firstWorkorder: boolean = false;

  countries: Country[] | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private stateService: StateService,
    private countriesService: CountriesService,
    private workordersService: WorkordersService) { }

  ngOnInit(): void {
    let reqeust = this.stateService.createStandardRequest();
    this.countriesService
      .list(reqeust)
      .subscribe({
        next: (countriesresponse) => {
          this.stateService.setState(countriesresponse.state);
          if (countriesresponse.success) {

            this.workordersService
              .list(reqeust)
              .subscribe({
                next: (workordersresponse) => {
                  this.stateService.setState(workordersresponse.state);
                  if (workordersresponse.success) {

                    this.firstWorkorder = workordersresponse.workorders.length == 0;
                    this.countries = countriesresponse.countries;

                    const name = (this.stateService.getState()?.user?.userName ?? '') + "'s workorder";

                    this.workorderForm = this.fb.group({
                      countryId: [this.countries[0].id],
                      name: [name, Validators.required],
                      address: [''],
                      postalcode: [''],
                      place: [''],
                      phoneNumber: [this.stateService.getState()?.user?.phoneNumber ?? ''],
                      email: [this.stateService.getState()?.user?.email ?? '', Validators.email],
                      website: [''],
                      btwNumber: [''],
                      kvkNumber: [''],
                      iban: [''],
                    });

                  }
                }
              })
          }
        }
      });
  }

  onSubmit(): void {
    if (!this.workorderForm.valid) {
      ValidateForm.validateAllFormFields(this.workorderForm);
      return;
    }

    const formData = this.workorderForm.value;
    const workorder: Workorder = {
      Id: 0,
      ...formData
    };
    this.workordersService
      .create({
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
