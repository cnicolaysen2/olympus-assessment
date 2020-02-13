import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {UnityD3TestareaComponent} from './unity-d3-testarea.component';

const routes: Routes = [{
  path: '',
  component: UnityD3TestareaComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UnityD3TestareaRoutingModule { }
