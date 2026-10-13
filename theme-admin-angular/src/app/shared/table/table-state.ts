// Server-side paging/sorting state for W3CRM tables.
// sortOrder keeps the ng-zorro values ('ascend' | 'descend') the stored procedures expect.
export type SortOrder = 'ascend' | 'descend' | '';

export class TableState {
  public page = 1;
  public pageSize = 10;
  public total = 0;
  public sortField = '';
  public sortOrder: SortOrder = '';

  public toggleSort(field: string): void {
    if (this.sortField !== field) {
      this.sortField = field;
      this.sortOrder = 'ascend';
    } else if (this.sortOrder === 'ascend') {
      this.sortOrder = 'descend';
    } else {
      this.sortField = '';
      this.sortOrder = '';
    }
    this.page = 1;
  }

  public sortIcon(field: string): string {
    if (this.sortField !== field || !this.sortOrder) {
      return 'fa-sort text-muted';
    }
    return this.sortOrder === 'ascend' ? 'fa-sort-up' : 'fa-sort-down';
  }

  public toParams(): { page: number; pageSize: number; sortField: string; sortOrder: string } {
    return {
      page: this.page,
      pageSize: this.pageSize,
      sortField: this.sortField,
      sortOrder: this.sortOrder,
    };
  }
}
