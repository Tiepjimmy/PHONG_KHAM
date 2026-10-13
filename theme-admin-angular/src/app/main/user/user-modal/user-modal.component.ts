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
  COMMA,
  ERROR,
  STRING_EMPTY,
  SUCCESS,
} from 'src/app/helpers/constant-helper';
import { PersonService } from 'src/app/services/person.service';
import { UserGroupService } from 'src/app/services/user-group.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-modal',
  templateUrl: './user-modal.component.html',
})
export class UserModalComponent implements OnInit {
  public isLoading = false;
  public isLoadingUserGroupId = false;
  public formGroup!: UntypedFormGroup;
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public submitted = false;
  public listUserGroup: any[] = [];
  public isReadonly = false;
  public isLoadingPerson = false;
  public listPerson: any[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private userGroupService: UserGroupService,
    private userService: UserService,
    private personService: PersonService,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.getAllDataUserGroup();
    this.formGroup = this.formBuilder.group({
      id: [STRING_EMPTY],
      userName: [STRING_EMPTY, [Validators.required]],
      isActive: [true],
      isAdmin: [true],
      listUserGroupId: [[], [Validators.required]],
      personId: [STRING_EMPTY],
      isDefault: [false],
    });
    if (this.data) {
      this.isLoading = true;
      this.userService.getById(this.data.id).subscribe(
        (data) => {
          if (data.responseCode === 200) {
            const item = data.data;
            item.listUserGroupId = item.listUserGroupId
              ? item.listUserGroupId.split(COMMA)
              : item.listUserGroupId;
            this.personSearch(item.fullName, true);
            this.formGroup.patchValue(item);
            this.isReadonly = true;
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

  private getAllDataUserGroup(): void {
    this.isLoadingUserGroupId = true;
    this.userGroupService.getAllData().subscribe(
      (data) => {
        if (data.responseCode === 200) {
          this.listUserGroup = data.listData;
        }
      },
      () => {
        this.isLoadingUserGroupId = false;
        notificationCommon(this.notifyService, ERROR);
      },
      () => {
        this.isLoadingUserGroupId = false;
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
    params.userName = params.userName || STRING_EMPTY;
    params.isActive = params.isActive || false;
    params.isAdmin = params.isAdmin || false;
    params.listUserGroupId = params.listUserGroupId || STRING_EMPTY;
    params.isDefault = params.isDefault || false;
    params.personId = params.personId || STRING_EMPTY;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    if (params.id !== STRING_EMPTY) {
      this.userService.update(formData).subscribe(
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
      this.userService.insert(formData).subscribe(
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

  public personSearch(value: string, isEdit: boolean): void {
    if (value && value.length > 2) {
      this.isLoadingPerson = true;
      this.personService.getAllDataByKey(value, isEdit).subscribe(
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
    } else {
      this.listPerson = [];
    }
  }
}
