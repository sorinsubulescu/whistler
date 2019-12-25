import { GetPostsDto } from './../models/Post/GetPostsDto';
import { RestPostService } from '../../core/data-access/post/rest-post.service';
import { Component, OnInit } from '@angular/core';
import { Post } from '../models/Post/Post';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {

  constructor(private restPostService: RestPostService) { }

  public postList: Array<Post>;

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData = (): void => {
    this.restPostService.getPosts().subscribe(
      (getPostsDto: GetPostsDto) => {

        this.postList = getPostsDto.posts;
        this.postList.sort((a: Post, b: Post) => b.likes - a.likes);
      });
  }

  refresh = (): void =>
    this.fetchData()

}
