import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NotFoundRoutingModule } from './not-found-routing.module';
import { NotFoundComponent } from './not-found.component';

@NgModule({
  declarations: [NotFoundComponent],
  exports: [NotFoundComponent],
  imports: [CommonModule, NotFoundRoutingModule, ReactiveFormsModule],
})
export class NotFoundModule {}
