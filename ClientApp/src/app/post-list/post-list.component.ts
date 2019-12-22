import { RestService } from './../rest.service';
import { Component, OnInit } from '@angular/core';
import { Post } from '../models/Post';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {

  constructor(private restService: RestService) { }

  private postList: Array<Post>;

  ngOnInit() {
    this.fetchData();
  }

  fetchData = () => {
    this.restService.getPosts().subscribe(
      (posts: Array<Post>) => {
        this.postList = posts;
        this.postList.sort((a, b) => b.Likes - a.Likes);
      });
  }

  refresh = () =>
    this.fetchData()

}
