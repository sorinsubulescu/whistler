import { GetPostsDto } from './../../../app/models/Post/GetPostsDto';
import { AddPostParameters } from '../../../app/models/Post/AddPostParameters';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RestClient } from '../RestClient';
import { AuthenticationService } from 'src/core/authentication/services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class RestPostService extends RestClient {

  constructor(
    protected httpClient: HttpClient,
    protected authenticationService: AuthenticationService
  ) {
    super(httpClient, authenticationService);
  }

  addPost = (addPostParameters: AddPostParameters): Observable<Response> =>
    this.callEndpoint<Response>(() =>
      this.post(
        `api/post`,
        addPostParameters
      ))

  getPosts = (): Observable<GetPostsDto> =>
    this.callEndpoint<GetPostsDto>(() =>
      this.get(`api/post`)
    )

  likePost = (postId: string): Observable<Response> =>
    this.callEndpoint<Response>(() => this.put(
      `api/post/like/${postId}`,
      null
    ))

  dislikePost = (postId: string): Observable<Response> =>
    this.callEndpoint<Response>(() => this.put(
      `api/post/dislike/${postId}`,
      null
    ))

  deletePost = (postId: string): Observable<Response> =>
    this.callEndpoint<Response>(() => this.delete(
      `api/post/${postId}`
    ))

}
