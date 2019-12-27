import { Router } from '@angular/router';
import { LogoutUserParameters } from './../models/User/LogoutUserParameters';
import { Component } from '@angular/core';
import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
import { RestUserService } from 'src/core/data-access/user/rest-user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {

  constructor(
    public authenticationService: AuthenticationService,
    private restUserService: RestUserService,
    private router: Router
  ) { }

  public logout(): void {
    const logoutUserParameters = new LogoutUserParameters(
      this.authenticationService.accessToken,
      this.authenticationService.refreshToken
    );

    this.restUserService
      .logoutUser(logoutUserParameters)
      .subscribe({
        next: (): void => {
          this.authenticationService.removeUserCookies();
          this.router.navigate(['/login']);
        },
        error: (): void => {
          this.authenticationService.removeUserCookies();
          this.router.navigate(['/login']);
        }
      });
  }

  public getProfilePictureLink = (): string => {
    const baseUrl = environment.baseUrl;
    return `${baseUrl}/whstore/profile/${this.authenticationService.currentUser.profilePictureFileName}`;
  }
}
