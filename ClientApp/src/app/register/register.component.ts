import { RestUserService } from 'src/core/data-access/user/rest-user.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { ValidationHelper } from 'src/core/validation/ValidationHelper';
import { RegisterUserParameters } from '../models/User/RegisterUserParameters';
import { RestResponse } from '../models/Misc/RestResponse';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  public registerForm: FormGroup;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private restUserService: RestUserService
  ) { }

  public ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      fullName: ['', [Validators.required, ValidationHelper.fullNameValidation]],
      userName: ['', [Validators.required, ValidationHelper.userNameValidation]],
      email: ['', [Validators.required, ValidationHelper.emailValidation]],
      password: ['', [Validators.required, ValidationHelper.passwordValidation]],
      confirmPassword: ['', [Validators.required, this.confirmPasswordValidation]]
    });
  }

  public confirmPasswordValidation = (control: FormControl): object => {
    if (this.registerForm) {
      if (control.value === this.registerForm.controls.password.value) {
        return null;
      } else {
        return { invalidConfirmPassword: true };
      }
    }
  }

  public isFormGroupValid(propertyName: string): boolean {
    return ValidationHelper.isFormGroupValid(this.registerForm, propertyName);
  }

  public onSubmit(showLoader: boolean = true): void {

    if (this.registerForm.invalid) { return; }

    const registerUserParameters = new RegisterUserParameters(this.registerForm);

    this.restUserService.registerUser(registerUserParameters)
      .subscribe({
        next: (response: RestResponse): void => {
          this.router.navigate(['/login']);
        },
        error: (error: HttpErrorResponse): void => {
          throw new Error('Not implemented exception');
        }
      });
  }

}
