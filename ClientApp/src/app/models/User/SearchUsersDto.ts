import { RestResponse } from 'src/app/models/Misc/RestResponse';
import { UserDto } from 'src/app/models/User/UserDto';

export class SearchUsersDto extends RestResponse {
    public matchingUsers: Array<UserDto>;

    constructor()
    {
        super();
        this.matchingUsers = new Array<UserDto>();
    }
}
