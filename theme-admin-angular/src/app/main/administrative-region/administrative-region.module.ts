import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { AdministrativeRegionModalComponent } from './administrative-region-modal/administrative-region-modal.component';
import { AdministrativeRegionRoutingModule } from './administrative-region-routing.module';
import { AdministrativeRegionComponent } from './administrative-region.component';

@NgModule({
  declarations: [
    AdministrativeRegionComponent,
    AdministrativeRegionModalComponent,
  ],
  imports: [
    SharedUiModule,
    AdministrativeRegionRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class AdministrativeRegionModule {}
