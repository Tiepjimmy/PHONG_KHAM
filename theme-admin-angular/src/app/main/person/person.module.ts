import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { PersonModalComponent } from './person-modal/person-modal.component';
import { PersonRoutingModule } from './person-routing.module';
import { PersonComponent } from './person.component';

@NgModule({
  declarations: [PersonComponent, PersonModalComponent],
  imports: [
    SharedUiModule,
    PersonRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class PersonModule {}
