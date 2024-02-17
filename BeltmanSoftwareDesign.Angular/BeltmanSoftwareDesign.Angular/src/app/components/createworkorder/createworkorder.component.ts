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
import { Invoice } from '../../interfaces/invoice';
import { InvoiceService } from '../../apiservices/invoice.service';

@Component({
  selector: 'app-createworkorder',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf, ReactiveFormsModule, RouterLink],
  templateUrl: './createworkorder.component.html',
  styleUrl: './createworkorder.component.css'
})
export class CreateWorkorderComponent implements OnInit {

  workorderForm: FormGroup = this.fb.group({
    startDate: ['', Validators.required],
    startTime: ['', Validators.required],
    stopDate: ['', Validators.required],
    stopTime: ['', Validators.required],
    description: ['', Validators.required],
    projectName: ['', Validators.required],
    customerName: ['', Validators.required],
  });

  firstWorkorder: boolean = false;

  workorders: Workorder[] = [];
  invoices: Invoice[] = [];
  projects: Project[] = [];
  customers: Customer[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private stateService: StateService,
    private workorderService: WorkorderService,
    private invoiceService: InvoiceService,
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
            this.readInvoices();
          }
        }
      });
  }
  readInvoices() {
    let request = this.stateService.createStandardRequest();
    this.invoiceService
      .list(request)
      .subscribe({
        next: (invoiceresponse) => {
          this.stateService.setState(invoiceresponse.state);
          if (invoiceresponse.success) {
            this.invoices = invoiceresponse.invoices;
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
            this.readCustomers();
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
    this.firstWorkorder = this.workorders.length == 0;
    
    let date = new Date();
    let datestring = date.toISOString().split('T')[0];
    let timestring = date.toISOString().split('T')[1].split('Z')[0];

    this.workorderForm = this.fb.group({
      startDate: [datestring, Validators.required],
      startTime: [timestring, Validators.required],
      stopDate: [datestring, Validators.required],
      stopTime: [timestring, Validators.required],
      description: ['', Validators.required],
      projectName: ['', Validators.required],
      customerName: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (!this.workorderForm.valid) {
      ValidateForm.validateAllFormFields(this.workorderForm);
      return;
    }
    
    const formData = this.workorderForm.value;
    let start = formData.startDate + ' ' + formData.startTime;
    let stop = formData.stopDate + ' ' + formData.stopTime;
    const workorder: Workorder = {
      id: 0,
      start: new Date(start),
      stop: new Date(stop),
      invoiceWorkorders: [],
      workorderAttachments: [],
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
            this.router.navigate(['/editworkorder/' + response.workorder?.id]);
          }
        }
      });

  }
}
