/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RestPostService } from './rest-post.service';

describe('Service: Rest', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RestPostService]
    });
  });

  it('should ...', inject([RestPostService], (service: RestPostService) => {
    expect(service).toBeTruthy();
  }));
});
