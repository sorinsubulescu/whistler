import { GetPostsDto } from './../models/Post/GetPostsDto';
import { RestPostService } from '../../core/data-access/post/rest-post.service';
import { Component, OnInit, Input } from '@angular/core';
import { PostDto } from '../models/Post/PostDto';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent{
  @Input() postList: Array<PostDto>;
  constructor(private restPostService: RestPostService) { }

  public removePostFromList = (post: PostDto): void => {
    const index = this.postList.indexOf(post);
    this.postList.splice(index, 1);
  }

}
