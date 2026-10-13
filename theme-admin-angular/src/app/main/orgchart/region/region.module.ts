import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../../share/loading/loading.module';
import { RegionModalComponent } from './region-modal/region-modal.component';
import { RegionRoutingModule } from './region-routing.module';
import { RegionComponent } from './region.component';

@NgModule({
  declarations: [
    RegionComponent,
    RegionModalComponent,
  ],
  imports: [
    SharedUiModule,
    RegionRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class RegionModule {}
