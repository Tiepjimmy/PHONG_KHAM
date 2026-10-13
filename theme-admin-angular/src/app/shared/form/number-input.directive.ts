import { Directive, ElementRef, forwardRef, HostListener, Input, Renderer2 } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

// <input appNumberInput formControlName="x">: model is a number (or null), shown with
// thousands separators like the nz-input-number currency formatter. Replaces nz-input-number.
@Directive({
  selector: 'input[appNumberInput]',
  standalone: true,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NumberInputDirective),
      multi: true,
    },
  ],
})
export class NumberInputDirective implements ControlValueAccessor {
  @Input() public min: number | null = null;
  @Input() public max: number | null = null;
  @Input() public decimals = 0;

  private onChange: (value: number | null) => void = () => {};
  private onTouched: () => void = () => {};

  constructor(private elementRef: ElementRef<HTMLInputElement>, private renderer: Renderer2) {
    this.renderer.setAttribute(this.elementRef.nativeElement, 'inputmode', 'decimal');
  }

  @HostListener('input')
  public handleInput(): void {
    const value = this.parse(this.elementRef.nativeElement.value);
    this.onChange(value);
  }

  @HostListener('blur')
  public handleBlur(): void {
    let value = this.parse(this.elementRef.nativeElement.value);
    if (value !== null) {
      if (this.min !== null && value < this.min) value = this.min;
      if (this.max !== null && value > this.max) value = this.max;
    }
    this.onChange(value);
    this.writeValue(value);
    this.onTouched();
  }

  public writeValue(value: any): void {
    this.renderer.setProperty(this.elementRef.nativeElement, 'value', this.format(value));
  }

  public registerOnChange(fn: (value: number | null) => void): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  public setDisabledState(isDisabled: boolean): void {
    this.renderer.setProperty(this.elementRef.nativeElement, 'disabled', isDisabled);
  }

  private parse(text: string): number | null {
    const cleaned = (text ?? '').replace(/,/g, '').trim();
    if (cleaned === '' || cleaned === '-') {
      return null;
    }
    const value = Number(cleaned);
    return isNaN(value) ? null : value;
  }

  private format(value: any): string {
    if (value === null || value === undefined || value === '' || isNaN(Number(value))) {
      return '';
    }
    const [integer, fraction] = Number(value).toFixed(this.decimals).split('.');
    const grouped = integer.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    return fraction && Number(fraction) !== 0 ? `${grouped}.${fraction}` : grouped;
  }
}
