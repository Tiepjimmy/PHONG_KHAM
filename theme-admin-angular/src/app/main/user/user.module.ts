import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { UserModalComponent } from './user-modal/user-modal.component';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';

@NgModule({
  declarations: [UserComponent, UserModalComponent],
  imports: [
    SharedUiModule,
    UserRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class UserModule {}
