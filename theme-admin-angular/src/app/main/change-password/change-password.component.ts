import { Component, Input, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import Swal from 'sweetalert2';
import { NotifyService } from 'src/app/shared/notify.service';
import { notificationCommon } from 'src/app/helpers/common-helper';
import {
  AUTHEN,
  ERROR,
  NOTIFICATION_TITLE,
  STRING_EMPTY,
  SUCCESS,
} from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
})
export class ChangePasswordComponent implements OnInit {
  @Input() public title = STRING_EMPTY;
  @Input() public data: any;
  public isLoading = false;
  public submitted = false;
  public changePassWordForm!: UntypedFormGroup;

  constructor(
    private authenticationService: AuthenticationService,
    private formBuilder: UntypedFormBuilder,
    private notifyService: NotifyService,
    private activeModal: NgbActiveModal,
    private userService: UserService,
    private router: Router,
  ) {}

  public ngOnInit(): void {
    this.changePassWordForm = this.formBuilder.group({
      id: [this.authenticationService.userValue?.id],
      passWordOld: [STRING_EMPTY, [Validators.required]],
      passWordNew: [STRING_EMPTY, [Validators.required]],
      passWordEnter: [STRING_EMPTY, [Validators.required]],
    });
  }

  public isInvalid(name: string): boolean {
    const control = this.changePassWordForm.controls[name];
    return control.invalid && (this.submitted || control.dirty || control.touched);
  }

  public destroyModal(reLoad: any): void {
    this.activeModal.close({ reLoad });
  }

  public changePassWord(): void {
    this.submitted = true;
    if (!this.changePassWordForm.valid) {
      return;
    }
    const params = this.changePassWordForm.value;
    let message = STRING_EMPTY;
    if (params.passWordOld === params.passWordNew) {
      message += 'Mật khẩu mới không được trùng với mật khẩu hiện tại <br>';
    }
    if (params.passWordNew !== params.passWordEnter) {
      message += 'Mật khẩu mới chưa khớp với mật khẩu nhập lại <br>';
    }
    if (params.passWordNew.length < 9) {
      message += 'Mật khẩu mới từ 8 ký tự trở lên <br>';
    }
    if (message != STRING_EMPTY) {
      notificationCommon(this.notifyService, ERROR, message);
      return;
    }
    this.isLoading = true;
    const formData: FormData = new FormData();
    Object.keys(params).forEach((key) => formData.append(key, params[key]));
    this.userService.changePassword(formData).subscribe(
      (data) => {
        if (data.responseCode === 200) {
          notificationCommon(
            this.notifyService,
            SUCCESS,
            data.responseMessage
          );
          this.changePassWordForm.reset();
          this.submitted = false;
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
        // Blocking warning alert (was a modal warning dialog), OK logs the user out
        Swal.fire({
          title: NOTIFICATION_TITLE,
          text: 'Thông tin mật khẩu của bạn đã thay đổi. Vui lòng chọn OK để đăng nhập lại',
          icon: 'warning',
          confirmButtonText: 'OK',
          confirmButtonColor: 'var(--primary)',
          allowOutsideClick: false,
          allowEscapeKey: false,
        }).then(() => {
          localStorage.removeItem(AUTHEN);
          this.activeModal.close({ reLoad: false });
          this.router.navigate(['/dang-nhap']);
        });
      }
    );
  }
}
