import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotLoggedInComponent } from './notloggedin.component';

describe('NotloggedinComponent', () => {
  let component: NotLoggedInComponent;
  let fixture: ComponentFixture<NotLoggedInComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NotLoggedInComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NotLoggedInComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
