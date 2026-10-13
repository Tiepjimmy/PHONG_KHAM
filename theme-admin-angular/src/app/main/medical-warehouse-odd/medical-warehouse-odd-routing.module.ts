import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MedicalWarehouseOddComponent } from './medical-warehouse-odd.component';

const routes: Routes = [
  {
    path: '',
    component: MedicalWarehouseOddComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MedicalWarehouseOddRoutingModule {}
