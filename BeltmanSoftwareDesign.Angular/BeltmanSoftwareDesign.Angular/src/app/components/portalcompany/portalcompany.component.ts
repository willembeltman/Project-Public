import { Component } from '@angular/core';
import { StateService } from '../../services/state.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-portalcompany',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './portalcompany.component.html',
  styleUrl: './portalcompany.component.css'
})
export class PortalCompanyComponent {

  constructor(private stateService: StateService) {

  }

  getCurrentCompanyId() {
    return this.stateService.getState()?.currentCompany?.id ?? 0;
  }
  getCurrentCompanyName() {
    return this.stateService.getState()?.currentCompany?.name ?? "no company created";
  }
}
