import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ObjectMenuComponent } from './object-menu.component';

const routes: Routes = [
  {
    path: '',
    component: ObjectMenuComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ObjectMenuRoutingModule {}
