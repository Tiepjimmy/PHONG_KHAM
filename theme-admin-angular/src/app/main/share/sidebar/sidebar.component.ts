import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { DOMAIN_ROUTER, SOURCE } from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { THEME_ICONS } from 'src/app/shared/theme-icons';

// Clinic menu name keywords -> W3CRM sidebar icon (first match wins)
const MENU_ICONS: [RegExp, string][] = [
  [/trang chu|dashboard/, 'Dashboard'],
  [/benh nhan|kham|ket qua/, 'Manage Clients'],
  [/kho thuoc|thuoc/, 'Finance'],
  [/danh muc/, 'Reports'],
  [/he thong|cau hinh/, 'CMS'],
  [/hanh chinh|vung|khu vuc|tinh|huyen|xa/, 'Projects'],
  [/tai khoan|nguoi dung|bac si|nhan su/, 'Employees'],
  [/bao cao|thong ke/, 'Performance'],
  [/khach hang|ban hang|co hoi|san pham/, 'Core HR'],
  [/to chuc|chi nhanh|phong ban/, 'Apps'],
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent implements OnInit {
  @Output() public mouseHoverEvent = new EventEmitter<boolean>();

  public listMenu: any[] = [];
  public domain = SOURCE.concat(DOMAIN_ROUTER, SOURCE);
  public currentUrl = '';
  public openMenu: any = null;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.listMenu = this.authenticationService.userValue?.listMenu ?? [];
    this.syncActiveMenu(this.router.url);
    this.router.events
      .pipe(filter((e) => e instanceof NavigationEnd))
      .subscribe((e) => this.syncActiveMenu((e as NavigationEnd).urlAfterRedirects));
  }

  public iconFor(menu: any): string {
    const name = (menu?.objectMenuName ?? '')
      .toLowerCase()
      .normalize('NFD')
      // after NFD the accents are separate non-ASCII marks: drop them, keep đ as d
      .replace(/[^\x00-\x7F]/g, (c: string) => (c === 'đ' ? 'd' : ''));
    const match = MENU_ICONS.find(([re]) => re.test(name));
    return THEME_ICONS[match ? match[1] : 'Tasks'];
  }

  public link(menu: any): string {
    return this.domain + menu.metaLink;
  }

  public isCurrent(menu: any): boolean {
    return this.currentUrl === this.link(menu);
  }

  public isParentActive(menu: any): boolean {
    return menu.children?.some((x: any) => this.isCurrent(x)) ?? false;
  }

  public toggleMenu(menu: any): void {
    this.openMenu = this.openMenu === menu ? null : menu;
  }

  public hoverAdd(value: boolean): void {
    this.mouseHoverEvent.emit(value);
  }

  private syncActiveMenu(url: string): void {
    this.currentUrl = url.split('?')[0];
    const parent = this.listMenu.find((x) => this.isParentActive(x));
    if (parent) {
      this.openMenu = parent;
    }
  }
}
