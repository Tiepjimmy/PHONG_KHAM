import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { MedicalWarehouseOddModalComponent } from './medical-warehouse-odd-modal/medical-warehouse-odd-modal.component';
import { MedicalWarehouseOddRoutingModule } from './medical-warehouse-odd-routing.module';
import { MedicalWarehouseOddComponent } from './medical-warehouse-odd.component';

@NgModule({
  declarations: [
    MedicalWarehouseOddComponent,
    MedicalWarehouseOddModalComponent,
  ],
  imports: [
    SharedUiModule,
    MedicalWarehouseOddRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class MedicalWarehouseOddModule {}
