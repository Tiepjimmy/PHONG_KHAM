import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QUESTION_MARK, URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class UserRoleService {
  constructor(private httpClient: HttpClient) {}

  public getByUserGroupId(userGroupId: any): Observable<any> {
    const param = QUESTION_MARK.concat(`userGroupId=${userGroupId}`);
    return this.httpClient.get<any>(
      `${URL_API}UserRole/GetByUserGroupId${param}`
    );
  }

  public update(params: any): Observable<any> {
    return this.httpClient.put<any>(`${URL_API}UserRole/Update`, params);
  }
}
