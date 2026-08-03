import { Module } from '@nestjs/common';
import { APP_FILTER } from '@nestjs/core';
import { PrismaModule } from './prisma/prisma.module';
import { CommonModule } from './common/common.module';
import { AllExceptionsFilter } from './common/filters/all-exceptions.filter';
import { UserModule } from './modules/user/user.module';
import { LinesModule } from './modules/transportation/lines/lines.module';
import { RoutesModule } from './modules/transportation/routes/routes.module';
import { StationsModule } from './modules/transportation/stations/stations.module';
import { PassengersModule } from './modules/transportation/passengers/passengers.module';
import { VehiclesModule } from './modules/transportation/vehicles/vehicles.module';
import { ShiftsModule } from './modules/transportation/shifts/shifts.module';
import { ExceptionsModule } from './modules/transportation/exceptions/exceptions.module';
import { DeductionsModule } from './modules/transportation/deductions/deductions.module';
import { RepricingModule } from './modules/transportation/repricing/repricing.module';
import { SuppliersModule } from './modules/transportation/suppliers/suppliers.module';
import { CostsModule } from './modules/transportation/costs/costs.module';
import { DashboardModule } from './modules/transportation/dashboard/dashboard.module';
import { ExcelModule } from './modules/transportation/excel/excel.module';
import { NotificationsModule } from './modules/transportation/notifications/notifications.module';

@Module({
  imports: [
    PrismaModule,
    CommonModule,
    UserModule,
    // Transportation modules:
    LinesModule,
    RoutesModule,
    StationsModule,
    PassengersModule,
    VehiclesModule,
    ShiftsModule,
    ExceptionsModule,
    DeductionsModule,
    RepricingModule,
    SuppliersModule,
    CostsModule,
    DashboardModule,
    ExcelModule,
    NotificationsModule,
  ],
  providers: [{ provide: APP_FILTER, useClass: AllExceptionsFilter }],
})
export class AppModule {}
