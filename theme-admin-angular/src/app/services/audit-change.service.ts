import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QUESTION_MARK, URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class AuditChangeService {
  constructor(private httpClient: HttpClient) {}

  public getAll(request: {
    page: any;
    pageSize: any;
    sortField: any;
    sortOrder: any;
    idChange: any;
    action: any;
    entityName: any;
  }): Observable<any> {
    const params = QUESTION_MARK.concat(
      `page=${request.page}`,
      `&pageSize=${request.pageSize}`,
      `&sortField=${request.sortField}`,
      `&sortOrder=${request.sortOrder}`,
      `&idChange=${request.idChange}`,
      `&action=${request.action}`,
      `&entityName=${request.entityName}`
    );
    return this.httpClient.get<any>(`${URL_API}AuditChange/GetAll${params}`);
  }

  public getAllEntityName(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}AuditChange/GetAllEntityName`);
  }

  public getAllAction(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}AuditChange/GetAllAction`);
  }

  public deleteData(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}AuditChange/DeleteData`);
  }
}
