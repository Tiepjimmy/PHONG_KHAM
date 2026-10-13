import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModule } from '../share/loading/loading.module';
import { AuditChangeRoutingModule } from './audit-change-routing.module';
import { AuditChangeComponent } from './audit-change.component';

@NgModule({
  declarations: [AuditChangeComponent],
  imports: [
    SharedUiModule,
    AuditChangeRoutingModule,
    LoadingModule,
    NotFoundModule,
  ],
})
export class AuditChangeModule {}
