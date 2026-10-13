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
import { AreaService } from 'src/app/services/area.service';
import { RegionService } from 'src/app/services/region.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ModalService } from 'src/app/shared/modal.service';
import { NotifyService } from 'src/app/shared/notify.service';
import { TableState } from 'src/app/shared/table/table-state';
import { AreaModalComponent } from './area-modal/area-modal.component';

@Component({
  selector: 'app-area',
  templateUrl: './area.component.html',
})
export class AreaComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public isLoadingRegion = false;
  public listRegion: any[] = [];

  constructor(
    private authenticationService: AuthenticationService,
    private areaService: AreaService,
    private regionService: RegionService,
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
    // this.getAllDataAdministrativeUnit();
    this.getAllDataByRegion();
    this.searchForm = this.formBuilder.group({
      searchText: [STRING_EMPTY],
      regionId: [STRING_EMPTY],
    });
    if (this.permission?.allowAccess) {
      this.getData();
    }
  }

  private getData(): void {
    this.isLoading = true;
    const { searchText, regionId } = this.searchForm.value;
    const param = {
      ...this.table.toParams(),
      searchText: searchText ?? STRING_EMPTY,
      regionId: regionId ?? STRING_EMPTY,
    };
    this.areaService.getAll(param).subscribe(
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
      .openForResult(AreaModalComponent, {
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
    this.areaService.delete(id).subscribe(
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

  private getAllDataByRegion(): void {
    this.isLoadingRegion = true;
    this.regionService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listRegion = data.listData;
        }
      },
      () => {
        this.isLoadingRegion = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingRegion = false;
      }
    );
  }
}
