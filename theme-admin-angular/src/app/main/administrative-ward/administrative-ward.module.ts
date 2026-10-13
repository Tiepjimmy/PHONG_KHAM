import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { AdministrativeWardModalComponent } from './administrative-ward-modal/administrative-ward-modal.component';
import { AdministrativeWardRoutingModule } from './administrative-ward-routing.module';
import { AdministrativeWardComponent } from './administrative-ward.component';

@NgModule({
  declarations: [
    AdministrativeWardComponent,
    AdministrativeWardModalComponent,
  ],
  imports: [
    SharedUiModule,
    AdministrativeWardRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class AdministrativeWardModule {}
