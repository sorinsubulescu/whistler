import { DeleteCommentParameters } from './../../../app/models/Comment/DeleteCommentParameters';
import { RestResponse } from 'src/app/models/Misc/RestResponse';
import { AddCommentParameters } from './../../../app/models/Comment/AddCommentParameters';
import { PostDto } from './../../../app/models/Post/PostDto';
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

  getPostById = (postId: string): Observable<PostDto> =>
    this.callEndpoint<PostDto>(() =>
      this.get(`api/post/${postId}`)
    )

  getPosts = (): Observable<GetPostsDto> =>
    this.callEndpoint<GetPostsDto>(() =>
      this.get(`api/post`)
    )

  getPostsByUserId = (userId: string): Observable<GetPostsDto> =>
    this.callEndpoint<GetPostsDto>(() =>
      this.get(`api/post/user/${userId}`)
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

  addComment = (addCommentParameters: AddCommentParameters, postId: string): Observable<Response> =>
    this.callEndpoint<Response>(() => this.put(
      `api/post/comment/${postId}`, addCommentParameters
    ))

  deleteComment = (deleteCommentParameters: DeleteCommentParameters, postId: string): Observable<Response> =>
    this.callEndpoint<Response>(() => this.put(
      `api/post/delete_comment/${postId}`, deleteCommentParameters
    ))
}
