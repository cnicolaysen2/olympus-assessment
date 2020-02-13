import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';

import { UnityViewRoutingModule } from './unity-view-routing.module';
import { UnityViewComponent } from './unity-view.component';
import {SharedModule} from '../../shared/shared.module';
import {ScriptLoaderService} from './script-loader.service';
import {ChartsModule} from "../charts/charts.module";

@NgModule({
  declarations: [UnityViewComponent],
  imports: [
    CommonModule,
    SharedModule,
    UnityViewRoutingModule,
    ChartsModule
  ],
  providers: [
    ScriptLoaderService
  ],
  entryComponents: [
    UnityViewComponent
  ],
  exports: [
    UnityViewComponent
  ],
})
export class UnityViewModule { }
