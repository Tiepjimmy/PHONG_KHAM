import { Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import moment from 'moment';
import { NotifyService } from '../shared/notify.service';
import {
  BMP,
  COMMA,
  DATE_FORMAT_YYYYMMDD,
  DOC,
  DOCX,
  DOMAIN_ROUTER,
  DOT,
  ENC,
  ERROR,
  GIF,
  INTERNAL_SERVER_ERROR,
  JPEG,
  JPG,
  NOTIFICATION_TITLE,
  PDF,
  PNG,
  PPT,
  PPTX,
  RAR,
  SOURCE,
  STRING_EMPTY,
  TAR,
  XLS,
  XLSX,
  Z7Z,
  ZIP,
} from './constant-helper';

export function checkClassActive(
  router: Router,
  renderer: Renderer2,
  listMenu?: any[]
): void {
  const url = router.url;
  let firstMenu = SOURCE;
  if (listMenu) {
    firstMenu = firstMenu.concat(listMenu[0].metaLink);
  }
  if (
    !url.startsWith('/dang-nhap') &&
    url !== SOURCE.concat(DOMAIN_ROUTER, firstMenu) &&
    !checkPermission(url, listMenu)
  ) {
    router?.navigate(['/khong-co-quyen-truy-cap']);
    return;
  }
  // Active menu state is handled by the W3CRM sidebar (main/share/sidebar)
  if (url.startsWith('/dang-nhap')) {
    renderer.addClass(document.body, 'body-login');
  } else {
    renderer.removeClass(document.body, 'body-login');
  }
}

export function notificationCommon(
  notifyService: NotifyService,
  type: string,
  message?: string
): void {
  notifyService.show(type, message);
}

export const checkValidateBetweenTwoDate = (startDate: any, endDate: any) => {
  if (
    startDate &&
    endDate &&
    new Date(moment(startDate).format(DATE_FORMAT_YYYYMMDD)) >
      new Date(moment(endDate).format(DATE_FORMAT_YYYYMMDD))
  ) {
    return true;
  }
  return false;
};

export const checkValidateGreaterTwoDate = (startDate: any, endDate: any) => {
  if (
    startDate &&
    endDate &&
    new Date(moment(startDate).format(DATE_FORMAT_YYYYMMDD)) >=
      new Date(moment(endDate).format(DATE_FORMAT_YYYYMMDD))
  ) {
    return true;
  }
  return false;
};

export const convertUrlToList = (url: any) => {
  return url.split(SOURCE);
};

export const listFileType = () => {
  return DOT.concat(
    DOC,
    COMMA,
    DOT,
    DOCX,
    COMMA,
    DOT,
    XLS,
    COMMA,
    DOT,
    XLSX,
    COMMA,
    DOT,
    BMP,
    COMMA,
    DOT,
    GIF,
    COMMA,
    DOT,
    JPEG,
    COMMA,
    DOT,
    JPG,
    COMMA,
    DOT,
    PNG,
    COMMA,
    DOT,
    Z7Z,
    COMMA,
    DOT,
    RAR,
    COMMA,
    DOT,
    TAR,
    COMMA,
    DOT,
    ENC,
    COMMA,
    DOT,
    ZIP,
    COMMA,
    DOT,
    PDF,
    COMMA,
    DOT,
    PPT,
    COMMA,
    DOT,
    PPTX
  );
};

export const randomCode = (stringLength: number) => {
  //const chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz';
  const chars = '0123456789';
  let randomstring = STRING_EMPTY;
  for (let i = 0; i < stringLength; i++) {
    const rnum = Math.floor(Math.random() * chars.length);
    randomstring += chars.substring(rnum, rnum + 1);
  }
  return randomstring;
};

export function checkPermission(url: string, listMenu?: any[]): boolean {
  const listMenuLevel1 = listMenu?.filter((x) => x.children.length === 0);
  const listMenuLevel2 = listMenu?.flatMap((x) => x.children);
  const per = listMenuLevel1
    ?.concat(listMenuLevel2)
    .find((x) => SOURCE.concat(DOMAIN_ROUTER, SOURCE, x.metaLink) === url);
  if (!per) {
    return false;
  }
  return true;
}

export const permission = (url: string, listMenu?: any[]) => {
  const listMenuLevel1 = listMenu?.filter((x) => x.children.length === 0);
  const listMenuLevel2 = listMenu?.flatMap((x) => x.children);
  const per = listMenuLevel1
    ?.concat(listMenuLevel2)
    .find((x) => SOURCE.concat(DOMAIN_ROUTER, SOURCE, x.metaLink) === url);
  if (!per) {
    return undefined;
  }
  return {
    allowAccess: per.allowAccess,
    allowAdd: per.allowAdd,
    allowEdit: per.allowEdit,
    allowDelete: per.allowDelete,
    allowPrint: per.allowPrint,
    allowExport: per.allowExport,
    allowImport: per.allowImport,
  };
};

import b64toBlob from 'b64-to-blob';
import { saveAs } from 'file-saver';
export const downloadFile = (
  fileContent: any,
  contentType: any,
  fileName: any
) => {
  const blob = b64toBlob(fileContent, contentType);
  saveAs(blob, fileName);
};
