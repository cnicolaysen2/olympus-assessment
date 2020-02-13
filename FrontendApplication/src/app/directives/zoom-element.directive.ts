import {Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2} from "@angular/core";

@Directive({
  selector: '[appZoomElement]'
})
export class ZoomElementDirective {

  @Output() currentZoom = new EventEmitter<number>();
  @Input() selection: any;

  private _zoom: number = 1;

  get zoom(): number {
    return this._zoom;
  }

  set zoom(value: number) {
    this._zoom = value;
    this.currentZoom.emit(this._zoom)
  }

  constructor(
    private renderer: Renderer2,
    private el: ElementRef
  ){}

  @HostListener('mousewheel', ['$event']) onMousewheel(event) {

    if (event.wheelDelta > 0.1){
      this.zoom += 0.05
    } else if (event.wheelDelta < 0.2){
      this.zoom -= 0.05
    }

    this.el.nativeElement.style.setProperty('zoom', this.zoom);

    document.querySelectorAll('div.selection-area').forEach((item) => {
      (<any>item).style.zoom = this.zoom;
    });

  }

}
