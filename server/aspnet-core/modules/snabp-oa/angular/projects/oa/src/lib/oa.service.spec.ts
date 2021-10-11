import { TestBed } from '@angular/core/testing';

import { OaService } from './oa.service';

describe('OaService', () => {
  let service: OaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
