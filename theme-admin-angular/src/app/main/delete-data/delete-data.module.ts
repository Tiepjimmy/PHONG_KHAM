import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { NotFoundModule } from 'src/app/not-found/not-found.module';
import { LoadingModule } from '../share/loading/loading.module';
import { DeleteDataRoutingModule } from './delete-data-routing.module';
import { DeleteDataComponent } from './delete-data.component';

@NgModule({
  declarations: [DeleteDataComponent],
  imports: [
    CommonModule,
    DeleteDataRoutingModule,
    LoadingModule,
    NotFoundModule,
  ],
})
export class DeleteDataModule {}
