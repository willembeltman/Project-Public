import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { StateService } from '../../services/state.service';

@Component({
  selector: 'app-portalsettings',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './portalsettings.component.html',
  styleUrl: './portalsettings.component.css'
})
export class PortalSettingsComponent {

  constructor(private stateService: StateService) {

  }

  getUserId() {
    return this.stateService.getState()?.user?.id;
  }
  getCurrentCompanyId() {
    return this.stateService.getState()?.currentCompany?.id ?? 0;
  }
  getCurrentCompanyName() {
    return this.stateService.getState()?.currentCompany?.name ?? "";
  }
}
