import { Component, OnInit, Renderer2 } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import {
  checkClassActive,
  downloadFile,
  notificationCommon,
  permission,
} from 'src/app/helpers/common-helper';
import { DATE_FORMAT, ERROR } from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { MedicineWarehouseService } from 'src/app/services/medicine-warehouse.service';
import { NotifyService } from 'src/app/shared/notify.service';
import { TableState } from 'src/app/shared/table/table-state';

@Component({
  selector: 'app-medical-warehouse-expiry',
  templateUrl: './medical-warehouse-expiry.component.html',
})
export class MedicalWarehouseExpiryComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public dateFormat = DATE_FORMAT;
  public listWarehouse = [
    { id: 'true', name: 'Kho thuốc chính' },
    { id: 'false', name: 'Kho thuốc lẻ' },
  ];

  constructor(
    private authenticationService: AuthenticationService,
    private medicineWarehouseService: MedicineWarehouseService,
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
      expiry: [0],
      isMain: ['true'],
    });
    if (this.permission?.allowAccess) {
      this.getData();
    }
  }

  private getData(): void {
    this.isLoading = true;
    const { expiry, isMain } = this.searchForm.value;
    const param = {
      ...this.table.toParams(),
      expiry: expiry || 0,
      isMain: isMain ?? 'true',
    };
    this.medicineWarehouseService.getExpiry(param).subscribe(
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

  public print(): void {
    this.isLoading = true;
    const { expiry, isMain } = this.searchForm.value;
    const param = {
      expiry: expiry,
      isMain: isMain,
    };
    this.medicineWarehouseService.downloadExpiry(param).subscribe(
      (response) => {
        if (response.responseCode === 200) {
          downloadFile(
            response.fileContent,
            response.contentType,
            response.fileName
          );
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
}
