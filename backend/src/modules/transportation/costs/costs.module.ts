import { Module } from '@nestjs/common';
import { CostsController } from './costs.controller';
import { CostsService } from './costs.service';

@Module({
  controllers: [CostsController],
  providers: [CostsService],
})
export class CostsModule {}
