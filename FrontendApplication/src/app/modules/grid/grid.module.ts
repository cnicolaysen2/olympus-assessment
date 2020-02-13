import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GridRoutingModule } from './grid-routing.module';
import { GridComponent } from './grid.component';
import {SharedModule} from '../../shared/shared.module';
import {GridsterModule} from 'angular-gridster2';
import {UnityViewModule} from '../unity-view/unity-view.module';
import {UnityViewComponent} from '../unity-view/unity-view.component';

@NgModule({
  declarations: [GridComponent],
  imports: [
    CommonModule,
    GridRoutingModule,
    SharedModule,
    GridsterModule,
    UnityViewModule
  ]
})
export class GridModule { }
