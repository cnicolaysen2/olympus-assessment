import { Component, OnInit } from '@angular/core';
import * as Plotly from 'plotly.js/dist/plotly';
import { AngularFirestore } from '@angular/fire/firestore';
import { OlympusStore } from '../../../ngrx/models';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../ngrx/store';
import { VolcanoRangeSetAction } from '../../../ngrx/actions';
import { OlympusStoreService } from '../../../services/firestore.service';

@Component({
  selector: 'app-volcano-range',
  templateUrl: './volcano-range.component.html',
  styleUrls: ['./volcano-range.component.scss']
})
export class VolcanoRangeComponent implements OnInit {

  constructor(private afs: AngularFirestore,
              public store: Store<fromRoot.State>,
              public service: OlympusStoreService) { }

  ngOnInit() {

    Plotly.d3.csv('./assets/data/volcano_db.csv', (err, rows) => {
      function unpack(rowsy, key) {
        return rowsy.map((row) => {
          return row[key];
        });
      }

      const data = [{
        y: unpack(rows, 'Elev'),
        type: 'histogram',
        showlegend: false,
        yaxis: 'x',
        xaxis: 'y',
        marker: {
          color: 'red'
        }
      }];

      const layout = {
        paper_bgcolor: 'black',
        plot_bgcolor: 'black',
        font: { color: 'white' },
        margin: {
          l: 30,
          r: 10,
          b: 20,
          t: 10,
        },
        width: 150,
        yaxis: {
          tickangle: 90,
          anchor: 'x',
          ticksuffix: 'm',
        },
        xaxis: {
          anchor: 'y',
          showgrid: false,
          fixedrange: true
        },
      };

      Plotly.plot('volcano-range', data, layout, { responsive: true , showLink: false })
      .then(plotlyInstance => {
        plotlyInstance.on('plotly_relayout', (value) => {
          if (value['yaxis.autorange'] === undefined) {
            this.setValue([value['yaxis.range[0]'], value['yaxis.range[1]']]);
          } else {
            this.setValue([-6000, 7000]);
          }
        });
      });

    });
  }

  setValue(value) {
    this.service.setVolcanoRange(value);
    // this.store.dispatch(new VolcanoRangeSetAction(value));
  }
}
