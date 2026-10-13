import { Component, OnInit, Renderer2, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import moment from 'moment';
import { ChartComponent } from 'ng-apexcharts';
import { NotifyService } from 'src/app/shared/notify.service';
import { ChartOptionsBar } from 'src/app/helpers/chart-options-bar';
import { ChartOptionsPolarArea } from 'src/app/helpers/chart-options-polar-area';
import {
  checkClassActive,
  notificationCommon,
} from 'src/app/helpers/common-helper';
import {
  DATE_FORMAT,
  DATE_FORMAT_DDMMyyyy1,
  ERROR,
} from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DashboardService } from 'src/app/services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {
  public data: any;
  public isLoading = false;

  public chartOptionsBar!: Partial<ChartOptionsBar>;
  public chartOptionsPolarArea!: Partial<ChartOptionsPolarArea>;
  public dateFormat = DATE_FORMAT;
  public dateLogin = moment(new Date()).format(DATE_FORMAT_DDMMyyyy1);
  public userName: any;
  public fullName: any;
  public userGroupName: any;

  constructor(
    private authenticationService: AuthenticationService,
    private dashboardService: DashboardService,
    private notifyService: NotifyService,
    private router: Router,
    private renderer: Renderer2
  ) {}

  public ngOnInit(): void {
    this.userName = this.authenticationService.userValue?.userName;
    this.fullName = this.authenticationService.userValue?.fullName;
    this.userGroupName = this.authenticationService.userValue?.userGroupName;
    checkClassActive(
      this.router,
      this.renderer,
      this.authenticationService.userValue?.listMenu
    );
    // this.getAll();
  }

  private getAll(): void {
    this.getCount();
    this.getListChartOptionsBar();
    this.getListChartOptionsPolarArea();
  }

  private getCount(): void {
    this.isLoading = true;
    this.dashboardService.getCount().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.data = data.data;
        }
      },
      () => {
        this.isLoading = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoading = false;
      }
    );
  }

  private getListChartOptionsBar(): void {
    this.dashboardService.getListChartOptionsBar().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.chartOptionsBarView(data.data.chartOptionsBar);
        }
      },
      () => {
        notificationCommon(this.notifyService, ERROR);
      },
      () => {}
    );
  }

  private getListChartOptionsPolarArea(): void {
    this.dashboardService.getListChartOptionsPolarArea().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.chartOptionsPolarAreaView(data.data.chartOptionsPolarArea);
        }
      },
      () => {
        notificationCommon(this.notifyService, ERROR);
      },
      () => {}
    );
  }

  private chartOptionsBarView(data: any): void {
    this.chartOptionsBar = {
      series: [{ name: 'Công việc', data: data.data }],
      chart: {
        height: 350,
        type: 'bar',
        toolbar: {
          show: false,
        },
      },
      colors: data.color,
      plotOptions: {
        bar: {
          columnWidth: '30%',
          distributed: true,
        },
      },
      dataLabels: {
        enabled: false,
      },
      legend: {
        show: true,
        horizontalAlign: 'center',
      },
      grid: {
        show: true,
      },
      xaxis: {
        categories: data.categories,
        labels: {
          style: {
            colors: data.color,
            fontSize: '12px',
          },
        },
      },
      yaxis: {
        axisBorder: {
          show: true,
        },
        axisTicks: {
          show: true,
        },
        labels: {
          show: true,
          formatter: function (val) {
            return val.toFixed(0);
          },
        },
      },
    };
  }

  private chartOptionsPolarAreaView(data: any): void {
    this.chartOptionsPolarArea = {
      series: data.data,
      chart: {
        height: 350,
        type: 'polarArea',
      },
      labels: data.categories,
      stroke: {
        colors: ['#fff'],
      },
      fill: {
        opacity: 0.8,
      },
      yaxis: {
        show: false,
      },
      legend: {
        position: 'bottom',
      },
    };
  }
}
