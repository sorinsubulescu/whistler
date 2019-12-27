import { MyProfileHeaderComponent } from './my-profile-header/my-profile-header.component';
import { ProfilePageComponent } from './profile-page/profile-page.component';
import { StartupService } from './../core/startup/startup.service';
import { startupServiceFactory } from './../core/startup/startup-service.factory';
import { CookieService } from 'ngx-cookie-service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PostFormComponent } from './post-form/post-form.component';
import { RestPostService } from '../core/data-access/post/rest-post.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PostListComponent } from './post-list/post-list.component';
import { PostEntryComponent } from './post-entry/post-entry.component';
import { HomeComponent } from './home/home.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { RestUserService } from 'src/core/data-access/user/rest-user.service';
import { AuthErrorInterceptor } from 'src/core/authentication/helpers/autherror.interceptor';

@NgModule({
   declarations: [
      AppComponent,
      PostFormComponent,
      PostListComponent,
      PostEntryComponent,
      HomeComponent,
      NavBarComponent,
      NotFoundComponent,
      LoginComponent,
      RegisterComponent,
      ProfilePageComponent,
      MyProfileHeaderComponent
   ],
   imports: [
      BrowserModule,
      FormsModule,
      AppRoutingModule,
      HttpClientModule,
      ReactiveFormsModule
   ],
   providers: [
      RestPostService,
      RestUserService,
      CookieService,
      {
         provide: HTTP_INTERCEPTORS,
         useClass: AuthErrorInterceptor,
         multi: true
      },
      {
         provide: APP_INITIALIZER,
         useFactory: startupServiceFactory,
         deps: [StartupService],
         multi: true
     }
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
