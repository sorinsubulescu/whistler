import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
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
    private authenticationService: AuthenticationService
  ) { }

  public postList: Array<PostDto>;
  ngOnInit(): void {
    this.fetchPosts();
  }

  fetchPosts = (): void => {
    this.restPostService.getPostsByUserId(this.authenticationService.currentUser.id).subscribe(
      (getPostsDto: GetPostsDto) => {

        this.postList = getPostsDto.posts;
      });
  }

  public refreshPage = (): void => {
    this.fetchPosts();
  }
}
