import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MyViewRoutingModule } from './my-view-routing.module';
import { MyViewComponent } from './my-view.component';

@NgModule({
  declarations: [MyViewComponent],
  imports: [
    CommonModule,
    MyViewRoutingModule
  ]
})
export class MyViewModule { }
