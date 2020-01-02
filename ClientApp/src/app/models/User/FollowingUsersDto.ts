import { RestResponse } from 'src/app/models/Misc/RestResponse';
import { UserDto } from 'src/app/models/User/UserDto';

export class FollowingUsersDto extends RestResponse{
    public followingUsers: Array<UserDto>;

    constructor() {
        super();
        this.followingUsers = new Array<UserDto>();
    }
}
