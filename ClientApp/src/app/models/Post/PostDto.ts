import { UserDto } from '../User/UserDto';

export class PostDto {
    public id: string;
    public message: string;
    public likeCount: number;
    public owner: UserDto;
}
