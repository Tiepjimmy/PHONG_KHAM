import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { PatientModalComponent } from './patient-modal/patient-modal.component';
import { PatientRoutingModule } from './patient-routing.module';
import { PatientComponent } from './patient.component';

@NgModule({
  declarations: [PatientComponent, PatientModalComponent],
  imports: [
    SharedUiModule,
    PatientRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class PatientModule {}
