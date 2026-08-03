import { CanActivate, ExecutionContext, HttpException, HttpStatus, Injectable } from '@nestjs/common';
import { PrismaService } from '../../prisma/prisma.service';
import { decryptToken } from '../crypto/token.crypto';
import { fail } from '../response/base-response';

// Per-request validation — replicates the old ValidateHeader:
//   1. CompanyName header must be an allowed tenant code (single-tenant: one DB).
//   2. UserToken header decrypts to a session id; the session must be Active and
//      not expired. On failure returns the documented Err-P200 / Err-P2 envelope
//      at HTTP 200 (the Flutter/Next clients string-match these codes).
function envelope(code: string, msg: string): HttpException {
  return new HttpException(fail(code, msg), HttpStatus.OK);
}

export interface Validation {
  userId: number;
  companyName: string;
}

@Injectable()
export class HeaderAuthGuard implements CanActivate {
  constructor(private readonly prisma: PrismaService) {}

  async canActivate(context: ExecutionContext): Promise<boolean> {
    const req = context.switchToHttp().getRequest();
    const companyName = String(req.headers['companyname'] ?? '');
    const userToken = String(req.headers['usertoken'] ?? '');

    const allowed = (process.env.ALLOWED_COMPANIES ?? 'demo')
      .split(',')
      .map((s) => s.trim().toLowerCase());
    if (!companyName || !allowed.includes(companyName.toLowerCase())) {
      throw envelope('Err-P200', 'Invalid or unknown CompanyName');
    }
    if (!userToken) throw envelope('Err-P2', 'Invalid or expired UserToken');

    let sessionId: number;
    try {
      sessionId = Number(decryptToken(decodeURIComponent(userToken)));
    } catch {
      throw envelope('Err-P2', 'Invalid or expired UserToken');
    }
    if (!sessionId || Number.isNaN(sessionId)) {
      throw envelope('Err-P2', 'Invalid or expired UserToken');
    }

    const session = await this.prisma.userSession.findFirst({
      where: { id: sessionId, active: true, endDate: { gt: new Date() } },
    });
    if (!session) throw envelope('Err-P2', 'Invalid or expired UserToken');

    req.validation = { userId: session.userId, companyName } as Validation;
    return true;
  }
}
