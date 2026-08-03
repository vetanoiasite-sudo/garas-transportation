import { Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { NotificationsService } from './notifications.service';

// Base path /api/Transportation; every action requires CompanyName + UserToken
// via the guard. Notifications belong to the calling user (populated only for
// super admins, who are the approval group).
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class NotificationsController {
  constructor(private readonly service: NotificationsService) {}

  @Get('GetNotifications')
  list(
    @CurrentUser() v: Validation,
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
  ) {
    return this.service.list(v.userId, Number(pageNo) || 1, Number(noOfItems) || 20);
  }

  @Get('GetUnreadNotificationsCount')
  unread(@CurrentUser() v: Validation) {
    return this.service.unreadCount(v.userId);
  }

  @Post('MarkNotificationRead')
  markRead(@CurrentUser() v: Validation, @Headers('id') id?: string) {
    return this.service.markRead(v.userId, Number(id));
  }

  @Post('MarkAllNotificationsRead')
  markAllRead(@CurrentUser() v: Validation) {
    return this.service.markAllRead(v.userId);
  }
}
