import { NgModule } from '@angular/core';

import { NgApexchartsModule } from 'ng-apexcharts';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModule } from '../share/loading/loading.module';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';

@NgModule({
  declarations: [DashboardComponent],
  imports: [
    SharedUiModule,
    DashboardRoutingModule,
    NgApexchartsModule,
    LoadingModule,
  ],
})
export class DashboardModule {}
