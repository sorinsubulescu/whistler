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

  likePost = () => {
    this.restService.likePost(this.post.Id).subscribe(
      () => this.postStatusChanged.emit()
      );
  }

  dislikePost = () => {
    this.restService.dislikePost(this.post.Id).subscribe(
      () => this.postStatusChanged.emit()
    );
  }

  deletePost = () => {
    this.restService.deletePost(this.post.Id).subscribe(
      () => this.postStatusChanged.emit()
    );
  }

  ngOnInit() {
  }

}
