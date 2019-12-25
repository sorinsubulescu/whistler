export class EnumerateUsersDto extends Response {
    public entries: Array<AuthenticateUserDto>;

    constructor() {
        super();
    }
}

export class AuthenticateUserDto extends Response {
    public fullName: string;
    public email: string;
    public token: string;
    public refreshToken: string;

    constructor() {
        super();
    }
}
