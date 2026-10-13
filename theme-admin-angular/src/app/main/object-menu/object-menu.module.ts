import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { ObjectMenuModalComponent } from './object-menu-modal/object-menu-modal.component';
import { ObjectMenuRoutingModule } from './object-menu-routing.module';
import { ObjectMenuComponent } from './object-menu.component';

@NgModule({
  declarations: [ObjectMenuComponent, ObjectMenuModalComponent],
  imports: [
    SharedUiModule,
    ObjectMenuRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class ObjectMenuModule {}
