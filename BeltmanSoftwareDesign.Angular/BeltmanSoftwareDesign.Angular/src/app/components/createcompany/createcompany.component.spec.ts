import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCompanyComponent } from './createcompany.component';

describe('CreatecompanyComponent', () => {
  let component: CreateCompanyComponent;
  let fixture: ComponentFixture<CreateCompanyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateCompanyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
