import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { UnitMeasureModalComponent } from './unit-measure-modal/unit-measure-modal.component';
import { UnitMeasureRoutingModule } from './unit-measure-routing.module';
import { UnitMeasureComponent } from './unit-measure.component';

@NgModule({
  declarations: [UnitMeasureComponent, UnitMeasureModalComponent],
  imports: [
    SharedUiModule,
    UnitMeasureRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class UnitMeasureModule {}
