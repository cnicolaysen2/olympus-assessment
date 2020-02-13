import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {UnityViewComponent} from './unity-view.component';

const routes: Routes = [
  {
    path: '',
    component: UnityViewComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UnityViewRoutingModule { }
