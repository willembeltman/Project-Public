import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditWorkorderComponent } from './editworkorder.component';

describe('EditworkorderComponent', () => {
  let component: EditWorkorderComponent;
  let fixture: ComponentFixture<EditWorkorderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditWorkorderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditWorkorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
