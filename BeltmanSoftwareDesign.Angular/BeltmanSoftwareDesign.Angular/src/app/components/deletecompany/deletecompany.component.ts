import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { StateService } from '../../services/state.service';
import { CountriesService } from '../../apiservices/countries.service';
import { CompaniesService } from '../../apiservices/companies.service';
import { Company } from '../../interfaces/company';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-deletecompany',
  standalone: true,
  imports: [NgIf, RouterLink],
  templateUrl: './deletecompany.component.html',
  styleUrl: './deletecompany.component.css'
})
export class DeleteCompanyComponent {
  companyId: number | null = 0;
  company: Company | null = null;
  loaded: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private stateService: StateService,
    private countriesService: CountriesService,
    private companiesService: CompaniesService,
    private route: ActivatedRoute) {
    this.companyId = parseInt(this.route.snapshot.paramMap.get('id') ?? "0");
  }

  ngOnInit(): void {
    this.companiesService
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
              this.company = company;
              this.loaded = true;
            }
          }
        }
      });
  }

  onSubmit(): void {
    if (confirm("Are you sure?")) {
      this.companiesService
        .delete({
          bearerId: this.stateService.getState()?.bearerId ?? null,
          currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null,
          companyId: this.companyId ?? 0
        })
        .subscribe({
          next: (response) => {
            this.stateService.setState(response.state);
            if (response.success) {
              this.router.navigate(['/companies']);
            }
          }
        });
    }
  }

}
