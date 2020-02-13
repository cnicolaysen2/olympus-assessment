import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnityD3TestareaComponent } from './unity-d3-testarea.component';

describe('UnityD3TestareaComponent', () => {
  let component: UnityD3TestareaComponent;
  let fixture: ComponentFixture<UnityD3TestareaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnityD3TestareaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnityD3TestareaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
