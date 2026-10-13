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
import { AdministrativeWardService } from 'src/app/services/administrative-ward.service';
import { AdministrativeDistrictService } from 'src/app/services/administrative-district.service';
import { AdministrativeUnitService } from 'src/app/services/administrative-unit.service';

@Component({
  selector: 'app-administrative-ward-modal',
  templateUrl: './administrative-ward-modal.component.html',
})
export class AdministrativeWardModalComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public formGroup!: UntypedFormGroup;
  public isLoadingAdministrativeUnit = false;
  public listAdministrativeUnit: any[] = [];
  public isLoadingAdministrativeDistrict = false;
  public listAdministrativeDistrict: any[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private administrativeWardService: AdministrativeWardService,
    private administrativeUnitService: AdministrativeUnitService,
    private administrativeDistrictService: AdministrativeDistrictService,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.getAllDataAdministrativeUnit();
    this.getAllDataAdministrativeDistrict();
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      code: [STRING_EMPTY, [Validators.required]],
      codeName: [STRING_EMPTY],
      nameVN: [STRING_EMPTY],
      nameEN: [STRING_EMPTY],
      fullNameVN: [STRING_EMPTY],
      fullNameEN: [STRING_EMPTY],
      administrativeUnitId: [STRING_EMPTY],
      administrativeDistrictId: [STRING_EMPTY],
      isDefault: [false],
    });
    if (this.data) {
      this.isLoading = true;
      this.administrativeWardService.getById(this.data.id).subscribe(
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
    params.codeVN = params.codeVN || STRING_EMPTY;
    params.codeEN = params.codeEN || STRING_EMPTY;
    params.nameVN = params.nameVN || STRING_EMPTY;
    params.nameEN = params.nameEN || STRING_EMPTY;
    params.fullNameVN = params.fullNameVN || STRING_EMPTY;
    params.fullNameEN = params.fullNameEN || STRING_EMPTY;
    params.administrativeUnitId = params.administrativeUnitId || STRING_EMPTY;
    params.administrativeDistrictId =
      params.administrativeDistrictId || STRING_EMPTY;
    params.isDefault = params.isDefault || false;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.administrativeWardService.update(formData).subscribe(
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
      this.administrativeWardService.insert(formData).subscribe(
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

  private getAllDataAdministrativeUnit(): void {
    this.isLoadingAdministrativeUnit = true;
    this.administrativeUnitService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listAdministrativeUnit = data.listData;
        }
      },
      () => {
        this.isLoadingAdministrativeUnit = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingAdministrativeUnit = false;
      }
    );
  }

  private getAllDataAdministrativeDistrict(): void {
    this.isLoadingAdministrativeDistrict = true;
    this.administrativeDistrictService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listAdministrativeDistrict = data.listData;
        }
      },
      () => {
        this.isLoadingAdministrativeDistrict = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingAdministrativeDistrict = false;
      }
    );
  }
}
