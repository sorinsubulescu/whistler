import { RestUserService } from 'src/core/data-access/user/rest-user.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticateUserDto } from '../models/User/AuthenticateUserDto';
import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ValidationHelper } from 'src/core/validation/ValidationHelper';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
  private returnUrl: string;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private restUserService: RestUserService,
    private authenticationService: AuthenticationService,
    private router: Router,
  ) { }

  public ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username: [''],
      password: ['']
    });

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  public isFormGroupValid(propertyName: string): boolean {
    return ValidationHelper.isFormGroupValid(this.loginForm, propertyName);
  }

  public onSubmit(showLoader: boolean = true): void {

    if (this.loginForm.invalid) {
        return;
    }

    this.restUserService.authenticateUser(this.loginForm.value)
        .subscribe({
            next: (response: AuthenticateUserDto): void => {
                this.authenticationService.fullName = response.fullName;
                this.authenticationService.userEmail = response.email;
                this.authenticationService.accessToken = response.token;
                this.authenticationService.refreshToken = response.refreshToken;

                this.router.navigate([this.returnUrl]);
            },
            error: (error: HttpErrorResponse): void => {
              throw new Error(('Not Implemented Exception'));
            }
        });
}

}
