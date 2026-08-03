import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../common/auth/header-auth.guard';
import { CurrentUser } from '../../common/auth/current-user.decorator';
import { UserService, UserAdminDto } from './user.service';
import { LoginDto } from './dto/login.dto';

// Route base is /User (no api/ prefix — matches the documented contract).
@Controller('User')
export class UserController {
  constructor(private readonly service: UserService) {}

  @Post('Login')
  login(@Body() dto: LoginDto) {
    return this.service.login(dto);
  }

  @Post('Logout')
  logout(@Headers('usertoken') token?: string) {
    return this.service.logout(token);
  }

  @Get('GetUserData')
  @UseGuards(HeaderAuthGuard)
  getUserData(@CurrentUser() v: Validation) {
    return this.service.getUserData(v.userId);
  }

  /** GET /User/GetAllUsers — login users (with roles) for supervisor pickers. */
  @Get('GetAllUsers')
  @UseGuards(HeaderAuthGuard)
  getAllUsers() {
    return this.service.getAllUsers();
  }

  /** GET /User/GetUserList — paginated users for the Users admin screen. */
  @Get('GetUserList')
  @UseGuards(HeaderAuthGuard)
  getUserList(
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
    @Headers('roleid') roleId?: string,
  ) {
    return this.service.getUserList(Number(pageNo) || 1, Number(noOfItems) || 20, Number(roleId) || undefined);
  }

  /** POST /User/AddUser — create a login user with a role. */
  @Post('AddUser')
  @UseGuards(HeaderAuthGuard)
  addUser(@Body() body: UserAdminDto) {
    return this.service.addUser(body);
  }

  /** POST /User/UpdateUser — edit a login user's profile, role and status. */
  @Post('UpdateUser')
  @UseGuards(HeaderAuthGuard)
  updateUser(@Body() body: UserAdminDto) {
    return this.service.updateUser(body);
  }

  /** POST /User/DeleteUser — deactivate a login user (Id header). */
  @Post('DeleteUser')
  @UseGuards(HeaderAuthGuard)
  deleteUser(@Headers('id') id?: string) {
    return this.service.deleteUser(Number(id));
  }
}
