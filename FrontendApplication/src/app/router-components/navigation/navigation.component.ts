import { Component } from '@angular/core';
import {NavigationService} from "./navigation.service";
import {GridsterDragndropService} from "../../services/gridster-dragndrop.service";

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {

  constructor(
    public navigationService: NavigationService,
    public gridsterDragndropService: GridsterDragndropService){}

  tabs = [1, 2, 3];
  isCollapsed = true;
  addComponentSidebarStatus = false;
  switchValue;

  dragStartHandler(ev, obj) {
    ev.dataTransfer.setData ('text/plain', 'Drag Me Button');
    ev.dataTransfer.dropEffect = 'copy';
    this.gridsterDragndropService.dataTransfer = obj;
  }

  addComponent(){

  }
}
