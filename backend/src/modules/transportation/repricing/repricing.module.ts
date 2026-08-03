import { Module } from '@nestjs/common';
import { RepricingController } from './repricing.controller';
import { RepricingService } from './repricing.service';

@Module({
  controllers: [RepricingController],
  providers: [RepricingService],
})
export class RepricingModule {}
