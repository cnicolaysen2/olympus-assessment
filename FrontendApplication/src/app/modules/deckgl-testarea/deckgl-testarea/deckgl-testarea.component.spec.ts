import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeckglTestareaComponent } from './deckgl-testarea.component';

describe('DeckglTestareaComponent', () => {
  let component: DeckglTestareaComponent;
  let fixture: ComponentFixture<DeckglTestareaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeckglTestareaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeckglTestareaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
