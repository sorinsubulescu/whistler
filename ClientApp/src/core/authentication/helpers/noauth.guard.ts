import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from 'src/core/authentication/services/authentication.service';

@Injectable({ providedIn: 'root' })
export class NoAuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) { }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (!this.authenticationService.accessToken) {
            return true;
        }

        this.router.navigate(['/home']);
        return false;
    }
}
