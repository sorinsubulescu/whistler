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
    private restPostService: RestPostService
  ) { }

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
    })
  }

  public getProfilePictureLink = (): string => {
    const baseUrl = environment.baseUrl;
    return `${baseUrl}/whstore/profile/${this.post.owner.profilePictureFileName}`;
  }

  ngOnInit(): void {
  }

}
