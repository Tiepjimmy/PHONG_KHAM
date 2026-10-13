import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, of, throwError } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { URL_API } from './constant-helper';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) {}

  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    //handle your auth error or rethrow
    if (err.status === 401 || err.status === 403) {
      //navigate /delete cookies or whatever
      this.router.navigate(['/dang-nhap']);
      // if you've caught / handled the error, you don't want to rethrow it unless you also want downstream consumers to have to handle it as well.
      return of(err.message); // or EMPTY may be appropriate here
    }
    return throwError(err);
  }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // add auth header with jwt if user is logged in and request is to the api url
    const user = this.authenticationService.userValue;
    if (user !== null) {
      const isLoggedIn = user && user.token;
      const isApiUrl = request.url.startsWith(URL_API);
      if (isLoggedIn && isApiUrl) {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${user.token}`,
          },
        });
      }
      return next
        .handle(request)
        .pipe(catchError((x) => this.handleAuthError(x)));
    }
    return next
      .handle(request)
      .pipe(catchError((x) => this.handleAuthError(x)));
  }
}
