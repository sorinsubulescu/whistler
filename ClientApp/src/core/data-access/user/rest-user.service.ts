import { SearchUsersDto } from './../../../app/models/User/SearchUsersDto';
import { UserBriefInfoDto } from './../../../app/models/User/UserBriefInfoDto';
import { RegisterUserParameters } from './../../../app/models/User/RegisterUserParameters';
import { AuthenticateUserParameters } from './../../../app/models/User/AuthenticateUserParameters';
import { AuthenticateUserDto } from './../../../app/models/User/AuthenticateUserDto';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { RefreshUserTokenParameters } from 'src/app/models/User/RefreshUserTokenParameters';
import { RestResponse } from 'src/app/models/Misc/RestResponse';
import { RestClient } from '../RestClient';
import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
import { LogoutUserParameters } from 'src/app/models/User/LogoutUserParameters';
import { UserDto } from 'src/app/models/User/UserDto';
import { EditUserParameters } from 'src/app/models/User/EditUserParameters';
import { FollowUserParameters } from 'src/app/models/User/FollowUserParameters';
import { UnfollowUserParameters } from 'src/app/models/User/UnfollowUserParameters';

@Injectable({
  providedIn: 'root'
})
export class RestUserService extends RestClient {

  constructor(
    protected httpClient: HttpClient,
    protected authenticationService: AuthenticationService
  ) {
    super(httpClient, authenticationService);
  }

  refreshUserToken = (refreshUserTokenParameters: RefreshUserTokenParameters): Observable<AuthenticateUserDto> =>
    this.callEndpoint<AuthenticateUserDto>(() =>
      this.post(
        `api/user/refreshToken`,
        refreshUserTokenParameters
      ))

  authenticateUser = (authenticateUserParameters: AuthenticateUserParameters): Observable<AuthenticateUserDto> =>
    this.callEndpoint<AuthenticateUserDto>(() =>
      this.post(
        `api/user/authenticate`,
        authenticateUserParameters
      ))

  registerUser = (registerUserParameters: RegisterUserParameters): Observable<RestResponse> =>
    this.callEndpoint<RestResponse>(() =>
      this.post(
        `api/user/register`,
        registerUserParameters
      ))

  logoutUser = (logoutUserParameters: LogoutUserParameters): Observable<RestResponse> =>
    this.callEndpoint<RestResponse>(() =>
      this.post(`api/user/logout`, logoutUserParameters)
    )

  getCurrentUser = (): Observable<UserDto> =>
    this.callEndpoint<UserDto>(() => this.get('api/user/me'))

  getUserById = (userId: string): Observable<UserDto> =>
    this.callEndpoint<UserDto>(() => this.get(`api/user/${userId}`))

  editProfilePicture = (formData: FormData): Observable<RestResponse> =>
    this.callEndpoint<RestResponse>(() =>
      this.put('api/user/profile_picture', formData)
    )

  editUser = (editUserParameters: EditUserParameters): Observable<RestResponse> =>
    this.callEndpoint<RestResponse>(() => this.patch('api/user', editUserParameters))

  getUserBriefInfo = (userId: string): Observable<UserBriefInfoDto> =>
    this.callEndpoint<UserBriefInfoDto>(() => this.get(`api/user/brief_info/${userId}`))

  followUser = (followUserParameters: FollowUserParameters): Observable<RestResponse> =>
    this.callEndpoint<RestResponse>(() => this.post('api/user/follow', followUserParameters))

  unfollowUser = (unfollowUserParameters: UnfollowUserParameters): Observable<RestResponse> =>
    this.callEndpoint<RestResponse>(() => this.post('api/user/unfollow', unfollowUserParameters))

  searchUsers = (searchTerm: string): Observable<SearchUsersDto> =>
    this.callEndpoint<SearchUsersDto>(() => this.get(`api/user/search/${searchTerm}`))
}
