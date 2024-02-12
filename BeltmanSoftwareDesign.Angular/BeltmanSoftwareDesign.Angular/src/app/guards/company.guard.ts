import { StateService } from '../services/state.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CompanyGaurd implements CanActivate {
  constructor(private state: StateService, private router: Router) {

  }
  canActivate(): boolean {
    if (
      this.state.getState()?.user != null &&
      this.state.getState()?.user != undefined &&
      this.state.getState()?.currentCompany != null &&
      this.state.getState()?.currentCompany != undefined) {
      return true;
    }
    else {
      this.router.navigate(['/nocompany']);
      return false;
    }
  }
}
