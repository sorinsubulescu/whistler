import { UserDto } from './UserDto';

export class EnumerateUsersDto extends Response {
    public entries: Array<AuthenticateUserDto>;

    constructor() {
        super();
    }
}

export class AuthenticateUserDto extends Response {
    public currentUser: UserDto;
    public token: string;
    public refreshToken: string;

    constructor() {
        super();
    }
}
