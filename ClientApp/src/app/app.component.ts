import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(private router: Router) {}

  public showNavbar = (): boolean => {
    if (this.router.url.includes('login')) {
        return false;
    }

    if (this.router.url.includes('register')) {
        return false;
    }

    if (this.router.url === '/404') {
        return false;
    }

    return true;
}
}
