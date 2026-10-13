import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnitMeasureComponent } from './unit-measure.component';

const routes: Routes = [
  {
    path: '',
    component: UnitMeasureComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UnitMeasureRoutingModule {}
