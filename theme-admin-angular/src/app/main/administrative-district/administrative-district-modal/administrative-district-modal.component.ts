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
import { AdministrativeDistrictService } from 'src/app/services/administrative-district.service';
import { AdministrativeProvinceService } from 'src/app/services/administrative-province.service';
import { AdministrativeUnitService } from 'src/app/services/administrative-unit.service';

@Component({
  selector: 'app-administrative-district-modal',
  templateUrl: './administrative-district-modal.component.html',
})
export class AdministrativeDistrictModalComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public formGroup!: UntypedFormGroup;
  public isLoadingAdministrativeUnit = false;
  public listAdministrativeUnit: any[] = [];
  public isLoadingAdministrativeProvince = false;
  public listAdministrativeProvince: any[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private administrativeDistrictService: AdministrativeDistrictService,
    private administrativeUnitService: AdministrativeUnitService,
    private administrativeProvinceService: AdministrativeProvinceService,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.getAllDataAdministrativeUnit();
    this.getAllDataAdministrativeProvince();
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      code: [STRING_EMPTY, [Validators.required]],
      codeName: [STRING_EMPTY],
      nameVN: [STRING_EMPTY],
      nameEN: [STRING_EMPTY],
      fullNameVN: [STRING_EMPTY],
      fullNameEN: [STRING_EMPTY],
      administrativeUnitId: [STRING_EMPTY],
      administrativeProvinceId: [STRING_EMPTY],
      isDefault: [false],
    });
    if (this.data) {
      this.isLoading = true;
      this.administrativeDistrictService.getById(this.data.id).subscribe(
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
    params.administrativeProvinceId =
      params.administrativeProvinceId || STRING_EMPTY;
    params.isDefault = params.isDefault || false;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.administrativeDistrictService.update(formData).subscribe(
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
      this.administrativeDistrictService.insert(formData).subscribe(
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

  private getAllDataAdministrativeProvince(): void {
    this.isLoadingAdministrativeProvince = true;
    this.administrativeProvinceService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listAdministrativeProvince = data.listData;
        }
      },
      () => {
        this.isLoadingAdministrativeProvince = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingAdministrativeProvince = false;
      }
    );
  }
}
