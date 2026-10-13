import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MedicalWarehouseMainComponent } from './medical-warehouse-main.component';

const routes: Routes = [
  {
    path: '',
    component: MedicalWarehouseMainComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MedicalWarehouseMainRoutingModule {}
