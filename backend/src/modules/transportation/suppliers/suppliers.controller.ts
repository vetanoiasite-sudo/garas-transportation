import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { SuppliersService } from './suppliers.service';
import { decodeHeader } from '../../../common/http/header.util';

// Base path /api/Transportation (faithful to the documented URLs); every action
// requires the CompanyName + UserToken headers via the guard. Filters/pagination
// ride in HTTP headers (Node lowercases them), bodies are JSON — as documented (§6.12).
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class SuppliersController {
  constructor(private readonly service: SuppliersService) {}

  // NOTE: not in API.md — needed by the suppliers screen (list + create).
  @Get('getSuppliers')
  getSuppliers(
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
    @Headers('name') name?: string,
    @Headers('phone') phone?: string,
    @Headers('mobile') mobile?: string,
  ) {
    return this.service.getSuppliers(Number(pageNo) || 1, Number(noOfItems) || 20, {
      name: decodeHeader(name),
      phone: phone || undefined,
      mobile: mobile || undefined,
    });
  }

  @Post('addSupplier')
  addSupplier(@Body() body: SupplierBody) {
    return this.service.addSupplier(body);
  }

  // Single-supplier read/update (closes the frontend's mock fallback for the
  // supplier detail page). Not in API.md — added.
  @Get('getSupplier')
  getSupplier(@Headers('id') id?: string) {
    return this.service.getSupplier(Number(id));
  }

  @Post('updateSupplier')
  updateSupplier(@Body() body: SupplierBody & { Id: number }) {
    return this.service.updateSupplier(Number(body?.Id), body);
  }

  @Get('AccountsAllMonthsForSupplier')
  accountsAllMonths(
    @Headers('year') year?: string,
    @Headers('supplierid') supplierId?: string,
    @Headers('routeid') routeId?: string,
    @Headers('month') month?: string,
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
  ) {
    return this.service.accountsAllMonths(Number(pageNo) || 1, Number(noOfItems) || 20, {
      year: Number(year) || undefined,
      supplierId: Number(supplierId) || undefined,
      routeId: Number(routeId) || undefined,
      month: Number(month) || undefined,
    });
  }

  @Get('AccountsAllMonthsForSupplierExcel')
  accountsAllMonthsExcel(
    @Headers('year') year?: string,
    @Headers('supplierid') supplierId?: string,
    @Headers('routeid') routeId?: string,
    @Headers('month') month?: string,
  ) {
    return this.service.accountsAllMonthsExcel({
      year: Number(year) || undefined,
      supplierId: Number(supplierId) || undefined,
      routeId: Number(routeId) || undefined,
      month: Number(month) || undefined,
    });
  }

  @Get('AccountsAllRoundsForSupplier')
  accountsAllRounds(
    @Headers('year') year?: string,
    @Headers('month') month?: string,
    @Headers('supplierid') supplierId?: string,
    @Headers('routeid') routeId?: string,
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
  ) {
    return this.service.accountsAllRounds(Number(pageNo) || 1, Number(noOfItems) || 20, {
      year: Number(year) || undefined,
      month: Number(month) || undefined,
      supplierId: Number(supplierId) || undefined,
      routeId: Number(routeId) || undefined,
    });
  }

  @Get('getAllSupplierPayment')
  getAllSupplierPayment(
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
    @Headers('fromdate') fromDate?: string,
    @Headers('todate') toDate?: string,
    @Headers('supplierid') supplierId?: string,
  ) {
    return this.service.getAllSupplierPayment(Number(pageNo) || 1, Number(noOfItems) || 20, {
      fromDate: fromDate || undefined,
      toDate: toDate || undefined,
      supplierId: Number(supplierId) || undefined,
    });
  }

  @Post('AddSupplierPayment')
  addSupplierPayment(@Body() body: SupplierPaymentBody) {
    return this.service.addSupplierPayment(body);
  }
}

// Shape of the addSupplier payload (documented PascalCase keys).
export interface SupplierBody {
  Name?: string;
  Email?: string;
  Phone?: string;
  Mobile?: string;
  Fax?: string;
  Address?: string;
  // Contact persons: new ones have no Id, existing ones carry their Id.
  Contacts?: Array<{ Id?: number; Name?: string; Mobile?: string }>;
}

// Shape of the AddSupplierPayment payload (documented PascalCase keys, §6.12).
export interface SupplierPaymentBody {
  SupplierId?: number;
  Payment?: number;
  DatePayment?: string;
  StartDate?: string;
  TypeOfDebt?: string; // Normal | Advance
  NumberOfMonths?: number;
}
