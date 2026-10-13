import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { AdministrativeUnitModalComponent } from './administrative-unit-modal/administrative-unit-modal.component';
import { AdministrativeUnitRoutingModule } from './administrative-unit-routing.module';
import { AdministrativeUnitComponent } from './administrative-unit.component';

@NgModule({
  declarations: [
    AdministrativeUnitComponent,
    AdministrativeUnitModalComponent,
  ],
  imports: [
    SharedUiModule,
    AdministrativeUnitRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class AdministrativeUnitModule {}
