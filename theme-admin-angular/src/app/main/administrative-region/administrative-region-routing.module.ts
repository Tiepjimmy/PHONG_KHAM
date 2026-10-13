import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministrativeRegionComponent } from './administrative-region.component';

const routes: Routes = [
  {
    path: '',
    component: AdministrativeRegionComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdministrativeRegionRoutingModule {}
