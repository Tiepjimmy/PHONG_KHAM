import {
  Component,
  ElementRef,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import moment from 'moment';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from 'src/app/shared/notify.service';
import {
  listFileType,
  notificationCommon,
} from 'src/app/helpers/common-helper';
import {
  BASE_IMAGE,
  BMP,
  DATE_FORMAT,
  DATE_FORMAT_yyyy_MM_DD,
  DOC,
  DOCX,
  DOT,
  ENC,
  ERROR,
  FILE_LIMITED,
  FILE_MAX_SIZE,
  FILE_REQUIRED,
  FILE_WRONG_FORMAT,
  GIF,
  IMAGE_FORMAT_ERROR,
  JPEG,
  JPG,
  MY_FILE,
  MY_FILES,
  PDF,
  PNG,
  PPT,
  PPTX,
  RAR,
  STRING_EMPTY,
  SUCCESS,
  TAR,
  XLS,
  XLSX,
  Z7Z,
  ZIP,
} from 'src/app/helpers/constant-helper';
import { DocumentFileService } from 'src/app/services/document-file.service';
import { TestDemoService } from 'src/app/services/test-demo.service';

@Component({
  selector: 'app-test-demo-modal',
  templateUrl: './test-demo-modal.component.html',
})
export class TestDemoModalComponent implements OnInit {
  public isLoading = false;
  public isLoadingFieldDropdownId = false;
  public formGroup!: UntypedFormGroup;
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public submitted = false;
  public listFieldDropdownId: any[] = [];
  public dateFormat = DATE_FORMAT;
  private imageBase = BASE_IMAGE;
  public imageSrc = this.imageBase;
  @ViewChild(MY_FILE) private myFileVariable!: ElementRef;
  private listFile: any[] = [];
  public listFileType = STRING_EMPTY;
  public listDocumentFile: any[] = [];
  @ViewChild(MY_FILES) private myFilesVariable!: ElementRef;
  // public formatterCurrencyVND = (value: number): string =>
  //   `${value.toString().replace(PATTERN_NUMBER_CURRENCY, COMMA)}`;
  // public parserCurrencyVND = (value: string): string =>
  //   value.replace(PATTERN_NUMBER_REMOVE_CURRENCY, STRING_EMPTY);

  constructor(
    private formBuilder: UntypedFormBuilder,
    private testDemoService: TestDemoService,
    private documentFileService: DocumentFileService,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.getAllDataDropdown();
    this.listFileType = listFileType();
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      fieldString: [STRING_EMPTY, [Validators.required]],
      fieldDateTime: [STRING_EMPTY],
      fieldDropdownId: [STRING_EMPTY],
      fieldNumber: [STRING_EMPTY],
      fieldTextArea: [STRING_EMPTY],
      fieldImageSingle: [STRING_EMPTY],
      fieldCheckBox: [false],
      fieldSwitch: [false],
      file: [null],
    });
    if (this.data) {
      this.isLoading = true;
      this.testDemoService.getById(this.data.id).subscribe(
        (data) => {
          if (data.responseCode === 200) {
            const item = data.data;
            item.fieldDateTime = item.fieldDateTime
              ? new Date(item.fieldDateTime)
              : STRING_EMPTY;
            this.formGroup.patchValue(item);
            this.imageSrc =
              item.fieldImageSingle !== STRING_EMPTY
                ? item.fieldImageSingle
                : this.imageBase;
            this.listDocumentFile = item.listDocumentFile;
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

  private getAllDataDropdown(): void {
    this.isLoadingFieldDropdownId = true;
    this.testDemoService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listFieldDropdownId = data.listData;
        }
      },
      () => {
        this.isLoadingFieldDropdownId = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingFieldDropdownId = false;
      }
    );
  }

  public destroyModal(reLoad: any): void {
    this.activeModal.close({ reLoad });
  }

  public isInvalid(name: string): boolean {
    const control = this.formGroup.controls[name];
    return control.invalid && (this.submitted || control.dirty || control.touched);
  }

  public submitForm(): void {
    this.submitted = true;
    if (!this.formGroup.valid) {
      return;
    }
    this.isLoading = true;
    const params = this.formGroup.value;
    params.fieldString = params.fieldString || STRING_EMPTY;
    params.fieldDateTime = params.fieldDateTime
      ? moment(params.fieldDateTime).format(DATE_FORMAT_yyyy_MM_DD)
      : STRING_EMPTY;
    params.fieldDropdownId = params.fieldDropdownId || STRING_EMPTY;
    params.fieldNumber = params.fieldNumber || STRING_EMPTY;
    params.fieldTextArea = params.fieldTextArea || STRING_EMPTY;
    params.fieldCheckBox = params.fieldCheckBox || false;
    params.fieldSwitch = params.fieldSwitch || false;
    const formData: FormData = new FormData();
    for (var i = 0; i < this.listFile.length; i++) {
      formData.append('files', this.listFile[i]);
    }
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.testDemoService.update(formData).subscribe(
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
    } else {
      this.testDemoService.insert(formData).subscribe(
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

  public onClickOpenSelectFile(): void {
    document.getElementById('file')?.click();
  }

  public onFileChange(event: any): void {
    const reader = new FileReader();
    const messError = IMAGE_FORMAT_ERROR;
    const fileAttach = event.target.files[0];
    const fileType = fileAttach
      ? fileAttach.name.split(DOT).pop()
      : STRING_EMPTY;

    if (event.target.files && event.target.files.length) {
      if (
        fileType.toLowerCase() !== PNG.toLowerCase() &&
        fileType.toLowerCase() !== JPG.toLowerCase() &&
        fileType.toLowerCase() !== JPEG.toLowerCase()
      ) {
        this.myFileVariable.nativeElement.value = STRING_EMPTY;
        this.formGroup.patchValue({
          file: null,
        });
        notificationCommon(this.notifyService, ERROR, messError);
        return;
      }
      if (fileAttach.size >= 10485760) {
        this.myFileVariable.nativeElement.value = STRING_EMPTY;
        notificationCommon(this.notifyService, ERROR, FILE_MAX_SIZE);
        return;
      }
      const [file] = event.target.files;
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.imageSrc = reader.result as string;
        this.formGroup.patchValue({
          file,
        });
      };
    } else {
      this.imageSrc = this.imageBase;
      this.myFileVariable.nativeElement.value = STRING_EMPTY;
      this.formGroup.patchValue({
        file: null,
      });
    }
  }

  public onFilesChange(event: any): void {
    this.listFile = [];
    let totalSize = 0;
    for (var i = 0; i < event.target.files.length; i++) {
      const size = event.target.files[0].size;
      totalSize += size;
      const fileType = event.target.files[0]
        ? event.target.files[0].name.split(DOT).pop()
        : STRING_EMPTY;
      if (
        fileType != DOC &&
        fileType != DOCX &&
        fileType != XLS &&
        fileType != XLSX &&
        fileType != BMP &&
        fileType != GIF &&
        fileType != JPEG &&
        fileType != JPG &&
        fileType != PNG &&
        fileType != Z7Z &&
        fileType != RAR &&
        fileType != TAR &&
        fileType != ENC &&
        fileType != ZIP &&
        fileType != PDF &&
        fileType != PPT &&
        fileType != PPTX
      ) {
        this.myFilesVariable.nativeElement.value = STRING_EMPTY;
        notificationCommon(
          this.notifyService,
          ERROR,
          FILE_WRONG_FORMAT
        );
        return;
      }
      if (size >= 10485760) {
        this.myFilesVariable.nativeElement.value = STRING_EMPTY;
        notificationCommon(this.notifyService, ERROR, FILE_MAX_SIZE);
        return;
      }
      this.listFile.push(event.target.files[i]);
    }
    if (this.listFile.length <= 0) {
      this.myFilesVariable.nativeElement.value = STRING_EMPTY;
      notificationCommon(this.notifyService, ERROR, FILE_REQUIRED);
      return;
    }
    if (this.listFile.length > 10) {
      this.myFilesVariable.nativeElement.value = STRING_EMPTY;
      notificationCommon(this.notifyService, ERROR, FILE_LIMITED);
      return;
    }
    if (totalSize >= 10485760) {
      this.myFilesVariable.nativeElement.value = STRING_EMPTY;
      notificationCommon(this.notifyService, ERROR, FILE_MAX_SIZE);
      return;
    }
  }

  public openFileBrower(url: any): void {
    window.open(url, '_blank');
  }

  public async deleteFile(id: any): Promise<void> {
    if (!(await this.notifyService.confirmDelete())) {
      return;
    }
    this.documentFileService.delete(id).subscribe(
      (result) => {
        if (result.responseCode === 200) {
          notificationCommon(
            this.notifyService,
            SUCCESS,
            result.responseMessage
          );
          this.listDocumentFile = this.listDocumentFile.filter(
            (x) => x.id !== id
          );
        } else {
          notificationCommon(
            this.notifyService,
            ERROR,
            result.responseMessage
          );
        }
      },
      () => {
        notificationCommon(this.notifyService, ERROR);
      },
      () => {}
    );
  }
}
