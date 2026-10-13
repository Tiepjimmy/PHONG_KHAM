import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { AdministrativeDistrictModalComponent } from './administrative-district-modal/administrative-district-modal.component';
import { AdministrativeDistrictRoutingModule } from './administrative-district-routing.module';
import { AdministrativeDistrictComponent } from './administrative-district.component';

@NgModule({
  declarations: [
    AdministrativeDistrictComponent,
    AdministrativeDistrictModalComponent,
  ],
  imports: [
    SharedUiModule,
    AdministrativeDistrictRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class AdministrativeDistrictModule {}
