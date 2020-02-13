import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {SharedModule} from "../../shared/shared.module";
import {PieChartComponent} from "./pie-chart/pie-chart.component";
import {LineChartComponent} from "./line-chart/line-chart.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {NgZorroAntdModule} from "ng-zorro-antd";
import {HttpClientModule} from "@angular/common/http";
import { StratigraphyChartComponent } from './stratigraphy-chart/stratigraphy-chart.component';
import { StackedBarChartComponent } from './stacked-bar-chart/stacked-bar-chart.component';

const COMPONENTS = [
  PieChartComponent,
  LineChartComponent,
  StratigraphyChartComponent,
  StackedBarChartComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    CommonModule,
    SharedModule
  ],
  exports: [
    ...COMPONENTS
  ]
})
export class ChartsModule { }
