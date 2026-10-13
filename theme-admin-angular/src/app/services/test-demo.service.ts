import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QUESTION_MARK, URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class TestDemoService {
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
    return this.httpClient.get<any>(`${URL_API}TestDemo/GetAll${params}`);
  }

  public insert(params: any): Observable<any> {
    return this.httpClient.post<any>(`${URL_API}TestDemo/Insert`, params);
  }

  public update(params: any): Observable<any> {
    return this.httpClient.put<any>(`${URL_API}TestDemo/Update`, params);
  }

  public delete(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.delete<any>(`${URL_API}TestDemo/Delete${param}`);
  }

  public getById(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.get<any>(`${URL_API}TestDemo/GetById${param}`);
  }

  public getAllData(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}TestDemo/GetAllData`);
  }

  public exportToExcel(request: { searchText: any }): Observable<any> {
    const param = QUESTION_MARK.concat(`searchText=${request.searchText}`);
    return this.httpClient.get(`${URL_API}TestDemo/ExportToExcel${param}`);
  }

  public downloadToWord(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}TestDemo/DownloadToWord`);
  }
}
