import { RestResponse } from 'src/app/models/Misc/RestResponse';
import { UserDto } from 'src/app/models/User/UserDto';
export class FollowersDto extends RestResponse {
    public followers: Array<UserDto>;

    constructor() {
        super();
        this.followers = new Array<UserDto>();
    }
}
