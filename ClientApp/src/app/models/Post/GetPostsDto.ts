import { RestResponse } from '../Misc/RestResponse';
import { PostDto } from './PostDto';

export class GetPostsDto extends RestResponse {
    posts: Array<PostDto>;
}
