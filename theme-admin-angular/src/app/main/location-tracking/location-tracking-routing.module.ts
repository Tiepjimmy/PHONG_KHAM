import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LocationTrackingComponent } from './location-tracking.component';

const routes: Routes = [
  {
    path: '',
    component: LocationTrackingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LocationTrackingRoutingModule {}
