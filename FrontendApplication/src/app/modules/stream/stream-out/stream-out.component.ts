import { Component, OnInit } from '@angular/core';
import * as Plotly from 'plotly.js/dist/plotly';
import { AngularFirestore } from '@angular/fire/firestore';
import { OlympusStore } from '../../../ngrx/models';

@Component({
  selector: 'app-stream-out',
  templateUrl: './stream-out.component.html',
  styleUrls: ['./stream-out.component.scss']
})
export class StreamOutComponent implements OnInit {

  constructor(private afs: AngularFirestore) { }

  ngOnInit() {
    const layout = {
      height: '380',
      width: '650',
      responsive: true,
    };

    Plotly.plot('graph', [{
      y: [1].map(this.rand),
      mode: 'lines',
      line: { color: '#80CAF6' }
    }], layout);

    let oldVal = 0;

    const interval = setInterval(() => {
      const value = this.rand();

      Plotly.extendTraces('graph', {
        y: [[value]]
      }, [0]);

      this.setValue(value);
      oldVal = value;

    }, 200);
  }

  rand() {
    return (Math.random() * 2) - 1;
  }

  setValue(value) {
    const docRef = this.afs.collection<OlympusStore>('OlympusStore').doc('randomwalk');
    docRef.update({
      ['tip']: value
    }).catch((reason) => {
      console.log(reason.message);
    });
  }
}
