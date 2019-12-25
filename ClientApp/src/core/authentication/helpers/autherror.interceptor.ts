import { AuthenticateUserDto } from './../../../app/models/User/AuthenticateUserDto';
import { RestUserService } from 'src/core/data-access/user/rest-user.service';
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, flatMap } from 'rxjs/operators';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { RefreshUserTokenParameters } from 'src/app/models/User/RefreshUserTokenParameters';

@Injectable()
export class AuthErrorInterceptor implements HttpInterceptor {
    constructor(
        private authenticationService: AuthenticationService,
        private router: Router,
        private restUserService: RestUserService
    ) { }

    private readonly UNAUTHORIZED_STATUS_CODE = 401;
    private isRefreshing = false;

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(catchError((err: HttpErrorResponse) => {

            if (err.status !== this.UNAUTHORIZED_STATUS_CODE) {
                return throwError(err);
            }

            if (this.isRefreshing) {
                return throwError(err);
            } else {
                this.isRefreshing = true;
                const refreshUserTokenParameters =
                    new RefreshUserTokenParameters(
                        this.authenticationService.accessToken,
                        this.authenticationService.refreshToken
                    );
                return this.restUserService.refreshUserToken(refreshUserTokenParameters)
                    .pipe(
                        flatMap((response: AuthenticateUserDto) => {
                            this.authenticationService.accessToken = response.token;
                            this.authenticationService.refreshToken = response.refreshToken;
                            this.isRefreshing = false;
                            return next.handle(req.clone({ setHeaders: { Authorization: `Bearer ${response.token}` } }));
                        }),
                        catchError((error: HttpErrorResponse) => {
                            if (error.url.endsWith('refreshToken')) {
                                this.authenticationService.removeUserCookies();
                                this.router.navigate(['/login']);
                            }
                            this.isRefreshing = false;
                            return throwError(error);
                        })
                    );
            }
        }));
    }
}
