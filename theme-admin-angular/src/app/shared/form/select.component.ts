import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  forwardRef,
  Input,
  OnChanges,
  Output,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Select2Data, Select2Module, Select2UpdateEvent } from 'ng-select2-component';

// Form-friendly adapter over the W3CRM select (ng-select2-component, same as plugins/select2):
// takes raw API lists via items/bindValue/bindLabel and works with formControlName.
@Component({
  selector: 'app-select',
  standalone: true,
  imports: [CommonModule, Select2Module],
  templateUrl: './select.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectComponent),
      multi: true,
    },
  ],
})
export class SelectComponent implements ControlValueAccessor, OnChanges {
  @Input() public items: any[] | null | undefined = [];
  @Input() public bindValue = 'id';
  @Input() public bindLabel = 'name';
  @Input() public placeholder = 'Chọn dữ liệu';
  @Input() public multiple = false;
  @Input() public allowClear = true;
  @Input() public searchable = true;
  @Input() public invalid = false;
  @Input() public loading = false;
  @Output() public selectionChange = new EventEmitter<any>();

  public data: Select2Data = [];
  public value: any = null;
  public disabled = false;

  private onChange: (value: any) => void = () => {};
  private onTouched: () => void = () => {};

  public ngOnChanges(): void {
    this.data = (this.items ?? []).map((item) => ({
      value: item?.[this.bindValue],
      label: item?.[this.bindLabel] ?? '',
    }));
  }

  public onUpdate(event: Select2UpdateEvent<any>): void {
    const next = event.value ?? (this.multiple ? [] : null);
    if (this.same(next, this.value)) {
      return;
    }
    this.value = next;
    this.onChange(next);
    this.selectionChange.emit(next);
  }

  public onBlur(): void {
    this.onTouched();
  }

  public writeValue(value: any): void {
    this.value = this.multiple ? (value ?? []) : value === '' ? null : value;
  }

  public registerOnChange(fn: (value: any) => void): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  public setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  private same(a: any, b: any): boolean {
    if (Array.isArray(a) && Array.isArray(b)) {
      return a.length === b.length && a.every((x, i) => x === b[i]);
    }
    return (a ?? null) === (b ?? null);
  }
}
