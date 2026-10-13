import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

// Footer for W3CRM tables: "Hiển thị x-y của z bản ghi", page size and windowed page links
@Component({
  selector: 'app-table-pager',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './table-pager.component.html',
})
export class TablePagerComponent {
  @Input() public page = 1;
  @Input() public pageSize = 10;
  @Input() public total = 0;
  @Input() public pageSizeOptions = [10, 20, 30, 50];
  @Output() public pageChange = new EventEmitter<number>();
  @Output() public pageSizeChange = new EventEmitter<number>();

  public get totalPage(): number {
    return Math.max(1, Math.ceil(this.total / this.pageSize));
  }

  public get from(): number {
    return this.total === 0 ? 0 : (this.page - 1) * this.pageSize + 1;
  }

  public get to(): number {
    return Math.min(this.page * this.pageSize, this.total);
  }

  // At most 5 page numbers around the current page
  public get pages(): number[] {
    const start = Math.max(1, Math.min(this.page - 2, this.totalPage - 4));
    const end = Math.min(this.totalPage, start + 4);
    const result: number[] = [];
    for (let i = start; i <= end; i++) {
      result.push(i);
    }
    return result;
  }

  public goTo(page: number): void {
    if (page < 1 || page > this.totalPage || page === this.page) {
      return;
    }
    this.pageChange.emit(page);
  }

  public changeSize(size: number): void {
    this.pageSizeChange.emit(Number(size));
  }
}
