import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  constructor(
      public httpClient: HttpClient,
      private cookieService: CookieService
      ) {}

    public get fullName(): string {
        return this.cookieService.get('full_name');
    }

    public set fullName(fullName: string) {
        this.cookieService.set('full_name', fullName, undefined, '/');
    }

    public get userEmail(): string {
        return this.cookieService.get('user_email');
    }

    public set userEmail(userEmail: string) {
        this.cookieService.set('user_email', userEmail, undefined, '/');
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
        this.cookieService.delete('full_name', '/');
        this.cookieService.delete('user_email', '/');
    }
}
