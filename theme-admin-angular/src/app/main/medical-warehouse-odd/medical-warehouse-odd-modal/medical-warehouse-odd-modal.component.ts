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
  STRING_EMPTY,
  SUCCESS,
} from 'src/app/helpers/constant-helper';
import { MedicineWarehouseService } from 'src/app/services/medicine-warehouse.service';
import { SupplierService } from 'src/app/services/supplier.service';
import { UnitMeasureService } from 'src/app/services/unit-measure.service';

@Component({
  selector: 'app-medical-warehouse-odd-modal',
  templateUrl: './medical-warehouse-odd-modal.component.html',
})
export class MedicalWarehouseOddModalComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public formGroup!: UntypedFormGroup;
  public isLoadingSupplier = false;
  public listSupplier: any[] = [];
  public isLoadingUnitMeasure = false;
  public listUnitMeasure: any[] = [];
  public dateFormat = DATE_FORMAT;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private medicineWarehouseService: MedicineWarehouseService,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal,
    private supplierService: SupplierService,
    private unitMeasureService: UnitMeasureService
  ) {}

  public ngOnInit(): void {
    this.getAllDataSupplier();
    this.getAllDataUnitMeasure();
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      name: [STRING_EMPTY, [Validators.required]],
      quantity: [0],
      supplierId: [STRING_EMPTY, [Validators.required]],
      unitMeasureId: [STRING_EMPTY],
      priceInput: [0, [Validators.required]],
      dateInput: [STRING_EMPTY, [Validators.required]],
      price: [0, [Validators.required]],
      expiry: [STRING_EMPTY, [Validators.required]],
      priceTotal: [0, Validators.required],
      note: [STRING_EMPTY],
      isMain: [false],
    });
    if (this.data) {
      this.isLoading = true;
      this.medicineWarehouseService.getById(this.data.id).subscribe(
        (data) => {
          if (data.responseCode === 200) {
            const item = data.data;
            item.dateInput = item.dateInput
              ? new Date(item.dateInput)
              : STRING_EMPTY;
            item.expiry = item.expiry ? new Date(item.expiry) : STRING_EMPTY;
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
    params.quantity = params.quantity || 0;
    params.supplierId = params.supplierId || STRING_EMPTY;
    params.unitMeasureId = params.unitMeasureId || STRING_EMPTY;
    params.priceInput = params.priceInput || 0;
    params.dateInput = params.dateInput
      ? moment(params.dateInput).format(DATE_FORMAT_yyyy_MM_DD)
      : STRING_EMPTY;
    params.price = params.price || 0;
    params.expiry = params.expiry
      ? moment(params.expiry).format(DATE_FORMAT_yyyy_MM_DD)
      : STRING_EMPTY;
    params.priceTotal = params.priceTotal || 0;
    params.note = params.note || STRING_EMPTY;
    params.isMain = false;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.medicineWarehouseService.update(formData).subscribe(
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
      this.medicineWarehouseService.insert(formData).subscribe(
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

  private getAllDataSupplier(): void {
    this.isLoadingSupplier = true;
    this.supplierService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listSupplier = data.listData;
        }
      },
      () => {
        this.isLoadingSupplier = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingSupplier = false;
      }
    );
  }

  private getAllDataUnitMeasure(): void {
    this.isLoadingUnitMeasure = true;
    this.unitMeasureService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listUnitMeasure = data.listData;
        }
      },
      () => {
        this.isLoadingUnitMeasure = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingUnitMeasure = false;
      }
    );
  }

  public onChangePriceInput(value: any): void {
    const quantity = this.formGroup.controls['quantity'].value;
    let val = 0;
    if (quantity) {
      val = value * quantity;
    }
    this.formGroup.controls['priceTotal'].setValue(val);
  }

  public onChangeQuantity(value: any): void {
    const priceInput = this.formGroup.controls['priceInput'].value;
    let val = 0;
    if (priceInput) {
      val = value * priceInput;
    }
    this.formGroup.controls['priceTotal'].setValue(val);
  }
}
