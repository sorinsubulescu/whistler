import { GetPostsDto } from './../models/Post/GetPostsDto';
import { RestPostService } from '../../core/data-access/post/rest-post.service';
import { Component, OnInit } from '@angular/core';
import { Post } from '../models/Post/Post';
import { PostDto } from '../models/Post/PostDto';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {

  constructor(private restPostService: RestPostService) { }

  public postList: Array<PostDto>;

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData = (): void => {
    this.restPostService.getPosts().subscribe(
      (getPostsDto: GetPostsDto) => {

        this.postList = getPostsDto.posts;
      });
  }

  refresh = (): void =>
    this.fetchData()

}
