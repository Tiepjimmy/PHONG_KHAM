import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministrativeProvinceComponent } from './administrative-province.component';

const routes: Routes = [
  {
    path: '',
    component: AdministrativeProvinceComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdministrativeProvinceRoutingModule {}
