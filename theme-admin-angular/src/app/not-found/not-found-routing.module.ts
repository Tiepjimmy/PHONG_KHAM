import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './not-found.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/khong-co-quyen-truy-cap' },
  {
    path: '',
    component: NotFoundComponent,
    children: [
      {
        path: 'khong-co-quyen-truy-cap',
        loadChildren: () =>
          import('../not-found/not-found.module').then((m) => m.NotFoundModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NotFoundRoutingModule {}
