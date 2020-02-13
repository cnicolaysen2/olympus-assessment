import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GridRoutingModule } from './grid-routing.module';
import { GridComponent } from './grid.component';
import {SharedModule} from '../../shared/shared.module';
import {GridsterModule} from 'angular-gridster2';
import {UnityViewModule} from '../unity-view/unity-view.module';
import {UnityViewComponent} from '../unity-view/unity-view.component';
import { StreamOutComponent } from '../stream/stream-out/stream-out.component';
import { StreamInLibModule } from 'stream-in-lib';
import { Volcano1Component } from '../stream/volcano/volcano1.component';
import { Volcano2Component } from '../stream/volcano/volcano2.component';
import { VolcanoRangeComponent } from '../stream/volcano/volcano-range.component';

@NgModule({
  declarations: [
    GridComponent,
    StreamOutComponent,
    Volcano1Component,
    Volcano2Component,
    VolcanoRangeComponent
  ],
  imports: [
    CommonModule,
    GridRoutingModule,
    SharedModule,
    GridsterModule,
    UnityViewModule,
    StreamInLibModule
  ]
})
export class GridModule { }
