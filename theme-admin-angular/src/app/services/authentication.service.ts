import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import Swal from 'sweetalert2';
import { map } from 'rxjs/operators';
import {
  AUTHEN,
  NOTIFICATION_TITLE,
  UN_AUTHORIZED,
  URL_API,
} from '../helpers/constant-helper';
import { User } from '../helpers/user';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private userSubject: BehaviorSubject<User | null>;
  public user: Observable<User | null>;

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.userSubject = new BehaviorSubject<User | null>(
      JSON.parse(localStorage.getItem(AUTHEN) || '{}')
    );
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): User | null {
    return this.userSubject.value;
  }

  public signIn(userName: string, passWord: string): any {
    return this.http
      .post<any>(`${URL_API}Authentication/Login`, { userName, passWord })
      .pipe(
        map((user) => {
          // store user details and basic auth credentials in local storage to keep user logged in between page refreshes
          if (user.data) {
            const date = new Date().getTime();
            const { expireTime } = user.data;
            const exp = date + expireTime * 1000;
            const data = { ...user.data, expireTime: exp };
            localStorage.setItem(AUTHEN, JSON.stringify(data));
            this.userSubject.next(user.data);
            this.autoLogout(user.data.expireTime * 1000);
            return user;
          }
          return user;
        })
      );
  }

  public logout(): void {
    localStorage.removeItem(AUTHEN);
    this.userSubject.next(null);
    this.router.navigate(['/dang-nhap']);
  }

  private autoLogout(expireDate: number): any {
    setTimeout(() => {
      Swal.fire({
        icon: 'warning',
        title: NOTIFICATION_TITLE,
        text: UN_AUTHORIZED,
        confirmButtonText: 'Đồng ý',
        confirmButtonColor: 'var(--primary)',
        allowOutsideClick: false,
        allowEscapeKey: false,
      }).then(() => {
        localStorage.removeItem(AUTHEN);
        this.userSubject.next(null);
        this.router.navigate(['/dang-nhap']);
      });
    }, expireDate);
  }

  public autoLogin(): void {
    const userData: {
      userName: string;
      token: string;
      expireTime: number;
    } = JSON.parse(localStorage.getItem(AUTHEN) || '{}');
    if (!userData.userName) {
      this.logout();
      return;
    }
    const todaysDate = new Date().getTime();
    const expireDate = userData.expireTime;
    if (todaysDate > expireDate) {
      this.logout();
    } else {
      this.autoLogout(expireDate - todaysDate);
    }
  }
}
