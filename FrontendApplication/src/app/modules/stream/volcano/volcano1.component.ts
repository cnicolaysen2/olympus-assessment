import { Component, OnInit } from '@angular/core';
import * as Plotly from 'plotly.js/dist/plotly';
import { AngularFirestore } from '@angular/fire/firestore';
import { OlympusStore, Volcano } from '../../../ngrx/models';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../ngrx/store';

@Component({
  selector: 'app-volcano1',
  templateUrl: './volcano1.component.html',
  styleUrls: ['./volcano1.component.scss']
})
export class Volcano1Component implements OnInit {
  private dataOrig;
  public layout1;
  public trace1;

  constructor(private afs: AngularFirestore, public store: Store<fromRoot.State>) { }

  unpackData(rows, key) {
    return rows.map((row) => {
      return row[key];
    });
  }

  filterElev(data, value) {
    return data.filter((row) => {
      if (row.Elev > value[0] && row.Elev < value[1]) {
        return row;
      }
    });
  }

  ngOnInit() {
    Plotly.d3.csv('./assets/data/volcano_db.csv', (err, rows) => {
      this.dataOrig = rows;

      this.trace1 = {
        type: 'scattergeo',
        locationmode: 'world',
        lon: this.unpackData(rows, 'Longitude'),
        lat: this.unpackData(rows, 'Latitude'),
        hoverinfo: 'text',
        text: this.unpackData(rows, 'Elev'),
        mode: 'markers',
        showlegend: false,
        marker: {
          size: 4,
          color: this.unpackData(rows, 'Elev'),
          colorscale: 'Reds',
          opacity: 0.8,
          symbol: 'circle',
          line: {
            width: 1
          }
        }
      };

      this.layout1 = {
        autosize: true,
        paper_bgcolor: 'black',
        plot_bgcolor: 'black',
        title: 'Volcano Elev Map',
        font: { color: 'white' },
        colorbar: true,
        margin: {t: 30, b: 20, r: 10, l: 10},
        mapbox: {
          center: { lon: 60, lat: 30 },
          style: 'outdoors',
          zoom: 1
        }
      };

      Plotly.plot('volcano1', [this.trace1], this.layout1);
      this.getValue();
    });
  }

  getValue() {
    // this.store.select(fromRoot.getVolcanoRange)
    // .subscribe((value: Volcano) => {
    //   if (value && value.range && value.range.length > 0) {
    this.store.select(fromRoot.getVolcanoRange).subscribe((value: Volcano) => {
      if (value.range && value.range.length > 0) {
        const cleanData = this.filterElev(this.dataOrig, value.range);

        this.trace1.lon = this.unpackData(cleanData, 'Longitude');
        this.trace1.lat = this.unpackData(cleanData, 'Latitude');
        this.trace1.text = this.unpackData(cleanData, 'Elev');
        this.trace1.marker.color = this.unpackData(cleanData, 'Elev');

        Plotly.react('volcano1', [this.trace1], this.layout1, { showLink: false });
      }
    });
  }
}
