import { Component, Input, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from 'src/app/shared/notify.service';
import { notificationCommon } from 'src/app/helpers/common-helper';
import { ERROR, STRING_EMPTY, SUCCESS } from 'src/app/helpers/constant-helper';
import { AdministrativeUnitService } from 'src/app/services/administrative-unit.service';

@Component({
  selector: 'app-administrative-unit-modal',
  templateUrl: './administrative-unit-modal.component.html',
})
export class AdministrativeUnitModalComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public formGroup!: UntypedFormGroup;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private administrativeUnitService: AdministrativeUnitService,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      codeNameVN: [STRING_EMPTY, [Validators.required]],
      codeNameEN: [STRING_EMPTY],
      fullNameVN: [STRING_EMPTY],
      fullNameEN: [STRING_EMPTY],
      shortNameVN: [STRING_EMPTY],
      shortNameEN: [STRING_EMPTY],
      isDefault: [false],
    });
    if (this.data) {
      this.isLoading = true;
      this.administrativeUnitService.getById(this.data.id).subscribe(
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
    params.codeNameVN = params.codeNameVN || STRING_EMPTY;
    params.codeNameEN = params.codeNameEN || STRING_EMPTY;
    params.fullNameVN = params.fullNameVN || STRING_EMPTY;
    params.fullNameEN = params.fullNameEN || STRING_EMPTY;
    params.shortNameVN = params.shortNameVN || STRING_EMPTY;
    params.shortNameEN = params.shortNameEN || STRING_EMPTY;
    params.isDefault = params.isDefault || false;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.administrativeUnitService.update(formData).subscribe(
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
      this.administrativeUnitService.insert(formData).subscribe(
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
