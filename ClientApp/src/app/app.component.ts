import { PostListComponent } from './post-list/post-list.component';
import { Component, ViewChild } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  @ViewChild(PostListComponent) postListComponent: PostListComponent;
  title = 'whister';

  refreshPostList = () => {
    this.postListComponent.refresh();
  }
}
