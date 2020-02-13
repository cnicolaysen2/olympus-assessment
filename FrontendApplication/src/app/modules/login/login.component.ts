import { Component, OnInit, AfterViewInit } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../ngrx/store';
import * as firebase from 'firebase';
import { OAuthCredential } from '@firebase/auth-types';
import { VolcanoRangeUpdateAction } from '../../ngrx/actions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, AfterViewInit {

  constructor(public store: Store<fromRoot.State>,
              private router: Router,
              public afAuth: AngularFireAuth) {}

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    // This promise completes when the user gets back from the auth redirect flow.
    this.afAuth.auth.getRedirectResult().then((result) => {
      if (result.credential) {
        const credential: OAuthCredential = result.credential as OAuthCredential;
        const userToken = credential.accessToken;
        localStorage.setItem('tokenId', userToken);
        const user = result.user;
      } else {
      }
    }).catch((error) => {
      // Handle Errors here.
      const errorCode = error.code;
      const errorMessage = error.message;
      // The email of the user's account used.
      const email = error.email;
      // The firebase.auth.AuthCredential type that was used.
      const credential = error.credential;

      console.error(error);
    });

    // This listener is called when the user is signed in or out, and that is where we update the UI
    this.afAuth.auth.onAuthStateChanged((user) => {
      if (user) {
        // User is signed in.
        const displayName = user.displayName;
        const email = user.email;
        const emailVerified = user.emailVerified;
        const photoURL = user.photoURL;
        const isAnonymous = user.isAnonymous;
        const uid = user.uid;
        const providerData = user.providerData;

        console.log('User is signed in.');

        this.store.dispatch(new VolcanoRangeUpdateAction());

        this.router.navigate(['/grid']);

      } else {
        console.log('User is signed out.');
      }
    });
  }

  public loginClick(event) {

    if (!this.afAuth.auth.currentUser) {
      const provider = new firebase.auth.GoogleAuthProvider();

      provider.addScope('https://www.googleapis.com/auth/plus.login');
      provider.addScope('profile');
      provider.addScope('email');
      provider.setCustomParameters({
        login_hint: 'me@slb.com'
      });

      this.afAuth.auth.signInWithRedirect(provider)
      .then(success => {
        console.log('Signed in OK!');
      })
      .catch( error => console.error('ERROR', error));

    } else {

      this.afAuth.auth.signOut()
      .then(() => {
        console.log('Signed out successfully');
      })
      .catch((error) => {
        console.log(error);
      });

    }
  }
}
