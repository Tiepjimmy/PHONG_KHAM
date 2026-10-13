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
  ERROR,
  STRING_EMPTY,
  SUCCESS,
  UPDATE_MODAL_TITLE,
} from 'src/app/helpers/constant-helper';
import { AdministrativeWardService } from 'src/app/services/administrative-ward.service';
import { AdministrativeDistrictService } from 'src/app/services/administrative-district.service';
import { AdministrativeUnitService } from 'src/app/services/administrative-unit.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ModalService } from 'src/app/shared/modal.service';
import { NotifyService } from 'src/app/shared/notify.service';
import { TableState } from 'src/app/shared/table/table-state';
import { AdministrativeWardModalComponent } from './administrative-ward-modal/administrative-ward-modal.component';

@Component({
  selector: 'app-administrative-ward',
  templateUrl: './administrative-ward.component.html',
})
export class AdministrativeWardComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public isLoadingAdministrativeUnit = false;
  public listAdministrativeUnit: any[] = [];
  public isLoadingAdministrativeDistrict = false;
  public listAdministrativeDistrict: any[] = [];

  constructor(
    private authenticationService: AuthenticationService,
    private administrativeWardService: AdministrativeWardService,
    private administrativeUnitService: AdministrativeUnitService,
    private administrativeDistrictService: AdministrativeDistrictService,
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
    this.getAllDataAdministrativeUnit();
    this.getAllDataAdministrativeDistrict();
    this.searchForm = this.formBuilder.group({
      searchText: [STRING_EMPTY],
      administrativeUnitId: [STRING_EMPTY],
      administrativeDistrictId: [STRING_EMPTY],
    });
    if (this.permission?.allowAccess) {
      this.getData();
    }
  }

  private getData(): void {
    this.isLoading = true;
    const { searchText, administrativeUnitId, administrativeDistrictId } =
      this.searchForm.value;
    const param = {
      ...this.table.toParams(),
      searchText: searchText ?? STRING_EMPTY,
      administrativeUnitId: administrativeUnitId ?? STRING_EMPTY,
      administrativeDistrictId: administrativeDistrictId ?? STRING_EMPTY,
    };
    this.administrativeWardService.getAll(param).subscribe(
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
      .openForResult(AdministrativeWardModalComponent, {
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
    this.administrativeWardService.delete(id).subscribe(
      (result) => {
        if (result.responseCode === 200) {
          notificationCommon(
            this.notifyService,
            SUCCESS,
            result.responseMessage
          );
          // Step back when the last row of the last page was removed
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

  private getAllDataAdministrativeUnit(): void {
    this.isLoadingAdministrativeUnit = true;
    this.administrativeUnitService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listAdministrativeUnit = data.listData;
        }
      },
      () => {
        this.isLoadingAdministrativeUnit = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingAdministrativeUnit = false;
      }
    );
  }

  private getAllDataAdministrativeDistrict(): void {
    this.isLoadingAdministrativeDistrict = true;
    this.administrativeDistrictService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listAdministrativeDistrict = data.listData;
        }
      },
      () => {
        this.isLoadingAdministrativeDistrict = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingAdministrativeDistrict = false;
      }
    );
  }
}
