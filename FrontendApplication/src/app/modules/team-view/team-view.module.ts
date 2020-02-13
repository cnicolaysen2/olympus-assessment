import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeamViewRoutingModule } from './team-view-routing.module';
import { TeamViewComponent } from './team-view.component';

@NgModule({
  declarations: [TeamViewComponent],
  imports: [
    CommonModule,
    TeamViewRoutingModule
  ]
})
export class TeamViewModule { }
