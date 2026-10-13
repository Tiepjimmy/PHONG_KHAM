import { Component, OnInit, Renderer2 } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import {
  checkClassActive,
  downloadFile,
  notificationCommon,
  permission,
} from 'src/app/helpers/common-helper';
import {
  ADD_MODAL_TITLE,
  DATE_FORMAT,
  ERROR,
  PATH_TEMPLATE_EXCEL,
  STRING_EMPTY,
  SUCCESS,
  UPDATE_MODAL_TITLE,
} from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { TestDemoService } from 'src/app/services/test-demo.service';
import { ModalService } from 'src/app/shared/modal.service';
import { NotifyService } from 'src/app/shared/notify.service';
import { TableState } from 'src/app/shared/table/table-state';
import { ImportExcelComponent } from '../share/import-excel/import-excel.component';
import { TestDemoModalComponent } from './test-demo-modal/test-demo-modal.component';

@Component({
  selector: 'app-test-demo',
  templateUrl: './test-demo.component.html',
})
export class TestDemoComponent implements OnInit {
  public listData: any[] = [];
  public searchForm!: UntypedFormGroup;
  public isLoading = false;
  public table = new TableState();
  public permission: any;
  public dateFormat = DATE_FORMAT;

  constructor(
    private authenticationService: AuthenticationService,
    private testDemoService: TestDemoService,
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
    const param = {
      ...this.table.toParams(),
      searchText: searchText ?? STRING_EMPTY,
    };
    this.testDemoService.getAll(param).subscribe(
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
      .openForResult(TestDemoModalComponent, {
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
    this.testDemoService.delete(id).subscribe(
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

  public exportToExcel(): void {
    this.isLoading = true;
    const { searchText } = this.searchForm.value;
    const param = {
      searchText: searchText,
    };
    this.testDemoService.exportToExcel(param).subscribe(
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

  public print(): void {
    window.print();
  }

  public importToExcel(): void {
    const data = {
      tableName: 'TestDemo',
      fileNameUrl: PATH_TEMPLATE_EXCEL.concat('TestDemo_Template.xlsx'),
    };
    this.modalService
      .openForResult(ImportExcelComponent, {
        title: 'Nhập dữ liệu từ tập tin excel',
        data,
        size: 'lg',
      })
      .subscribe((result) => {
        if (result?.reLoad) {
          this.getData();
        }
      });
  }

  public downloadToWord(): void {
    this.isLoading = true;
    this.testDemoService.downloadToWord().subscribe(
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
