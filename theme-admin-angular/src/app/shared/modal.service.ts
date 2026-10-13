import { Injectable, Type } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';

export interface OpenModalOptions {
  title?: string;
  data?: any;
  size?: 'sm' | 'lg' | 'xl';
}

// Opens a W3CRM (ng-bootstrap) modal. The modal component declares
// @Input() title / @Input() data and closes with NgbActiveModal.close(result).
@Injectable({
  providedIn: 'root',
})
export class ModalService {
  constructor(private ngbModal: NgbModal) {}

  public open<T>(component: Type<T>, options: OpenModalOptions = {}): NgbModalRef {
    const modalRef = this.ngbModal.open(component, {
      size: options.size ?? 'lg',
      centered: true,
      backdrop: 'static',
      keyboard: false,
      scrollable: true,
    });
    if (options.title !== undefined) {
      modalRef.componentInstance.title = options.title;
    }
    if (options.data !== undefined) {
      modalRef.componentInstance.data = options.data;
    }
    return modalRef;
  }

  // Emits the close() result (dismiss emits nothing)
  public openForResult<T>(component: Type<T>, options: OpenModalOptions = {}): Observable<any> {
    return this.open(component, options).closed;
  }
}
