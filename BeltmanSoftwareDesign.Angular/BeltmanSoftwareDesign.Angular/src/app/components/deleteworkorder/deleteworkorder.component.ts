import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { StateService } from '../../services/state.service';
import { CountriesService } from '../../apiservices/countries.service';
import { WorkordersService } from '../../apiservices/workorders.service';
import { Workorder } from '../../interfaces/workorder';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-deleteworkorder',
  standalone: true,
  imports: [NgIf, RouterLink],
  templateUrl: './deleteworkorder.component.html',
  styleUrl: './deleteworkorder.component.css'
})
export class DeleteWorkorderComponent {
  workorderId: number | null = 0;
  workorder: Workorder | null = null;
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
    this.workordersService
      .read({
        bearerId: this.stateService.getState()?.bearerId ?? null,
        currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null,
        workorderId: this.workorderId ?? 0
      })
      .subscribe({
        next: (response) => {
          if (response.success) {
            let workorder = response.workorder;
            if (workorder != null) {
              this.workorder = workorder;
              this.loaded = true;
            }
          }
        }
      });
  }

  onSubmit(): void {
    if (confirm("Are you sure?")) {
      this.workordersService
        .delete({
          bearerId: this.stateService.getState()?.bearerId ?? null,
          currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null,
          workorderId: this.workorderId ?? 0
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

}
