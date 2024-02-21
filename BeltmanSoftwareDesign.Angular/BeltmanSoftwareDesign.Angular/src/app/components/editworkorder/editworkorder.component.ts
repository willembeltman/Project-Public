import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Country } from '../../interfaces/country';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { StateService } from '../../services/state.service';
import ValidateForm from '../register/validateform';
import { Workorder } from '../../interfaces/workorder';
import { NgForOf, NgIf } from '@angular/common';
import { CountryService } from '../../apiservices/country.service';
import { WorkorderService } from '../../apiservices/workorder.service';
import { Invoice } from '../../interfaces/invoice';
import { Project } from '../../interfaces/project';
import { Customer } from '../../interfaces/customer';
import { InvoiceService } from '../../apiservices/invoice.service';
import { WorkorderReadResponse } from '../../interfaces/response/workorderreadresponse';
import { ProjectService } from '../../apiservices/project.service';
import { CustomerService } from '../../apiservices/customer.service';

@Component({
  selector: 'app-editworkorder',
  standalone: true,
  imports: [FormsModule, NgIf, NgForOf, ReactiveFormsModule, RouterLink],
  templateUrl: './editworkorder.component.html',
  styleUrl: './editworkorder.component.css'
})
export class EditWorkorderComponent {
  urlWorkorderId: number | null = null;

  workorder: Workorder | null = null;
  invoices: Invoice[] = [];
  projects: Project[] = [];
  customers: Customer[] = [];

  standardRequest: any;

  loaded: boolean = false;

  workorderForm: FormGroup = this.fb.group({
    startDate: ['', Validators.required],
    startTime: ['', Validators.required],
    stopDate: ['', Validators.required],
    stopTime: ['', Validators.required],
    description: ['', Validators.required],
    projectName: [''],
    customerName: [''],
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private stateService: StateService,
    private invoiceService: InvoiceService,
    private workorderService: WorkorderService,
    private projectService: ProjectService,
    private customerService: CustomerService,
    private route: ActivatedRoute) {
    this.standardRequest = this.stateService.createStandardRequest();
    this.urlWorkorderId = parseInt(this.route.snapshot.paramMap.get('id') ?? "0");
  }

  ngOnInit(): void {
    this.readInvoices();
  }

  readInvoices() {
    this.invoiceService
      .list(this.standardRequest)
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
    this.projectService
      .list(this.standardRequest)
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
    this.customerService
      .list(this.standardRequest)
      .subscribe({
        next: (customersResponse) => {
          this.stateService.setState(customersResponse.state);
          if (customersResponse.success) {
            this.customers = customersResponse.customers;
            this.readWorkorder();
          }
        }
      });
  }
  readWorkorder() {
    this.workorderService
      .read({
        workorderId: this.urlWorkorderId ?? 0,
        ...this.standardRequest
      })
      .subscribe({
        next: (response) => {
          this.stateService.setState(response.state);
          if (response.success) {
            if (response.workorder == null) return;
            this.workorder = response.workorder;

            if (this.workorder?.start == null) return;
            const startdate = new Date(this.workorder?.start);
            const startdatestring = startdate.toISOString().split('T')[0];
            const starttimestring = startdate.toISOString().split('T')[1].split('.')[0];

            if (this.workorder?.stop == null) return;
            const stopdate = new Date(this.workorder?.stop);
            const stopdatestring = stopdate.toISOString().split('T')[0];
            const stoptimestring = stopdate.toISOString().split('T')[1].split('.')[0];

            this.workorderForm = this.fb.group({
              id: [this.workorder?.id, Validators.required],
              startDate: [startdatestring, Validators.required],
              startTime: [starttimestring, Validators.required],
              stopDate: [stopdatestring, Validators.required],
              stopTime: [stoptimestring, Validators.required],
              description: [this.workorder?.description, Validators.required],
              projectName: [this.workorder?.projectName],
              customerName: [this.workorder?.customerName],
            });

            this.loaded = true;
          }
        }
      });
  }

  onSubmit(): void {
    if (!this.workorderForm.valid) {
      ValidateForm.validateAllFormFields(this.workorderForm);
      return;
    }
    //return;

    const formData = this.workorderForm.value;
    const start = formData.startDate + ' ' + formData.startTime;
    const stop = formData.stopDate + ' ' + formData.stopTime;
    const workorder: Workorder = {
      start: new Date(start),
      stop: new Date(stop),
      invoiceWorkorders: [],
      workorderAttachments: [],
      ...formData
    };
    this.workorderService
      .update({
        workorder: workorder,
        ...this.standardRequest
      })
      .subscribe({
        next: (response) => {
          this.stateService.setState(response.state);
          if (response.success) {
            this.router.navigate(['/workorders']);
          }
        }
      });
  }
}
