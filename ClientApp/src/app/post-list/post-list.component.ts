import { GetPostsDto } from './../models/Post/GetPostsDto';
import { RestService } from './../rest.service';
import { Component, OnInit } from '@angular/core';
import { Post } from '../models/Post/Post';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {

  constructor(private restService: RestService) { }

  public postList: Array<Post>;

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData = (): void => {
    this.restService.getPosts().subscribe(
      (getPostsDto: GetPostsDto) => {

        this.postList = getPostsDto.posts;
        this.postList.sort((a: Post, b: Post) => b.likes - a.likes);
      });
  }

  refresh = (): void =>
    this.fetchData()

}
