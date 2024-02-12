import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentcompanyComponent } from './currentcompany.component';

describe('CurrentcompanyComponent', () => {
  let component: CurrentcompanyComponent;
  let fixture: ComponentFixture<CurrentcompanyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CurrentcompanyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CurrentcompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
