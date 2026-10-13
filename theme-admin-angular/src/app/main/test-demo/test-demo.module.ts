import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { SharedUiModule } from 'src/app/shared/shared-ui.module';
import { LoadingModalModule } from '../share/loading-modal/loading-modal.module';
import { LoadingModule } from '../share/loading/loading.module';
import { TestDemoModalComponent } from './test-demo-modal/test-demo-modal.component';
import { TestDemoRoutingModule } from './test-demo-routing.module';
import { TestDemoComponent } from './test-demo.component';

@NgModule({
  declarations: [TestDemoComponent, TestDemoModalComponent],
  imports: [
    SharedUiModule,
    TestDemoRoutingModule,
    LoadingModule,
    NotFoundModule,
    LoadingModalModule,
  ],
})
export class TestDemoModule {}
