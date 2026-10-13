import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestDemoComponent } from './test-demo.component';

const routes: Routes = [
  {
    path: '',
    component: TestDemoComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TestDemoRoutingModule {}
