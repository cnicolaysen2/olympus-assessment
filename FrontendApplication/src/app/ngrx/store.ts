import { Volcano, Isosurface, Filtered, RandomWalk, Eye } from './models';

export interface State {
    volcanoRange: Volcano;
    isosurfaceEye: Isosurface;
    filteredOut: Filtered;
    randomwalkTip: RandomWalk;
  }

export const getVolcanoRange = (state: State): Volcano => {
  return state.volcanoRange;
};

export const setVolcanoRange = (state: State, range: Array<number> ) => {
  state.volcanoRange.range = range;
};
export const getIsosurfaceEye = (state: State): Isosurface => {
  return state.isosurfaceEye;
};
export const setIsosurfaceEye = (state: State, eye: Eye) => {
  state.isosurfaceEye.eye = eye;
};
export const getFilteredOut = (state: State): Filtered => {
  return state.filteredOut;
};
export const setRandomwalkTip = (state: State, tip: number) => {
  state.randomwalkTip.tip = tip;
};
