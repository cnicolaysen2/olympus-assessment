import { TestBed } from '@angular/core/testing';

import { GridsterDragndropService } from './gridster-dragndrop.service';

describe('GridsterDragndropService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GridsterDragndropService = TestBed.get(GridsterDragndropService);
    expect(service).toBeTruthy();
  });
});
