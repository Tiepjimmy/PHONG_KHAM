import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { LoadingModalComponent } from './loading-modal.component';

@NgModule({
  declarations: [LoadingModalComponent],
  exports: [LoadingModalComponent],
  imports: [CommonModule],
})
export class LoadingModalModule {}
