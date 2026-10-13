import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministrativeWardComponent } from './administrative-ward.component';

const routes: Routes = [
  {
    path: '',
    component: AdministrativeWardComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdministrativeWardRoutingModule {}
