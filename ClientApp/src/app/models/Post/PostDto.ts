import { CommentDto } from './../Comment/CommentDto';
import { UserDto } from '../User/UserDto';

export class PostDto extends Response{
    public id: string;
    public message: string;
    public likeCount: number;
    public likedByUserIds: Array<string>;
    public owner: UserDto;
    public comments: Array<CommentDto>;
}
