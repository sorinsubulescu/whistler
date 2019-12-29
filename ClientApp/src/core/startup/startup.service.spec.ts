/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { StartupService } from './startup.service';

describe('Service: Startup', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [StartupService]
    });
  });

  it('should ...', inject([StartupService], (service: StartupService) => {
    expect(service).toBeTruthy();
  }));
});
