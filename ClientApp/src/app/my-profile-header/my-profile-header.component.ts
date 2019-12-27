import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
import { Component, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { RestUserService } from 'src/core/data-access/user/rest-user.service';
import { environment } from 'src/environments/environment';
import { UserDto } from '../models/User/UserDto';

@Component({
  selector: 'app-my-profile-header',
  templateUrl: './my-profile-header.component.html',
  styleUrls: ['./my-profile-header.component.scss']
})
export class MyProfileHeaderComponent {
  @ViewChild('file', { static: true}) file: ElementRef;
  @Output() informationUpdated: EventEmitter<any> = new EventEmitter();
  constructor(
    public authenticationService: AuthenticationService,
    public restUserService: RestUserService
    ) { }

  public showUpdatePictureInfo = false;

  public uploadProfilePicture = (): void => {
    this.file.nativeElement.click();
  }

  public profilePictureFileSelectionChanged = (event: Event & { target: HTMLInputElement }): void => {
    if (event.target.files.length <= 0) {
      return;
    }

    const file = event.target.files[0];
    const formData = new FormData();
    formData.append('file', file);

    this.restUserService.editProfilePicture(formData).subscribe({
      next: (): void => {
        this.informationUpdated.emit();
        this.refreshCurrentUser();
      }
    });
  }

  private refreshCurrentUser = (): void => {
    this.restUserService.getCurrentUser().subscribe({
        next: (userDto: UserDto): void => {
            this.authenticationService.currentUser = userDto;
        }
    });
}

  public getProfilePictureLink = (): string => {
    const baseUrl = environment.baseUrl;
    return `${baseUrl}/whstore/profile/${this.authenticationService.currentUser.profilePictureFileName}`;
  }
}
