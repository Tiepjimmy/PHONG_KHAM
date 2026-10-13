import { Component, OnInit, Renderer2 } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NotifyService } from 'src/app/shared/notify.service';
import { first } from 'rxjs';
import {
  checkClassActive,
  notificationCommon,
  randomCode,
} from '../helpers/common-helper';
import {
  DOMAIN_ROUTER,
  ERROR,
  LOGO_LOGIN_NAME,
  PROJECT_NAME,
  SOURCE,
  STRING_EMPTY,
} from '../helpers/constant-helper';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
})
export class SignInComponent implements OnInit {
  public isLoading = false;
  public signinForm!: UntypedFormGroup;
  private returnUrl!: string;
  private captch: string | undefined;
  public projectName = PROJECT_NAME;
  public logoName = LOGO_LOGIN_NAME;
  public showPassword = false;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private notifyService: NotifyService,
    private renderer: Renderer2
  ) {
    if (this.authenticationService.userValue) {
      this.router.navigate([SOURCE]);
    }
  }

  public ngOnInit(): void {
    const listMenu = this.authenticationService.userValue?.listMenu;
    checkClassActive(this.router, this.renderer, listMenu);
    this.captch = randomCode(4);
    this.signinForm = this.formBuilder.group({
      userName: [STRING_EMPTY, [Validators.required]],
      passWord: [STRING_EMPTY, [Validators.required]],
      captch: [STRING_EMPTY, [Validators.required]],
      code: [{ value: this.captch, disabled: true }],
    });
    this.returnUrl =
      this.route.snapshot.queryParams['returnUrl'] ||
      SOURCE.concat(DOMAIN_ROUTER, SOURCE, 'trang-chu');
  }

  public isInvalid(name: string): boolean {
    const control = this.signinForm.controls[name];
    return control.invalid && (control.dirty || control.touched);
  }

  public submitForm(): void {
    if (!this.signinForm.valid) {
      Object.values(this.signinForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
      return;
    }
    if (this.signinForm.controls['captch'].value !== this.captch) {
      this.captch = randomCode(4);
      this.signinForm.reset();
      this.signinForm.controls['code'].setValue(this.captch);
      notificationCommon(
        this.notifyService,
        ERROR,
        'Mã xác nhận không đúng'
      );
      return;
    }
    this.isLoading = true;
    this.authenticationService
      .signIn(
        this.signinForm.controls['userName'].value,
        this.signinForm.controls['passWord'].value
      )
      .pipe(first())
      .subscribe(
        // tslint:disable-next-line:no-unused-expression
        (data: { data: null; message: string }) => {
          if (data.data == null) {
            notificationCommon(this.notifyService, ERROR, data.message);
          } else {
            // tslint:disable-next-line:no-unused-expression
            this.router.navigate([this.returnUrl]);
          }
        },
        (error: any) => {
          if (error.status === 0) {
            notificationCommon(
              this.notifyService,
              ERROR,
              'Không thể kết nối với server, vui lòng liên hệ với quản trị viên'
            );
          } else {
            notificationCommon(this.notifyService, ERROR);
          }
          this.isLoading = false;
        },
        () => {
          this.isLoading = false;
        }
      );
  }
}
