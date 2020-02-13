import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeckglLayersComponent } from './deckgl-layers.component';

describe('DeckglLayersComponent', () => {
  let component: DeckglLayersComponent;
  let fixture: ComponentFixture<DeckglLayersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeckglLayersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeckglLayersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
