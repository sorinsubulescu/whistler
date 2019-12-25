import { Post } from './Post';
import { RestResponse } from '../Misc/RestResponse';

export class GetPostsDto extends RestResponse {
    posts: Array<Post>;
}
