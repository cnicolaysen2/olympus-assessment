import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AngularFirestore } from '@angular/fire/firestore';
import { OlympusStore, Volcano,  } from '../ngrx/models';
import { map } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';

@Injectable()
export class OlympusStoreService {
  private unsub;

  constructor(private afs: AngularFirestore, private snackBar: MatSnackBar) {
  }

  getVolcanoRange(): Observable<Volcano> {
    const docRef = this.afs.collection<Volcano>('OlympusStore').doc('volcano');
    return docRef.valueChanges() as Observable<Volcano>;
  }

  setVolcanoRange(range: Array<number>) {
    const docRef = this.afs.collection<Volcano>('OlympusStore').doc('volcano');
    docRef.update({
      ['range']: range
    }).catch((reason) => {
      console.log(reason.message);
    });
  }
}
