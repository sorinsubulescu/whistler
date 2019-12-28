import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
import { Injectable } from '@angular/core';
import { RestUserService } from '../data-access/user/rest-user.service';
import { UserDto } from 'src/app/models/User/UserDto';

@Injectable({
  providedIn: 'root'
})
export class StartupService {


  constructor(
    private authenticationService: AuthenticationService,
    private restUserService: RestUserService,
  ) { }

  // This is the method you want to call at bootstrap
  // Important: It should return a Promise
  public async load(): Promise<any> {
    if (this.isUserAuthenticated()) {
      return this.restUserService.getCurrentUser().toPromise()
        .then((userDto: UserDto) => {
          this.authenticationService.currentUser = userDto;
        })
        .catch(() => {
          return;
        });

    }
  }

  private isUserAuthenticated = (): boolean => {
    const accessToken = this.authenticationService.accessToken;

    if (accessToken) {
      return true;
    }

    return false;
  }
}
