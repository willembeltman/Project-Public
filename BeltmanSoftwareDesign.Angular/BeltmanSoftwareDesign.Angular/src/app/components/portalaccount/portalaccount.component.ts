import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { StateService } from '../../services/state.service';

@Component({
  selector: 'app-portalaccount',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './portalaccount.component.html',
  styleUrl: './portalaccount.component.css'
})
export class PortalAccountComponent {
  constructor(private stateService: StateService) {

  }
  getUserId() {
    return this.stateService.getState()?.user?.id;
  }
  getUserName() {
    return this.stateService.getState()?.user?.userName;
  }
}
