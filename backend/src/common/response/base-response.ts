// Response envelopes — faithful to the documented CoreApi contract (API.md §4).
// PascalCase keys are intentional (the old backend serialized PascalCase and the
// Flutter/Next clients depend on them byte-for-byte).

export interface ErrorItem {
  ErrorCode: string;
  ErrorMSG: string;
}

export interface PaginationHeader {
  CurrentPage: number;
  ItemsPerPage: number;
  TotalItems: number;
  TotalPages: number;
}

export interface BaseResponse {
  Result: boolean;
  Errors: ErrorItem[];
}

export interface BaseResponseWithData<T> extends BaseResponse {
  Data: T;
}

export interface BaseResponseWithDataAndHeader<T> extends BaseResponse {
  Data: T;
  PaginationHeader: PaginationHeader;
}

/** Write/ack response (mirrors PostResponseModel: adds Id + Message). */
export interface PostResponse extends BaseResponse {
  Id?: string | number;
  Message?: string;
}

export function success<T>(data: T): BaseResponseWithData<T> {
  return { Result: true, Errors: [], Data: data };
}

export function successList<T>(
  data: T,
  pagination: PaginationHeader,
): BaseResponseWithDataAndHeader<T> {
  return { Result: true, Errors: [], Data: data, PaginationHeader: pagination };
}

export function successWrite(id?: string | number, message?: string): PostResponse {
  return { Result: true, Errors: [], Id: id, Message: message };
}

export function fail(errorCode: string, errorMsg: string): BaseResponse {
  return { Result: false, Errors: [{ ErrorCode: errorCode, ErrorMSG: errorMsg }] };
}

export function paginationHeader(
  currentPage: number,
  itemsPerPage: number,
  totalItems: number,
): PaginationHeader {
  return {
    CurrentPage: currentPage,
    ItemsPerPage: itemsPerPage,
    TotalItems: totalItems,
    TotalPages: Math.max(1, Math.ceil(totalItems / Math.max(1, itemsPerPage))),
  };
}
