import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import moment from 'moment';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from 'src/app/shared/notify.service';
import { notificationCommon } from 'src/app/helpers/common-helper';
import {
  AUTHEN,
  BASE_IMAGE,
  DATE_FORMAT,
  DATE_FORMAT_yyyy_MM_DD,
  DOT,
  ERROR,
  IMAGE_FORMAT_ERROR,
  JPEG,
  JPG,
  MY_FILE,
  PNG,
  STRING_EMPTY,
} from 'src/app/helpers/constant-helper';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
})
export class ProfileComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public listSex = [
    { id: 'Nam', name: 'Nam' },
    { id: 'Nữ', name: 'Nữ' },
    { id: 'Khác', name: 'Khác' },
  ];
  public profileForm!: UntypedFormGroup;
  public dateFormat = DATE_FORMAT;
  private imageBase = BASE_IMAGE;
  public imageSrc = this.imageBase;
  @ViewChild(MY_FILE) private myFileVariable!: ElementRef;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal,
    private personService: PersonService
  ) {}

  public ngOnInit(): void {
    this.isLoading = true;
    this.profileForm = this.formBuilder.group({
      id: [STRING_EMPTY],
      userName: [{ value: STRING_EMPTY, disabled: true }],
      fullName: [STRING_EMPTY, [Validators.required]],
      phoneNumber: [STRING_EMPTY, [Validators.required]],
      dateOfBirth: [STRING_EMPTY, [Validators.required]],
      sex: [STRING_EMPTY, [Validators.required]],
      address: [STRING_EMPTY],
      file: [null],
    });
    this.personService.getProfile().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          const item = data.data;
          item.dateOfBirth = item.dateOfBirth
            ? new Date(item.dateOfBirth)
            : STRING_EMPTY;
          this.profileForm.patchValue(item);
          this.imageSrc =
            item.avatar !== STRING_EMPTY ? item.avatar : this.imageBase;
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

  public destroyModal(reLoad: any): void {
    this.activeModal.close({ reLoad });
  }

  public isInvalid(name: string): boolean {
    const control = this.profileForm.controls[name];
    return control.invalid && (this.submitted || control.dirty || control.touched);
  }

  public submitForm(): void {
    this.submitted = true;
    if (!this.profileForm.valid) {
      return;
    }
    this.isLoading = true;
    const params = this.profileForm.value;
    params.fullName = params.fullName || STRING_EMPTY;
    params.phoneNumber = params.phoneNumber || STRING_EMPTY;
    params.dateOfBirth = params.dateOfBirth
      ? moment(params.dateOfBirth).format(DATE_FORMAT_yyyy_MM_DD)
      : STRING_EMPTY;
    params.sex = params.sex || STRING_EMPTY;
    params.address = params.address || STRING_EMPTY;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    this.personService.updateProfile(formData).subscribe(
      (data) => {
        if (data.responseCode === 200) {
          const info = JSON.parse(localStorage.getItem(AUTHEN) || '{}');
          info.fullName = params.fullName;
          info.avatar = data.responseMessage;
          localStorage.removeItem(AUTHEN);
          localStorage.setItem(AUTHEN, JSON.stringify(info));
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
        location.reload();
      }
    );
  }

  public onFileChange(event: any): void {
    const reader = new FileReader();
    const messError = IMAGE_FORMAT_ERROR;
    const fileType = event.target.files[0]
      ? event.target.files[0].name.split(DOT).pop()
      : STRING_EMPTY;

    if (event.target.files && event.target.files.length) {
      if (
        fileType.toLowerCase() !== PNG.toLowerCase() &&
        fileType.toLowerCase() !== JPG.toLowerCase() &&
        fileType.toLowerCase() !== JPEG.toLowerCase()
      ) {
        this.myFileVariable.nativeElement.value = STRING_EMPTY;
        this.profileForm.patchValue({
          file: null,
        });
        notificationCommon(this.notifyService, ERROR, messError);
        return;
      }
      const [file] = event.target.files;
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.imageSrc = reader.result as string;
        this.profileForm.patchValue({
          file,
        });
      };
    } else {
      this.imageSrc = this.imageBase;
      this.myFileVariable.nativeElement.value = STRING_EMPTY;
      this.profileForm.patchValue({
        file: null,
      });
    }
  }

  public onClickOpenSelectFile(): void {
    document.getElementById('file')?.click();
  }
}
