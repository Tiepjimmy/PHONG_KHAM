import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { MedicalWarehouseMainModalComponent } from './medical-warehouse-main-modal/medical-warehouse-main-modal.component';
import { MedicalWarehouseMainRoutingModule } from './medical-warehouse-main-routing.module';
import { MedicalWarehouseMainComponent } from './medical-warehouse-main.component';

@NgModule({
  declarations: [
    MedicalWarehouseMainComponent,
    MedicalWarehouseMainModalComponent,
  ],
  imports: [
    SharedUiModule,
    MedicalWarehouseMainRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class MedicalWarehouseMainModule {}
