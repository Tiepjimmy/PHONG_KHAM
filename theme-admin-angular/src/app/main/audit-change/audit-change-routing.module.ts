import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuditChangeComponent } from './audit-change.component';

const routes: Routes = [
  {
    path: '',
    component: AuditChangeComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuditChangeRoutingModule {}
