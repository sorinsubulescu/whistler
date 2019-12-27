import { UserDto } from '../User/UserDto';

export class PostDto extends Response{
    public id: string;
    public message: string;
    public likeCount: number;
    public owner: UserDto;
}
