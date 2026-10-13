import { Component, forwardRef, Injectable, Input } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import {
  NgbDateParserFormatter,
  NgbDatepickerModule,
  NgbDateStruct,
} from '@ng-bootstrap/ng-bootstrap';

// dd/MM/yyyy display for the W3CRM (ng-bootstrap) datepicker
@Injectable()
export class ViDateParserFormatter extends NgbDateParserFormatter {
  public parse(value: string): NgbDateStruct | null {
    const parts = (value ?? '').trim().split(/[/.-]/).map(Number);
    if (parts.length !== 3 || parts.some(isNaN)) {
      return null;
    }
    const [day, month, year] = parts;
    return { day, month, year };
  }

  public format(date: NgbDateStruct | null): string {
    if (!date) {
      return '';
    }
    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${pad(date.day)}/${pad(date.month)}/${date.year}`;
  }
}

// <app-date-picker formControlName="x">: W3CRM ngbDatepicker whose model is a Date (or null),
// accepting Date or API date strings when patched (same contract nz-date-picker had).
@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [FormsModule, NgbDatepickerModule],
  templateUrl: './date-picker.component.html',
  providers: [
    { provide: NgbDateParserFormatter, useClass: ViDateParserFormatter },
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DatePickerComponent),
      multi: true,
    },
  ],
})
export class DatePickerComponent implements ControlValueAccessor {
  @Input() public placeholder = 'dd/mm/yyyy';
  @Input() public invalid = false;
  @Input() public inputId = '';

  public model: NgbDateStruct | null = null;
  public disabled = false;
  public readonly minDate: NgbDateStruct = { year: 1900, month: 1, day: 1 };
  public readonly maxDate: NgbDateStruct = { year: 2100, month: 12, day: 31 };

  private onChange: (value: Date | null) => void = () => {};
  private onTouched: () => void = () => {};

  public onModelChange(value: NgbDateStruct | string | null): void {
    // ngbDatepicker emits a string while the typed text is not a valid date
    this.model = value && typeof value === 'object' ? value : null;
    this.onChange(this.model ? new Date(this.model.year, this.model.month - 1, this.model.day) : null);
  }

  public onBlur(): void {
    this.onTouched();
  }

  public writeValue(value: any): void {
    this.model = this.toStruct(value);
  }

  public registerOnChange(fn: (value: Date | null) => void): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  public setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  private toStruct(value: any): NgbDateStruct | null {
    if (!value) {
      return null;
    }
    if (typeof value === 'string' && /^\d{4}-\d{2}-\d{2}/.test(value)) {
      const [year, month, day] = value.substring(0, 10).split('-').map(Number);
      return { year, month, day };
    }
    const date = value instanceof Date ? value : new Date(value);
    return isNaN(date.getTime())
      ? null
      : { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDate() };
  }
}
