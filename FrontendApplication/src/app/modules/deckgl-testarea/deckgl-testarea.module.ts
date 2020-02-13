import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DeckglTestareaRoutingModule } from './deckgl-testarea-routing.module';
import { DeckglTestareaComponent } from './deckgl-testarea/deckgl-testarea.component';
import { DeckglLayersComponent } from './deckgl-layers/deckgl-layers.component';

@NgModule({
  declarations: [DeckglTestareaComponent, DeckglLayersComponent],
  imports: [
    CommonModule,
    DeckglTestareaRoutingModule
  ]
})
export class DeckglTestareaModule { }
