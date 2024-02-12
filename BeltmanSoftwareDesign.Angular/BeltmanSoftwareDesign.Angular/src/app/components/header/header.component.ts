import { Component } from '@angular/core';
import { StateService } from '../../services/state.service';
import { NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NgIf, RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  constructor(private state: StateService) {

  }

  isLoggedIn(): boolean {
    return this.state.getState()?.user != null && this.state.getState()?.user != undefined;
  }

  getUserName() {
    return this.state.getState()?.user?.userName;
  }

  getCurrentCompanyName() {
    return this.state.getState()?.currentCompany?.name ?? "no company selected";
  }
}
