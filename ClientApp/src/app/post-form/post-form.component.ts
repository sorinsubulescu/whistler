import { AddPostParameters } from './../models/AddPostParameters';
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

  private message: string;

  private addPost = () => {
    const addPostParameters: AddPostParameters = {
      Message: this.message
    };
    this.restService.addPost(addPostParameters).subscribe(() => {
      this.postSubmitted.emit();
    });
  }

  ngOnInit() {
  }

}
