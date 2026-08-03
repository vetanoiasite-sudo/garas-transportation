import { ArgumentsHost, Catch, ExceptionFilter, HttpException, HttpStatus } from '@nestjs/common';
import { Response } from 'express';
import { fail } from '../response/base-response';

// Catch-all → returns the documented error envelope. Handled HttpExceptions
// (e.g. the auth guard throwing Err-P2/Err-P200 with status 200) pass their
// envelope through; anything else becomes Err10 with the raw message (the old
// backend leaked ex.Message under Err10 — kept faithful, but at HTTP 200).
@Catch()
export class AllExceptionsFilter implements ExceptionFilter {
  catch(exception: unknown, host: ArgumentsHost) {
    const res = host.switchToHttp().getResponse<Response>();

    if (exception instanceof HttpException) {
      const status = exception.getStatus();
      const body = exception.getResponse();
      // If the thrower already shaped an envelope, send it as-is.
      if (body && typeof body === 'object' && 'Result' in body) {
        return res.status(status === HttpStatus.OK ? 200 : status).json(body);
      }
      const msg = typeof body === 'string' ? body : (body as any)?.message ?? exception.message;
      return res.status(200).json(fail('Err10', String(msg)));
    }

    const message = exception instanceof Error ? exception.message : 'Unhandled error';
    return res.status(200).json(fail('Err10', message));
  }
}
