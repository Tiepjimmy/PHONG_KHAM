import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MedicalWarehouseExpiryComponent } from './medical-warehouse-expiry.component';

const routes: Routes = [
  {
    path: '',
    component: MedicalWarehouseExpiryComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MedicalWarehouseExpiryRoutingModule {}
