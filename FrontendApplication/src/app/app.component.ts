import { Component, OnInit, AfterViewInit, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { VolcanoRangeUpdateAction } from './ngrx/actions';
import { MatSnackBar, MatDialog } from '@angular/material';
import { Store } from '@ngrx/store';
import * as fromRoot from './ngrx/store';
import * as firebase from 'firebase';
import 'firebase/messaging';
import { OAuthCredential } from '@firebase/auth-types';
import { AngularFireAuth } from '@angular/fire/auth';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit, AfterViewInit {
  public title = 'slb-olympus';
  public swRegistration = null;
  public messaging;
  public isDisabled = true;

  constructor(public store: Store<fromRoot.State>,
              private addDialog: MatDialog,
              public snackBar: MatSnackBar,
              public afAuth: AngularFireAuth) {

  }

  ngOnInit() {

  }

  ngAfterViewInit(): void {
  }
}
