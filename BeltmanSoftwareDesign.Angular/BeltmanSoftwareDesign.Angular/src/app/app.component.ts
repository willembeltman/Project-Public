import { Component, OnInit, Renderer2 } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { NgIf } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { StateService } from './services/state.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CustomInterceptor } from './interceptors/custom.interceptor';

enum TestEnum {
  Type1 = 0,
  Type2 = 1,
  Type3 = 3,
}


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterLink, RouterOutlet, NgIf, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: CustomInterceptor,
    multi: true
  }]
})
export class AppComponent implements OnInit {
  title = 'Beltman Software Design';

  constructor(private stateService: StateService, private renderer: Renderer2) {


  }

  ngOnInit(): void {
    this.renderer.listen('window', 'DOMContentLoaded', () => {
        this.stateService.localStorageReady();
    });
    
  }
  
}
