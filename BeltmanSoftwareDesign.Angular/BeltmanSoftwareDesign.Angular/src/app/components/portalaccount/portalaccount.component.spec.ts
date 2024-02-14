import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PortalAccountComponent } from './portalaccount.component';

describe('ManageComponent', () => {
  let component: PortalAccountComponent;
  let fixture: ComponentFixture<PortalAccountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PortalAccountComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PortalAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
