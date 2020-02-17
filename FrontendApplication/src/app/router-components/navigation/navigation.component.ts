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
          icon: 'layout-correlation-calib.svg',
          cols: 16,
          rows: 8
        },
        {
          name: 'layout-sweet-spot',
          icon: 'layout-sweet-spot.svg',
          cols: 16,
          rows: 8
        },
        {
          name: 'layout-well-correlation',
          icon: 'layout-well-correlation.svg',
          cols: 16,
          rows: 8
        }
      ],
      components: [
        {
          name: '3d',
          icon: 'component-3d.svg',
          cols: 4,
          rows: 3
        },
        {
          name: 'chart',
          icon: 'component-chart.svg',
          cols: 4,
          rows: 3
        },
        {
          name: 'correlation',
          icon: 'component-correlation.svg',
          cols: 4,
          rows: 3
        },
        {
          name: 'intersection',
          icon: 'component-intersection.svg',
          cols: 4,
          rows: 3
        },
        {
          name: 'map',
          icon: 'component-map.svg',
          cols: 4,
          rows: 3
        },
        {
          name: 'unity',
          icon: 'component-unity.svg',
          cols: 16,
          rows: 8
        }
      ]
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
