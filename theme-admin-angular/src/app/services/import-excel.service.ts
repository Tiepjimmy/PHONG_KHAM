import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class ImportExcelService {
  constructor(private httpClient: HttpClient) {}

  public save(params: any): Observable<any> {
    return this.httpClient.post<any>(`${URL_API}ImportExcel/Save`, params);
  }
}
