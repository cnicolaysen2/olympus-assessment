import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AngularFireAuth } from '@angular/fire/auth';


@Injectable()
export class LoginGuard implements CanActivate {
  constructor(private afAuth: AngularFireAuth) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    // This listener is called when the user is signed in or out, and that is where we update the UI
    return new Promise((resolve, reject) => {
      this.afAuth.auth.onAuthStateChanged((user) => {
        return (user) ? resolve(true) : resolve(false);
      });
    });
  }
}
