import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CompaniesComponent } from '../companies/companies.component';
import { CompaniesService } from '../../apiservices/companies.service';
import { StateService } from '../../services/state.service';
import { NgIf } from '@angular/common';

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
    private companiesService: CompaniesService) {
  }

  ngOnInit() {
    let request = this.stateService.createStandardRequest();
    this.companiesService
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
