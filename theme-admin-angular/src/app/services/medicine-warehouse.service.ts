import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QUESTION_MARK, URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class MedicineWarehouseService {
  constructor(private httpClient: HttpClient) {}

  public getAll(request: {
    page: any;
    pageSize: any;
    sortField: any;
    sortOrder: any;
    searchText: any;
    supplierId: any;
    quantity: any;
    isMain: any;
  }): Observable<any> {
    const params = QUESTION_MARK.concat(
      `page=${request.page}`,
      `&pageSize=${request.pageSize}`,
      `&sortField=${request.sortField}`,
      `&sortOrder=${request.sortOrder}`,
      `&searchText=${request.searchText}`,
      `&supplierId=${request.supplierId}`,
      `&quantity=${request.quantity}`,
      `&isMain=${request.isMain}`
    );
    return this.httpClient.get<any>(
      `${URL_API}MedicineWarehouse/GetAll${params}`
    );
  }

  public insert(params: any): Observable<any> {
    return this.httpClient.post<any>(
      `${URL_API}MedicineWarehouse/Insert`,
      params
    );
  }

  public update(params: any): Observable<any> {
    return this.httpClient.put<any>(
      `${URL_API}MedicineWarehouse/Update`,
      params
    );
  }

  public delete(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.delete<any>(
      `${URL_API}MedicineWarehouse/Delete${param}`
    );
  }

  public getById(id: any): Observable<any> {
    const param = QUESTION_MARK.concat(`id=${id}`);
    return this.httpClient.get<any>(
      `${URL_API}MedicineWarehouse/GetById${param}`
    );
  }

  public getAllData(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}MedicineWarehouse/GetAllData`);
  }

  public getExpiry(request: {
    page: any;
    pageSize: any;
    expiry: any;
    isMain: any;
  }): Observable<any> {
    const params = `?page=${request.page}&pageSize=${request.pageSize}
    &expiry=${request.expiry}
    &isMain=${request.isMain}`;
    return this.httpClient.get<any>(
      `${URL_API}MedicineWarehouse/GetExpiry${params}`
    );
  }

  public downloadExpiry(request: {
    expiry: any;
    isMain: any;
  }): Observable<any> {
    const params = `?expiry=${request.expiry}&isMain=${request.isMain}`;
    return this.httpClient.get<any>(
      `${URL_API}MedicineWarehouse/DownloadExpiry${params}`
    );
  }
}
