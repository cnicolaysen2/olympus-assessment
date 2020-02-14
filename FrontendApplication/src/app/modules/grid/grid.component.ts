import {
  ChangeDetectionStrategy, ChangeDetectorRef, Component, ComponentFactoryResolver, HostListener, OnInit, ViewChild,
  ViewEncapsulation
} from '@angular/core';

import {CompactType, DisplayGrid, GridsterConfig, GridsterItem, GridType} from 'angular-gridster2';
import {UnityViewComponent} from '../unity-view/unity-view.component';

import Selection from 'node_modules/@simonwep/selection-js/src/selection';
import * as _ from 'node_modules/@simonwep/selection-js/src/utils';

import {GridsterDragndropService} from '../../services/gridster-dragndrop.service';


@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class GridComponent implements OnInit {


  get toolbarStatus(): boolean {
    return this._toolbarStatus;
  }

  set toolbarStatus(value: boolean) {
    this._toolbarStatus = value;
  }

  get toolbarPosition(): {x: number, y: number} {
    return this._toolbarPosition;
  }

  set toolbarPosition(value: {x: number, y: number}) {
    this._toolbarPosition = value;
  }

  get zoom() {
    return this._zoom;
  }

  set zoom(value) {
    this._zoom = value;
  }

  @ViewChild('gridsterContainer') gridsterContainer;
  @ViewChild('tools') tools;


  options: GridsterConfig;
  dashboard: GridsterItem[];
  private _toolbarStatus = false;
  private _toolbarPosition = {x: 0, y: 0};
  private _zoom;
  selection;
  gridsterState;

  ngOnInit() {



    // gridster
    // All values of options is correct. Please be careful when you changes any options in this file.
    // Because it's can broke some keys.
    this.options = {
        gridType: GridType.Fixed,
        compactType: CompactType.None,
        margin: 5,
        outerMargin: true,
        outerMarginTop: null,
        outerMarginRight: null,
        outerMarginBottom: null,
        outerMarginLeft: null,
        useTransformPositioning: true,
        mobileBreakpoint: 640,
        minCols: 1,
        maxCols: 100,
        minRows: 1,
        maxRows: 100,
        maxItemCols: 100,
        minItemCols: 1,
        maxItemRows: 100,
        minItemRows: 1,
        maxItemArea: 2500,
        minItemArea: 1,
        defaultItemCols: 4,
        defaultItemRows: 3,
        fixedColWidth: 60,
        fixedRowHeight: 60,
        keepFixedHeightInMobile: false,
        keepFixedWidthInMobile: false,
        scrollSensitivity: 10,
        scrollSpeed: 20,
        enableEmptyCellClick: false,
        enableEmptyCellContextMenu: false,
        enableEmptyCellDrop: true,
        enableEmptyCellDrag: false,
        emptyCellClickCallback: this.emptyCellClick.bind(this),
        emptyCellContextMenuCallback: this.emptyCellClick.bind(this),
        // emptyCellDropCallback: this.emptyCellClick.bind(this),
        emptyCellDropCallback: this.drop.bind(this),
        emptyCellDragCallback: this.emptyCellClick.bind(this),
        emptyCellDragMaxCols: 50,
        emptyCellDragMaxRows: 50,
        ignoreMarginInRow: false,
        draggable: {
            enabled: true,
        },
        resizable: {
            enabled: true,
        },
        swap: true,
        pushItems: true,
        disablePushOnDrag: true,
        disablePushOnResize: false,
        pushDirections: {north: true, east: true, south: true, west: true},
        pushResizeItems: false,
        displayGrid: DisplayGrid.OnDragAndResize,
        disableWindowResize: false,
        disableWarnings: false,
        scrollToNewItems: false
    };

    this.dashboard = [
        /*// {cols: 5, rows: 6, y: 0, x: 0, dragEnabled: false, resizeEnabled: false, component: 'UnityViewComponent'},
        {cols: 10, rows: 6, y: 0, x: 0, type: 'component', name: 'map'},
        {cols: 6, rows: 6, y: 0, x: 10, type: 'component', name: '3d'},
        {cols: 9, rows: 6, y: 0, x: 16, type: 'component'},
        {cols: 15, rows: 4, y: 6, x: 10, type: 'component'},
        {cols: 6, rows: 6, y: 10, x: 10, type: 'component'},
        {cols: 6, rows: 6, y: 10, x: 16, type: 'component'},
        {cols: 6, rows: 6, y: 10, x: 22, type: 'component'},*/
    ];

    this.initSelection();


  }

  initSelection() {
    // Selective tool
    const self = this;
    this.selection = new Selection({


      // Query selectors for elements from where a selection can be start
      startareas: ['.gridster-container-wrap'],

      // Query selectors for elements which will be used as boundaries for the selection
      boundaries: ['html'],

      // All elements in this container can be selected
      containers: ['.gridster-container-wrap'],

      // All elemets with the class 'selectable' selectable.
      selectables: ['gridster-item'],

      startThreshold: 10,

      singleClick: true,

      onSelect(evt) {
        // Check if clicked element is already selected
        const selected = evt.target.classList.contains('selected');

        // Remove class if the user isn't pressing the control key or ⌘ key and the
        // current target is already selected
        if (!evt.originalEvent.ctrlKey && !evt.originalEvent.metaKey) {

          // Remove class from every element that is selected
          evt.selectedElements.forEach(s => s.classList.remove('selected'));

          // Clear previous selection
          this.clearSelection();
        }

        if (!selected) {
          // Select element
          evt.target.classList.add('selected');
          this.keepSelection();
        } else {
          // Unselect element
          evt.target.classList.remove('selected');
          this.removeFromSelection(evt.target);
        }
      },

      onStart({selectedElements, originalEvent, eventName, areaElement}) {
        self._toolbarStatus = false;
        self.cdr.detectChanges();
        // console.log(originalEvent, eventName, areaElement);
        // this.clearSelection();

        areaElement.style.width = parseInt(areaElement.style.width) / self.zoom + 'px';
        areaElement.style.height = parseInt(areaElement.style.height) / self.zoom + 'px';
        areaElement.style.top = parseInt(areaElement.style.top) / self.zoom + 'px';
        areaElement.style.bottom = parseInt(areaElement.style.bottom) / self.zoom + 'px';
        areaElement.style.left = parseInt(areaElement.style.left) / self.zoom + 'px';
        areaElement.style.right = parseInt(areaElement.style.right) / self.zoom + 'px';

        document.querySelectorAll('div.selection-area').forEach((item) => {
          (item as any).style.zoom = self.zoom;
        });

        // } else {
        //   this.enable();
        // }

        // Remove class if the user isn't pressing the control key or ⌘ key
        if (!originalEvent.ctrlKey && !originalEvent.metaKey) {

          // Unselect all elements
          selectedElements.forEach(s => s.classList.remove('selected'));

          // Clear previous selection
          this.clearSelection();
        }
      },

      onMove({originalEvent, areaElement}) {
        // if (originalEvent){
        //   console.log(this);
        //   console.log(originalEvent);
        //   originalEvent.preventDefault();
        //   this.cancel();
        //   this.clearSelection();
        // }

        // {selectedElements, changedElements, originalEvent}
        // console.log(selectedElements)
        // console.log(areaElement.style.width);
        // console.log(self.zoom);
        areaElement.style.width = parseInt(areaElement.style.width) / self.zoom + 'px';
        areaElement.style.height = parseInt(areaElement.style.height) / self.zoom + 'px';
        areaElement.style.top = parseInt(areaElement.style.top) / self.zoom + 'px';
        areaElement.style.bottom = parseInt(areaElement.style.bottom) / self.zoom + 'px';
        areaElement.style.left = parseInt(areaElement.style.left) / self.zoom + 'px';
        areaElement.style.right = parseInt(areaElement.style.right) / self.zoom + 'px';
        // Add a custom class to the elements that where selected.
        // console.log(selectionEvent);


        // console.log(areaElement.getBoundingClientRect());

        const gridsterItems = document.querySelectorAll('gridster-item');
        const activedItems = [];

        gridsterItems.forEach((node) => {
          const demension = node.getBoundingClientRect();
          const zoomedNode = demension;

          if (_.intersects(areaElement.getBoundingClientRect(), zoomedNode)) {
            // console.log(areaElement.getBoundingClientRect(), zoomedNode, node);
            // console.log(node);
            node.classList.add('selected');
            activedItems.push(node);
          } else {
            node.classList.remove('selected');
          }
        });

        self.gridsterState = activedItems;
      },

      onStop({originalEvent, areaElement}) {
        // -35 because minus nav
        // -10 because it's good UX pattern design
        // console.log(originalEvent);
        self._toolbarStatus = true;
        self._toolbarPosition = {
          x: originalEvent.clientX - 10,
          y: originalEvent.clientY - 10 - 35 ,
        };

        // areaElement.style.width = parseInt(areaElement.style.width)/self.zoom + 'px';
        // areaElement.style.height = parseInt(areaElement.style.height)/self.zoom + 'px';
        // areaElement.style.top = parseInt(areaElement.style.top)/self.zoom + 'px';
        // areaElement.style.bottom = parseInt(areaElement.style.bottom)/self.zoom + 'px';
        // areaElement.style.left = parseInt(areaElement.style.left)/self.zoom + 'px';
        // areaElement.style.right = parseInt(areaElement.style.right)/self.zoom + 'px';

        self.cdr.detectChanges();
        this.keepSelection();
        if (originalEvent) {
          originalEvent.preventDefault();
          originalEvent.stopPropagation();
        }
      }
    });
  }

  toolbar(event) {
    event.preventDefault();
    event.stopPropagation();
  }

  changedOptions() {
      if (this.options.api && this.options.api.optionsChanged) {
          this.options.api.optionsChanged();
      }
  }

  cancelSelection(event: Event) {
    // disable context menu
    event.preventDefault();
    event.stopPropagation();
    const selected = this.gridsterContainer.el.childNodes;

    if (selected) {
      // Unselect element
      selected.forEach((item) => {
        if (item.classList && item.classList.contains('selected')) {
          item.classList.toggle('selected');
        }
      });
      this._toolbarStatus = false;
      this.selection.removeFromSelection(event.target);
    }

    this.selection.clearSelection();
    return false;
  }

  emptyCellClick(event: MouseEvent, item: GridsterItem) {
    console.info('empty cell click', event, item);
    this.dashboard.push(item);
    this.gridsterDragndropService.dataTransfer;
    console.log(this.gridsterDragndropService.dataTransfer);
  }

  toggleZoom() {
    console.log('zoom');
  }

  drop(event: MouseEvent, item: GridsterItem) {
    console.info('empty cell click', event, item, arguments);
    item.cols = this.gridsterDragndropService.dataTransfer.cols;
    item.rows = this.gridsterDragndropService.dataTransfer.rows;
    item.type = this.gridsterDragndropService.dataTransfer.type;
    item.name = this.gridsterDragndropService.dataTransfer.name;
    this.options.minItemCols = this.gridsterDragndropService.dataTransfer.cols;
    this.options.minItemRows = this.gridsterDragndropService.dataTransfer.rows;

    this.cdr.detectChanges();
    console.log(item);

    this.dashboard.push(item);
  }

  deleteActive() {
    const removeItems = {};
    for (let i = 0; i < this.gridsterState.length; i++) {
      const gridster = this.gridsterState[i];
      if (gridster.localName === 'gridster-item' && gridster.classList.contains('selected')) {
        // console.log(gridster.getAttribute("data-index"));
        removeItems[gridster.getAttribute('data-index')] = this.dashboard[gridster.getAttribute('data-index')];

        // gridster.parentNode.removeChild(gridster);
      }
    }
    console.log(removeItems);

    for (const index in removeItems) {
      this.removeItem(null, removeItems[index]);
    }
    this.toolbarStatus = false;
    // console.log(this.gridsterState);
  }

  removeItem($event, item) {
    if ($event) {
      $event.preventDefault();
      $event.stopPropagation();
    }
    this.dashboard.splice(this.dashboard.indexOf(item), 1);
    // this.cdr.detach();
    // this.cdr.detectChanges();
  }

  addItem() {
      this.dashboard.push({x: 0, y: 0, cols: 4, rows: 3});
  }
  // tools

  constructor(
      private componentFactoryResolver: ComponentFactoryResolver,
      private cdr: ChangeDetectorRef,
      private gridsterDragndropService: GridsterDragndropService) {

  }
}
