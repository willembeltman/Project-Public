import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterSuccesComponent } from './registersucces.component';

describe('RegistersuccesComponent', () => {
  let component: RegisterSuccesComponent;
  let fixture: ComponentFixture<RegisterSuccesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterSuccesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegisterSuccesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
