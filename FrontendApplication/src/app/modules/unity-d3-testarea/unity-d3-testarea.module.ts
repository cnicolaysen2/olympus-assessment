import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UnityD3TestareaRoutingModule } from './unity-d3-testarea-routing.module';
import { UnityD3TestareaComponent } from './unity-d3-testarea.component';

@NgModule({
  declarations: [UnityD3TestareaComponent],
  imports: [
    CommonModule,
    UnityD3TestareaRoutingModule
  ]
})
export class UnityD3TestareaModule { }
