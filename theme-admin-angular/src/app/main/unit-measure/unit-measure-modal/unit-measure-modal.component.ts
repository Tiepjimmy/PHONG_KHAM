import { Component, Input, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { STRING_EMPTY } from 'src/app/helpers/constant-helper';
import { UnitMeasureService } from 'src/app/services/unit-measure.service';
import { NotifyService } from 'src/app/shared/notify.service';

@Component({
  selector: 'app-unit-measure-modal',
  templateUrl: './unit-measure-modal.component.html',
})
export class UnitMeasureModalComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public formGroup!: UntypedFormGroup;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private unitMeasureService: UnitMeasureService,
    private notify: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      name: [STRING_EMPTY, [Validators.required]],
    });
    if (this.data) {
      this.isLoading = true;
      this.unitMeasureService.getById(this.data.id).subscribe(
        (data) => {
          if (data.responseCode === 200) {
            this.formGroup.patchValue(data.data);
          }
        },
        () => {
          this.isLoading = false;
          this.notify.error();
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

  public destroyModal(reLoad: boolean): void {
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
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key] ?? STRING_EMPTY));
    const request =
      params.id && params.id !== STRING_EMPTY
        ? this.unitMeasureService.update(formData)
        : this.unitMeasureService.insert(formData);
    request.subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.notify.success(data.responseMessage);
          this.destroyModal(true);
        } else {
          this.notify.error(data.responseMessage);
        }
      },
      () => {
        this.isLoading = false;
        this.notify.error();
      },
      () => {
        this.isLoading = false;
      }
    );
  }
}
