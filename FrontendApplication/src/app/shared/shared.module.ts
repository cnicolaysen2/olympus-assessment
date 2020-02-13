import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgZorroAntdModule} from 'ng-zorro-antd';
import {HttpClientModule} from '@angular/common/http';
import { CardComponent } from './card/card.component';
import {ZoomElementDirective} from "../directives/zoom-element.directive";

const COMPONENTS = [
  CardComponent,
];

const DIRECTIVES = [
  ZoomElementDirective
];

const MODULES = [
  NgZorroAntdModule,
  FormsModule,
  HttpClientModule,
  ReactiveFormsModule
];

@NgModule({
  declarations: [
    ...COMPONENTS,
    ...DIRECTIVES
  ],
  imports: [
    CommonModule,
    ...MODULES
  ],
  exports: [
    ...MODULES,
    ...COMPONENTS,
    ...DIRECTIVES
  ],
})
export class SharedModule { }
