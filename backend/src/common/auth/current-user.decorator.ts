import { createParamDecorator, ExecutionContext } from '@nestjs/common';
import { Validation } from './header-auth.guard';

/** Injects the validated { userId, companyName } from the auth guard. */
export const CurrentUser = createParamDecorator(
  (_data: unknown, ctx: ExecutionContext): Validation => {
    return ctx.switchToHttp().getRequest().validation;
  },
);
