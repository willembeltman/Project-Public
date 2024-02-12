import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginSuccesComponent } from './loginsucces.component';

describe('LoginsuccesComponent', () => {
  let component: LoginSuccesComponent;
  let fixture: ComponentFixture<LoginSuccesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginSuccesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LoginSuccesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
