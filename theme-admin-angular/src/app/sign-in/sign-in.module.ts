import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { LoadingModule } from '../main/share/loading/loading.module';
import { SignInRoutingModule } from './sign-in-routing.module';
import { SignInComponent } from './sign-in.component';

@NgModule({
  declarations: [SignInComponent],
  imports: [
    CommonModule,
    SignInRoutingModule,
    ReactiveFormsModule,
    LoadingModule,
  ],
})
export class SignInModule {}
