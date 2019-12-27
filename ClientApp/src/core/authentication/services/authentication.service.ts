import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { UserDto } from 'src/app/models/User/UserDto';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  constructor(
      public httpClient: HttpClient,
      private cookieService: CookieService
      ) {}

    private _currentUser: UserDto = new UserDto();

    public get currentUser(): UserDto {
        return this._currentUser;
    }

    public set currentUser(currentUser: UserDto) {
        this._currentUser = currentUser;
    }

    public get accessToken(): string {
        return this.cookieService.get('access_token');
    }

    public set accessToken(accessToken: string) {
        this.cookieService.set('access_token', accessToken, undefined, '/');
    }

    public get refreshToken(): string {
        return this.cookieService.get('refresh_token');
    }

    public set refreshToken(refreshToken: string) {
        this.cookieService.set('refresh_token', refreshToken, undefined, '/');
    }

    public removeUserCookies(): void {
        this.cookieService.delete('access_token', '/');
        this.cookieService.delete('refresh_token', '/');
        this._currentUser = new UserDto();
    }
}
