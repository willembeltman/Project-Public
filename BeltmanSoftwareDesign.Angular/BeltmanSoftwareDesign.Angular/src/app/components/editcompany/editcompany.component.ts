import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Country } from '../../interfaces/country';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { StateService } from '../../services/state.service';
import ValidateForm from '../register/validateform';
import { Company } from '../../interfaces/company';
import { NgForOf, NgIf } from '@angular/common';
import { CountryService } from '../../apiservices/country.service';
import { CompanyService } from '../../apiservices/company.service';

@Component({
  selector: 'app-editcompany',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf, ReactiveFormsModule, RouterLink],
  templateUrl: './editcompany.component.html',
  styleUrl: './editcompany.component.css'
})
export class EditCompanyComponent {
  companyId: number | null = 0;
  countries: Country[] | null = null;
  companyForm: FormGroup = this.fb.group({
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
    private countryService: CountryService,
    private companyService: CompanyService,
    private route: ActivatedRoute) {
    this.companyId = parseInt(this.route.snapshot.paramMap.get('id') ?? "0");
  }

  ngOnInit(): void {
    this.countryService
      .list({
        bearerId: this.stateService.getState()?.bearerId ?? null,
        currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null,
      })
      .subscribe({
        next: (response) => {
          if (response.success) {
            this.countries = response.countries;

            this.companyService
              .read({
                bearerId: this.stateService.getState()?.bearerId ?? null,
                currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null,
                companyId: this.companyId ?? 0
              })
              .subscribe({
                next: (response) => {
                  if (response.success) {
                    let company = response.company;
                    if (company != null) {                      
                      this.companyForm = this.fb.group({
                        address: [company.address],
                        btwNumber: [company.btwNumber],
                        countryId: [company.countryId, Validators.required],
                        email: [company.email, [Validators.email, Validators.required]],
                        iban: [company.iban],
                        id: [company.id, Validators.required],
                        kvkNumber: [company.kvkNumber],
                        name: [company.name, Validators.required],
                        phoneNumber: [company.phoneNumber],
                        place: [company.place],
                        postalcode: [company.postalcode],
                        website: [company.website],
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
    if (!this.companyForm.valid) {
      ValidateForm.validateAllFormFields(this.companyForm);
      return;
    }
    //return;

    const formData = this.companyForm.value;
    const company: Company = {
      ...formData
    };
    this.companyService
      .update({
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
