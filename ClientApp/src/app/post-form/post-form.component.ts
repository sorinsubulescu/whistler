import { AddPostParameters } from '../models/Post/AddPostParameters';
import { RestService } from './../rest.service';
import { Component, OnInit, Output, EventEmitter, ViewChild, ElementRef, AfterViewInit } from '@angular/core';

import * as autosize from 'autosize';

@Component({
  selector: 'app-post-form',
  templateUrl: './post-form.component.html',
  styleUrls: ['./post-form.component.scss']
})
export class PostFormComponent implements OnInit, AfterViewInit {
  @Output() postSubmitted: EventEmitter<any> = new EventEmitter();
  @ViewChild('textarea', { static: true }) textarea: ElementRef;
  constructor(
    private restService: RestService
  ) { }

  ngAfterViewInit(): void {
    autosize(this.textarea.nativeElement);
  }

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
