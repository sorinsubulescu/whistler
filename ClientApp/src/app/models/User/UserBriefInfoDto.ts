import { RestResponse } from 'src/app/models/Misc/RestResponse';

export class UserBriefInfoDto extends RestResponse {
    public followingCount: number;
    public followersCount: number;
    public isFollowedByMe: boolean;
}
