using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NewGaras.Domain.Interfaces.Repositories;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Interfaces.ServicesInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Task = NewGaras.Infrastructure.Entities.Task;

namespace NewGaras.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<User, long> Users { get; }
        IBaseRepository<UserSession, long> UserSessions { get; }
        //IBaseRepository<UserDeviceConnection, long> UserDeviceConnections { get; }
        /*IInventoryItemRepository InventoryItems { get; }
        IDashboardManagementRepository DashboardManagement { get; }
        IMatrialRequestRepository MatrialRequests { get; }*/
        IHrUserRepository HrUsers { get; }
        IBaseRepository<Salary, long> Salaries { get; }
        IBaseRepository<ContractDetail, long> Contracts { get; }
        IBaseRepository<Branch, int> Branches { get; }
        IBaseRepository<Department, int> Departments { get; }
        IBaseRepository<Team, long> Teams { get; }
        IBaseRepository<UserTeam, int> UserTeams { get; }

        IBaseRepository<SystemLog, int> SystemLogs { get; }
        IBaseRepository<Currency, int> Currencies { get; }

        IBaseRepository<PaymentStrategy, int> PaymentStrategies { get; }

        IBaseRepository<JobTitle, int> JobTitles { get; }

        IBaseRepository<WeekDay, int> WeekDays { get; }

        IBaseRepository<BranchSchedule, long> BranchSchedules { get; }
        IBaseRepository<AllowncesType, int> AllowncesTypes { get; }

        IBaseRepository<SalaryAllownce, int> SalaryAllownces { get; }

        IBaseRepository<SalaryTax, long> SalaryTaxs { get; }
        IBaseRepository<OffDay, long> OffDays { get; }
        IBaseRepository<ChequeCashingStatus,int> ChequeCashingStatuses { get; }
        IBaseRepository<ProgressType,int> ProgressTypes { get; }
        IBaseRepository<DeliveryType,int> DeliveryTypes { get; }
        IBaseRepository<ProgressStatus, int> ProgressStatuses { get; }
        IBaseRepository<ProjectProgress, long> ProjectProgresses { get; }
        IBaseRepository<InsuranceCompanyName, long> InsuranceCompanyNames { get; }
        IBaseRepository<InventoryStore, int> InventoryStores { get; }
        IBaseRepository<Entities.InventoryItem, long> InventoryItems { get; }
        IBaseRepository<InventoryStoreItem, long> InventoryStoreItems { get; }
        IBaseRepository<InventoryMatrialRequest, long> InventoryMatrialRequests { get; }
        IBaseRepository<InventoryMatrialRelease, long> InventoryMatrialReleases { get; }
        IBaseRepository<InventoryMatrialRequestItem, long> InventoryMatrialRequestItems { get; }
        IBaseRepository<InventoryMatrialReleaseItem, long> InventoryMatrialReleaseItems { get; }
        IBaseRepository<UserPatient, long> UserPatients { get; }
        IBaseRepository<InventoryStoreKeeper, int> InventoryStoreKeepers { get; }
        IBaseRepository<UserPatientInsurance, long> UserPatientInsurances { get; }
        IBaseRepository<VInventoryMatrialReleaseItem, int> VInventoryMatrialReleaseItems { get; }
        IBaseRepository<VInventoryMatrialRequestItem, int> VInventoryMatrialRequestItems { get; }
        IBaseRepository<Country, int> Countries { get; }
        IBaseRepository<Account, long> Accounts { get; }
        IBaseRepository<AccountOfJournalEntry, long> AccountOfJournalEntries { get; }
        IBaseRepository<VProjectSalesOfferClient, long> VProjectSalesOfferClients { get; }
        IBaseRepository<SupplierAccount, long> SupplierAccounts { get; }
        IBaseRepository<VPurchasePo, long> VPurchasePos { get; }
        IBaseRepository<VAccountOfJournalEntryWithDaily, long> VAccountOfJournalEntryWithDailies { get; }
        IBaseRepository<DailyJournalEntryReverse, long> DailyJournalEntryReverses { get; }
        IBaseRepository<VDailyJournalEntry, long> VDailyJournalEntries { get; }
        IBaseRepository<PurchasePo, long> PurchasePos { get; }
        IBaseRepository<VProjectSalesOffer, long> VProjectSalesOffers { get; }
        IBaseRepository<DailyAdjustingEntryCostCenter, long> DailyAdjustingEntryCostCenters { get; }
        IBaseRepository<GeneralActiveCostCenter, long> GeneralActiveCostCenters { get; }
        IBaseRepository<DailyTranactionBeneficiaryToUser, long> DailyTranactionBeneficiaryToUsers { get; }
        IBaseRepository<Supplier, long> Suppliers { get; }
        IBaseRepository<AccountFinancialPeriod, int> AccountFinancialPeriods { get; }
        IBaseRepository<AccountOfJournalEntryOtherCurrency, long> AccountOfJournalEntryOtherCurrencies { get; }
        IBaseRepository<InvoiceCnandDn, long> InvoiceCnandDns { get; }
        IBaseRepository<VAccount, long> VAccounts { get; }
        IBaseRepository<AccountCategory, long> AccountCategories { get; }
        IBaseRepository<PurchasePaymentMethod, long> PurchasePaymentMethods { get; }
        IBaseRepository<AdvanciedType, long> AdvanciedTypes { get; }
        IBaseRepository<VInventoryStoreItem, long> VInventoryStoreItems { get; }
        IBaseRepository<InvoiceItem, long> InvoiceItems { get; }
        IBaseRepository<VSalesOfferProduct, long> VSalesOfferProducts { get; }
        IBaseRepository<SalesOfferProductTax, long> SalesOfferProductTaxes { get; }
        IBaseRepository<VInventoryItem, long> VInventoryItems { get; }
        IBaseRepository<PosClosingDay, long> PosClosingDays { get; }
        IBaseRepository<VehiclePerClient, long> VehiclePerClients { get; }
        IBaseRepository<CompanySerialIdentification, int> CompanySerialIdentifications { get; }
        IBaseRepository<VClientMobile, long> VClientMobiles { get; }
        IBaseRepository<ClientContactPerson, long> ClientContactPeople { get; }
        IBaseRepository<VClientPhone, long> VClientPhones { get; }
        IBaseRepository<ClientFax, long> ClientFaxes { get; }
        IBaseRepository<VClientSpeciality, long> VClientSpecialities { get; }
        IBaseRepository<VClientAddress, long> VClientAddresses { get; }
        IBaseRepository<VClientUseer, long> VClientUseers { get; }
        IBaseRepository<VVehicle, long> VVehicles { get; }
        IBaseRepository<VClientExpired, long> VClientExpiredes { get; }
        IBaseRepository<CompanySpecialty, int> CompanySpecialties { get; }
        IBaseRepository<VUserRole, long> VUserRoles { get; }
        IBaseRepository<InventoryMaterialRequestType, long> InventoryMaterialRequestTypes { get; }
        IBaseRepository<ProjectFabrication, long> ProjectFabrications { get; }
        IBaseRepository<ProjectFabricationBoq, long> ProjectFabricationBOQs { get; }
        IBaseRepository<VInventoryMatrialRequest, long> VInventoryMatrialRequests { get; }
        IBaseRepository<VPurchaseRequestItem, long> VPurchaseRequestItems { get; }
        IBaseRepository<VInventoryStoreKeeper, long> VInventoryStoreKeepers { get; }
        IBaseRepository<VProjectFabricationTotalHour, long> VProjectFabricationTotalHours { get; }
        IBaseRepository<ProjectInstallation, long> ProjectInstallations { get; }
        IBaseRepository<ProjectInstallationReport, long> ProjectInstallationReports { get; }
        IBaseRepository<ProjectFinishInstallationAttachment, long> ProjectFinishInstallationAttachments { get; }
        IBaseRepository<VProjectInstallationTotalHour, long> VProjectInstallationTotalHours { get; }
        IBaseRepository<Bompartition, long> Bompartitions { get; }
        IBaseRepository<BompartitionItem, long> BompartitionItems { get; }
        IBaseRepository<ClientAttachment, long> ClientAttachments { get; }
        IBaseRepository<InventoryInternalTransferOrder, long> InventoryInternalTransferOrders { get; }
        IBaseRepository<InventoryInternalTransferOrderItem, long> InventoryInternalTransferOrderItems { get; }
        IBaseRepository<VInventoryInternalTransferOrder, long> VInventoryInternalTransferOrders { get; }
        IBaseRepository<VInventoryInternalBackOrder, long> VInventoryInternalBackOrders { get; }
        IBaseRepository<InventoryInternalBackOrderItem, long> InventoryInternalBackOrderItems { get; }
        IBaseRepository<VInventoryMatrialRelease, long> VInventoryMatrialReleases { get; }
        IBaseRepository<VInventoryInternalBackOrderItem, long> VInventoryInternalBackOrderItems { get; }
        IBaseRepository<VInventoryInternalTransferOrderItem, long> VInventoryInternalTransferOrderItems { get; }
        IBaseRepository<InventoryReportItem, long> InventoryReportItems { get; }
        IBaseRepository<InventoryReportItemParent, long> InventoryReportItemParents { get; }
        IBaseRepository<Hrcustody, long> Hrcustodies { get; }
        IBaseRepository<PurchasePoinvoice, long> PurchasePoinvoices { get; }
        IBaseRepository<InventoryMatrialReleasePrintInfo, long> InventoryMatrialReleasePrintInfoes { get; }
        IBaseRepository<SalesRentOffer, long> SalesRentOffers { get; }
        IBaseRepository<SalesOfferInternalApproval, long> SalesOfferInternalApprovals { get; }
        IBaseRepository<MaintenanceReport, long> MaintenanceReports { get; }
        IBaseRepository<MaintenanceReportUser, long> MaintenanceReportUsers { get; }
        IBaseRepository<MaintenanceReportExpense, long> MaintenanceReportExpenses { get; }
        IBaseRepository<VehicleModel, int> VehicleModels { get; }
        IBaseRepository<VehicleMaintenanceType, long> VehicleMaintenanceTypes { get; }
        IBaseRepository<VehicleMaintenanceTypeForModel, long> VehicleMaintenanceTypeForModels { get; }
        IBaseRepository<VMaintenanceForDetail, long> VMaintenanceForDetails { get; }
        IBaseRepository<VisitsScheduleOfMaintenanceAttachment, long> VisitsScheduleOfMaintenanceAttachments { get; }
        IBaseRepository<TaskUserMonitor, long> TaskUserMonitors { get; }
        IBaseRepository<SalesTarget, int> SalesTargets { get; }
        IBaseRepository<VSalesOfferClient, long> VSalesOfferClients { get; }
        IBaseRepository<VDailyReportReportLineThroughApi, long> VDailyReportReportLineThroughApis { get; }
        IBaseRepository<Crmreport, long> Crmreports { get; }
        IBaseRepository<DailyReportExpense, long> DailyReportExpenses { get; }
        IBaseRepository<CrmcontactType, int> CrmcontactTypes { get; }
        IBaseRepository<CrmrecievedType, int> CrmrecievedTypes { get; }
        IBaseRepository<CrmreportReason, int> CrmreportReasons { get; }
        IBaseRepository<DailyReport, long> DailyReports { get; }
        IBaseRepository<VSalesOffer, long> VSalesOffers { get; }
        IBaseRepository<BankChequeTemplate, int> BankChequeTemplates { get; }
        IBaseRepository<CategoryType, int> CategoryTypes { get; }
        IBaseRepository<VehicleBrand, int> VehicleBrands { get; }
        IBaseRepository<VehicleBodyType, int> VehicleBodyTypes { get; }
        IBaseRepository<VehicleServiceScheduleCategory, long> VehicleServiceScheduleCategories { get; }
        IBaseRepository<SalesBranchTarget, long> SalesBranchTargets { get; }
        IBaseRepository<SalesBranchProductTarget, long> SalesBranchProductTargets { get; }
        IBaseRepository<VehicleColor, int> VehicleColors { get; }
        IBaseRepository<VehicleTransmission, int> VehicleTransmissions { get; }
        IBaseRepository<VehicleWheelsDrive, int> VehicleWheelsDrives { get; }
        IBaseRepository<EinvoiceSetting, int> EinvoiceSettings { get; }
        IBaseRepository<EinvoiceAttachment, long> EinvoiceAttachments { get; }
        IBaseRepository<EinvoiceCompanyActivity, long> EinvoiceCompanyActivities { get; }
        IBaseRepository<NotificationProcess, int> NotificationProcesses { get; }
        IBaseRepository<VBranchProduct, long> VBranchProducts { get; }
        IBaseRepository<VUserBranchGroup, long> VUserBranchGroups { get; }
        IBaseRepository<VSalesBranchUserTargetTargetYear, int> VSalesBranchUserTargetTargetYears { get; }
        IBaseRepository<BundleModule, long> BundleModules { get; }
        IBaseRepository<SupportedBy, long> SupportedBies { get; }
        IBaseRepository<ClientSpeciality, long> ClientSpecialities { get; }
        IBaseRepository<SupplierMobile, long> SupplierMobiles { get; }
        IBaseRepository<SupplierFax, long> SupplierFaxes { get; }
        IBaseRepository<SupplierSpeciality, long> SupplierSpecialities { get; }
        IBaseRepository<SupplierPhone, long> SupplierPhones { get; }
        IBaseRepository<SupplierPaymentTerm, long> SupplierPaymentTerms { get; }
        IBaseRepository<SupplierBankAccount, long> SupplierBankAccounts { get; }
        IBaseRepository<VSupplierSpeciality, long> VSupplierSpecialities { get; }
        IBaseRepository<VSupplierAddress, long> VSupplierAddresses { get; }
        IBaseRepository<SupplierContactPerson, long> SupplierContactPeople { get; }
        IBaseRepository<SupplierAttachment, long> SupplierAttachments { get; }
        IBaseRepository<VSupplier, long> VSuppliers { get; }
        IBaseRepository<PurchasePoitem, long> PurchasePoitems { get; }
        IBaseRepository<InventoryItemAttachment, long> InventoryItemAttachments { get; }
        IBaseRepository<VPrsupplierOfferItem, long> VPrsupplierOfferItems { get; }
        IBaseRepository<VClientConsultantAddress, long> VClientConsultantAddresses { get; }
        IBaseRepository<ClientConsultantFax, long> ClientConsultantFaxes { get; }
        IBaseRepository<VNotificationsDetail, long> VNotificationsDetails { get; }
        IBaseRepository<ProjectProgressUser, long> ProjectProgressUsers { get; }
        IBaseRepository<DoctorRoom, long> DoctorRooms { get; }
        IBaseRepository<PosNumber, int> PosNumbers { get; }

        /*IBaseRepository<HrContactInfo, int> ContactInfos { get; }
        IBaseRepository<HrUserAddress, int> UserAddresses { get; }
        IBaseRepository<HrNationality, int> HrNationalities { get; }
        IBaseRepository<HrMilitaryStatus, int> HrMilitaryStatuses { get; }
        IBaseRepository<HrMaritalStatus, int> HrMaritalStatuses { get; }
        IBaseRepository<HrBranch, int> HrBranches { get; }
        IBaseRepository<HrBranchContactInfo, int> BranchContactInfos { get; }
        IBaseRepository<HrBranchAddress, int> BranchAddresses { get; }
        IBaseRepository<HrGrade, int> Grades { get; }
        IBaseRepository<HrBranchDepartment, int> BranchDepartments { get; }
        IBaseRepository<HrTeam, long> Teams { get; }
        IBaseRepository<HrUserTeam, long> UserTeams { get; }
        IBaseRepository<HrJobTitle, int> JobTitles { get; }
        IBaseRepository<HrJobDescription, int> JobDescription { get; }
        IBaseRepository<HrJobDescriptionAttachment, int> JobDescriptionAttachment { get; }
        IBaseRepository<HrJobResponsabillity, int> JobResponsabillity { get; }
        IBaseRepository<HrJobResponsbillityAttachment, int> JobResponsbillityAttachment { get; }
        IBaseRepository<HrWorkInstruction, int> WorkInstruction { get; }
        IBaseRepository<HrWorkInstructionAttachment, int> WorkInstructionAttachment { get; }
        IBaseRepository<HrSkill, int> Skill { get; }
        IBaseRepository<HrSkillAttachment, int> SkillAttachment { get; }
        IBaseRepository<HrInterviewKpi, int> InterviewKpi { get; }
        IBaseRepository<HrOperationKpi, int> OperationKpi { get; }
        IBaseRepository<HrJobTitleDepartment, int> JobTitleDepartment { get; }*/
        IBaseRepository<Country, int> Country { get; }
        IBaseRepository<City, int> City { get; }
        IBaseRepository<Area, int> Area { get; }
        IBaseRepository<ContractLeaveSetting, int> ContractLeaveSetting { get; }
        IBaseRepository<DeductionType, int> DeductionTypes { get; }
        public IBaseRepository<SalaryDeductionTax, long> SalaryDeductionTaxs { get; }
        IBaseRepository<Attendance, long> Attendances { get; }

        IBaseRepository<BranchSetting, long> BranchSetting { get; }
        IBaseRepository<OverTimeAndDeductionRate, long> OverTimeAndDeductionRates { get; }

        IBaseRepository<VacationOverTimeAndDeductionRate, long> VacationOverTimeAndDeductionRates { get; }
        IBaseRepository<VacationDay, long> VacationDays { get; }

        IBaseRepository<WorkingHourseTracking, long> WorkingHoursTrackings { get; }
        IBaseRepository<VehicleMaintenanceJobOrderHistory, long> VehicleMaintenanceJobOrderHistories { get; }
        IBaseRepository<DayType, int> DayTypes { get; }
        IBaseRepository<SalaryType, int> SalaryTypes { get; }

        IBaseRepository<TaxType, int> TaxTypes { get; }

        IBaseRepository<Payroll, long> Payrolls { get; }

        IBaseRepository<ContractLeaveEmployee, long> ContractLeaveEmployees { get; }

        IBaseRepository<LeaveRequest, long> LeaveRequests { get; }
        IBaseRepository<ProjectInvoice, long> ProjectInvoices { get; }
        IBaseRepository<ProjectInvoiceItem, long> ProjectInvoiceItems { get; }

        IBaseRepository<ContractReportTo, long> ContractReportTos { get; }
        IBaseRepository<AdvanciedSettingAccount, long> AdvanciedSettingAccounts { get; }
        IBaseRepository<ProjectCheque, long> ProjectCheques { get; }
        IBaseRepository<VisitsScheduleOfMaintenance, long> VisitsScheduleOfMaintenances { get; }
        IBaseRepository<SalesOfferLocation, long> SalesOfferLocations { get; }
        IBaseRepository<VVisitsMaintenanceReportUser, long> VVisitsMaintenanceReportUsers { get; }
        IBaseRepository<InventoryItemCategory, int> InventoryItemCategories { get; }
        IBaseRepository<MaintenanceFor, long> MaintenanceFors { get; }
        IBaseRepository<SalesOfferProduct, long> SalesOfferProducts { get; }
        IBaseRepository<SalesMaintenanceOffer, long> SalesMaintenanceOffers { get; }
        IBaseRepository<ManagementOfMaintenanceOrder, long> ManagementOfMaintenanceOrders { get; }
        IBaseRepository<SalesOfferDiscount, long> SalesOfferDiscounts { get; }
        IBaseRepository<InvoiceExtraModification, long> InvoiceExtraModifications { get; }
        IBaseRepository<VSalesOfferExtraCost, long> VSalesOfferExtraCosts { get; }
        IBaseRepository<SalesOfferAttachment, long> SalesOfferAttachments { get; }
        IBaseRepository<SalesOfferItemAttachment, long> SalesOfferItemAttachments { get; }
        IBaseRepository<SalesOfferInvoiceTax, long> SalesOfferInvoiceTaxes { get; }
        IBaseRepository<SalesOfferExtraCost, long> SalesOfferExtraCosts { get; }
        IBaseRepository<Bom, long> Boms { get; }
        IBaseRepository<VBompartitionItemsInventoryItem, long> VBompartitionItemsInventoryItems { get; }
        IBaseRepository<VSalesOfferProductSalesOffer, long> VSalesOfferProductSalesOffers { get; }
        IBaseRepository<InventoryInternalBackOrder, long> InventoryInternalBackOrders { get; }
        IBaseRepository<PermissionLevel, int> PermissionLevels { get; }
        IBaseRepository<VMatrialReleaseSalesOffer, long> VMatrialReleaseSalesOffers { get; }
        IBaseRepository<VInvoice, long> VInvoices { get; }
        /*IBaseRepository<HrWeekDay, int> HrWeekDay { get; }
        IBaseRepository<Shift, int> Shift { get; }
        IBaseRepository<Vacation, int> Vacation { get; }
        IBaseRepository<OvertimeRates, int> OvertimeRates { get; }
        IBaseRepository<DeductionRates, int> DeductionRates { get; }
        IBaseRepository<AttendenceNPayrollSettings, int> AttendenceNPayrollSettings { get; }*/




        IBaseRepository<TaskRequirement, long> TaskRequirements { get; }
        IBaseRepository<VPurchaseRequestItemsPo, long> VPurchaseRequestItemsPos { get; }

        //-------------------------------------Patrick--------------------------------------
        IBaseRepository<Task, long> Tasks { get; }
        IBaseRepository<ExpensisType, long> ExpensisTypes { get; }
        IBaseRepository<TaskExpensi, long> TaskExpensis { get; }
        IBaseRepository<Client, long> Clients { get; }
        IBaseRepository<ProjectContactPerson, long> ProjectContactPersons { get; }
        IBaseRepository<SalesOffer, long> SalesOffers { get; }
        IBaseRepository<Priority, int> Prioritys { get; }
        IBaseRepository<Project, long> Projects { get; }
        IBaseRepository<ProjectAttachment, long> ProjectAttachments { get; }
        IBaseRepository<Governorate, int> Governorates { get; }
        IBaseRepository<Area, long> Areas { get; }
        IBaseRepository<ProjectWorkFlow, long> Workflows { get; }
        IBaseRepository<ProjectSprint, long> ProjectSprints { get; }
        IBaseRepository<ProjectCostingType, int> ProjectCostTypes { get; }
        IBaseRepository<BillingType, int> Billingtypes { get; }
        IBaseRepository<UserRole, long> UserRoles { get; }
        IBaseRepository<ProjectAssignUser, long> ProjectAssignUsers { get; }
        IBaseRepository<BankDetail, int> BankDetails { get; }
        IBaseRepository<PaymentMethod, int> PaymentMethods { get; }
        IBaseRepository<InventoryUom, int> InventoryUoms { get; }
        IBaseRepository<TaskUnitRateService, long> TaskUnitRateServices { get; }
        IBaseRepository<ProjectInvoiceCollected, long> ProjectInvoicesCollected { get; }
        IBaseRepository<TaskUserReply, long> TaskUserReplies { get; }
        IBaseRepository<TaskDetail, long> TaskDetails { get; }
        IBaseRepository<TaskType, int> TaskTypes { get; }
        IBaseRepository<TaskFlagsOwnerReciever, long> TaskFlagsOwnerRecievers { get; }
        IBaseRepository<ProjectWorkFlow, long> ProjectWorkFlows { get; }
        IBaseRepository<TaskPermission, long> TaskPermissons { get; }
        IBaseRepository<PaymentTerm, int> PaymentTerms { get; }
        IBaseRepository<ProjectPaymentTerm, long> ProjectPaymentTerms { get; }
        IBaseRepository<ProjectPaymentJournalEntry, long> ProjectPaymentJournalEntries { get; }
        IBaseRepository<DailyJournalEntry, long> DailyJournalEntries { get; }
        IBaseRepository<LetterOfCreditType, int> LetterOfCreditTypies { get; }
        IBaseRepository<ProjectLetterOfCredit, long> ProjectLetterOfCredits { get; }
        IBaseRepository<ProjectLetterOfCreditComment, long> ProjectLetterOfCreditComments { get; }
        IBaseRepository<TaskPermission, long> TaskPermissions { get; }
        IBaseRepository<ClientAccount, long> ClientAccounts { get; }
        IBaseRepository<Invoice, long> Invoices { get; }
        IBaseRepository<GroupUser, long> GroupUsers { get; }
        IBaseRepository<Group, long> Groups { get; }
        IBaseRepository<TaskCategory, long> TaskCategories { get; }
        IBaseRepository<IncomeType, long> IncomeTypes { get; }
        IBaseRepository<ShippingMethod, long> ShippingMethods { get; }
        IBaseRepository<CrmcontactType, int> CrmContactTypes { get; }
        IBaseRepository<CrmrecievedType, int> CrmRecievedTypes { get; }
        IBaseRepository<DailyReportThrough, int> DailyReportThroughs { get; }
        IBaseRepository<DeliveryAndShippingMethod, int> DeliveryAndShippingMethods { get; }
        IBaseRepository<SalesExtraCostType, long> SalesExtraCostTypes { get; }
        IBaseRepository<DailyTranactionBeneficiaryToGeneralType, long> DailyTranactionBeneficiaryToGeneralTypes { get; }
        IBaseRepository<DailyTranactionBeneficiaryToType, long> DailyTranactionBeneficiaryToTypes { get; }
        IBaseRepository<PurchasePoinvoiceTaxIncludedType, long> PurchasePoinvoiceTaxIncludedTypes { get; }
        IBaseRepository<PurchasePoinvoiceNotIncludedTaxType, long> PurchasePoinvoiceNotIncludedTaxTypes { get; }
        IBaseRepository<PurchasePoinvoiceExtraFeesType, long> PurchasePoinvoiceExtraFeesTypes { get; }
        IBaseRepository<PurchasePoinvoiceDeductionType, long> PurchasePoinvoiceDeductionTypes { get; }
        IBaseRepository<Speciality, int> Specialities { get; }
        IBaseRepository<SpecialitySupplier, int> SpecialitySuppliers { get; }
        IBaseRepository<TermsAndCondition,  long> TermsAndConditions { get; }
        IBaseRepository<TermsAndConditionsCategory, int> TermsAndConditionsCategories { get; }
        IBaseRepository<ClientAddress, long> ClientAddresses { get; }
        IBaseRepository<Role, int> Roles { get; }
        IBaseRepository<GroupRole, long> GroupRoles { get; }
        IBaseRepository<Gender,  int> Genders { get; }
        IBaseRepository<ImportantDate, int> ImportantDates { get; }
        IBaseRepository<RoleModule, long> RoleModules { get; }
        IBaseRepository<Module, long> Modules { get; }
        IBaseRepository<Tax, long> Taxes { get; }
        IBaseRepository<Notification, long> Notifications { get; }
        IBaseRepository<VInventoryStoreItemPrice, long> VInventoryStoreItemPrices { get; }
        IBaseRepository<Email, long> Emails { get; }
        IBaseRepository<EmailCc, long> EmailCcs { get; }
        IBaseRepository<EmailAttachment, long> EmailAttachments { get; }
        IBaseRepository<InventoryReport, long> InventoryReports { get; }
        IBaseRepository<InventoryAddingOrder, long> InventoryAddingOrders { get; }
        IBaseRepository<InventoryAddingOrderItem, long> InventoryAddingOrderItems { get; }
        IBaseRepository<InventoryStoreLocation, int> InventoryStoreLocations { get; }
        IBaseRepository<VInventoryAddingOrder, long> VInventoryAddingOrders { get; }
        IBaseRepository<PurchasePoitem, long> PurchasePOItems { get; }
        IBaseRepository<VInventoryAddingOrderItem, long> VInventoryAddingOrderItems { get; }
        IBaseRepository<VPurchasePoItem, long> VPurchasePoItems { get; }
        IBaseRepository<PurchasePoinvoice, long> PurchasePOInvoices { get; }
        IBaseRepository<VInventoryStoreItemMovement, long> VInventoryStoreItemMovements {  get; }
        IBaseRepository<ClientPhone,  long> ClientPhones { get; }
        IBaseRepository<ClientMobile, long> ClientMobiles { get; }
        IBaseRepository<ClientContactPerson, long> ClientContactPersons { get; }
        IBaseRepository<VPurchaseRequestItemsPo, long> VPurchaseRequestItemsPo { get; }
        IBaseRepository<PrsupplierOfferItem, long> PRSupplierOfferItems { get; }
        IBaseRepository<VCrmreport, long> VCrmreports { get; }
        IBaseRepository<DailyReportLine, long> DailyReportLines {  get; }
        IBaseRepository<ClientClassification, long> ClientClassifications { get; }
        IBaseRepository<VProjectSalesOfferClientAccount, long> VProjectSalesOfferClientAccounts {  get; }
        IBaseRepository<ClientConsultant, long> ClientConsultants {  get; }
        IBaseRepository<ClientConsultantAddress, long> ClientConsultantAddressis { get; }
        IBaseRepository<ClientConsultantFax, long> ClientConsultantFaxs { get; }
        IBaseRepository<ClientConsultantEmail, long> ClientConsultantEmails { get; }
        IBaseRepository<ClientConsultantMobile, long> ClientConsultantMobiles {  get; }
        IBaseRepository<ClientConsultantPhone, long> ClientConsultantPhones { get; }
        IBaseRepository<ClientConsultantSpecialilty, long> ClientConsultantSpecialilties { get; }
        IBaseRepository<VPurchaseRequest,  long> VPurchaseRequests { get; }
        //IBaseRepository<InventoryInternalTransferOrder, long> InventoryInternalTransferOrders { get; }
        IBaseRepository<VGroupUser, long> VGroupUsers { get; }
        IBaseRepository<VUserInfo, long> VUserInfos { get; }
        IBaseRepository<PurchaseRequest, long> PurchaseRequests { get; }
        IBaseRepository<PurchaseRequestItem, long> PurchaseRequestItems {  get; }
        IBaseRepository<SalesOfferTermsAndCondition, long> SalesOfferTermsAndConditions {  get; }
        IBaseRepository<PurchasePotype, long> PurchasePotypes { get; }
        IBaseRepository<PrsupplierOffer, long> PRSupplierOffers { get; }
        IBaseRepository<PurchasePo, long> PurchasePOes { get; }
        IBaseRepository<TaskBrowserTab, long> TaskBrowserTabs { get; }
        IBaseRepository<TaskApplicationOpen, long> TaskApplicationsOpen { get; }
        IBaseRepository<TaskScreenShot, long> TaskscreenShots { get; }
        IBaseRepository<Nationality, long> Nationalities { get; }
        IBaseRepository<VPurchasePoitemInvoicePoSupplier, long> VPurchasePoitemInvoicePoSuppliers {  get; }
        IBaseRepository<InventoryReportAttachment, long> InventoryReportAttachments { get; }
        IBaseRepository<PurchaseImportPosetting, long> PurchaseImportPOSettings {  get; }
        IBaseRepository<PurchasePoattachment, long> PurchasePOAttachments { get; }
        IBaseRepository<PuchasePoshipment, long> PuchasePOShipments { get; }
        IBaseRepository<PurchasePoshipmentDocument, long> PurchasePOShipmentDocuments { get; }
        IBaseRepository<PurchasePoshipmentShippingMethodDetail, long> PurchasePOShipmentShippingMethodDetails { get; }
        IBaseRepository<EmailType, int> EmailTypes { get; }
        IBaseRepository<EmailCategory, long> EmailCategories { get; }
        IBaseRepository<EmailCategoryType, long> EmailCategoryTypes { get; }
        IBaseRepository<NotificationSubscription, long> NotificationSubscriptions { get; }
        IBaseRepository<ContactThrough, int> ContactThroughs { get; }
        IBaseRepository<PofinalSelecteSupplier, long> PofinalSelecteSuppliers { get; }
        IBaseRepository<PurchasePoinvoiceExtraFee, long> PurchasePoinvoiceExtraFees { get; }
        IBaseRepository<SupplierAddress, long> SupplierAddresses { get; }
        IBaseRepository<VPoapprovalStatus, long> VPoapprovalStatus { get; }
        IBaseRepository<PurchasePoinvoiceTaxIncluded, long> PurchasePoinvoiceTaxIncluds {  get; }
        IBaseRepository<PurchasePoinvoiceType, long> PurchasePoinvoiceTypes { get; }
        IBaseRepository<SalesBranchUserTarget, long> SalesBranchUserTargets { get; }
        IBaseRepository<WorkshopStation, long> WorkshopStations { get; }
        IBaseRepository<ProjectFabricationWorkshopStationHistory, long> ProjectFabricationWorkshopStationHistories { get; }
        IBaseRepository<ProjectWorkshopStation, long> ProjectWorkshopStations { get; }
        IBaseRepository<VProjectFabricationProjectOfferEntity, long> VProjectFabricationProjectOfferEntities { get; }
        IBaseRepository<ContractType, long> ContractTypes { get; }
        IBaseRepository<VInventoryMatrialRequestWithItem, long> VInventoryMatrialRequestWithItems { get; }
        IBaseRepository<VInventoryStoreItemPriceReport, long> VInventoryStoreItemPriceReports { get; }
        IBaseRepository<InventoryItemContent,  long> InventoryItemContents { get; }
        IBaseRepository<VehicleIssuer, long> VehicleIssuers { get; }
        IBaseRepository<VehicleMaintenanceTypeServiceSheduleCategory, long> VehicleMaintenanceTypeServiceSheduleCategories { get; }
        IBaseRepository<EmailReceiver, long> EmailReceivers { get; }
        IBaseRepository<HremployeeAttachment, long> HremployeeAttachments { get; }
        IBaseRepository<AttendancePaySlip, long> AttendancePaySlips { get; }
        IBaseRepository<WorkingHour , long > WorkingHours { get; }
        IBaseRepository<WeekEnd, long> WeekEnds { get; }
        IBaseRepository<VGroupUserBranch, long> VGroupUserBranchs { get; }
        IBaseRepository<MedicalDoctorScheduleStatus, int> MedicalDoctorScheduleStatus { get; }
        IBaseRepository<MedicalDoctorPercentageType, int> MedicalDoctorPercentageTypes { get; }
        IBaseRepository<MedicalPatientType, int>  MedicalPatientTypes { get; }
        IBaseRepository<DoctorSchedule, long> DoctorSchedules { get; }
        IBaseRepository<MedicalExaminationOffer, long> MedicalExaminationOffers { get; }
        IBaseRepository<MedicalReservation, long> MedicalReservations { get; }

        IBaseRepository<MedicalDailyTreasuryBalance, long> MedicalDailyTreasuryBalances { get; }
        IBaseRepository<ProductGroup , long> productGroup { get; }
        IBaseRepository<Product, long> product { get; }
        IBaseRepository<ClientExtraInfo, long> ClientExtraInfos { get; }
        IBaseRepository<HremployeeAttachment, long> HREmployeeAttachments {  get; }
        IBaseRepository<MangmentOfMaintenanceRequestAttachment, long> MangmentOfMaintenanceRequestAttachments { get; }
        //-----------------------------------------------------Hany-------------------------------------------------
        IBaseRepository<LaboratoryMessagesReport , long> LaboratoryMessagesReports { get; }
        IBaseRepository<MaritalStatus , int> MaritalStatus { get; }
        IBaseRepository<ReservationInvoice , int> ReservationInvoice { get; }
        IBaseRepository<Reservation , int> Reservations { get; }

        IBaseRepository<VClientSalesPerson, long> VClientSalesPeople { get; }
        IBaseRepository<ClientSalesPerson, long> ClientSalesPeople { get; }
        IBaseRepository<ClientPaymentTerm, long> ClientPaymentTerms { get; }
        IBaseRepository<ClientBankAccount, long> ClientBankAccounts { get; }
        IBaseRepository<InvoiceType, int> InvoiceTypes { get; }
        IBaseRepository<ClientNational, int> clientNationals { get; }
        IBaseRepository<ClientInformation, int> clientInformations { get; }
        IBaseRepository<CostType, long> CostTypes { get; }

        IBaseRepository<TransportationLine , int> TransportationLines { get; }
        IBaseRepository<TransportationVehicle , int> TransportationVehicles { get; }
        IBaseRepository<VehicleType , int> VehicleTypes { get; }
        IBaseRepository<TransportationVehicleRoute , long> TransportationVehicleRoutes { get; }
        IBaseRepository<TransportationVehicleRouteDirection , long> TransportationVehicleRouteDirections { get; }
        IBaseRepository<TransportationVehicleRouteEmployee , long> TransportationVehicleRouteEmployees { get; }
        IBaseRepository<TransportationLineIncreaseRequest , long> TransportationLineIncreaseRequests { get; }
        IBaseRepository<TransportationLineIncreaseRequestLine , long> TransportationLineIncreaseRequestLines{ get; }
        IBaseRepository<TransportationVehicleRouteDeduction , long> TransportationVehicleRouteDeductions { get; }
        IBaseRepository<TransportationVehicleRouteAccount , long> TransportationVehicleRouteAccounts { get; }
        IBaseRepository<TransprotationUserAttedance , long> TransprotationUserAttedances { get; }
        IBaseRepository<TransportationVehicleRouteEmployeeException , long> TransportationVehicleRouteEmployeeExceptions { get; }
        IBaseRepository<TransportationRoundForRoute , int> TransportationRoundForRoutes { get; }
        IBaseRepository<VBusProgram , int> VBusProgram { get; }
        IBaseRepository<TransportionLineExceptionPrice , int> TransportionLineExceptionPrice { get; }
        IBaseRepository<TransportionLineSupplierPayment , int> TransportionLineSupplierPayment { get; }
        IBaseRepository<DistributionSupplierPayment , int> DistributionSupplierPayment { get; }


        //----------------------------------------------------------------------------------
        // Maintenance Contract
        IBaseRepository<MaintenanceRequestType , int> MaintenanceRequestTypes { get; }
        IBaseRepository<MangmentOfMaintenanceRequest, long> MangmentOfMaintenanceRequests { get; }
        IBaseRepository<MangmentOfMaintenanceRequestReport, long> MangmentOfMaintenanceRequestReports { get; }
        IBaseRepository<MangmentOfMaintenanceRequestReportAttachment, long> MangmentOfMaintenanceRequestReportAttachments { get; }

        int Complete();
        Task<int> CompleteAsync();
        void BeginTransaction();
        void ClearChangeTracker();
        void Detach<T>(T entity) where T : class;
        void CommitTransaction();        
        void RollbackTransaction();
    }
}
