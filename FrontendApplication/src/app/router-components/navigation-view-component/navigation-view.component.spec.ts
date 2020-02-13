import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavigationViewComponentComponent } from './navigation-view-component.component';

describe('NavigationViewComponentComponent', () => {
  let component: NavigationViewComponentComponent;
  let fixture: ComponentFixture<NavigationViewComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavigationViewComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavigationViewComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
