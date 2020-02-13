import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GridsterDragndropService {

  get dataTransfer(): any {
    return this._dataTransfer;
  }

  set dataTransfer(value: any) {
    this._dataTransfer = value;
  }

  private _dataTransfer: any;

  constructor() { }
}
