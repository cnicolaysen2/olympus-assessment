import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnityViewComponent } from './unity-view.component';

describe('UnityViewComponent', () => {
  let component: UnityViewComponent;
  let fixture: ComponentFixture<UnityViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnityViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnityViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
