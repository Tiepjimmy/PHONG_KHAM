import { Component, HostListener, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import {
  DOMAIN_ROUTER,
  LOGO_HEADER_NAME,
  PROJECT_NAME,
  PROJECT_SHORT_NAME,
  SOURCE,
  STRING_EMPTY,
} from '../helpers/constant-helper';
import { AuthenticationService } from '../services/authentication.service';
import { ModalService } from '../shared/modal.service';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ProfileComponent } from './profile/profile.component';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
})
export class MainComponent implements OnInit {
  public avatar = STRING_EMPTY;
  public fullName = STRING_EMPTY;
  public userName = STRING_EMPTY;
  public projectName = PROJECT_NAME;
  public projectShortName = PROJECT_SHORT_NAME;
  public logoName = LOGO_HEADER_NAME;
  public homeRouter = SOURCE.concat(DOMAIN_ROUTER);
  public toggleVal = false;
  public mouseOvered = false;

  constructor(
    private authenticationService: AuthenticationService,
    private modalService: ModalService,
    private router: Router
  ) {
    this.router.events
      .pipe(filter((e) => e instanceof NavigationEnd))
      .subscribe(() => setTimeout(() => this.handleMinHeight(), 500));
  }

  public ngOnInit(): void {
    const user = this.authenticationService.userValue;
    this.avatar = user?.avatar ?? STRING_EMPTY;
    this.fullName = user?.fullName ?? STRING_EMPTY;
    this.userName = user?.userName ?? STRING_EMPTY;
    const listMenu = user?.listMenu;
    if (listMenu?.length) {
      this.homeRouter = SOURCE.concat(DOMAIN_ROUTER, SOURCE, listMenu[0].metaLink);
    }
    this.onResize();
  }

  public logOut(): void {
    this.authenticationService.logout();
  }

  public openModalInfo(): void {
    this.modalService.open(ProfileComponent, {
      title: this.modalTitle(),
      data: {},
    });
  }

  public openModalChangePassword(): void {
    this.modalService.open(ChangePasswordComponent, {
      title: this.modalTitle(),
      data: {},
    });
  }

  private modalTitle(): string {
    return 'Thông tin cá nhân '.concat(
      '- (Tên đăng nhập: ',
      this.authenticationService.userValue?.userName ?? STRING_EMPTY,
      ')'
    );
  }

  public sidebarToggle(): void {
    this.toggleVal = !this.toggleVal;
    setTimeout(() => this.handleMinHeight(), 500);
  }

  public iconHoverToggle(value: boolean): void {
    this.mouseOvered = value;
  }

  // Same breakpoints as the W3CRM nav-header: mini sidebar on tablets, overlay on phones
  @HostListener('window:resize')
  public onResize(): void {
    const width = window.innerWidth;
    let sidebarStyle = localStorage.getItem('data-sidebar-style') ?? 'full';
    if (width < 1200 && width > 768) {
      this.toggleVal = true;
      sidebarStyle = 'mini';
    } else if (width <= 768) {
      this.toggleVal = false;
      sidebarStyle = 'overlay';
    } else {
      this.toggleVal = false;
    }
    document.body.setAttribute('data-sidebar-style', sidebarStyle);
    this.handleMinHeight();
  }

  private handleMinHeight(): void {
    const contentBody = document.getElementsByClassName(
      'content-body'
    )[0] as HTMLElement;
    if (contentBody) {
      contentBody.style.minHeight = window.innerHeight - 75 + 'px';
    }
  }
}
