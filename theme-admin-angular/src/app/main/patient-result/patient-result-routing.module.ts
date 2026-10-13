import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientResultComponent } from './patient-result.component';

const routes: Routes = [
  {
    path: '',
    component: PatientResultComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PatientResultRoutingModule {}
