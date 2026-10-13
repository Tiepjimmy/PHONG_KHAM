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
  CLOSE_BRACKET,
  DATE_FORMAT_ddMMyyyyHHmmss,
  ERROR,
  ERROR_GUIDE,
  ERROR_GUIDE_PATTERN,
  OPEN_BRACKET,
  STRING_EMPTY,
} from 'src/app/helpers/constant-helper';
import { AuditChangeService } from 'src/app/services/audit-change.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { TableState } from 'src/app/shared/table/table-state';

@Component({
  selector: 'app-audit-change',
  templateUrl: './audit-change.component.html',
})
export class AuditChangeComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public dateFormat = DATE_FORMAT_ddMMyyyyHHmmss;
  public isLoadingAction = false;
  public listAction: any[] = [];
  public isLoadingEntityName = false;
  public listEntityName: any[] = [];

  constructor(
    private authenticationService: AuthenticationService,
    private auditChangeService: AuditChangeService,
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
    this.getAllEntityName();
    this.getAllAction();
    this.searchForm = this.formBuilder.group({
      idChange: [STRING_EMPTY],
      action: [STRING_EMPTY],
      entityName: [STRING_EMPTY],
    });
    if (this.permission?.allowAccess) {
      this.getData();
    }
  }

  private getData(): void {
    this.isLoading = true;
    const { idChange, action, entityName } = this.searchForm.value;
    const param = {
      ...this.table.toParams(),
      idChange: idChange ?? STRING_EMPTY,
      action: action ?? STRING_EMPTY,
      entityName: entityName ?? STRING_EMPTY,
    };
    this.auditChangeService.getAll(param).subscribe(
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
    const { idChange } = this.searchForm.value;
    if (idChange) {
      const checkIsGuide = ERROR_GUIDE_PATTERN.test(idChange);
      if (!checkIsGuide) {
        notificationCommon(this.notifyService, ERROR, ERROR_GUIDE);
        return;
      }
    }
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

  private getAllEntityName(): void {
    this.isLoadingEntityName = true;
    this.auditChangeService.getAllEntityName().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          data.listData.forEach((item: any) => {
            const entityName = {
              entityName: item.entityName
                .replace(OPEN_BRACKET, STRING_EMPTY)
                .replace(CLOSE_BRACKET, STRING_EMPTY),
            };
            this.listEntityName.push(entityName);
          });
          this.listEntityName = [...this.listEntityName];
        }
      },
      () => {
        this.isLoadingEntityName = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingEntityName = false;
      }
    );
  }

  private getAllAction(): void {
    this.isLoadingAction = true;
    this.auditChangeService.getAllAction().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listAction = data.listData;
        }
      },
      () => {
        this.isLoadingAction = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingAction = false;
      }
    );
  }
}
