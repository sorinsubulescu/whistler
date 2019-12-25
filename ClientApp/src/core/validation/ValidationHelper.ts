import { FormControl, FormGroup } from '@angular/forms';

export class ValidationHelper {
    public static hasInputAnyErrorMessage = (property: FormControl): boolean =>
        property.errors ? true : false

    public static inputErrorMessage = (property: FormControl, errorMessage?: string): string => {
        if (ValidationHelper.isRequired(property)) {
            return 'FIELD_REQUIRED';
        }
        if (ValidationHelper.hasPattern(property)) {
            return 'ERROR_INVALID_DATA';
        }
        if (ValidationHelper.hasMinLength(property)) {
            return 'ERROR_MIN_LENGTH';
        }
        if (ValidationHelper.hasMaxLength(property)) {
            return 'ERROR_MAX_LENGTH';
        }
        if (errorMessage) { return errorMessage; }
    }

    public static fullNameValidation = (control: FormControl): object => {
        if (control.value.match(/^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$/)) {
            return null;
        } else {
            return { invalidFullName: true };
        }
    }

    public static userNameValidation = (control: FormControl): object => {
        // Should not contain special characters
        if (control.value.match(/^[a-zA-Z0-9]{3,16}/)) {
            return null;
        } else {
            return { invalidUserName: true };
        }
    }

    public static passwordValidation = (control: FormControl): object => {
        const passwordMinLength = 12;
        if (control.value.length >= passwordMinLength) {
            return null;
        } else {
            return { invalidPassword: true };
        }
    }

    public static emailValidation = (control: FormControl): object => {
        // RFC 5322 compliant regex
        // tslint:disable-next-line: max-line-length
        if (control.value.match(/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/)) {
          return null;
        } else {
          return { invalidEmailAddress: true };
        }
    }

    public static licenseValidation = (control: FormControl): object => {
        if (control.value.length > 0) {
          return null;
        } else {
          return { invalidLicense: true };
        }
    }

    public static isFormGroupValid(form: FormGroup, propertyName: string): boolean {
        if ( form.controls.hasOwnProperty(propertyName) ) {
            if ( form.controls[propertyName].touched ) {
                if ( form.controls[propertyName].errors !== null) {
                    return false;
                }
            }
        }
        return true;
    }

    private static hasError = (property: FormControl, error: string): boolean =>
        property.hasError(error)

    private static isRequired = (property: FormControl): boolean =>
        ValidationHelper.hasError(property, 'required')

    private static hasPattern = (property: FormControl): boolean =>
        ValidationHelper.hasError(property, 'pattern')

    private static hasMinLength = (property: FormControl): boolean =>
        ValidationHelper.hasError(property, 'minLength')

    private static hasMaxLength = (property: FormControl): boolean =>
        ValidationHelper.hasError(property, 'maxLength')
}
