import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { URL_API } from '../helpers/constant-helper';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  constructor(private httpClient: HttpClient) {}

  public getCount(): Observable<any> {
    return this.httpClient.get<any>(`${URL_API}Dashboard/GetCount`);
  }

  public getListChartOptionsBar(): Observable<any> {
    return this.httpClient.get<any>(
      `${URL_API}Dashboard/GetListChartOptionsBar`
    );
  }

  public getListChartOptionsPolarArea(): Observable<any> {
    return this.httpClient.get<any>(
      `${URL_API}Dashboard/GetListChartOptionsPolarArea`
    );
  }
}
