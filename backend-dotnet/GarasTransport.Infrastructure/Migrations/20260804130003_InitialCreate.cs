using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarasTransport.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftNumber = table.Column<int>(type: "int", nullable: false),
                    WeekDayId = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaritalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaritalStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    FromUserId = table.Column<int>(type: "int", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportationLineIncreaseRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPercent = table.Column<bool>(type: "bit", nullable: false),
                    IncreaseCost = table.Column<double>(type: "float", nullable: false),
                    ForAllLines = table.Column<bool>(type: "bit", nullable: false),
                    ApproximateToFiveFlag = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Approve = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationLineIncreaseRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportationLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OneWayDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckInRouteId = table.Column<int>(type: "int", nullable: true),
                    CheckOutRouteId = table.Column<int>(type: "int", nullable: true),
                    CheckInLatitude = table.Column<double>(type: "float", nullable: true),
                    CheckInLongitude = table.Column<double>(type: "float", nullable: true),
                    CheckOutLatitude = table.Column<double>(type: "float", nullable: true),
                    CheckOutLongitude = table.Column<double>(type: "float", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttendances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierContactPersons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierContactPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierContactPersons_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Payment = table.Column<double>(type: "float", nullable: false),
                    DatePayment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberOfMonths = table.Column<int>(type: "int", nullable: true),
                    TypeOfDebt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierPayments_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HrUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatusId = table.Column<int>(type: "int", nullable: true),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HrUsers_MaritalStatuses_MaritalStatusId",
                        column: x => x.MaritalStatusId,
                        principalTable: "MaritalStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HrUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportationVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationVehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DistributionSupplierPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    Payment = table.Column<double>(type: "float", nullable: false),
                    MonthNum = table.Column<int>(type: "int", nullable: false),
                    YearNum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionSupplierPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributionSupplierPayments_SupplierPayments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "SupplierPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportationVehicleRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransportationLineId = table.Column<int>(type: "int", nullable: false),
                    TransportationVehicleId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    SupplierContactPersonId = table.Column<int>(type: "int", nullable: true),
                    BranchScheduleId = table.Column<int>(type: "int", nullable: true),
                    SupervisorId = table.Column<int>(type: "int", nullable: true),
                    Serial = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PeriodFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FromTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOfRoute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineCost = table.Column<double>(type: "float", nullable: false),
                    OneWay = table.Column<bool>(type: "bit", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationVehicleRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRoutes_BranchSchedules_BranchScheduleId",
                        column: x => x.BranchScheduleId,
                        principalTable: "BranchSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRoutes_SupplierContactPersons_SupplierContactPersonId",
                        column: x => x.SupplierContactPersonId,
                        principalTable: "SupplierContactPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRoutes_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRoutes_TransportationLines_TransportationLineId",
                        column: x => x.TransportationLineId,
                        principalTable: "TransportationLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRoutes_TransportationVehicles_TransportationVehicleId",
                        column: x => x.TransportationVehicleId,
                        principalTable: "TransportationVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRoutes_Users_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportationLineIncreaseRequestLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    PriceBefore = table.Column<double>(type: "float", nullable: false),
                    PriceAfter = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationLineIncreaseRequestLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationLineIncreaseRequestLines_TransportationLineIncreaseRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "TransportationLineIncreaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationLineIncreaseRequestLines_TransportationVehicleRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportationVehicleRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportationRoundForRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    Serial = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OneWayDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PriceRound = table.Column<double>(type: "float", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationRoundForRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationRoundForRoutes_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationRoundForRoutes_TransportationVehicleRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportationVehicleRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportationVehicleRouteAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    Serial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfMonth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CountOfRounds = table.Column<double>(type: "float", nullable: false),
                    PriceOfRounds = table.Column<double>(type: "float", nullable: true),
                    DeductPerRounds = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationVehicleRouteAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteAccounts_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteAccounts_TransportationVehicleRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportationVehicleRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportationVehicleRouteDeductions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfDeduction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeductPerRound = table.Column<double>(type: "float", nullable: false),
                    Cause = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfDedct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoutePrice = table.Column<double>(type: "float", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationVehicleRouteDeductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteDeductions_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteDeductions_TransportationVehicleRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportationVehicleRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportationVehicleRouteDirections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    RouteDirection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationVehicleRouteDirections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteDirections_TransportationVehicleRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportationVehicleRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportationVehicleRouteEmployeeExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HrUserId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    DirectionId = table.Column<int>(type: "int", nullable: true),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    ReasonException = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationVehicleRouteEmployeeExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteEmployeeExceptions_HrUsers_HrUserId",
                        column: x => x.HrUserId,
                        principalTable: "HrUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteEmployeeExceptions_TransportationVehicleRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportationVehicleRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportionLineExceptionPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportionLineExceptionPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportionLineExceptionPrices_TransportationVehicleRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportationVehicleRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportationVehicleRouteEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    HrUserId = table.Column<int>(type: "int", nullable: false),
                    DirectionId = table.Column<int>(type: "int", nullable: true),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationLatitude = table.Column<double>(type: "float", nullable: true),
                    DurationLongitude = table.Column<double>(type: "float", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationVehicleRouteEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteEmployees_HrUsers_HrUserId",
                        column: x => x.HrUserId,
                        principalTable: "HrUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteEmployees_TransportationVehicleRouteDirections_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "TransportationVehicleRouteDirections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransportationVehicleRouteEmployees_TransportationVehicleRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "TransportationVehicleRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchSchedules_BranchId",
                table: "BranchSchedules",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionSupplierPayments_PaymentId",
                table: "DistributionSupplierPayments",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_HrUsers_MaritalStatusId",
                table: "HrUsers",
                column: "MaritalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_HrUsers_UserId",
                table: "HrUsers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierContactPersons_SupplierId",
                table: "SupplierContactPersons",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPayments_SupplierId",
                table: "SupplierPayments",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationLineIncreaseRequestLines_RequestId",
                table: "TransportationLineIncreaseRequestLines",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationLineIncreaseRequestLines_RouteId",
                table: "TransportationLineIncreaseRequestLines",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationRoundForRoutes_RouteId",
                table: "TransportationRoundForRoutes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationRoundForRoutes_Serial",
                table: "TransportationRoundForRoutes",
                column: "Serial");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationRoundForRoutes_SupplierId",
                table: "TransportationRoundForRoutes",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteAccounts_RouteId",
                table: "TransportationVehicleRouteAccounts",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteAccounts_SupplierId",
                table: "TransportationVehicleRouteAccounts",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteDeductions_RouteId",
                table: "TransportationVehicleRouteDeductions",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteDeductions_SupplierId",
                table: "TransportationVehicleRouteDeductions",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteDirections_RouteId",
                table: "TransportationVehicleRouteDirections",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteEmployeeExceptions_HrUserId",
                table: "TransportationVehicleRouteEmployeeExceptions",
                column: "HrUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteEmployeeExceptions_RouteId",
                table: "TransportationVehicleRouteEmployeeExceptions",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteEmployees_DirectionId",
                table: "TransportationVehicleRouteEmployees",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteEmployees_HrUserId",
                table: "TransportationVehicleRouteEmployees",
                column: "HrUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRouteEmployees_RouteId",
                table: "TransportationVehicleRouteEmployees",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRoutes_BranchScheduleId",
                table: "TransportationVehicleRoutes",
                column: "BranchScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRoutes_Serial",
                table: "TransportationVehicleRoutes",
                column: "Serial");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRoutes_SupervisorId",
                table: "TransportationVehicleRoutes",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRoutes_SupplierContactPersonId",
                table: "TransportationVehicleRoutes",
                column: "SupplierContactPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRoutes_SupplierId",
                table: "TransportationVehicleRoutes",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRoutes_TransportationLineId",
                table: "TransportationVehicleRoutes",
                column: "TransportationLineId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicleRoutes_TransportationVehicleId",
                table: "TransportationVehicleRoutes",
                column: "TransportationVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationVehicles_VehicleTypeId",
                table: "TransportationVehicles",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportionLineExceptionPrices_RouteId",
                table: "TransportionLineExceptionPrices",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAttendances_Serial",
                table: "UserAttendances",
                column: "Serial");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributionSupplierPayments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "TransportationLineIncreaseRequestLines");

            migrationBuilder.DropTable(
                name: "TransportationRoundForRoutes");

            migrationBuilder.DropTable(
                name: "TransportationVehicleRouteAccounts");

            migrationBuilder.DropTable(
                name: "TransportationVehicleRouteDeductions");

            migrationBuilder.DropTable(
                name: "TransportationVehicleRouteEmployeeExceptions");

            migrationBuilder.DropTable(
                name: "TransportationVehicleRouteEmployees");

            migrationBuilder.DropTable(
                name: "TransportionLineExceptionPrices");

            migrationBuilder.DropTable(
                name: "UserAttendances");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "SupplierPayments");

            migrationBuilder.DropTable(
                name: "TransportationLineIncreaseRequests");

            migrationBuilder.DropTable(
                name: "HrUsers");

            migrationBuilder.DropTable(
                name: "TransportationVehicleRouteDirections");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "MaritalStatuses");

            migrationBuilder.DropTable(
                name: "TransportationVehicleRoutes");

            migrationBuilder.DropTable(
                name: "BranchSchedules");

            migrationBuilder.DropTable(
                name: "SupplierContactPersons");

            migrationBuilder.DropTable(
                name: "TransportationLines");

            migrationBuilder.DropTable(
                name: "TransportationVehicles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "VehicleTypes");
        }
    }
}
