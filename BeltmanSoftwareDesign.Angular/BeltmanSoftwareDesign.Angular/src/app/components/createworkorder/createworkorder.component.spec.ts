import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateWorkorderComponent } from './createworkorder.component';

describe('CreateworkorderComponent', () => {
  let component: CreateWorkorderComponent;
  let fixture: ComponentFixture<CreateWorkorderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateWorkorderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateWorkorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
