import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QUESTION_MARK, URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class PatientDetailService {
  constructor(private httpClient: HttpClient) {}

  public getAll(request: {
    page: any;
    pageSize: any;
    sortField: any;
    sortOrder: any;
    searchText: any;
    phoneNumber: any;
    createdDate: any;
  }): Observable<any> {
    const params = QUESTION_MARK.concat(
      `page=${request.page}`,
      `&pageSize=${request.pageSize}`,
      `&sortField=${request.sortField}`,
      `&sortOrder=${request.sortOrder}`,
      `&searchText=${request.searchText}`,
      `&phoneNumber=${request.phoneNumber}`,
      `&createdDate=${request.createdDate}`
    );
    return this.httpClient.get<any>(`${URL_API}PatientDetail/GetAll${params}`);
  }

  public insert(params: any): Observable<any> {
    return this.httpClient.post<any>(`${URL_API}PatientDetail/Insert`, params);
  }

  public update(params: any): Observable<any> {
    return this.httpClient.put<any>(`${URL_API}PatientDetail/Update`, params);
  }

  public delete(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.delete<any>(
      `${URL_API}PatientDetail/Delete${param}`
    );
  }

  public getById(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.get<any>(`${URL_API}PatientDetail/GetById${param}`);
  }

  public getAllData(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}PatientDetail/GetAllData`);
  }

  public generateCouponCode(): Observable<any> {
    return this.httpClient.get<any>(
      `${URL_API}PatientDetail/GenerateCouponCode`
    );
  }

  public updateResult(params: any): Observable<any> {
    return this.httpClient.put<any>(
      `${URL_API}PatientDetail/UpdateResult`,
      params
    );
  }
}
