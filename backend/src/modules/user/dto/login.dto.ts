// POST /User/Login body (UserLogin). Field names match the documented contract.
export class LoginDto {
  Email!: string;
  Password?: string;
  CompanyName!: string;
  ExternalLoginFrom?: string;
}
