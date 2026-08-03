import { Module } from '@nestjs/common';
import { ExceptionsController } from './exceptions.controller';
import { ExceptionsService } from './exceptions.service';

@Module({
  controllers: [ExceptionsController],
  providers: [ExceptionsService],
})
export class ExceptionsModule {}
