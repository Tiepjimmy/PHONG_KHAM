import { Component, OnInit, Renderer2 } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NotifyService } from 'src/app/shared/notify.service';
import {
  checkClassActive,
  notificationCommon,
  permission,
} from 'src/app/helpers/common-helper';
import {
  DATE_FORMAT_ddMMyyyyHHmmss,
  ERROR,
  STRING_EMPTY,
} from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { LocationTrackingService } from 'src/app/services/location-tracking.service';
import { TableState } from 'src/app/shared/table/table-state';

@Component({
  selector: 'app-location-tracking',
  templateUrl: './location-tracking.component.html',
})
export class LocationTrackingComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public dateFormat = DATE_FORMAT_ddMMyyyyHHmmss;

  constructor(
    private authenticationService: AuthenticationService,
    private locationTrackingService: LocationTrackingService,
    private formBuilder: UntypedFormBuilder,
    private notifyService: NotifyService,
    private router: Router,
    private renderer: Renderer2
  ) {}

  public ngOnInit(): void {
    checkClassActive(
      this.router,
      this.renderer,
      this.authenticationService.userValue?.listMenu
    );
    this.permission = permission(
      this.router.url,
      this.authenticationService.userValue?.listMenu
    );
    this.searchForm = this.formBuilder.group({
      searchText: [STRING_EMPTY],
    });
    if (this.permission?.allowAccess) {
      this.getData();
    }
  }

  private getData(): void {
    this.isLoading = true;
    const { searchText } = this.searchForm.value;
    const param = {
      ...this.table.toParams(),
      searchText: searchText ?? STRING_EMPTY,
    };
    this.locationTrackingService.getAll(param).subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.table.total = data.totalRecords;
          this.listData = data.listData;
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

  public search(): void {
    this.table.page = 1;
    this.getData();
  }

  public sort(field: string): void {
    this.table.toggleSort(field);
    this.getData();
  }

  public changePage(page: number): void {
    this.table.page = page;
    this.getData();
  }

  public changePageSize(size: number): void {
    this.table.pageSize = size;
    this.table.page = 1;
    this.getData();
  }

  public reloadPage(): void {
    location.reload();
  }
}
