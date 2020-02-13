import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {DeckglTestareaComponent} from './deckgl-testarea/deckgl-testarea.component';
import {DeckglLayersComponent} from './deckgl-layers/deckgl-layers.component';

const routes: Routes = [{
  path: 'layers',
  component: DeckglLayersComponent
}, {
  path: 'testarea',
  component: DeckglTestareaComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DeckglTestareaRoutingModule { }
