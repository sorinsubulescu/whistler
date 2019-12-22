import { AddPostParameters } from './models/AddPostParameters';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { from, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Post } from './models/Post';

@Injectable({
  providedIn: 'root'
})
export class RestService {

  constructor(
    private httpClient: HttpClient
  ) { }

  addPost(addPostParameters: AddPostParameters): Observable<Response> {
    return this.httpClient.post(
      `http://localhost:5000/api/post`,
      addPostParameters
    ).pipe(
      map((response: Response) => {
        return response;
      })
    );
  }

  getPosts(): Observable<Array<Post>> {
    return this.httpClient.get(
      `http://localhost:5000/api/post`
    ).pipe(
      map((postList: Array<Post>) => {
        return postList;
      })
    );
  }

  likePost(postId: string): Observable<Response> {
    return this.httpClient.put(
      `http://localhost:5000/api/post/like/${postId}`,
      null
    ).pipe(
      map((response: Response) => {
        return response;
      })
    );
  }

  dislikePost(postId: string): Observable<Response> {
    return this.httpClient.put(
      `http://localhost:5000/api/post/dislike/${postId}`,
      null
    ).pipe(
      map((response: Response) => {
        return response;
      })
    );
  }

  deletePost(postId: string): Observable<Response> {
    return this.httpClient.delete(
      `http://localhost:5000/api/post/${postId}`
    ).pipe(
      map((response: Response) => {
        return response;
      })
    );
  }

}
