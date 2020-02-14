import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class ChartService {

    private eventChartVisibility = new EventEmitter<Array<any>>();

    public getChartVisibilityEmitter(): Observable<any> {
        return this.eventChartVisibility;
    }

    // chartService.getChartVisibilityEmitter().subscribe((data) => console.log(data));

    public setChartVisibility(chartId) {
        this.eventChartVisibility.emit(chartId);
    }
}
