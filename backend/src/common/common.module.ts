import { Global, Module } from '@nestjs/common';
import { HeaderAuthGuard } from './auth/header-auth.guard';

// Global so any controller can @UseGuards(HeaderAuthGuard).
@Global()
@Module({
  providers: [HeaderAuthGuard],
  exports: [HeaderAuthGuard],
})
export class CommonModule {}
