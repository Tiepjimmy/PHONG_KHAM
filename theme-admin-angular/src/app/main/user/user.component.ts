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
import { UserGroupService } from 'src/app/services/user-group.service';
import { UserService } from 'src/app/services/user.service';
import { ModalService } from 'src/app/shared/modal.service';
import { TableState } from 'src/app/shared/table/table-state';
import { UserModalComponent } from './user-modal/user-modal.component';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
})
export class UserComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public isLoadingUserGroupId = false;
  public listUserGroup: any[] = [];

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private userGroupService: UserGroupService,
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
    this.getAllData();
    this.searchForm = this.formBuilder.group({
      fullName: [STRING_EMPTY],
      userName: [STRING_EMPTY],
      userGroupId: [STRING_EMPTY],
    });
    if (this.permission?.allowAccess) {
      this.getData();
    }
  }

  private getAllData(): void {
    this.isLoadingUserGroupId = true;
    this.userGroupService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listUserGroup = data.listData;
        }
      },
      () => {
        this.isLoadingUserGroupId = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingUserGroupId = false;
      }
    );
  }

  private getData(): void {
    this.isLoading = true;
    const { fullName, userName, userGroupId } = this.searchForm.value;
    const param = {
      ...this.table.toParams(),
      fullName: fullName ?? STRING_EMPTY,
      userName: userName ?? STRING_EMPTY,
      userGroupId: userGroupId ?? STRING_EMPTY,
    };
    this.userService.getAll(param).subscribe(
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
      .openForResult(UserModalComponent, {
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
    this.userService.delete(id).subscribe(
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

  public async resetPassword(id: any): Promise<void> {
    if (
      !(await this.notifyService.confirmDelete(
        'Bạn có chắc chắn muốn reset lại mật khẩu không?'
      ))
    ) {
      return;
    }
    this.isLoading = true;
    this.userService.resetPassword(id).subscribe(
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

  public onChangeIsActive(id: any): void {
    this.isLoading = true;
    this.userService.saveIsActive(id).subscribe(
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

  public onChangeIsAdmin(id: any): void {
    this.isLoading = true;
    this.userService.saveIsAdmin(id).subscribe(
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
}
