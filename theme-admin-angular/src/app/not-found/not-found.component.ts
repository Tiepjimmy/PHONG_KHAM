import { Component, Input, OnInit } from '@angular/core';
import {
  DOMAIN_ROUTER,
  SOURCE,
  STRING_EMPTY,
} from '../helpers/constant-helper';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
})
export class NotFoundComponent implements OnInit {
  @Input() public isFullPage = STRING_EMPTY;
  public domainRouter = STRING_EMPTY;
  public checkFullpage = false;

  constructor(private authenticationService: AuthenticationService) {}

  public ngOnInit(): void {
    const listMenu = this.authenticationService.userValue?.listMenu;
    if (listMenu) {
      this.domainRouter = SOURCE.concat(
        DOMAIN_ROUTER,
        SOURCE.concat(listMenu[0].metaLink)
      );
    }
    if (this.isFullPage === 'false') {
      this.checkFullpage = false;
    } else {
      this.checkFullpage = true;
    }
  }
}
