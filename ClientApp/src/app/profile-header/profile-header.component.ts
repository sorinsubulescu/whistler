import { FollowUserParameters } from './../models/User/FollowUserParameters';
import { UserBriefInfoDto } from './../models/User/UserBriefInfoDto';
import { EditUserParameters } from '../models/User/EditUserParameters';
import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
import { Component, ViewChild, ElementRef, Output, EventEmitter, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { RestUserService } from 'src/core/data-access/user/rest-user.service';
import { environment } from 'src/environments/environment';
import { UserDto } from '../models/User/UserDto';
import { BehaviorSubject } from 'rxjs';
import { UnfollowUserParameters } from '../models/User/UnfollowUserParameters';

@Component({
  selector: 'app-profile-header',
  templateUrl: './profile-header.component.html',
  styleUrls: ['./profile-header.component.scss']
})
export class ProfileHeaderComponent implements OnInit, OnChanges {
  @ViewChild('file', { static: false }) file: ElementRef;
  @Output() informationUpdated: EventEmitter<any> = new EventEmitter();
  @Input() userId: string;
  constructor(
    public authenticationService: AuthenticationService,
    public restUserService: RestUserService
  ) { }

  private _user: UserDto = new UserDto();
  public user$: BehaviorSubject<UserDto> = new BehaviorSubject<UserDto>(this._user);
  private _userBriefInfo: UserBriefInfoDto = new UserBriefInfoDto();
  public userBriefInfo$: BehaviorSubject<UserBriefInfoDto> = new BehaviorSubject<UserBriefInfoDto>(this._userBriefInfo);

  public showUpdatePictureInfo = false;
  public showUpdateFullNameIcon = false;
  public editFullNameValue = this.authenticationService.currentUser.fullName;

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
        this.user$.next(this.authenticationService.currentUser);
      }
    });
  }

  private fetchUserBriefInfo = (): void => {
    this.restUserService.getUserBriefInfo(this.userId).subscribe({
      next: (userBriefInfoDto: UserBriefInfoDto): void => {
        this.userBriefInfo$.next(userBriefInfoDto);
      }
    });
  }

  public getProfilePictureLink = (): string => {
    const baseUrl = environment.baseUrl;
    let user: UserDto;
    this.user$.subscribe((userDto: UserDto) => user = userDto);
    return `${baseUrl}/whstore/profile/${user.profilePictureFileName}`;
  }

  public initializeEditFullNameModal = (): void => {
    this.editFullNameValue = this.authenticationService.currentUser.fullName;
  }

  public editFullName = (): void => {
    const editUserParameters = new EditUserParameters();
    editUserParameters.fullName = this.editFullNameValue;
    this.restUserService.editUser(editUserParameters).subscribe({
      next: (): void => {
        this.informationUpdated.emit();
        this.refreshCurrentUser();
      }
    });
  }

  public isCurrentUserPage = (): boolean => {
    if (this.userId === this.authenticationService.currentUser.id) {
      return true;
    }

    return false;
  }

  public ngOnInit(): void {
    if (this.isCurrentUserPage()) {
      this.user$.next(this.authenticationService.currentUser);
    } else {
      this.restUserService.getUserById(this.userId).subscribe({
        next: (userDto: UserDto): void => {
          this.user$.next(userDto);
        }
      });
    }

    this.fetchUserBriefInfo();
  }

  public ngOnChanges(changes: SimpleChanges): void {
    this.userId = changes.userId.currentValue;
    this.ngOnInit();
  }

  public hasProfilePicture = (): boolean => {
    let user: UserDto;
    this.user$.subscribe((userDto: UserDto) => user = userDto);
    return user.profilePictureFileName ? true : false;
  }

  public isUserFollowedByCurrentUser = (): boolean => {
    let isUserFollowed: boolean;
    this.userBriefInfo$.subscribe((userBriefInfo: UserBriefInfoDto) => isUserFollowed = userBriefInfo.isFollowedByMe);
    return isUserFollowed;
  }

  public followUser = (): void => {
    const followUserParameters: FollowUserParameters = {
      userToFollowId: this.userId
    };

    this.restUserService.followUser(followUserParameters).subscribe({
      next: (): void => {
        this.fetchUserBriefInfo();
      }
    });
  }

  public unfollowUser = (): void => {
    const unfollowUserParameters: UnfollowUserParameters = {
      userToUnfollowId: this.userId
    };

    this.restUserService.unfollowUser(unfollowUserParameters).subscribe({
      next: (): void => {
        this.fetchUserBriefInfo();
      }
    });
  }
}
