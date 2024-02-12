import { NgForOf, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Company } from '../../interfaces/company';
import { CompaniesService } from '../../apiservices/companies.service';
import { StateService } from '../../services/state.service';
import { Country } from '../../interfaces/country';
import { Router, RouterLink } from '@angular/router';
import ValidateForm from '../register/validateform';
import { CountriesService } from '../../apiservices/countries.service';

@Component({
  selector: 'app-createcompany',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf, ReactiveFormsModule, RouterLink],
  templateUrl: './createcompany.component.html',
  styleUrl: './createcompany.component.css'
})
export class CreateCompanyComponent implements OnInit {

  companyForm: FormGroup = this.fb.group({
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

  firstCompany: boolean = false;

  countries: Country[] | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private stateService: StateService,
    private countriesService: CountriesService,
    private companiesService: CompaniesService) { }

  ngOnInit(): void {
    let reqeust = this.stateService.createStandardRequest();
    this.countriesService
      .list(reqeust)
      .subscribe({
        next: (countriesresponse) => {
          this.stateService.setState(countriesresponse.state);
          if (countriesresponse.success) {

            this.companiesService
              .list(reqeust)
              .subscribe({
                next: (companiesresponse) => {
                  this.stateService.setState(companiesresponse.state);
                  if (companiesresponse.success) {

                    this.firstCompany = companiesresponse.companies.length == 0;
                    this.countries = countriesresponse.countries;

                    const name = (this.stateService.getState()?.user?.userName ?? '') + "'s company";

                    this.companyForm = this.fb.group({
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
    if (!this.companyForm.valid) {
      ValidateForm.validateAllFormFields(this.companyForm);
      return;
    }

    const formData = this.companyForm.value;
    const company: Company = {
      Id: 0,
      ...formData
    };
    this.companiesService
      .create({
        bearerId: this.stateService.getState()?.bearerId ?? null,
        currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null,
        company: company
      })
      .subscribe({
        next: (response) => {
          this.stateService.setState(response.state);
          if (response.success) {
            //this.cacheService.resetCompanies();
            this.router.navigate(['/companies']);
          }
        }
      });

  }
}
