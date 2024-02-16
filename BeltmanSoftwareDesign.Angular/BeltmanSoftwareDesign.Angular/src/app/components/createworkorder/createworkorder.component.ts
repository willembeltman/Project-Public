import { NgForOf, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Workorder } from '../../interfaces/workorder';
import { StateService } from '../../services/state.service';
import { Router, RouterLink } from '@angular/router';
import ValidateForm from '../register/validateform';
import { Project } from '../../interfaces/project';
import { Customer } from '../../interfaces/customer';
import { WorkorderService } from '../../apiservices/workorder.service';
import { ProjectService } from '../../apiservices/project.service';
import { CustomerService } from '../../apiservices/customer.service';

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

  workorders: Workorder[] | null = null;
  projects: Project[] | null = null;
  customers: Customer[] | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private stateService: StateService,
    private workorderService: WorkorderService,
    private projectService: ProjectService,
    private customerService: CustomerService) { }

  ngOnInit(): void {
    this.readWorkorders();
  }

  readWorkorders() {
    let request = this.stateService.createStandardRequest();
    this.workorderService
      .list(request)
      .subscribe({
        next: (workorderresponse) => {
          this.stateService.setState(workorderresponse.state);
          if (workorderresponse.success) {
            this.workorders = workorderresponse.workorders;
            this.readProjects();
          }
        }
      });
  }
  readProjects() {
    let request = this.stateService.createStandardRequest();
    this.projectService
      .list(request)
      .subscribe({
        next: (projectsresponse) => {
          this.stateService.setState(projectsresponse.state);
          if (projectsresponse.success) {
            this.projects = projectsresponse.projects;
            this.readProjects();
          }
        }
      });
  }
  readCustomers() {
    let request = this.stateService.createStandardRequest();
    this.customerService
      .list(request)
      .subscribe({
        next: (customersResponse) => {
          this.stateService.setState(customersResponse.state);
          if (customersResponse.success) {
            this.customers = customersResponse.customers;
            this.setup();
          }
        }
      });
  }
  setup() {   

    this.workorderForm = this.fb.group({
      //countryId: [this.countries[0].id],
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
    this.workorderService
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
