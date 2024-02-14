import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PortalCompanyComponent } from './portalcompany.component';

describe('CurrentcompanyComponent', () => {
  let component: PortalCompanyComponent;
  let fixture: ComponentFixture<PortalCompanyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PortalCompanyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PortalCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
