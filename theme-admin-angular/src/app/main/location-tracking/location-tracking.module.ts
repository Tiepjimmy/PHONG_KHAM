import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModule } from '../share/loading/loading.module';
import { LocationTrackingRoutingModule } from './location-tracking-routing.module';
import { LocationTrackingComponent } from './location-tracking.component';

@NgModule({
  declarations: [LocationTrackingComponent],
  imports: [
    SharedUiModule,
    LocationTrackingRoutingModule,
    LoadingModule,
    NotFoundModule,
  ],
})
export class LocationTrackingModule {}
