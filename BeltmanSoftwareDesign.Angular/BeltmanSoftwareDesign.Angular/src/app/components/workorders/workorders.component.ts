import { Component, OnInit } from '@angular/core';
import { StateService } from '../../services/state.service';
import { Workorder } from '../../interfaces/workorder';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WorkorderService } from '../../apiservices/workorder.service';

@Component({
  selector: 'app-workorders',
  standalone: true,
  imports: [FormsModule, NgIf, NgFor],
  templateUrl: './workorders.component.html',
  styleUrl: './workorders.component.css'
})
export class WorkordersComponent implements OnInit {

  public workorders: Workorder[] = [];

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
