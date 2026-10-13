import { Component, OnInit, Renderer2 } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { checkClassActive, permission } from 'src/app/helpers/common-helper';
import {
  ADD_MODAL_TITLE,
  STRING_EMPTY,
  UPDATE_MODAL_TITLE,
} from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UnitMeasureService } from 'src/app/services/unit-measure.service';
import { ModalService } from 'src/app/shared/modal.service';
import { NotifyService } from 'src/app/shared/notify.service';
import { TableState } from 'src/app/shared/table/table-state';
import { UnitMeasureModalComponent } from './unit-measure-modal/unit-measure-modal.component';

@Component({
  selector: 'app-unit-measure',
  templateUrl: './unit-measure.component.html',
})
export class UnitMeasureComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;

  constructor(
    private authenticationService: AuthenticationService,
    private unitMeasureService: UnitMeasureService,
    private formBuilder: UntypedFormBuilder,
    private notify: NotifyService,
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
    const param = { ...this.table.toParams(), searchText: searchText ?? STRING_EMPTY };
    this.unitMeasureService.getAll(param).subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.table.total = data.totalRecords;
          this.listData = data.listData;
        }
      },
      () => {
        this.isLoading = false;
        this.notify.error();
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
      .openForResult(UnitMeasureModalComponent, {
        title: data ? UPDATE_MODAL_TITLE : ADD_MODAL_TITLE,
        data,
      })
      .subscribe((result) => {
        if (result?.reLoad) {
          this.getData();
        }
      });
  }

  public async delete(id: any): Promise<void> {
    if (!(await this.notify.confirmDelete())) {
      return;
    }
    this.isLoading = true;
    this.unitMeasureService.delete(id).subscribe(
      (result) => {
        if (result.responseCode === 200) {
          this.notify.success(result.responseMessage);
          // Step back when the last row of the last page was removed
          if (this.listData.length === 1 && this.table.page > 1) {
            this.table.page--;
          }
          this.getData();
        } else {
          this.notify.error(result.responseMessage);
        }
      },
      () => {
        this.isLoading = false;
        this.notify.error();
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
