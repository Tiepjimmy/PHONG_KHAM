import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { DatePickerComponent } from './form/date-picker.component';
import { NumberInputDirective } from './form/number-input.directive';
import { SelectComponent } from './form/select.component';
import { TablePagerComponent } from './table/table-pager.component';

// Everything a clinic screen needs to render with the W3CRM theme (Bootstrap 5, ng-bootstrap, select2)
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModalModule,
    SelectComponent,
    DatePickerComponent,
    NumberInputDirective,
    TablePagerComponent,
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModalModule,
    SelectComponent,
    DatePickerComponent,
    NumberInputDirective,
    TablePagerComponent,
  ],
})
export class SharedUiModule {}
