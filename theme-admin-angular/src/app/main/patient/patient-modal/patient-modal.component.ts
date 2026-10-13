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
  DATE_FORMAT,
  DATE_FORMAT_yyyy_MM_DD,
  ERROR,
  ERROR_GUIDE_PATTERN,
  STRING_EMPTY,
  SUCCESS,
} from 'src/app/helpers/constant-helper';
import { AdministrativeDistrictService } from 'src/app/services/administrative-district.service';
import { AdministrativeProvinceService } from 'src/app/services/administrative-province.service';
import { AdministrativeWardService } from 'src/app/services/administrative-ward.service';
import { MedicalExaminationService } from 'src/app/services/medical-examination.service';
import { PatientService } from 'src/app/services/patient.service';
import { PersonService } from 'src/app/services/person.service';

@Component({
  selector: 'app-patient-modal',
  templateUrl: './patient-modal.component.html',
})
export class PatientModalComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public submittedDetail = false;
  public formGroup!: UntypedFormGroup;
  public formGroupDetail!: UntypedFormGroup;
  public dateFormat = DATE_FORMAT;
  public isLoadingAdministrativeProvince = false;
  public isLoadingAdministrativeDistrict = false;
  public isLoadingAdministrativeWard = false;
  public listAdministrativeProvince: any[] = [];
  public listAdministrativeDistrict: any[] = [];
  public listAdministrativeWard: any[] = [];
  public listPatientDetail: any[] = [];
  public isEdit = false;
  public listMedicalExamination: any[] = [];
  public isLoadingMedicalExamination = false;
  public listPerson: any[] = [];
  public isLoadingPerson = false;
  public totalPricePatient = 0;
  public listSex = [
    { id: 'Nam', name: 'Nam' },
    { id: 'Nữ', name: 'Nữ' },
    { id: 'Khác', name: 'Khác' },
  ];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private patientService: PatientService,
    private administrativeProvinceService: AdministrativeProvinceService,
    private administrativeDistrictService: AdministrativeDistrictService,
    private administrativeWardService: AdministrativeWardService,
    private medicalExaminationService: MedicalExaminationService,
    private notifyService: NotifyService,
    private personService: PersonService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.getAllDataAdministrativeProvince();
    this.getAllDataMedicalExamination();
    this.getAllDataPerson();
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      couponCode: [STRING_EMPTY],
      fullName: [STRING_EMPTY, [Validators.required]],
      dateOfBirth: [STRING_EMPTY, [Validators.required]],
      sex: [STRING_EMPTY],
      phoneNumber: [STRING_EMPTY, [Validators.required]],
      administrativeProvinceId: [STRING_EMPTY],
      administrativeDistrictId: [STRING_EMPTY],
      administrativeWardId: [STRING_EMPTY],
      address: [STRING_EMPTY],
      diagnosis: [STRING_EMPTY],
      result: [STRING_EMPTY],
      visit: [1],
      totalPrice: [0],
      listPatientDetail: [STRING_EMPTY],
    });
    this.formGroupDetail = this.formBuilder.group({
      id: [STRING_EMPTY],
      medicalExaminationId: [STRING_EMPTY, [Validators.required]],
      quantity: [1],
      personId: [STRING_EMPTY, [Validators.required]],
      result: [STRING_EMPTY],
    });
    if (this.data) {
      this.isLoading = true;
      this.patientService.getById(this.data.id).subscribe(
        (data) => {
          if (data.responseCode === 200) {
            const item = data.data;
            item.dateOfBirth = item.dateOfBirth
              ? new Date(item.dateOfBirth)
              : STRING_EMPTY;
            this.listPatientDetail = item.listPatientDetail;
            this.totalPricePatient = item.totalPrice;
            this.formGroup.patchValue(item);
            // Load the district/ward lists of the saved address so the selects can show it
            if (item.administrativeProvinceId) {
              this.getAllDataAdministrativeDistrict(item.administrativeProvinceId);
            }
            if (item.administrativeDistrictId) {
              this.getAllDataAdministrativeWard(item.administrativeDistrictId);
            }
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
      this.generateCouponCode();
    }
  }

  public isInvalid(name: string): boolean {
    const control = this.formGroup.controls[name];
    return control.invalid && (this.submitted || control.dirty || control.touched);
  }

  public isInvalidDetail(name: string): boolean {
    const control = this.formGroupDetail.controls[name];
    return control.invalid && (this.submittedDetail || control.dirty);
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
    params.couponCode = params.couponCode || STRING_EMPTY;
    params.fullName = params.fullName || STRING_EMPTY;
    params.dateOfBirth = params.dateOfBirth
      ? moment(params.dateOfBirth).format(DATE_FORMAT_yyyy_MM_DD)
      : STRING_EMPTY;
    params.sex = params.sex || STRING_EMPTY;
    params.phoneNumber = params.phoneNumber || STRING_EMPTY;
    params.administrativeProvinceId =
      params.administrativeProvinceId || STRING_EMPTY;
    params.administrativeDistrictId =
      params.administrativeDistrictId || STRING_EMPTY;
    params.administrativeWardId = params.administrativeWardId || STRING_EMPTY;
    params.address = params.address || STRING_EMPTY;
    params.diagnosis = params.diagnosis || STRING_EMPTY;
    params.result = params.result || STRING_EMPTY;
    params.visit = params.visit || STRING_EMPTY;
    params.totalPrice = this.totalPricePatient || 0;
    const listPatientDetailRequest = this.listPatientDetail.map((x) => {
      return {
        id: ERROR_GUIDE_PATTERN.test(x.id) ? x.id : null,
        patientId: x.patientId,
        medicalExaminationId: x.medicalExaminationId,
        personId: x.personId,
        price: x.price,
        quantity: x.quantity,
        totalPrice: x.totalPrice,
        result: x.result,
      };
    });
    params.listPatientDetail =
      JSON.stringify(listPatientDetailRequest) || STRING_EMPTY;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.patientService.update(formData).subscribe(
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
      this.patientService.insert(formData).subscribe(
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

  public ngModelChangeAdministrativeProvince(value: any): void {
    this.formGroup.controls['administrativeDistrictId'].setValue(STRING_EMPTY);
    this.formGroup.controls['administrativeWardId'].setValue(STRING_EMPTY);
    if (value) {
      this.getAllDataAdministrativeDistrict(value);
    } else {
      this.listAdministrativeDistrict = [];
    }
  }

  private getAllDataAdministrativeDistrict(
    administrativeProvinceId: any
  ): void {
    this.isLoadingAdministrativeDistrict = true;
    this.administrativeDistrictService
      .getAllDataByAdministrativeProvinceId(administrativeProvinceId)
      .subscribe(
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

  public ngModelChangeAdministrativeDistrict(value: any): void {
    this.formGroup.controls['administrativeWardId'].setValue(STRING_EMPTY);
    if (value) {
      this.getAllDataAdministrativeWard(value);
    } else {
      this.listAdministrativeWard = [];
    }
  }

  private getAllDataAdministrativeWard(administrativeDistrictId: any): void {
    this.isLoadingAdministrativeWard = true;
    this.administrativeWardService
      .getAllDataByAdministrativeDistrictId(administrativeDistrictId)
      .subscribe(
        (data) => {
          if (data.responseCode === 200) {
            this.listAdministrativeWard = data.listData;
          }
        },
        () => {
          this.isLoadingAdministrativeWard = false;
          notificationCommon(this.notifyService, ERROR);
        },
        () => {
          this.isLoadingAdministrativeWard = false;
        }
      );
  }

  private generateCouponCode(): void {
    this.patientService.generateCouponCode().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.formGroup.controls['couponCode'].setValue(data.data);
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

  public openReExamination(): void {
    this.data = null;
    this.formGroup.controls['id'].setValue(STRING_EMPTY);
    this.formGroup.controls['visit'].setValue(
      this.formGroup.controls['visit'].value + 1
    );
    this.formGroup.controls['result'].setValue(STRING_EMPTY);
    this.listPatientDetail = [];
    this.totalPricePatient = 0;
    this.generateCouponCode();
  }

  public onAddRow(): void {
    this.submittedDetail = true;
    if (!this.formGroupDetail.valid) {
      return;
    }
    const params = this.formGroupDetail.value;
    const medicalExamination = this.listMedicalExamination.find(
      (x) => x.id === params.medicalExaminationId
    );
    const person = this.listPerson.find((x) => x.id === params.personId);
    const item = {
      id: this.listPatientDetail.length + 1,
      medicalExaminationId: params.medicalExaminationId,
      medicalExaminationName: medicalExamination.name,
      personId: params.personId,
      personName: person.fullName,
      price: medicalExamination.price,
      quantity: params.quantity,
      totalPrice: medicalExamination.price * params.quantity,
      result: params.result,
    };
    this.listPatientDetail.push(item);
    this.caculateTotalPrice();
    this.formGroupDetail.reset();
    this.formGroupDetail.controls['quantity'].setValue(1);
    this.submittedDetail = false;
  }

  public onUpdateRow(): void {
    const params = this.formGroupDetail.value;
    const objIndex = this.listPatientDetail.findIndex(
      (x) => x.id === params.id
    );
    const medicalExamination = this.listMedicalExamination.find(
      (x) => x.id === params.medicalExaminationId
    );
    const person = this.listPerson.find((x) => x.id === params.personId);
    this.listPatientDetail[objIndex].medicalExaminationId =
      params.medicalExaminationId;
    this.listPatientDetail[objIndex].medicalExaminationName =
      medicalExamination.name;
    this.listPatientDetail[objIndex].personId = params.personId;
    this.listPatientDetail[objIndex].personName = person.fullName;
    this.listPatientDetail[objIndex].price = medicalExamination.price;
    this.listPatientDetail[objIndex].quantity = params.quantity;
    this.listPatientDetail[objIndex].totalPrice =
      medicalExamination.price * params.quantity;
    this.listPatientDetail[objIndex].result = params.result;
    this.listPatientDetail = [...this.listPatientDetail];
    this.caculateTotalPrice();
    this.isEdit = false;
    this.formGroupDetail.reset();
    this.formGroupDetail.controls['quantity'].setValue(1);
    this.submittedDetail = false;
  }

  public onCancelRow(): void {
    this.isEdit = false;
    this.formGroupDetail.reset();
    this.formGroupDetail.controls['quantity'].setValue(1);
    this.submittedDetail = false;
  }

  public onEditRow(value: any): void {
    this.isEdit = true;
    const data = this.listPatientDetail.find((x) => x.id === value);
    this.formGroupDetail.patchValue(data);
  }

  public async onDeleteRow(value: any): Promise<void> {
    if (!(await this.notifyService.confirmDelete('Bạn có chắc chắn muốn xóa?'))) {
      return;
    }
    this.listPatientDetail = this.listPatientDetail.filter(
      (x) => x.id !== value
    );
    this.caculateTotalPrice();
  }

  private getAllDataMedicalExamination(): void {
    this.isLoadingMedicalExamination = true;
    this.medicalExaminationService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listMedicalExamination = data.listData;
        }
      },
      () => {
        this.isLoadingMedicalExamination = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingMedicalExamination = false;
      }
    );
  }

  public getAllDataPerson(): void {
    this.isLoadingPerson = true;
    this.personService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listPerson = data.listData;
        }
      },
      () => {
        this.isLoadingPerson = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingPerson = false;
      }
    );
  }

  private caculateTotalPrice(): void {
    this.totalPricePatient = this.listPatientDetail.reduce(
      (acc, item) => acc + Number(item.totalPrice),
      0
    );
  }
}
