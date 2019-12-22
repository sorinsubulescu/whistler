import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PostFormComponent } from './post-form/post-form.component';
import { RestService } from './rest.service';
import { HttpClientModule } from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import { PostListComponent } from './post-list/post-list.component';
import { PostEntryComponent } from './post-entry/post-entry.component';

@NgModule({
   declarations: [
      AppComponent,
      PostFormComponent,
      PostListComponent,
      PostEntryComponent
   ],
   imports: [
      BrowserModule,
      FormsModule,
      AppRoutingModule,
      HttpClientModule
   ],
   providers: [
      RestService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }