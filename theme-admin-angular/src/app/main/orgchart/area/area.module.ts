import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../../share/loading/loading.module';
import { AreaModalComponent } from './area-modal/area-modal.component';
import { AreaRoutingModule } from './area-routing.module';
import { AreaComponent } from './area.component';

@NgModule({
  declarations: [
    AreaComponent,
    AreaModalComponent,
  ],
  imports: [
    SharedUiModule,
    AreaRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class AreaModule {}
