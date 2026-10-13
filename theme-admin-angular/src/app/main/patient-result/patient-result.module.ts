import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { PatientResultModalComponent } from './patient-result-modal/patient-result-modal.component';
import { PatientResultRoutingModule } from './patient-result-routing.module';
import { PatientResultComponent } from './patient-result.component';

@NgModule({
  declarations: [PatientResultComponent, PatientResultModalComponent],
  imports: [
    SharedUiModule,
    PatientResultRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class PatientResultModule {}
