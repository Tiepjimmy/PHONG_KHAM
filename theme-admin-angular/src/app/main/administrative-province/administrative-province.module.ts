import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { AdministrativeProvinceModalComponent } from './administrative-province-modal/administrative-province-modal.component';
import { AdministrativeProvinceRoutingModule } from './administrative-province-routing.module';
import { AdministrativeProvinceComponent } from './administrative-province.component';

@NgModule({
  declarations: [
    AdministrativeProvinceComponent,
    AdministrativeProvinceModalComponent,
  ],
  imports: [
    SharedUiModule,
    AdministrativeProvinceRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class AdministrativeProvinceModule {}
