import { Component, OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { NotifyService } from 'src/app/shared/notify.service';
import {
  checkClassActive,
  notificationCommon,
  permission,
} from 'src/app/helpers/common-helper';
import { ERROR, SUCCESS } from 'src/app/helpers/constant-helper';
import { AuditChangeService } from 'src/app/services/audit-change.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { LocationTrackingService } from 'src/app/services/location-tracking.service';

@Component({
  selector: 'app-delete-data',
  templateUrl: './delete-data.component.html',
})
export class DeleteDataComponent implements OnInit {
  public isLoading = false;
  public permission: any;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private renderer: Renderer2,
    private auditChangeService: AuditChangeService,
    private locationTrackingService: LocationTrackingService,
    private notifyService: NotifyService
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
  }

  public async deleteData(): Promise<void> {
    if (
      !(await this.notifyService.confirmDelete(
        'Bạn có chắc chắn muốn xóa tất cả dữ liệu cũ không?'
      ))
    ) {
      return;
    }
    this.isLoading = true;
    this.auditChangeService.deleteData().subscribe(
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

  public async deleteDataHistory(): Promise<void> {
    if (
      !(await this.notifyService.confirmDelete(
        'Bạn có chắc chắn muốn xóa tất cả dữ liệu lịch sử không?'
      ))
    ) {
      return;
    }
    this.isLoading = true;
    this.locationTrackingService.deleteData().subscribe(
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
}
