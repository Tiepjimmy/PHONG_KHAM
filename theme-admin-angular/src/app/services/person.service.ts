import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QUESTION_MARK, URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class PersonService {
  constructor(private httpClient: HttpClient) {}

  public getAll(request: {
    page: any;
    pageSize: any;
    sortField: any;
    sortOrder: any;
    searchText: any;
  }): Observable<any> {
    const params = QUESTION_MARK.concat(
      `page=${request.page}`,
      `&pageSize=${request.pageSize}`,
      `&sortField=${request.sortField}`,
      `&sortOrder=${request.sortOrder}`,
      `&searchText=${request.searchText}`
    );
    return this.httpClient.get<any>(`${URL_API}Person/GetAll${params}`);
  }

  public insert(params: any): Observable<any> {
    return this.httpClient.post<any>(`${URL_API}Person/Insert`, params);
  }

  public update(params: any): Observable<any> {
    return this.httpClient.put<any>(`${URL_API}Person/Update`, params);
  }

  public delete(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.delete<any>(`${URL_API}Person/Delete${param}`);
  }

  public getById(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.get<any>(`${URL_API}Person/GetById${param}`);
  }

  public getAllData(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}Person/GetAllData`);
  }

  public getAllDataByKey(key: any, isEdit: boolean): Observable<any> {
    const param = `?key=${key}&isEdit=${isEdit}`;
    return this.httpClient.get<any>(`${URL_API}Person/GetAllDataByKey${param}`);
  }

  public getProfile(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}Person/GetProfile`);
  }

  public updateProfile(params: any): Observable<any> {
    return this.httpClient.put<any>(`${URL_API}Person/UpdateProfile`, params);
  }
}
