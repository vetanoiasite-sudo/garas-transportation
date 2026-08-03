import { Module } from '@nestjs/common';
import { DeductionsController } from './deductions.controller';
import { DeductionsService } from './deductions.service';

@Module({
  controllers: [DeductionsController],
  providers: [DeductionsService],
})
export class DeductionsModule {}
