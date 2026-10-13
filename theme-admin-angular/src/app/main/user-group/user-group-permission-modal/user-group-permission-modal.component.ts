import { Component, Input, OnInit } from '@angular/core';
import {
  FormControl,
  UntypedFormBuilder,
  UntypedFormGroup,
} from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from 'src/app/shared/notify.service';
import { notificationCommon } from 'src/app/helpers/common-helper';
import { ERROR, STRING_EMPTY, SUCCESS } from 'src/app/helpers/constant-helper';
import { UserRoleService } from 'src/app/services/user-role.service';

@Component({
  selector: 'app-user-group-permission-modal',
  templateUrl: './user-group-permission-modal.component.html',
})
export class UserGroupPermissionModalComponent implements OnInit {
  public isLoading = false;
  public formGroup!: UntypedFormGroup;
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public listData: any[] = [];

  constructor(
    private userRoleService: UserRoleService,
    private formBuilder: UntypedFormBuilder,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal
  ) {}

  public ngOnInit(): void {
    this.formGroup = this.formBuilder.group({});
    if (this.data) {
      this.isLoading = true;
      this.userRoleService.getByUserGroupId(this.data.id).subscribe(
        (data) => {
          if (data.responseCode === 200) {
            const listObjectMenu = data.data.listMenu;
            for (let index1 = 0; index1 < listObjectMenu.length; index1++) {
              const element1 = listObjectMenu[index1];
              this.listData.push({
                id: element1.id,
                objectMenuName: element1.objectMenuName,
                levelShow: element1.levelShow,
                allowAccess: element1.allowAccess,
                allowAdd: element1.allowAdd,
                allowEdit: element1.allowEdit,
                allowDelete: element1.allowDelete,
                allowPrint: element1.allowPrint,
                allowExport: element1.allowExport,
                allowImport: element1.allowImport,
                isEdit: false,
              });
              this.formGroup.addControl(
                'id_allowAccess_'.concat(element1.id),
                new FormControl(element1.allowAccess)
              );
              this.formGroup.addControl(
                'id_allowAdd_'.concat(element1.id),
                new FormControl(element1.allowAdd)
              );
              this.formGroup.addControl(
                'id_allowEdit_'.concat(element1.id),
                new FormControl(element1.allowEdit)
              );
              this.formGroup.addControl(
                'id_allowDelete_'.concat(element1.id),
                new FormControl(element1.allowDelete)
              );
              this.formGroup.addControl(
                'id_allowPrint_'.concat(element1.id),
                new FormControl(element1.allowPrint)
              );
              this.formGroup.addControl(
                'id_allowExport_'.concat(element1.id),
                new FormControl(element1.allowExport)
              );
              this.formGroup.addControl(
                'id_allowImport_'.concat(element1.id),
                new FormControl(element1.allowImport)
              );
              for (
                let index2 = 0;
                index2 < element1.children.length;
                index2++
              ) {
                const element2 = element1.children[index2];
                this.listData.push({
                  id: element2.id,
                  objectMenuName: element2.objectMenuName,
                  levelShow: element2.levelShow,
                  allowAccess: element2.allowAccess,
                  allowAdd: element2.allowAdd,
                  allowEdit: element2.allowEdit,
                  allowDelete: element2.allowDelete,
                  allowPrint: element2.allowPrint,
                  allowExport: element2.allowExport,
                  allowImport: element2.allowImport,
                  isEdit: false,
                });
                this.formGroup.addControl(
                  'id_allowAccess_'.concat(element2.id),
                  new FormControl(element2.allowAccess)
                );
                this.formGroup.addControl(
                  'id_allowAdd_'.concat(element2.id),
                  new FormControl(element2.allowAdd)
                );
                this.formGroup.addControl(
                  'id_allowEdit_'.concat(element2.id),
                  new FormControl(element2.allowEdit)
                );
                this.formGroup.addControl(
                  'id_allowDelete_'.concat(element2.id),
                  new FormControl(element2.allowDelete)
                );
                this.formGroup.addControl(
                  'id_allowPrint_'.concat(element2.id),
                  new FormControl(element2.allowPrint)
                );
                this.formGroup.addControl(
                  'id_allowExport_'.concat(element2.id),
                  new FormControl(element2.allowExport)
                );
                this.formGroup.addControl(
                  'id_allowImport_'.concat(element2.id),
                  new FormControl(element2.allowImport)
                );
              }
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
    }
  }

  public destroyModal(reLoad: any): void {
    this.activeModal.close({ reLoad });
  }

  public onChangeAllow(id: any, type: string): void {
    const value = this.formGroup.controls['id_'.concat(type, '_', id)].value;
    const objIndex = this.listData.findIndex((x) => x.id === id);
    switch (type) {
      case 'allowAccess':
        this.listData[objIndex].allowAccess = value;
        break;
      case 'allowAdd':
        this.listData[objIndex].allowAdd = value;
        break;
      case 'allowEdit':
        this.listData[objIndex].allowEdit = value;
        break;
      case 'allowDelete':
        this.listData[objIndex].allowDelete = value;
        break;
      case 'allowPrint':
        this.listData[objIndex].allowPrint = value;
        break;
      case 'allowExport':
        this.listData[objIndex].allowExport = value;
        break;
      case 'allowImport':
        this.listData[objIndex].allowImport = value;
        break;
      default:
        break;
    }
    this.listData[objIndex].isEdit = true;
    this.listData = [...this.listData];
  }

  public submitForm(): void {
    this.isLoading = true;
    const params = this.formGroup.value;
    params.listPermission = JSON.stringify(
      this.listData.filter((x) => x.isEdit)
    );
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    this.userRoleService.update(formData).subscribe(
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
