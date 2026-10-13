import { NgModule } from '@angular/core';
import { SafeHtmlSvgPipe } from '../_services/svg-pipe/safe-html-svg.pipe';
import { SharedUiModule } from '../shared/shared-ui.module';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { MainRoutingModule } from './main-routing.module';
import { MainComponent } from './main.component';
import { ProfileComponent } from './profile/profile.component';
import { HeaderModule } from './share/header/header.module';
import { ImportExcelComponent } from './share/import-excel/import-excel.component';
import { LoadingModalModule } from './share/loading-modal/loading-modal.module';
import { LoadingModule } from './share/loading/loading.module';
import { SidebarComponent } from './share/sidebar/sidebar.component';

@NgModule({
  declarations: [
    MainComponent,
    SidebarComponent,
    ProfileComponent,
    ChangePasswordComponent,
    ImportExcelComponent,
  ],
  imports: [
    SharedUiModule,
    MainRoutingModule,
    HeaderModule,
    SafeHtmlSvgPipe,
    LoadingModule,
    LoadingModalModule,
  ],
  providers: [],
})
export class MainModule {}
