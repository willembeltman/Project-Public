import { Component } from '@angular/core';
import { StateService } from '../../services/state.service';

@Component({
  selector: 'app-portalanalytics',
  standalone: true,
  imports: [],
  templateUrl: './portalanalytics.component.html',
  styleUrl: './portalanalytics.component.css'
})
export class PortalAnalyticsComponent {

  constructor(private stateService: StateService) {

  }

  getCurrentCompanyName() {
    return this.stateService.getState()?.currentCompany?.name ?? "";
  }
}
