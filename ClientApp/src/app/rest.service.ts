import { GetPostsDto } from './models/Post/GetPostsDto';
import { AddPostParameters } from './models/Post/AddPostParameters';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { from, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Post } from './models/Post/Post';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RestService {

  constructor(
    private httpClient: HttpClient
  ) { }

  private baseUrl = environment.baseUrl;

  addPost(addPostParameters: AddPostParameters): Observable<Response> {
    return this.httpClient.post(
      `${this.baseUrl}/api/post`,
      addPostParameters
    ).pipe(
      map((response: Response) => {
        return response;
      })
    );
  }

  getPosts(): Observable<GetPostsDto> {
    return this.httpClient.get(
      `${this.baseUrl}/api/post`
    ).pipe(
      map((postList: GetPostsDto) => {
        return postList;
      })
    );
  }

  likePost(postId: string): Observable<Response> {
    return this.httpClient.put(
      `${this.baseUrl}/api/post/like/${postId}`,
      null
    ).pipe(
      map((response: Response) => {
        return response;
      })
    );
  }

  dislikePost(postId: string): Observable<Response> {
    return this.httpClient.put(
      `${this.baseUrl}/api/post/dislike/${postId}`,
      null
    ).pipe(
      map((response: Response) => {
        return response;
      })
    );
  }

  deletePost(postId: string): Observable<Response> {
    return this.httpClient.delete(
      `${this.baseUrl}/api/post/${postId}`
    ).pipe(
      map((response: Response) => {
        return response;
      })
    );
  }

}
