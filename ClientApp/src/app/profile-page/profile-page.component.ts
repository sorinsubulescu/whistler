import { ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { RestPostService } from 'src/core/data-access/post/rest-post.service';
import { PostDto } from '../models/Post/PostDto';
import { GetPostsDto } from '../models/Post/GetPostsDto';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss']
})
export class ProfilePageComponent implements OnInit {
  constructor(
    private restPostService: RestPostService,
    private activatedRoute: ActivatedRoute
  ) { }

  public userId: string;
  public postList: Array<PostDto>;

  public ngOnInit(): void {
    this.activatedRoute.params.subscribe(
      (params: Params) => {
       this.userId = params['id'];
       this.fetchPosts();
    });
  }

  fetchPosts = (): void => {
    this.restPostService.getPostsByUserId(this.userId).subscribe(
      (getPostsDto: GetPostsDto) => {

        this.postList = getPostsDto.posts;
      });
  }

  public refreshPage = (): void => {
    this.fetchPosts();
  }
}
