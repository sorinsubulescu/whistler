import { RestPostService } from './../../core/data-access/post/rest-post.service';
import { Component, OnInit } from '@angular/core';
import { GetPostsDto } from '../models/Post/GetPostsDto';
import { PostDto } from '../models/Post/PostDto';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private restPostService: RestPostService) {}

  public postList: Array<PostDto>;

  refreshPostList = (): void => {
    this.fetchPosts();
  }

  ngOnInit(): void {
    this.fetchPosts();
  }

  fetchPosts = (): void => {
    this.restPostService.getPosts().subscribe(
      (getPostsDto: GetPostsDto) => {

        this.postList = getPostsDto.posts;
      });
  }
}
