import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { UserGroupModalComponent } from './user-group-modal/user-group-modal.component';
import { UserGroupPermissionModalComponent } from './user-group-permission-modal/user-group-permission-modal.component';
import { UserGroupRoutingModule } from './user-group-routing.module';
import { UserGroupComponent } from './user-group.component';

@NgModule({
  declarations: [
    UserGroupComponent,
    UserGroupModalComponent,
    UserGroupPermissionModalComponent,
  ],
  imports: [
    SharedUiModule,
    UserGroupRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class UserGroupModule {}
