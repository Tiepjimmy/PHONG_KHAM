import { Component, OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import moment from 'moment';
import { catchError, forkJoin, map, Observable, of } from 'rxjs';
import { checkClassActive } from 'src/app/helpers/common-helper';
import {
  DATE_FORMAT,
  DATE_FORMAT_DDMMyyyy1,
  DATE_FORMAT_yyyy_MM_DD,
  DOMAIN_ROUTER,
  SOURCE,
  STRING_EMPTY,
} from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { MedicineWarehouseService } from 'src/app/services/medicine-warehouse.service';
import { PatientService } from 'src/app/services/patient.service';
import { SupplierService } from 'src/app/services/supplier.service';

interface PagedResult {
  total: number;
  list: any[];
}

// W3CRM-style home: stat cards, 7-day patient chart, recent patients and expiring medicine.
// Built from the existing list APIs (the Dashboard/* endpoints are disabled in the backend).
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {
  public isLoading = false;
  public dateFormat = DATE_FORMAT;
  public dateLogin = moment(new Date()).format(DATE_FORMAT_DDMMyyyy1);
  public userName: any;
  public fullName: any;
  public userGroupName: any;
  public avatar = STRING_EMPTY;

  public patientToday = 0;
  public patientWeek = 0;
  public medicineTotal = 0;
  public expiringTotal = 0;
  public supplierTotal = 0;
  public recentPatients: any[] = [];
  public expiringMedicines: any[] = [];
  public link = (path: string) => SOURCE.concat(DOMAIN_ROUTER, SOURCE, path);

  public chart: any = null;

  constructor(
    private authenticationService: AuthenticationService,
    private patientService: PatientService,
    private medicineWarehouseService: MedicineWarehouseService,
    private supplierService: SupplierService,
    private router: Router,
    private renderer: Renderer2
  ) {}

  public ngOnInit(): void {
    const user = this.authenticationService.userValue;
    this.userName = user?.userName;
    this.fullName = user?.fullName;
    this.userGroupName = user?.userGroupName;
    this.avatar = user?.avatar ?? STRING_EMPTY;
    checkClassActive(this.router, this.renderer, user?.listMenu);
    this.loadData();
  }

  private loadData(): void {
    const days = Array.from({ length: 7 }, (_, i) => moment().subtract(6 - i, 'days'));
    this.isLoading = true;
    forkJoin({
      week: forkJoin(days.map((d) => this.patients(d.format(DATE_FORMAT_yyyy_MM_DD), 1))),
      today: this.patients(moment().format(DATE_FORMAT_yyyy_MM_DD), 5),
      medicine: this.paged(
        this.medicineWarehouseService.getAll({
          page: 1, pageSize: 1, sortField: STRING_EMPTY, sortOrder: STRING_EMPTY,
          searchText: STRING_EMPTY, supplierId: STRING_EMPTY, quantity: 0, isMain: true,
        })
      ),
      expiring: this.paged(
        this.medicineWarehouseService.getExpiry({ page: 1, pageSize: 5, expiry: 0, isMain: 'true' })
      ),
      supplier: this.paged(
        this.supplierService.getAll({
          page: 1, pageSize: 1, sortField: STRING_EMPTY, sortOrder: STRING_EMPTY, searchText: STRING_EMPTY,
        })
      ),
    }).subscribe((result) => {
      this.patientToday = result.today.total;
      this.recentPatients = result.today.list;
      this.patientWeek = result.week.reduce((sum, x) => sum + x.total, 0);
      this.medicineTotal = result.medicine.total;
      this.expiringTotal = result.expiring.total;
      this.expiringMedicines = result.expiring.list;
      this.supplierTotal = result.supplier.total;
      this.chart = this.buildChart(
        days.map((d) => d.format('DD/MM')),
        result.week.map((x) => x.total)
      );
      this.isLoading = false;
    });
  }

  private patients(createdDate: string, pageSize: number): Observable<PagedResult> {
    return this.paged(
      this.patientService.getAll({
        page: 1, pageSize, sortField: STRING_EMPTY, sortOrder: STRING_EMPTY,
        searchText: STRING_EMPTY, phoneNumber: STRING_EMPTY, createdDate,
      })
    );
  }

  // A failing widget must not break the whole page
  private paged(request: Observable<any>): Observable<PagedResult> {
    return request.pipe(
      map((data) =>
        data?.responseCode === 200
          ? { total: data.totalRecords ?? 0, list: data.listData ?? [] }
          : { total: 0, list: [] }
      ),
      catchError(() => of({ total: 0, list: [] }))
    );
  }

  private buildChart(categories: string[], data: number[]): any {
    return {
      series: [{ name: 'Bệnh nhân', data }],
      chart: { type: 'bar', height: 300, toolbar: { show: false }, fontFamily: 'Poppins, sans-serif' },
      plotOptions: { bar: { columnWidth: '35%', borderRadius: 4 } },
      colors: ['var(--primary)'],
      dataLabels: { enabled: false },
      grid: { borderColor: '#eee', strokeDashArray: 4 },
      xaxis: { categories, axisBorder: { show: false }, axisTicks: { show: false } },
      yaxis: { min: 0, forceNiceScale: true, labels: { formatter: (v: number) => (Number.isInteger(v) ? v.toString() : '') } },
      tooltip: { y: { formatter: (v: number) => `${v} bệnh nhân` } },
    };
  }
}
