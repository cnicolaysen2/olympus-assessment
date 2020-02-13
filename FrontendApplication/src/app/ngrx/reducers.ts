import * as storeActions from './actions';
import { Volcano, Isosurface, Filtered, RandomWalk } from './models';
import { State } from '../ngrx/store';
import { OlympusStoreService } from '../services/firestore.service';

/* REDUCER LIST TO INIT STORE IN APPMODULE */
export const reducers = {
  volcanoRange: olympusStore_reducer,
};

export function olympusStore_reducer(
  // state = {
  //   volcanoRange: { range: [0, 1] },
  //   isosurfaceEye: { eye: { x: 0, y: 0, z: 0 } },
  //   filteredOut: { out: 0 },
  //   randomwalkTip: { tip: 0 }
  // },
  state = [],
  action: storeActions.VolcanoRangeUpdatedAction) {
    switch (action.type) {
      case storeActions.VOLCANORANGE_UPDATED:
        return action.payload;
      case storeActions.VOLCANORANGE_SET:
        // state.volcanoRange = action.payload;
        return action.payload;
      default:
      return state;
  }
}
