import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
import { Injectable } from '@angular/core';
import { RestUserService } from '../data-access/user/rest-user.service';
import { UserDto } from 'src/app/models/User/UserDto';
import { interval } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class StartupService {


  constructor(
    private authenticationService: AuthenticationService,
    private restUserService: RestUserService
  ) { }

  // This is the method you want to call at bootstrap
  // Important: It should return a Promise
  public async load(): Promise<any> {
    if (this.isUserAuthenticated()) {
      this.restUserService.getCurrentUser().subscribe({
        next: (userDto: UserDto): void => {
          this.authenticationService.currentUser = userDto;
        }
      });

      return await this.restUserService.getCurrentUser().toPromise().then((userDto: UserDto) => {
        this.authenticationService.currentUser = userDto;
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
