import { StateService } from '../services/state.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UserGaurd implements CanActivate {
  constructor(private state: StateService, private router: Router) {

  }
  canActivate(): boolean {
    const state = this.state.getState();
    if (
      state?.user != null &&
      state?.user != undefined) {
      return true;
    }
    else {
      this.router.navigate(['/notloggedin']);
      return false;
    }
  }
}
