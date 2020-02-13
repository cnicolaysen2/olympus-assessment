import { Volcano } from './models';
import { Action } from '@ngrx/store';

export const VOLCANORANGE_UPDATE = '[VolcanoRange] UpdateAll';
export const VOLCANORANGE_UPDATED = '[VolcanoRange] UpdatedAll';
export const VOLCANORANGE_SET = '[VolcanoRange] SetAll';

export class VolcanoRangeUpdateAction implements Action {
  type = VOLCANORANGE_UPDATE;
}

export class VolcanoRangeUpdatedAction implements Action {
  type = VOLCANORANGE_UPDATED;

  constructor(public payload: Volcano) {
  }
}

export class VolcanoRangeSetAction implements Action {
  type = VOLCANORANGE_SET;

  constructor(public range: Array<number>) {
  }
}
