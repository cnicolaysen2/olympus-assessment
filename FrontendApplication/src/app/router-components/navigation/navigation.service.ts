import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {

  constructor() { }

  private _toggleUnityDrawler;

    get toggleUnityDrawler() {
        return this._toggleUnityDrawler;
    }

    set toggleUnityDrawler(value) {
        this._toggleUnityDrawler = value;
    }



}
