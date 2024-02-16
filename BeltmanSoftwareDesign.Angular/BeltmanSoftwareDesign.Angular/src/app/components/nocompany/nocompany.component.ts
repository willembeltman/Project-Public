import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CompaniesComponent } from '../companies/companies.component';
import { StateService } from '../../services/state.service';
import { NgIf } from '@angular/common';
import { CompanyService } from '../../apiservices/company.service';

@Component({
  selector: 'app-nocompany',
  standalone: true,
  imports: [RouterLink, CompaniesComponent, NgIf],
  templateUrl: './nocompany.component.html',
  styleUrl: './nocompany.component.css'
})
export class NoCompanyComponent implements OnInit {

  noCompanyError: boolean = false;

  constructor(
    private router: Router,
    private stateService: StateService,
    private companyService: CompanyService) {
  }

  ngOnInit() {
    let request = this.stateService.createStandardRequest();
    this.companyService
      .list(request)
      .subscribe({
        next: (response) => {
          this.stateService.setState(response.state);
          if (response.success) {
            this.noCompanyError = response.companies.length == 0
          }
        }
      });
  }
  handleUpdate() {
    this.router.navigate(['/companies']);
  }
}
