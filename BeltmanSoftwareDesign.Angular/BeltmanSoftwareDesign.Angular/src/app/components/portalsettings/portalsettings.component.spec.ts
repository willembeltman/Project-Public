import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PortalSettingsComponent } from './portalsettings.component';

describe('SettingsComponent', () => {
  let component: PortalSettingsComponent;
  let fixture: ComponentFixture<PortalSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PortalSettingsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PortalSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
