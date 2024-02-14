import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PortalAnalyticsComponent } from './portalanalytics.component';

describe('PortalAnalyticsComponent', () => {
  let component: PortalAnalyticsComponent;
  let fixture: ComponentFixture<PortalAnalyticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PortalAnalyticsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PortalAnalyticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
