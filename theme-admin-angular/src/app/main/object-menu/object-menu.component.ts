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
  ADD_MODAL_TITLE,
  ERROR,
  STRING_EMPTY,
  SUCCESS,
  UPDATE_MODAL_TITLE,
} from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ObjectMenuService } from 'src/app/services/object-menu.service';
import { ModalService } from 'src/app/shared/modal.service';
import { TableState } from 'src/app/shared/table/table-state';
import { ObjectMenuModalComponent } from './object-menu-modal/object-menu-modal.component';

@Component({
  selector: 'app-object-menu',
  templateUrl: './object-menu.component.html',
})
export class ObjectMenuComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public isLoadingParentId = false;
  public listParent: any[] = [];

  constructor(
    private authenticationService: AuthenticationService,
    private objectMenuService: ObjectMenuService,
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
    this.getAllDataParent();
    this.searchForm = this.formBuilder.group({
      searchText: [STRING_EMPTY],
      parentId: [STRING_EMPTY],
    });
    if (this.permission?.allowAccess) {
      this.getData();
    }
  }

  private getData(): void {
    this.isLoading = true;
    const { searchText, parentId } = this.searchForm.value;
    const param = {
      ...this.table.toParams(),
      searchText: searchText ?? STRING_EMPTY,
      parentId: parentId ?? STRING_EMPTY,
    };
    this.objectMenuService.getAll(param).subscribe(
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
      .openForResult(ObjectMenuModalComponent, {
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
    this.objectMenuService.delete(id).subscribe(
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

  public onChangeIsActive(id: any): void {
    this.isLoading = true;
    this.objectMenuService.saveIsActive(id).subscribe(
      (data) => {
        if (data.responseCode === 200) {
          notificationCommon(
            this.notifyService,
            SUCCESS,
            data.responseMessage
          );
        } else {
          notificationCommon(
            this.notifyService,
            ERROR,
            data.responseMessage
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

  private getAllDataParent(): void {
    this.isLoadingParentId = true;
    this.objectMenuService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listParent = data.listData;
        }
      },
      () => {
        this.isLoadingParentId = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingParentId = false;
      }
    );
  }
}
