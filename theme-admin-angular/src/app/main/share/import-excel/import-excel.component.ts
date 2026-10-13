import {
  Component,
  ElementRef,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from 'src/app/shared/notify.service';
import { notificationCommon } from 'src/app/helpers/common-helper';
import {
  DOT,
  ERROR,
  FILE_EXCEL_WRONG_FORMAT,
  FILE_MAX_SIZE,
  FILE_REQUIRED,
  MY_FILE,
  STRING_EMPTY,
  SUCCESS,
  XLS,
  XLSX,
} from 'src/app/helpers/constant-helper';
import { ImportExcelService } from 'src/app/services/import-excel.service';

@Component({
  selector: 'app-import-excel',
  templateUrl: './import-excel.component.html',
})
export class ImportExcelComponent implements OnInit {
  public isLoading = false;
  @Input() public title = STRING_EMPTY;
  // { tableName: string; fileNameUrl: string }
  @Input() public data: any;
  @ViewChild(MY_FILE) myFilesVariable!: ElementRef;
  private file: any = null;
  public formGroup!: UntypedFormGroup;

  constructor(
    private activeModal: NgbActiveModal,
    private formBuilder: UntypedFormBuilder,
    private notifyService: NotifyService,
    private importExcelService: ImportExcelService
  ) {}

  public ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      id: [0],
      tableName: [STRING_EMPTY],
    });
  }

  public destroyModal(reLoad: any): void {
    this.activeModal.close({ reLoad });
  }

  public onFilesChange(event: any): void {
    this.file = null;
    for (var i = 0; i < event.target.files.length; i++) {
      const size = event.target.files[0].size;
      const fileType = event.target.files[0]
        ? event.target.files[0].name.split(DOT).pop()
        : STRING_EMPTY;
      if (fileType != XLS && fileType != XLSX) {
        this.myFilesVariable.nativeElement.value = STRING_EMPTY;
        notificationCommon(
          this.notifyService,
          ERROR,
          FILE_EXCEL_WRONG_FORMAT
        );
        return;
      }
      if (size >= 15728640) {
        this.myFilesVariable.nativeElement.value = STRING_EMPTY;
        notificationCommon(this.notifyService, ERROR, FILE_MAX_SIZE);
        return;
      }
      this.file = event.target.files[i];
    }
    if (this.file === null) {
      this.myFilesVariable.nativeElement.value = STRING_EMPTY;
      notificationCommon(this.notifyService, ERROR, FILE_REQUIRED);
    }
  }

  public downloadTemplate(): void {
    window.open(this.data?.fileNameUrl);
  }

  public submitForm(): void {
    if (this.file === null) {
      notificationCommon(this.notifyService, ERROR, FILE_REQUIRED);
      return;
    }
    this.isLoading = true;
    const params = this.formGroup.value;
    params.tableName = this.data?.tableName;
    const formData: FormData = new FormData();
    formData.append('file', this.file);
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    this.importExcelService.save(formData).subscribe(
      (data) => {
        if (data.responseCode === 200) {
          notificationCommon(
            this.notifyService,
            SUCCESS,
            data.responseMessage
          );
          this.destroyModal(true);
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
