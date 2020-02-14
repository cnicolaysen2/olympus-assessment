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

  draggableComponents = {
    tabs: {
      layers: [
        {
          name: 'layout-correlation-calib',
          cols: 16,
          rows: 8
        },
        {
          name: 'layout-sweet-spot',
          cols: 16,
          rows: 8
        },
        {
          name: 'layout-well-correlation',
          cols: 16,
          rows: 8
        }
      ],
      components: []
    }
  };

  tabs = [1, 2, 3];
  isCollapsed = true;
  addComponentSidebarStatus = false;
  switchValue;

  dragStartHandler(ev, obj) {
    ev.dataTransfer.setData ('text/plain', 'Drag Me Button');
    ev.dataTransfer.dropEffect = 'copy';
    this.gridsterDragndropService.dataTransfer = obj;
  }

  addComponent() {

  }
}
