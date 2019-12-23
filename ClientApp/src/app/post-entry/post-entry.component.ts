import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Post } from '../models/Post';
import { RestService } from '../rest.service';

@Component({
  selector: 'app-post-entry',
  templateUrl: './post-entry.component.html',
  styleUrls: ['./post-entry.component.scss']
})
export class PostEntryComponent implements OnInit {
  @Input() post: Post;
  @Output() postStatusChanged: EventEmitter<any> = new EventEmitter();
  constructor(
    private restService: RestService
  ) { }

  likePost = (): void => {
    this.restService.likePost(this.post.id).subscribe(
      () => this.postStatusChanged.emit()
      );
  }

  dislikePost = (): void => {
    this.restService.dislikePost(this.post.id).subscribe(
      () => this.postStatusChanged.emit()
    );
  }

  deletePost = (): void => {
    this.restService.deletePost(this.post.id).subscribe(
      () => this.postStatusChanged.emit()
    );
  }

  ngOnInit(): void {
  }

}
