import { Component, OnInit } from '@angular/core';
import * as Plotly from 'plotly.js/dist/plotly';
import { AngularFirestore } from '@angular/fire/firestore';
import { OlympusStore, Volcano } from '../../../ngrx/models';

@Component({
  selector: 'app-volcano2',
  templateUrl: './volcano2.component.html',
  styleUrls: ['./volcano2.component.scss']
})
export class Volcano2Component implements OnInit {
  private dataOrig;
  public layout2;
  public trace2;

  constructor(private afs: AngularFirestore) { }

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


      this.trace2 = {
        x: this.unpackData(rows, 'Status'),
        y: this.unpackData(rows, 'Type'),
        z: this.unpackData(rows, 'Elev'),
        xaxis: 'x2',
        yaxis: 'y2',
        zaxis: 'z2',
        marker: {
          size: 2,
          color: this.unpackData(rows, 'Elev'),
          colorscale: 'Reds',
          line: { color: 'transparent' }
        },
        mode: 'markers',
        type: 'scatter3d',
        text: this.unpackData(rows, 'Country'),
        hoverinfo: 'x+y+z+text',
        showlegend: false
      };

      this.layout2 = {
        autosize: true,
        paper_bgcolor: 'black',
        plot_bgcolor: 'black',
        title: 'Volcano Elev Scatter3D',
        font: { color: 'white' },
        margin: {t: 30, b: 10, r: 0, l: 0},
        scene: {

          xaxis: {
            title: 'Status',
            showticklabels: false,
            showgrid: true,
            gridcolor: 'white'
          },
          yaxis: {
            title: 'Type',
            showticklabels: false,
            showgrid: true,
            gridcolor: 'white'
          },
          zaxis: {
            title: 'Elev',
            showgrid: true,
            gridcolor: 'white'
          }
        },
      };

      Plotly.plot('volcano2', [this.trace2], this.layout2);
      this.getValue();
    });
  }

  getValue() {
    const docRef = this.afs.collection<OlympusStore>('OlympusStore').doc('volcano');
    docRef.valueChanges().subscribe((doc) => {
        const value = doc as Volcano;
        const cleanData = this.filterElev(this.dataOrig, value.range);

        this.trace2.x = this.unpackData(cleanData, 'Status');
        this.trace2.y = this.unpackData(cleanData, 'Type');
        this.trace2.z = this.unpackData(cleanData, 'Elev');
        this.trace2.text = this.unpackData(cleanData, 'Country');
        this.trace2.marker.color = this.unpackData(cleanData, 'Elev');

        Plotly.react('volcano2', [this.trace2], this.layout2);
    });
  }
}
