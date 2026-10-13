import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import {
  DOMAIN_ROUTER,
  SOURCE,
  STRING_EMPTY,
} from 'src/app/helpers/constant-helper';
import { AuthenticationService } from 'src/app/services/authentication.service';

// W3CRM page title bar (.page-titles), rendered once in the main layout and updated per route
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit {
  public titleHeader = STRING_EMPTY;
  public titlePage = STRING_EMPTY;
  public isShowBreadcrumb = false;
  public title = STRING_EMPTY;
  public titleSubPage = STRING_EMPTY;
  public domainRouter = STRING_EMPTY;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {}

  public ngOnInit(): void {
    this.update(this.router.url);
    this.router.events
      .pipe(filter((e) => e instanceof NavigationEnd))
      .subscribe((e) => this.update((e as NavigationEnd).urlAfterRedirects));
  }

  private update(url: string): void {
    const currentUrl = url.split('?')[0];
    const listMenu = this.authenticationService.userValue?.listMenu;
    if (!listMenu?.length) {
      return;
    }
    const home = listMenu[0];
    this.domainRouter = SOURCE.concat(DOMAIN_ROUTER, SOURCE, home.metaLink);
    this.title = home.objectMenuName;
    const link = (x: any) => SOURCE.concat(DOMAIN_ROUTER, SOURCE, x.metaLink);
    const menu =
      listMenu.flatMap((x) => x.children).find((x) => link(x) === currentUrl) ??
      listMenu.find((x) => link(x) === currentUrl);
    this.titleSubPage = menu?.objectMenuName ?? STRING_EMPTY;
    this.titleHeader = menu?.shortName || menu?.objectMenuName || STRING_EMPTY;
    this.titlePage =
      listMenu.find((x) => x.id === menu?.parentId)?.objectMenuName ?? STRING_EMPTY;
    this.isShowBreadcrumb = !!this.titlePage;
  }
}
