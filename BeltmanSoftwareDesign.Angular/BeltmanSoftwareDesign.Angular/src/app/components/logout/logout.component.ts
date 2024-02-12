import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StateService } from '../../services/state.service';

@Component({
  selector: 'app-logout',
  standalone: true,
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent implements OnInit {

  constructor(
    private router: Router,
    private state: StateService) {
  }

  ngOnInit(): void {
    this.state.setState(null);
    this.router.navigate(['/logoutsucces']);
  }
}
