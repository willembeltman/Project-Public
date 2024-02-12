import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteCompanyComponent } from './deletecompany.component';

describe('DeletecompanyComponent', () => {
  let component: DeleteCompanyComponent;
  let fixture: ComponentFixture<DeleteCompanyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteCompanyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeleteCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
