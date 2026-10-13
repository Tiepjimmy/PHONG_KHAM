import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { SupplierModalComponent } from './supplier-modal/supplier-modal.component';
import { SupplierRoutingModule } from './supplier-routing.module';
import { SupplierComponent } from './supplier.component';

@NgModule({
  declarations: [
    SupplierComponent,
    SupplierModalComponent,
  ],
  imports: [
    SharedUiModule,
    SupplierRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class SupplierModule {}
