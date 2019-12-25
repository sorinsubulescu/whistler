import { Component, ViewChild } from '@angular/core';
import { PostListComponent } from '../post-list/post-list.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  @ViewChild(PostListComponent, { static: true }) postListComponent: PostListComponent;

  refreshPostList = (): void => {
    this.postListComponent.refresh();
  }
}
