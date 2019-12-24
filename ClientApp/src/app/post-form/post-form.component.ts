import { AddPostParameters } from '../models/Post/AddPostParameters';
import { RestService } from './../rest.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-post-form',
  templateUrl: './post-form.component.html',
  styleUrls: ['./post-form.component.scss']
})
export class PostFormComponent implements OnInit {
  @Output() postSubmitted: EventEmitter<any> = new EventEmitter();
  constructor(
    private restService: RestService
  ) { }

  public message: string;

  public addPost = (): void => {
    if (!this.isPostValid()) {
      return;
    }

    const addPostParameters: AddPostParameters = {
      message: this.message
    };
    this.restService.addPost(addPostParameters).subscribe(() => {
      this.postSubmitted.emit();
      this.message = '';
    });
  }

  public isPostValid = (): boolean => {
    if (this.message) { return true; }
    return false;
  }

  ngOnInit(): void {
  }

}
