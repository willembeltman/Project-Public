import { Component, OnInit } from '@angular/core';
import { StateService } from '../../services/state.service';
import { Workorder } from '../../interfaces/workorder';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WorkorderService } from '../../apiservices/workorder.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-workorders',
  standalone: true,
  imports: [FormsModule, NgIf, NgFor, RouterLink, DatePipe],
  templateUrl: './workorders.component.html',
  styleUrl: './workorders.component.css'
})
export class WorkordersComponent implements OnInit {

  public workorders: Workorder[] | null = null;

  constructor(
    private workorderService: WorkorderService,
    private state: StateService) {
    
  }

  ngOnInit(): void {
    let companyid: number | null = null;
    const companyidfromstate = this.state.getState()?.user?.currentCompanyId;
    if (companyidfromstate != null && companyidfromstate != undefined) {
      companyid = companyidfromstate;
    }
    let bearerId: string | null = null;
    var bearerIdFromState = this.state.getState()?.bearerId;
    if (bearerIdFromState != null && bearerIdFromState != undefined) {
      bearerId = bearerIdFromState;
    }

    this.workorderService
      .list({
        bearerId: bearerId,        
        currentCompanyId: companyid
      })
      .subscribe(
        (response) => {
          this.workorders = response.workorders;
          this.state.setState(response.state);
        },
        (error) => {
          console.error('List workorders failed:', error);
        });
  }

}
