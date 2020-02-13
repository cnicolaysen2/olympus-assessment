import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StratigraphyChartComponent } from './stratigraphy-chart.component';

describe('StratigraphyChartComponent', () => {
  let component: StratigraphyChartComponent;
  let fixture: ComponentFixture<StratigraphyChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StratigraphyChartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StratigraphyChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
