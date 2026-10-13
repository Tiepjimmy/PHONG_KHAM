import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QUESTION_MARK, URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private httpClient: HttpClient) {}

  public getAll(request: {
    page: any;
    pageSize: any;
    sortField: any;
    sortOrder: any;
    fullName: any;
    userName: any;
    userGroupId: any;
  }): Observable<any> {
    const params = QUESTION_MARK.concat(
      `page=${request.page}`,
      `&pageSize=${request.pageSize}`,
      `&sortField=${request.sortField}`,
      `&sortOrder=${request.sortOrder}`,
      `&fullName=${request.fullName}`,
      `&userName=${request.userName}`,
      `&userGroupId=${request.userGroupId}`
    );
    return this.httpClient.get<any>(`${URL_API}User/GetAll${params}`);
  }

  public insert(params: any): Observable<any> {
    return this.httpClient.post<any>(`${URL_API}User/Insert`, params);
  }

  public update(params: any): Observable<any> {
    return this.httpClient.put<any>(`${URL_API}User/Update`, params);
  }

  public delete(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.delete<any>(`${URL_API}User/Delete${param}`);
  }

  public getById(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.get<any>(`${URL_API}User/GetById${param}`);
  }

  public getAllData(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}User/GetAllData`);
  }

  public resetPassword(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.get<any>(`${URL_API}User/ResetPassword${param}`);
  }

  public changePassword(params: any): Observable<any> {
    return this.httpClient.put<any>(`${URL_API}User/ChangePassword`, params);
  }

  public saveIsActive(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.get<any>(`${URL_API}User/SaveIsActive${param}`);
  }

  public saveIsAdmin(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.get<any>(`${URL_API}User/SaveIsAdmin${param}`);
  }

  public getAllUser(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}User/GetAllUser`);
  }
}
