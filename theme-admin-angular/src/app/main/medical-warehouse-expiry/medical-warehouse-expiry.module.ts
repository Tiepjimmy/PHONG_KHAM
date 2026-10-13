import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { MedicalWarehouseExpiryRoutingModule } from './medical-warehouse-expiry-routing.module';
import { MedicalWarehouseExpiryComponent } from './medical-warehouse-expiry.component';

@NgModule({
  declarations: [MedicalWarehouseExpiryComponent],
  imports: [
    SharedUiModule,
    MedicalWarehouseExpiryRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class MedicalWarehouseExpiryModule {}
