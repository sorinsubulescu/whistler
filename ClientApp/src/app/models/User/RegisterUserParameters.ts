import { FormGroup } from '@angular/forms';

export class RegisterUserParameters {
    public fullName: string;
    public userName: string;
    public password: string;
    public email: string;


    constructor()
    // tslint:disable-next-line:unified-signatures
    constructor(formGroup: FormGroup)
    constructor(formGroup?: FormGroup) {

        this.fullName = formGroup.value.fullName ||  '';
        this.userName = formGroup.value.userName || '';
        this.password = formGroup.value.password || '';
        this.email = formGroup.value.email || '';
    }
}
