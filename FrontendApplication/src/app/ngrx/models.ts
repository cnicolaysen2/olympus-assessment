export interface RandomWalk {
  tip: number;
}
export interface Filtered {
  out: number;
}
export interface Eye {
  x: number;
  y: number;
  z: number;
}
export interface Isosurface {
  eye: Eye;
}
export interface Volcano {
  range: Array<number>;
}

export interface OlympusStore {
  randomwalk: RandomWalk;
  filtered: Filtered;
  isosurface: Isosurface;
  volcano: Volcano;
}
