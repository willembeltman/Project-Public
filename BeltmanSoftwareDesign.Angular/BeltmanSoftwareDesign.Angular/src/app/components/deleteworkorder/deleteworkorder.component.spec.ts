import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteWorkorderComponent } from './deleteworkorder.component';

describe('DeleteworkorderComponent', () => {
  let component: DeleteWorkorderComponent;
  let fixture: ComponentFixture<DeleteWorkorderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteWorkorderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeleteWorkorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
