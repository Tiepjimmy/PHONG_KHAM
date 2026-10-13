import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministrativeDistrictComponent } from './administrative-district.component';

const routes: Routes = [
  {
    path: '',
    component: AdministrativeDistrictComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdministrativeDistrictRoutingModule {}
