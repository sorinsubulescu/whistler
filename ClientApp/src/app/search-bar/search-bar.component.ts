import { Router } from '@angular/router';
import { UserDto } from 'src/app/models/User/UserDto';
import { SearchUsersDto } from './../models/User/SearchUsersDto';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { RestUserService } from 'src/core/data-access/user/rest-user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss']
})
export class SearchBarComponent implements OnInit {
  @ViewChild('searchInput', { static: false }) searchInput: ElementRef;
  public searchTerm$ = new Subject<string>();
  private _matchedUsers = new SearchUsersDto();
  public matchedUsers$ = new BehaviorSubject<SearchUsersDto>(this._matchedUsers);
  private readonly SEARCH_DEBOUNCE_TIME = 400;

  constructor(
    private restUserService: RestUserService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.debounceSearchField();
  }

  public search = (searchTerm: string): void => {

    this.searchTerm$.next(searchTerm);
  }

  private debounceSearchField = (): void => {
    this.searchTerm$.pipe(
      debounceTime(this.SEARCH_DEBOUNCE_TIME),
      distinctUntilChanged())
      .subscribe(
        {
          next: (value: string): void => {
            if (!value) {
              this.matchedUsers$.next(new SearchUsersDto());
            } else {
              this.searchUsers(value);
            }
          }
        });
  }

  private searchUsers = (searchTerm: string): void => {
    this.restUserService.searchUsers(searchTerm).subscribe({
      next: (searchUsersDto: SearchUsersDto): void => {
        this.matchedUsers$.next(searchUsersDto);
      }
    });
  }

  public getProfilePictureLink = (user: UserDto): string => {
    const baseUrl = environment.baseUrl;
    return `${baseUrl}/whstore/profile/${user.profilePictureFileName}`;
  }

  public goToProfile = (user: UserDto): void => {
    this.router.navigate([`/profile/${user.id}`]);
    this.searchInput.nativeElement.value = '';
    this.searchTerm$.next('');
  }

}
