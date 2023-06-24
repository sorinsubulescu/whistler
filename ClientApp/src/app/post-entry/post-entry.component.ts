import { DeleteCommentParameters } from './../models/Comment/DeleteCommentParameters';
import { CommentDto } from './../models/Comment/CommentDto';
import { AddCommentParameters } from './../models/Comment/AddCommentParameters';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { RestPostService } from '../../core/data-access/post/rest-post.service';
import { PostDto } from '../models/Post/PostDto';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-post-entry',
  templateUrl: './post-entry.component.html',
  styleUrls: ['./post-entry.component.scss']
})
export class PostEntryComponent implements OnInit {
  @Input() post: PostDto;
  @Output() postWasDeleted: EventEmitter<any> = new EventEmitter();
  constructor(
    private restPostService: RestPostService,
    public authenticationService: AuthenticationService,
    private router: Router
  ) { }

  public commentToAdd: string;

  likePost = (): void => {
    this.restPostService.likePost(this.post.id).subscribe(
      () => this.refreshPost()
    );
  }

  dislikePost = (): void => {
    this.restPostService.dislikePost(this.post.id).subscribe(
      () => this.refreshPost()
    );
  }

  deletePost = (): void => {
    this.restPostService.deletePost(this.post.id).subscribe(
      () => this.postWasDeleted.emit()
    );
  }

  private refreshPost = (): void => {
    this.restPostService.getPostById(this.post.id).subscribe({
      next: (post: PostDto): void => {
        this.post = post;
      }
    });
  }

  public getProfilePictureLink = (): string => {
    const baseUrl = environment.baseUrl;
    return `${baseUrl}/whstore/profile/${this.post.owner.profilePictureFileName}`;
  }

  public goToProfile = (): void => {
    this.router.navigate([`/profile/${this.post.owner.id}`]);
  }

  public goToProfileByUserId = (userId: string): void => {
    this.router.navigate([`/profile/${userId}`]);
  }

  public isPostLiked = (): boolean => {
    const currentUserId = this.authenticationService.currentUser.id;

    return this.post.likedByUserIds.includes(currentUserId);
  }

  public isCurrentUserPostOwner = (): boolean => {
    return this.authenticationService.currentUser.id === this.post.owner.id;
  }

  public getCurrentUserProfilePictureLink = (): string => {
    const baseUrl = environment.baseUrl;
    return `${baseUrl}/whstore/profile/${this.authenticationService.currentUser.profilePictureFileName}`;
  }

  public getUserProfilePictureLink = (profilePictureFileName: string): string => {
    const baseUrl = environment.baseUrl;
    return `${baseUrl}/whstore/profile/${profilePictureFileName}`;
  }

  public addComment = (): void => {
    const addCommentParameters: AddCommentParameters = {
      message: this.commentToAdd
    };

    this.commentToAdd = '';

    this.restPostService.addComment(addCommentParameters, this.post.id).subscribe({
      next: (): void => this.refreshPost()
    });
  }

  public isCurrentUserCommentOwner = (comment: CommentDto): boolean => {
    return this.authenticationService.currentUser.id === comment.ownerId;
  }

  public deleteComment = (comment: CommentDto): void => {
    const deleteCommentParameters: DeleteCommentParameters = {
      commentId: comment.id
    };

    this.restPostService.deleteComment(deleteCommentParameters, this.post.id).subscribe({
      next: (): void => this.refreshPost()
    });
  }

  ngOnInit(): void {
  }

}
