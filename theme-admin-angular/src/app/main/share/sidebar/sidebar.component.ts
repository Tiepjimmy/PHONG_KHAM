import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { DOMAIN_ROUTER, SOURCE } from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';

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
