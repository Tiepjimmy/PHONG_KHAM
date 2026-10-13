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
  public menuSearch = STRING_EMPTY;
  public menuLeaves: any[] = [];
  public isDark = false;
  public isFullscreen = false;

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
      this.menuLeaves = listMenu.flatMap((x: any) => (x.children?.length ? x.children : [x]));
    }
    // Same storage key the W3CRM header uses for light/dark mode
    this.isDark = localStorage.getItem('data-theme-version') === 'dark';
    document.body.setAttribute('data-theme-version', this.isDark ? 'dark' : 'light');
    this.onResize();
  }

  public searchMenu(): void {
    const text = this.menuSearch.trim().toLowerCase();
    if (!text) {
      return;
    }
    const menu =
      this.menuLeaves.find((x) => x.objectMenuName?.toLowerCase() === text) ??
      this.menuLeaves.find((x) => x.objectMenuName?.toLowerCase().includes(text));
    if (menu) {
      this.router.navigate([SOURCE.concat(DOMAIN_ROUTER, SOURCE, menu.metaLink)]);
      this.menuSearch = STRING_EMPTY;
    }
  }

  public toggleThemeMode(): void {
    this.isDark = !this.isDark;
    const mode = this.isDark ? 'dark' : 'light';
    localStorage.setItem('data-theme-version', mode);
    document.body.setAttribute('data-theme-version', mode);
  }

  public toggleFullscreen(): void {
    if (!document.fullscreenElement) {
      document.documentElement.requestFullscreen?.();
      this.isFullscreen = true;
    } else {
      document.exitFullscreen?.();
      this.isFullscreen = false;
    }
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
