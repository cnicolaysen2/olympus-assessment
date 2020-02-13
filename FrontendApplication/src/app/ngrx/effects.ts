import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Actions, Effect } from '@ngrx/effects';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/takeUntil';
import * as volcanoRange from './actions';
import { OlympusStoreService } from '../services/firestore.service';
import { VolcanoRangeUpdatedAction, VolcanoRangeSetAction } from './actions';

@Injectable()
export class OlympusStoreEffects {
  constructor(
    private olympusStoreService: OlympusStoreService,
    private actions$: Actions,
  ) {}

  @Effect()
  update$: Observable<VolcanoRangeUpdatedAction> = this.actions$
    .ofType(volcanoRange.VOLCANORANGE_UPDATE)
    .switchMap(() =>
      this.olympusStoreService.getVolcanoRange().switchMap(data => {
        return [new VolcanoRangeUpdatedAction(data)];
      })
    );

  // @Effect()
  // set$: VolcanoRangeSetAction = this.actions$
  //   .ofType(volcanoRange.VOLCANORANGE_SET)
  //   .switchMap((action) =>
  //     this.olympusStoreService.setVolcanoRange(action.range).switchMap(data => {
  //       return [new VolcanoRangeUpdatedAction(data)];
  //     })
  //   );
}
