import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class ImportDataService {
  constructor(private httpClient: HttpClient) {}

  public getAllTableSql(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}Import/GetAllTableSql`);
  }

  public save(params: any): Observable<any> {
    return this.httpClient.post<any>(`${URL_API}Import/Save`, params);
  }
}
