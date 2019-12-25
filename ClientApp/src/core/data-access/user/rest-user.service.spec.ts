/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RestUserService } from './rest-user.service';

describe('Service: RestUser', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RestUserService]
    });
  });

  it('should ...', inject([RestUserService], (service: RestUserService) => {
    expect(service).toBeTruthy();
  }));
});
