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
  PROMOTE,
  STRING_EMPTY,
  SUCCESS,
} from 'src/app/helpers/constant-helper';
import { ObjectMenuService } from 'src/app/services/object-menu.service';

@Component({
  selector: 'app-object-menu-modal',
  templateUrl: './object-menu-modal.component.html',
})
export class ObjectMenuModalComponent implements OnInit {
  public isLoading = false;
  public isLoadingParentId = false;
  public formGroup!: UntypedFormGroup;
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public submitted = false;
  public listParent: any[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private objectMenuService: ObjectMenuService,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.getAllData();
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      name: [STRING_EMPTY, [Validators.required]],
      shortName: [STRING_EMPTY],
      parentId: [STRING_EMPTY],
      metaLink: [STRING_EMPTY, [Validators.required]],
      iconName: [STRING_EMPTY],
      levelShow: [1, [Validators.required]],
      orderShow: [1, [Validators.required]],
      dataTarget: [STRING_EMPTY],
      ariaControls: [STRING_EMPTY],
      isActive: [false],
      isDefault: [false],
    });
    if (this.data) {
      this.isLoading = true;
      this.objectMenuService.getById(this.data.id).subscribe(
        (data) => {
          if (data.responseCode === 200) {
            const item = data.data;
            if (item.dataTarget) {
              item.dataTarget = item.dataTarget.replace(PROMOTE, STRING_EMPTY);
            }
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

  private getAllData(): void {
    this.isLoadingParentId = true;
    this.objectMenuService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listParent = data.listData;
        }
      },
      () => {
        this.isLoadingParentId = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingParentId = false;
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
    params.name = params.name || STRING_EMPTY;
    params.shortName = params.shortName || STRING_EMPTY;
    params.parentId = params.parentId || STRING_EMPTY;
    params.metaLink = params.metaLink || STRING_EMPTY;
    params.iconName = params.iconName || STRING_EMPTY;
    params.levelShow = params.levelShow || STRING_EMPTY;
    params.orderShow = params.orderShow || STRING_EMPTY;
    params.dataTarget =
      params.dataTarget !== null && params.dataTarget.length > 0
        ? PROMOTE.concat(params.dataTarget)
        : STRING_EMPTY || STRING_EMPTY;
    params.ariaControls = params.ariaControls || STRING_EMPTY;
    params.isActive = params.isActive || false;
    params.isDefault = params.isDefault || false;
    if (params.levelShow > 2) {
      notificationCommon(
        this.notifyService,
        ERROR,
        'Hệ thống chỉ hỗ trợ tối đa 2 cấp hiển thị'
      );
      this.isLoading = false;
      return;
    }
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.objectMenuService.update(formData).subscribe(
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
      this.objectMenuService.insert(formData).subscribe(
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

  public ngModelChangeParentId(value: any): void {
    const levelShow = this.listParent.find((x) => x.id === value)?.levelShow;
    if (levelShow) {
      if (levelShow >= 2) {
        this.formGroup.controls['levelShow'].setValue(1);
        this.formGroup.controls['parentId'].setValue(STRING_EMPTY);
        notificationCommon(
          this.notifyService,
          ERROR,
          'Hệ thống chỉ đáp ứng 2 cấp menu. Vui lòng chọn danh mục cha khác!'
        );
      } else {
        this.formGroup.controls['levelShow'].setValue(levelShow + 1);
      }
    }
  }

  public onChangeDataTarget(value: any): void {
    if (value) {
      this.formGroup.controls['ariaControls'].setValue(value);
    } else {
      this.formGroup.controls['ariaControls'].setValue(STRING_EMPTY);
    }
  }
}
