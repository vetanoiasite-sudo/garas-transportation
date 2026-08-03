import { Module } from '@nestjs/common';
import { LinesController } from './lines.controller';
import { LinesService } from './lines.service';

@Module({
  controllers: [LinesController],
  providers: [LinesService],
})
export class LinesModule {}
