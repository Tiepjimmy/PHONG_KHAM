import { Component, Input, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import moment from 'moment';
import { NotifyService } from 'src/app/shared/notify.service';
import { notificationCommon } from 'src/app/helpers/common-helper';
import {
  BASE_IMAGE,
  DATE_FORMAT,
  DATE_FORMAT_yyyy_MM_DD,
  ERROR,
  STRING_EMPTY,
  SUCCESS,
} from 'src/app/helpers/constant-helper';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-person-modal',
  templateUrl: './person-modal.component.html',
})
export class PersonModalComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public formGroup!: UntypedFormGroup;
  public dateFormat = DATE_FORMAT;
  private imageBase = BASE_IMAGE;
  public imageSrc = this.imageBase;
  public isShowImg = false;
  public listSex = [
    { id: 'Nam', name: 'Nam' },
    { id: 'Nữ', name: 'Nữ' },
    { id: 'Khác', name: 'Khác' },
  ];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private personService: PersonService,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      fullName: [STRING_EMPTY, [Validators.required]],
      phoneNumber: [STRING_EMPTY],
      dateOfBirth: [STRING_EMPTY],
      sex: [STRING_EMPTY],
      address: [STRING_EMPTY],
    });
    if (this.data) {
      this.isLoading = true;
      this.personService.getById(this.data.id).subscribe(
        (data) => {
          if (data.responseCode === 200) {
            const item = data.data;
            item.dateOfBirth = item.dateOfBirth
              ? new Date(item.dateOfBirth)
              : STRING_EMPTY;
            this.formGroup.patchValue(item);
            this.imageSrc =
              item.avatar !== STRING_EMPTY ? item.avatar : this.imageBase;
            this.isShowImg = true;
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

  public isInvalid(name: string): boolean {
    const control = this.formGroup.controls[name];
    return control.invalid && (this.submitted || control.dirty || control.touched);
  }

  public destroyModal(reLoad: any): void {
    this.activeModal.close({ reLoad });
  }

  public submitForm(): void {
    this.submitted = true;
    if (!this.formGroup.valid) {
      return;
    }
    this.isLoading = true;
    const params = this.formGroup.value;
    params.fullName = params.fullName || STRING_EMPTY;
    params.phoneNumber = params.phoneNumber || STRING_EMPTY;
    params.dateOfBirth = params.dateOfBirth
      ? moment(params.dateOfBirth).format(DATE_FORMAT_yyyy_MM_DD)
      : STRING_EMPTY;
    params.sex = params.sex || STRING_EMPTY;
    params.address = params.address || STRING_EMPTY;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.personService.update(formData).subscribe(
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
      this.personService.insert(formData).subscribe(
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
}
