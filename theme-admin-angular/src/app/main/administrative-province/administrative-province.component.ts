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
import { AdministrativeProvinceService } from 'src/app/services/administrative-province.service';
import { AdministrativeRegionService } from 'src/app/services/administrative-region.service';
import { AdministrativeUnitService } from 'src/app/services/administrative-unit.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ModalService } from 'src/app/shared/modal.service';
import { NotifyService } from 'src/app/shared/notify.service';
import { TableState } from 'src/app/shared/table/table-state';
import { AdministrativeProvinceModalComponent } from './administrative-province-modal/administrative-province-modal.component';

@Component({
  selector: 'app-administrative-province',
  templateUrl: './administrative-province.component.html',
})
export class AdministrativeProvinceComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public isLoadingAdministrativeUnit = false;
  public listAdministrativeUnit: any[] = [];
  public isLoadingAdministrativeRegion = false;
  public listAdministrativeRegion: any[] = [];

  constructor(
    private authenticationService: AuthenticationService,
    private administrativeProvinceService: AdministrativeProvinceService,
    private administrativeUnitService: AdministrativeUnitService,
    private administrativeRegionService: AdministrativeRegionService,
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
    this.getAllDataAdministrativeRegion();
    this.searchForm = this.formBuilder.group({
      searchText: [STRING_EMPTY],
      administrativeUnitId: [STRING_EMPTY],
      administrativeRegionId: [STRING_EMPTY],
    });
    if (this.permission?.allowAccess) {
      this.getData();
    }
  }

  private getData(): void {
    this.isLoading = true;
    const { searchText, administrativeUnitId, administrativeRegionId } =
      this.searchForm.value;
    const param = {
      ...this.table.toParams(),
      searchText: searchText ?? STRING_EMPTY,
      administrativeUnitId: administrativeUnitId ?? STRING_EMPTY,
      administrativeRegionId: administrativeRegionId ?? STRING_EMPTY,
    };
    this.administrativeProvinceService.getAll(param).subscribe(
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
      .openForResult(AdministrativeProvinceModalComponent, {
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
    this.administrativeProvinceService.delete(id).subscribe(
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

  private getAllDataAdministrativeRegion(): void {
    this.isLoadingAdministrativeRegion = true;
    this.administrativeRegionService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listAdministrativeRegion = data.listData;
        }
      },
      () => {
        this.isLoadingAdministrativeRegion = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingAdministrativeRegion = false;
      }
    );
  }
}
