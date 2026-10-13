import { Component, Input, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from 'src/app/shared/notify.service';
import { notificationCommon } from 'src/app/helpers/common-helper';
import {
  ERROR,
  STRING_EMPTY,
  SUCCESS,
} from 'src/app/helpers/constant-helper';
import { MedicalExaminationService } from 'src/app/services/medical-examination.service';

@Component({
  selector: 'app-medical-examination-modal',
  templateUrl: './medical-examination-modal.component.html',
})
export class MedicalExaminationModalComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public formGroup!: UntypedFormGroup;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private medicalExaminationService: MedicalExaminationService,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      name: [STRING_EMPTY, [Validators.required]],
      price: [0],
      normalIndex: [STRING_EMPTY],
    });
    if (this.data) {
      this.isLoading = true;
      this.medicalExaminationService.getById(this.data.id).subscribe(
        (data) => {
          if (data.responseCode === 200) {
            const item = data.data;
            this.formGroup.patchValue(item);
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
    params.name = params.name || STRING_EMPTY;
    params.price = params.price || 0;
    params.normalIndex = params.normalIndex || STRING_EMPTY;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.medicalExaminationService.update(formData).subscribe(
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
      this.medicalExaminationService.insert(formData).subscribe(
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
