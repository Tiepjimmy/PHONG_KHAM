import { Component, OnInit, Renderer2 } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import {
  checkClassActive,
  notificationCommon,
  permission,
} from 'src/app/helpers/common-helper';
import {
  ADD_MODAL_TITLE,
  DATE_FORMAT,
  ERROR,
  STRING_EMPTY,
  SUCCESS,
  UPDATE_MODAL_TITLE,
} from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { MedicineWarehouseService } from 'src/app/services/medicine-warehouse.service';
import { SupplierService } from 'src/app/services/supplier.service';
import { ModalService } from 'src/app/shared/modal.service';
import { NotifyService } from 'src/app/shared/notify.service';
import { TableState } from 'src/app/shared/table/table-state';
import { MedicalWarehouseMainModalComponent } from './medical-warehouse-main-modal/medical-warehouse-main-modal.component';

@Component({
  selector: 'app-medical-warehouse-main',
  templateUrl: './medical-warehouse-main.component.html',
})
export class MedicalWarehouseMainComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public dateFormat = DATE_FORMAT;
  public isLoadingSupplierId = false;
  public listSupplier: any[] = [];
  public listQuantity = [
    { id: 0, name: 'Tất cả' },
    { id: 1, name: 'Tồn kho' },
  ];

  constructor(
    private authenticationService: AuthenticationService,
    private medicineWarehouseService: MedicineWarehouseService,
    private supplierService: SupplierService,
    private formBuilder: UntypedFormBuilder,
    private notifyService: NotifyService,
    private modalService: ModalService,
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
    this.getAllDataSupplier();
    this.searchForm = this.formBuilder.group({
      searchText: [STRING_EMPTY],
      supplierId: [STRING_EMPTY],
      quantity: [0],
      isMain: [true],
    });
    if (this.permission?.allowAccess) {
      this.getData();
    }
  }

  private getAllDataSupplier(): void {
    this.isLoadingSupplierId = true;
    this.supplierService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listSupplier = data.listData;
        }
      },
      () => {
        this.isLoadingSupplierId = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingSupplierId = false;
      }
    );
  }

  private getData(): void {
    this.isLoading = true;
    const { searchText, supplierId, quantity } = this.searchForm.value;
    const param = {
      ...this.table.toParams(),
      searchText: searchText ?? STRING_EMPTY,
      supplierId: supplierId ?? STRING_EMPTY,
      quantity: quantity ?? 0,
      isMain: true,
    };
    this.medicineWarehouseService.getAll(param).subscribe(
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

  public openModal(data?: any): void {
    this.modalService
      .openForResult(MedicalWarehouseMainModalComponent, {
        title: data ? UPDATE_MODAL_TITLE : ADD_MODAL_TITLE,
        data,
        size: 'lg',
      })
      .subscribe((result) => {
        if (result?.reLoad) {
          this.getData();
        }
      });
  }

  public async delete(id: any): Promise<void> {
    if (!(await this.notifyService.confirmDelete())) {
      return;
    }
    this.isLoading = true;
    this.medicineWarehouseService.delete(id).subscribe(
      (result) => {
        if (result.responseCode === 200) {
          notificationCommon(
            this.notifyService,
            SUCCESS,
            result.responseMessage
          );
          if (this.listData.length === 1 && this.table.page > 1) {
            this.table.page--;
          }
          this.getData();
        } else {
          notificationCommon(
            this.notifyService,
            ERROR,
            result.responseMessage
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

  public reloadPage(): void {
    location.reload();
  }
}
