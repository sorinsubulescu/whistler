import { AuthenticationService } from 'src/core/authentication/services/authentication.service';
import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map, catchError, first } from 'rxjs/operators';

export abstract class RestClient {
    constructor(
        public http: HttpClient,
        protected authenticationService: AuthenticationService
    ) { }
    private baseUrl = environment.baseUrl;

    private getUrl = (endpoint: string): string => `${this.baseUrl}/${endpoint}`;

    public get = (endpoint: string, headers?: HttpHeaders): Observable<Response> => {
        headers = this.appendAuthorizationHeader(headers);

        return this.http.get<Response>(this.getUrl(endpoint), { headers });
    }

    public post = (endpoint: string, body?: any, headers?: HttpHeaders): Observable<Response> => {
        headers = this.appendAuthorizationHeader(headers);

        return this.http.post<Response>(this.getUrl(endpoint), body, { headers });
    }

    public patch = (endpoint: string, body: any, headers?: HttpHeaders): Observable<Response> => {
        headers = this.appendAuthorizationHeader(headers);

        return this.http.patch<Response>(this.getUrl(endpoint), body, { headers });
    }

    public put = (endpoint: string, body: any, headers?: HttpHeaders): Observable<Response> => {
        headers = this.appendAuthorizationHeader(headers);

        return this.http.put<Response>(this.getUrl(endpoint), body, { headers });
    }

    public delete = (endpoint: string, headers?: HttpHeaders): Observable<Response> => {
        headers = this.appendAuthorizationHeader(headers);

        return this.http.delete<Response>(this.getUrl(endpoint), { headers });
    }

    private appendAuthorizationHeader = (headers: HttpHeaders): HttpHeaders => {
        if (! this.authenticationService.accessToken) {
            return headers;
        }
        if (headers) {
            headers.append('Authorization', `Bearer ${this.authenticationService.accessToken}`);
        } else {
            headers = new HttpHeaders({ Authorization: `Bearer ${this.authenticationService.accessToken}` });
        }

        return headers;
    }

    protected callEndpoint<T extends Response>(method: () => Observable<Response>, showLoader: boolean = true): Observable<T> {

        return method().pipe(map((response: T) => {
            return response;
        }), catchError((err: HttpErrorResponse): Observable<T> => {
            return throwError(err);
        }), first());
    }
}
