using Microsoft.EntityFrameworkCore;
using NewGaras.Domain.Interfaces.Repositories;
using NewGaras.Domain.Models;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Entities;

using NewGaras.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Task = NewGaras.Infrastructure.Entities.Task;

namespace NewGaras.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GarasTestContext _context;


        public void ClearChangeTracker()
        {
            _context.ChangeTracker.Clear();
        }

        // Implement Detach method
        public void Detach<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
        public IBaseRepository<User, long> Users { get; private set; }
        public IBaseRepository<UserSession, long> UserSessions { get; private set; }
        public IBaseRepository<JobTitle, int> JobTitles { get; private set; }
        public IBaseRepository<VClientConsultantAddress, long> VClientConsultantAddresses { get; private set; }
        /*public IBaseRepository<UserDeviceConnection, long> UserDeviceConnections { get; private set; }
        public IInventoryItemRepository InventoryItems { get; private set; }
        public IDashboardManagementRepository DashboardManagement { get; private set; }
        public IMatrialRequestRepository MatrialRequests { get; private set; }
        public IHrUserRepository HrUsers { get; private set; }
        public IBaseRepository<HrContactInfo, int> ContactInfos { get; private set; }
        public IBaseRepository<HrUserAddress, int> UserAddresses { get; private set; }
        public IBaseRepository<HrNationality, int> HrNationalities { get; private set; }
        public IBaseRepository<HrMilitaryStatus, int> HrMilitaryStatuses { get; private set; }
        public IBaseRepository<HrMaritalStatus, int> HrMaritalStatuses { get; private set; }
        public IBaseRepository<HrBranch, int> HrBranches { get; private set; }
        public IBaseRepository<HrBranchContactInfo, int> BranchContactInfos { get; private set; }
        public IBaseRepository<HrBranchAddress, int> BranchAddresses { get; private set; }
        public IBaseRepository<HrGrade, int> Grades { get; private set; }
        public IBaseRepository<HrBranchDepartment, int> BranchDepartments { get; private set; }
        public IBaseRepository<HrTeam, long> Teams { get; private set; }
        public IBaseRepository<HrUserTeam, long> UserTeams { get; private set; }
        
        public IBaseRepository<HrJobDescription, int> JobDescription { get; private set; }
        public IBaseRepository<HrJobDescriptionAttachment, int> JobDescriptionAttachment { get; private set; }
        public IBaseRepository<HrJobResponsabillity, int> JobResponsabillity { get; private set; }
        public IBaseRepository<HrJobResponsbillityAttachment, int> JobResponsbillityAttachment { get; private set; }
        public IBaseRepository<HrWorkInstruction, int> WorkInstruction { get; private set; }
        public IBaseRepository<HrWorkInstructionAttachment, int> WorkInstructionAttachment { get; private set; }
        public IBaseRepository<HrSkill, int> Skill { get; private set; }
        public IBaseRepository<HrSkillAttachment, int> SkillAttachment { get; private set; }
        public IBaseRepository<HrInterviewKpi, int> InterviewKpi { get; private set; }
        public IBaseRepository<HrOperationKpi, int> OperationKpi { get; private set; }
        public IBaseRepository<HrJobTitleDepartment, int> JobTitleDepartment { get; private set; }*/
        public IBaseRepository<Country, int> Country { get; private set; }
        public IBaseRepository<City, int> City { get; private set; }
        public IBaseRepository<Area, int> Area { get; private set; }
        public IBaseRepository<PosNumber, int> PosNumbers { get; private set; }

        public IHrUserRepository HrUsers { get; private set; }

        public IBaseRepository<Salary, long> Salaries { get; private set; }
        public IBaseRepository<ContractDetail, long> Contracts { get; private set; }
        public IBaseRepository<Branch,int> Branches { get; private set; }
        public IBaseRepository<Department,int> Departments { get; private set; }
        public IBaseRepository<Team,long> Teams { get; private set; }
        public IBaseRepository<UserTeam,int> UserTeams { get; private set; }
        public IBaseRepository<SystemLog,int> SystemLogs { get; private set; }

        public IBaseRepository<Currency, int> Currencies { get; private set; }
        public IBaseRepository<PaymentStrategy, int> PaymentStrategies { get; private set; }
        public IBaseRepository<WeekDay, int> WeekDays { get; private set; }

        public IBaseRepository<AllowncesType, int> AllowncesTypes { get; private set; }

        public IBaseRepository<SalaryAllownce, int> SalaryAllownces { get; private set; }
        public IBaseRepository<BranchSchedule, long> BranchSchedules { get; private set; }

        public IBaseRepository<SalaryTax, long> SalaryTaxs { get; private set; }
        public IBaseRepository<ContractLeaveSetting, int> ContractLeaveSetting { get; private set; }
        public IBaseRepository<DeductionType, int> DeductionTypes { get; private set; }
        public IBaseRepository<Attendance, long> Attendances { get; private set; }
        public IBaseRepository<OffDay, long> OffDays { get; private set; }

        public IBaseRepository<SalaryDeductionTax, long> SalaryDeductionTaxs { get; private set; }

        public IBaseRepository<BranchSetting, long> BranchSetting { get; private set; }
        public IBaseRepository<ProjectCheque, long> ProjectCheques { get; private set; }
        public IBaseRepository<ChequeCashingStatus, int> ChequeCashingStatuses { get; private set; }
        public IBaseRepository<ProgressType, int> ProgressTypes { get; private set; }
        public IBaseRepository<DeliveryType, int> DeliveryTypes { get; private set; }
        public IBaseRepository<ProgressStatus, int> ProgressStatuses { get; private set; }
        public IBaseRepository<ProjectProgress, long> ProjectProgresses { get; private set; }
        public IBaseRepository<InsuranceCompanyName, long> InsuranceCompanyNames { get; private set; }
        public IBaseRepository<Account, long> Accounts { get; private set; }
        public IBaseRepository<AccountOfJournalEntry, long> AccountOfJournalEntries { get; private set; }
        public IBaseRepository<VProjectSalesOfferClient, long> VProjectSalesOfferClients { get; private set; }
        public IBaseRepository<SupplierAccount, long> SupplierAccounts { get; private set; }
        public IBaseRepository<VPurchasePo, long> VPurchasePos { get; private set; }
        public IBaseRepository<VAccountOfJournalEntryWithDaily, long> VAccountOfJournalEntryWithDailies { get; private set; }
        public IBaseRepository<DailyJournalEntryReverse, long> DailyJournalEntryReverses { get; private set; }
        public IBaseRepository<VDailyJournalEntry, long> VDailyJournalEntries { get; private set; }
        public IBaseRepository<PurchasePo, long> PurchasePos { get; private set; }
        public IBaseRepository<VProjectSalesOffer, long> VProjectSalesOffers { get; private set; }
        public IBaseRepository<DailyAdjustingEntryCostCenter, long> DailyAdjustingEntryCostCenters { get; private set; }
        public IBaseRepository<GeneralActiveCostCenter, long> GeneralActiveCostCenters { get; private set; }
        public IBaseRepository<DailyTranactionBeneficiaryToUser, long> DailyTranactionBeneficiaryToUsers { get; private set; }
        public IBaseRepository<Supplier, long> Suppliers { get; private set; }
        public IBaseRepository<AccountFinancialPeriod, int> AccountFinancialPeriods { get; private set; }
        public IBaseRepository<AccountOfJournalEntryOtherCurrency, long> AccountOfJournalEntryOtherCurrencies { get; private set; }
        public IBaseRepository<InvoiceCnandDn, long> InvoiceCnandDns { get; private set; }
        public IBaseRepository<VAccount, long> VAccounts { get; private set; }
        public IBaseRepository<AccountCategory, long> AccountCategories { get; private set; }
        public IBaseRepository<PurchasePaymentMethod, long> PurchasePaymentMethods { get; private set; }
        public IBaseRepository<AdvanciedType, long> AdvanciedTypes { get; private set; }
        public IBaseRepository<VisitsScheduleOfMaintenance, long> VisitsScheduleOfMaintenances { get; private set; }
        public IBaseRepository<SalesOfferLocation, long> SalesOfferLocations { get; private set; }
        public IBaseRepository<VVisitsMaintenanceReportUser, long> VVisitsMaintenanceReportUsers { get; private set; }
        public IBaseRepository<InventoryItemCategory, int> InventoryItemCategories { get; private set; }
        public IBaseRepository<MaintenanceFor, long> MaintenanceFors { get; private set; }
        public IBaseRepository<SalesOfferProduct, long> SalesOfferProducts { get; private set; }
        public IBaseRepository<SalesMaintenanceOffer, long> SalesMaintenanceOffers { get; private set; }
        public IBaseRepository<ManagementOfMaintenanceOrder, long> ManagementOfMaintenanceOrders { get; private set; }
        public IBaseRepository<SalesOfferDiscount, long> SalesOfferDiscounts { get; private set; }
        public IBaseRepository<VInventoryStoreItem, long> VInventoryStoreItems { get; private set; }
        public IBaseRepository<InvoiceItem, long> InvoiceItems { get; private set; }
        public IBaseRepository<VSalesOfferProduct, long> VSalesOfferProducts { get; private set; }
        public IBaseRepository<SalesOfferProductTax, long> SalesOfferProductTaxes { get; private set; }
        public IBaseRepository<VInventoryItem, long> VInventoryItems { get; private set; }
        public IBaseRepository<VClientMobile, long> VClientMobiles { get; private set; }
        public IBaseRepository<ClientContactPerson, long> ClientContactPeople { get; private set; }
        public IBaseRepository<VClientPhone, long> VClientPhones { get; private set; }
        public IBaseRepository<ClientFax, long> ClientFaxes { get; private set; }
        public IBaseRepository<VClientSpeciality, long> VClientSpecialities { get; private set; }
        public IBaseRepository<VClientAddress, long> VClientAddresses { get; private set; }
        public IBaseRepository<VClientUseer, long> VClientUseers { get; private set; }
        public IBaseRepository<VVehicle, long> VVehicles { get; private set; }
        public IBaseRepository<VClientExpired, long> VClientExpiredes { get; private set; }
        public IBaseRepository<InvoiceExtraModification, long> InvoiceExtraModifications { get; private set;}
        public IBaseRepository<VSalesOfferExtraCost, long> VSalesOfferExtraCosts { get; private set;}
        public IBaseRepository<SalesOfferAttachment, long> SalesOfferAttachments { get; private set;}
        public IBaseRepository<SalesOfferItemAttachment, long> SalesOfferItemAttachments { get; private set;}
        public IBaseRepository<SalesOfferInvoiceTax, long> SalesOfferInvoiceTaxes { get; private set;}
        public IBaseRepository<SalesOfferExtraCost, long> SalesOfferExtraCosts { get; private set;}
        public IBaseRepository<Bom, long> Boms { get; private set;}
        public IBaseRepository<VBompartitionItemsInventoryItem, long> VBompartitionItemsInventoryItems { get; private set;}
        public IBaseRepository<VSalesOfferProductSalesOffer, long> VSalesOfferProductSalesOffers { get; private set;}
        public IBaseRepository<VInventoryInternalBackOrder, long> VInventoryInternalBackOrders { get; private set;}
        public IBaseRepository<VInventoryMatrialRelease, long> VInventoryMatrialReleases { get; private set;}
        public IBaseRepository<MaintenanceReport, long> MaintenanceReports { get; private set;}
        public IBaseRepository<SalesTarget, int> SalesTargets { get; private set;}
        public IBaseRepository<VDailyReportReportLineThroughApi, long> VDailyReportReportLineThroughApis { get; private set;}
        public IBaseRepository<Crmreport, long> Crmreports { get; private set;}
        public IBaseRepository<CategoryType, int> CategoryTypes { get; private set;}
        public IBaseRepository<VehicleBodyType, int> VehicleBodyTypes { get; private set;}
        public IBaseRepository<EinvoiceAttachment, long> EinvoiceAttachments { get; private set;}
        public IBaseRepository<NotificationProcess, int> NotificationProcesses { get; private set;}
        public IBaseRepository<VBranchProduct, long> VBranchProducts { get; private set;}
        public IBaseRepository<VUserBranchGroup, long> VUserBranchGroups { get; private set;}
        public IBaseRepository<SupportedBy, long> SupportedBies { get; private set;}
        public IBaseRepository<ClientSpeciality, long> ClientSpecialities { get; private set;}
        public IBaseRepository<SupplierMobile, long> SupplierMobiles { get; private set;}
        public IBaseRepository<PurchasePoitem, long> PurchasePoitems { get; private set;}
        public IBaseRepository<InventoryItemAttachment, long> InventoryItemAttachments { get; private set;}
        public IBaseRepository<VPrsupplierOfferItem, long> VPrsupplierOfferItems { get; private set;}
        public IBaseRepository<ClientConsultantFax, long> ClientConsultantFaxes { get; private set;}
        public IBaseRepository<VNotificationsDetail, long> VNotificationsDetails { get; private set;}
        public IBaseRepository<DoctorRoom, long> DoctorRooms { get; private set;}





        public IBaseRepository<TaskRequirement, long> TaskRequirements { get; private set; }
        //-------------------------------------Patrick-----------------------------------------------
        public IBaseRepository<Task, long> Tasks { get; private set; }
        public IBaseRepository<ExpensisType, long> ExpensisTypes { get; private set; }
        public IBaseRepository<TaskExpensi, long> TaskExpensis { get; private set; }
        public IBaseRepository<Client, long> Clients { get; private set; }
        public IBaseRepository<ProjectContactPerson, long> ProjectContactPersons { get; private set; }
        public IBaseRepository<SalesOffer, long> SalesOffers { get; private set; }
        public IBaseRepository<Priority , int> Prioritys { get; private set; }
        public IBaseRepository<Project , long> Projects { get; private set; }
        public IBaseRepository<ProjectAttachment, long> ProjectAttachments { get; private set; }
        public IBaseRepository<Governorate, int> Governorates { get; private set; }
        public IBaseRepository<Area, long> Areas { get; private set; }
        public IBaseRepository<ProjectWorkFlow, long> Workflows { get; private set; }
        public IBaseRepository<ProjectSprint, long> ProjectSprints { get; private set; }
        public IBaseRepository<ProjectCostingType, int> ProjectCostTypes { get; private set; }
        public IBaseRepository<BillingType, int> Billingtypes { get; private set; }
        public IBaseRepository<UserRole, long> UserRoles { get; private set; }
        public IBaseRepository<ProjectAssignUser, long> ProjectAssignUsers { get; private set; }
        public IBaseRepository<BankDetail, int> BankDetails { get; private set; }
        public IBaseRepository<PaymentMethod, int> PaymentMethods { get; private set; }
        public IBaseRepository<InventoryUom, int> InventoryUoms { get; private set; }
        public IBaseRepository<TaskUnitRateService, long> TaskUnitRateServices { get; private set; }
        public IBaseRepository<ProjectInvoiceCollected, long> ProjectInvoicesCollected { get; private set; }
        public IBaseRepository<TaskUserReply, long> TaskUserReplies { get; private set; }
        public IBaseRepository<TaskDetail, long> TaskDetails { get; private set; }
        public IBaseRepository<TaskType, int> TaskTypes { get; private set; }
        public IBaseRepository<TaskFlagsOwnerReciever, long> TaskFlagsOwnerRecievers { get; private set; }
        public IBaseRepository<ProjectWorkFlow, long> ProjectWorkFlows { get; private set; }    
        public IBaseRepository<TaskPermission, long> TaskPermissons { get; private set; }
        public IBaseRepository<PaymentTerm,  int> PaymentTerms { get; private set; }
        public IBaseRepository<ProjectPaymentTerm, long> ProjectPaymentTerms { get; private set; }
        public IBaseRepository<ProjectPaymentJournalEntry, long> ProjectPaymentJournalEntries { get; private set; }
        public IBaseRepository<DailyJournalEntry, long> DailyJournalEntries { get; private set; }
        public IBaseRepository<LetterOfCreditType, int> LetterOfCreditTypies { get; private set; }
        public IBaseRepository<ProjectLetterOfCredit, long> ProjectLetterOfCredits { get; private set; }
        public IBaseRepository<ProjectLetterOfCreditComment, long> ProjectLetterOfCreditComments { get; private set; }
        public IBaseRepository<TaskPermission, long> TaskPermissions { get; private set; }
        public IBaseRepository<GroupUser, long> GroupUsers { get; private set; }
        public IBaseRepository<Group, long> Groups { get; private set; }
        public IBaseRepository<TaskCategory, long> TaskCategories { get; private set; }
        public IBaseRepository<IncomeType, long> IncomeTypes { get; private set; }
        public IBaseRepository<ShippingMethod, long> ShippingMethods { get; private set; }
        public IBaseRepository<CrmcontactType, int> CrmContactTypes { get; private set; }
        public IBaseRepository<CrmrecievedType, int> CrmRecievedTypes { get; private set; }
        public IBaseRepository<DailyReportThrough, int> DailyReportThroughs {  get; private set; }
        public IBaseRepository<DeliveryAndShippingMethod, int> DeliveryAndShippingMethods { get; private set; }
        public IBaseRepository<SalesExtraCostType, long> SalesExtraCostTypes {  get; private set; }
        public IBaseRepository<DailyTranactionBeneficiaryToGeneralType, long> DailyTranactionBeneficiaryToGeneralTypes { get; private set; }
        public IBaseRepository<DailyTranactionBeneficiaryToType, long> DailyTranactionBeneficiaryToTypes { get; private set; }
        public IBaseRepository<PurchasePoinvoiceTaxIncludedType, long> PurchasePoinvoiceTaxIncludedTypes { get; private set; }
        public IBaseRepository<PurchasePoinvoiceNotIncludedTaxType, long> PurchasePoinvoiceNotIncludedTaxTypes { get; private set; }
        public IBaseRepository<PurchasePoinvoiceExtraFeesType, long> PurchasePoinvoiceExtraFeesTypes {  get; private set; }
        public IBaseRepository<PurchasePoinvoiceDeductionType, long> PurchasePoinvoiceDeductionTypes { get; private set; }
        public IBaseRepository<Speciality, int> Specialities { get; private set; }
        public IBaseRepository<SpecialitySupplier, int> SpecialitySuppliers { get; private set; }
        public IBaseRepository<TermsAndCondition, long> TermsAndConditions {  get; private set; }
        public IBaseRepository<TermsAndConditionsCategory, int> TermsAndConditionsCategories { get; private set; }
        public IBaseRepository<ClientAddress, long> ClientAddresses { get; private set; }
        public IBaseRepository<Role, int> Roles { get; private set; }
        public IBaseRepository<GroupRole , long> GroupRoles { get; private set; }
        public IBaseRepository<Gender, int> Genders {  get; private set; }
        public IBaseRepository<ImportantDate, int> ImportantDates { get; private set; }
        public IBaseRepository<RoleModule, long> RoleModules { get; private set; }
        public IBaseRepository<Module, long> Modules { get; private set; }
        public IBaseRepository<Tax, long> Taxes {  get; private set; }
        public IBaseRepository<Notification, long> Notifications { get; private set; }
        public IBaseRepository<VInventoryStoreItemPrice, long> VInventoryStoreItemPrices {  get; private set; }
        public IBaseRepository<Email, long> Emails { get; private set; }
        public IBaseRepository<EmailCc, long> EmailCcs { get; private set; }
        public IBaseRepository<EmailAttachment, long> EmailAttachments { get; private set; }
        public IBaseRepository<InventoryReport, long> InventoryReports { get; private set; }
        public IBaseRepository<InventoryAddingOrder, long> InventoryAddingOrders { get; private set; }
        public IBaseRepository<InventoryAddingOrderItem, long> InventoryAddingOrderItems { get; private set; }
        public IBaseRepository<InventoryStoreLocation , int> InventoryStoreLocations { get; private set; }
        public IBaseRepository<VInventoryAddingOrder, long> VInventoryAddingOrders { get; private set; }
        public IBaseRepository<PurchasePoitem, long> PurchasePOItems {  get; private set; }
        public IBaseRepository<VInventoryAddingOrderItem, long> VInventoryAddingOrderItems { get; private set; }
        public IBaseRepository<VPurchasePoItem, long> VPurchasePoItems {  get; private set; }
        public IBaseRepository<PurchasePoinvoice, long> PurchasePOInvoices {  get; private set; }
        public IBaseRepository<VInventoryStoreItemMovement, long> VInventoryStoreItemMovements { get; private set; }
        public IBaseRepository<ClientPhone, long> ClientPhones { get; private set; }
        public IBaseRepository<ClientMobile, long> ClientMobiles { get; private set; }
        public IBaseRepository<ClientContactPerson, long> ClientContactPersons {  get; private set; }
        public IBaseRepository<VPurchaseRequestItemsPo, long> VPurchaseRequestItemsPo {  get; private set; }
        //public IBaseRepository<PrSupplierOfferItem, long> PRSupplierOfferItems {  get; private set; }
        public IBaseRepository<PrsupplierOfferItem, long> PRSupplierOfferItems { get; private set; }

        public IBaseRepository<VCrmreport, long> VCrmreports {  get; private set; }
        public IBaseRepository<DailyReportLine, long> DailyReportLines {  get; private set; }
        public IBaseRepository<ClientClassification, long> ClientClassifications { get; private set; }
        public IBaseRepository<VProjectSalesOfferClientAccount, long> VProjectSalesOfferClientAccounts { get; private set; }
        public IBaseRepository<ClientConsultant, long> ClientConsultants { get; private set; }
        public IBaseRepository<ClientConsultantAddress, long> ClientConsultantAddressis {  get; private set; }
        public IBaseRepository<ClientConsultantFax, long> ClientConsultantFaxs {  get; private set; }
        public IBaseRepository<ClientConsultantEmail, long> ClientConsultantEmails { get; private set; }
        public IBaseRepository<ClientConsultantMobile, long> ClientConsultantMobiles { get; private set; }
        public IBaseRepository<ClientConsultantPhone, long> ClientConsultantPhones {  get; private set; }
        public IBaseRepository<ClientConsultantSpecialilty, long> ClientConsultantSpecialilties {  get; private set; }
        public IBaseRepository<VPurchaseRequest, long> VPurchaseRequests {  get; private set; }
        public IBaseRepository<InventoryInternalTransferOrder, long> InventoryInternalTransferOrders { get; private set; }
        public IBaseRepository<VGroupUser, long> VGroupUsers { get; private set; }
        public IBaseRepository<VUserInfo, long> VUserInfos { get; private set; }
        public IBaseRepository<InventoryInternalBackOrder, long> InventoryInternalBackOrders { get; private set; }
        public IBaseRepository<VInventoryInternalBackOrderItem, long> VInventoryInternalBackOrderItems { get; private set; }
        public IBaseRepository<VInventoryInternalTransferOrderItem, long> VInventoryInternalTransferOrderItems { get; private set; }
        public IBaseRepository<InventoryReportItem, long> InventoryReportItems { get; private set; }
        public IBaseRepository<InventoryReportItemParent, long> InventoryReportItemParents { get; private set; }
        public IBaseRepository<Hrcustody, long> Hrcustodies { get; private set; }
        public IBaseRepository<PurchasePoinvoice, long> PurchasePoinvoices { get; private set; }
        public IBaseRepository<InventoryMatrialReleasePrintInfo, long> InventoryMatrialReleasePrintInfoes { get; private set; }
        public IBaseRepository<PurchaseRequest, long> PurchaseRequests {  get; private set; }
        public IBaseRepository<PurchaseRequestItem, long> PurchaseRequestItems {  get; private set; }
        public IBaseRepository<SalesOfferInternalApproval, long> SalesOfferInternalApprovals {  get; private set; }
        public IBaseRepository<SalesOfferTermsAndCondition, long> SalesOfferTermsAndConditions {  get; private set; }
        public IBaseRepository<PurchasePotype, long> PurchasePotypes {  get; private set; }
        public IBaseRepository<PrsupplierOffer, long> PRSupplierOffers {  get; private set; }
        public IBaseRepository<PurchasePo, long> PurchasePOes { get; private set; }
        public IBaseRepository<TaskBrowserTab, long> TaskBrowserTabs {  get; private set; }
        public IBaseRepository<TaskApplicationOpen, long> TaskApplicationsOpen {  get; private set; }
        public IBaseRepository<TaskScreenShot, long> TaskscreenShots { get; private set; }
        public IBaseRepository<Nationality, long> Nationalities {  get; private set; }
        public IBaseRepository<VPurchasePoitemInvoicePoSupplier, long> VPurchasePoitemInvoicePoSuppliers {  get; private set; }
        public IBaseRepository<InventoryReportAttachment, long> InventoryReportAttachments {  get; private set; }
        public IBaseRepository<MaintenanceReportUser, long> MaintenanceReportUsers {  get; private set; }
        public IBaseRepository<PurchaseImportPosetting, long> PurchaseImportPOSettings {  get; private set; }
        public IBaseRepository<PurchasePoattachment, long> PurchasePOAttachments {  get; private set; }
        public IBaseRepository<PuchasePoshipment, long> PuchasePOShipments {  get; private set; }
        public IBaseRepository<PurchasePoshipmentDocument, long> PurchasePOShipmentDocuments {  get; private set; }
        public IBaseRepository<PurchasePoshipmentShippingMethodDetail, long> PurchasePOShipmentShippingMethodDetails {  get; private set; }
        public IBaseRepository<EmailType, int> EmailTypes { get; private set; }

        public IBaseRepository<EmailCategory, long> EmailCategories {  get; private set; }
        public IBaseRepository<EmailCategoryType, long> EmailCategoryTypes { get; private set; }
        public IBaseRepository<NotificationSubscription, long> NotificationSubscriptions { get; private set; }
        public IBaseRepository<ContactThrough, int> ContactThroughs {  get; private set; }
        public IBaseRepository<PofinalSelecteSupplier, long> PofinalSelecteSuppliers {  get; private set; }
        public IBaseRepository<PurchasePoinvoiceExtraFee, long> PurchasePoinvoiceExtraFees { get; private set; }
        public IBaseRepository<SupplierAddress, long> SupplierAddresses { get; private set; }
        //public IBaseRepository<VInventoryAddingOrder, long> VInventoryAddingOrders { get; private set; }
        public IBaseRepository<VPoapprovalStatus, long> VPoapprovalStatus { get; private set; }
        public IBaseRepository<PurchasePoinvoiceTaxIncluded, long> PurchasePoinvoiceTaxIncluds { get; private set; }
        public IBaseRepository<PurchasePoinvoiceType, long> PurchasePoinvoiceTypes { get; private set; }
        public IBaseRepository<SalesBranchUserTarget, long> SalesBranchUserTargets { get; private set; }
        public IBaseRepository<PermissionLevel, int> PermissionLevels { get; private set; }
        public IBaseRepository<VMatrialReleaseSalesOffer, long> VMatrialReleaseSalesOffers { get; private set; }
        public IBaseRepository<WorkshopStation, long> WorkshopStations { get; private set; }
        public IBaseRepository<ProjectFabricationWorkshopStationHistory, long> ProjectFabricationWorkshopStationHistories { get; private set; }
        public IBaseRepository<ProjectWorkshopStation, long> ProjectWorkshopStations { get; private set; }
        public IBaseRepository<VProjectFabricationProjectOfferEntity, long> VProjectFabricationProjectOfferEntities { get; private set; }
        public IBaseRepository<VInvoice, long> VInvoices { get; private set; }
        public IBaseRepository<ContractType, long> ContractTypes { get; private set; }
        public IBaseRepository<VInventoryMatrialRequestWithItem, long> VInventoryMatrialRequestWithItems { get; private set; }
        public IBaseRepository<VInventoryStoreItemPriceReport, long> VInventoryStoreItemPriceReports {  get; private set; }
        public IBaseRepository<InventoryItemContent, long> InventoryItemContents { get; set; }
        public IBaseRepository<VehicleIssuer, long> VehicleIssuers { get; set; }
        public IBaseRepository<VehicleMaintenanceTypeServiceSheduleCategory, long> VehicleMaintenanceTypeServiceSheduleCategories { get; set; }
        public IBaseRepository<EmailReceiver, long> EmailReceivers {  get; set; }
        public IBaseRepository<VClientSalesPerson, long> VClientSalesPeople {  get; set; }
        public IBaseRepository<ClientSalesPerson, long> ClientSalesPeople {  get; set; }
        public IBaseRepository<ClientPaymentTerm, long> ClientPaymentTerms { get; set; }
        public IBaseRepository<ClientBankAccount, long> ClientBankAccounts { get; set; }
        public IBaseRepository<HremployeeAttachment, long> HremployeeAttachments { get; set; }
        public IBaseRepository<AttendancePaySlip, long> AttendancePaySlips { get; set; }
        public IBaseRepository<WorkingHour,long> WorkingHours { get; set; }
        public IBaseRepository<WeekEnd, long> WeekEnds { get; set; }
        public IBaseRepository<VGroupUserBranch, long> VGroupUserBranchs { get; set; }
        public IBaseRepository<MedicalDoctorScheduleStatus, int> MedicalDoctorScheduleStatus { get; set; }
        public IBaseRepository<MedicalDoctorPercentageType, int> MedicalDoctorPercentageTypes { get; set; }
        public IBaseRepository<MedicalPatientType, int>  MedicalPatientTypes { get; set; }
        public IBaseRepository<DoctorSchedule, long> DoctorSchedules { get; set; }
        public IBaseRepository<MedicalExaminationOffer, long> MedicalExaminationOffers { get; set; } 
        public IBaseRepository<MedicalReservation, long> MedicalReservations { get; set; }
        public IBaseRepository<MedicalDailyTreasuryBalance, long> MedicalDailyTreasuryBalances { get; set; }
        public IBaseRepository<ClientExtraInfo, long> ClientExtraInfos { get; set; }
        public IBaseRepository<HremployeeAttachment, long> HREmployeeAttachments { get; set; }
        public IBaseRepository<MangmentOfMaintenanceRequestAttachment, long> MangmentOfMaintenanceRequestAttachments { set; get; }
        //-------------------------------------------------------------------------------------------------

        public IBaseRepository<OverTimeAndDeductionRate, long> OverTimeAndDeductionRates { get; private set; }
        public IBaseRepository<VacationOverTimeAndDeductionRate, long> VacationOverTimeAndDeductionRates { get; private set; }
        public IBaseRepository<VacationDay, long> VacationDays { get; private set; }
        public IBaseRepository<WorkingHourseTracking, long> WorkingHoursTrackings { get; private set; }
        public IBaseRepository<DayType, int> DayTypes { get; private set; }
        public IBaseRepository<SalaryType, int> SalaryTypes { get; private set; }

        public IBaseRepository<TaxType, int> TaxTypes { get; private set; }
        public IBaseRepository<Payroll, long> Payrolls { get; private set; }
        public IBaseRepository<ContractLeaveEmployee, long> ContractLeaveEmployees { get; private set; }
        public IBaseRepository<LeaveRequest, long> LeaveRequests { get; private set; }
        public IBaseRepository<ProjectInvoice, long> ProjectInvoices { get; private set; }
        public IBaseRepository<ProjectInvoiceItem, long> ProjectInvoiceItems { get; private set; }
        public IBaseRepository<ContractReportTo, long> ContractReportTos { get; private set; }
        public IBaseRepository<ClientAccount, long> ClientAccounts { get; private set; }
        public IBaseRepository<Invoice, long> Invoices { get; private set; }
        public IBaseRepository<AdvanciedSettingAccount, long> AdvanciedSettingAccounts { get; private set; }
        public IBaseRepository<InventoryStore, int> InventoryStores { get; private set; }
        public IBaseRepository<Entities.InventoryItem, long> InventoryItems { get; private set; }
        public IBaseRepository<InventoryStoreItem, long> InventoryStoreItems { get; private set; }
        public IBaseRepository<InventoryMatrialRequest, long> InventoryMatrialRequests { get; private set; }
        public IBaseRepository<InventoryMatrialRelease, long> InventoryMatrialReleases { get; private set; }
        public IBaseRepository<InventoryMatrialRequestItem, long> InventoryMatrialRequestItems { get; private set; }
        public IBaseRepository<InventoryMatrialReleaseItem, long> InventoryMatrialReleaseItems { get; private set; }
        public IBaseRepository<UserPatient, long> UserPatients { get; private set; }
        public IBaseRepository<InventoryStoreKeeper, int> InventoryStoreKeepers { get; private set; }
        public IBaseRepository<UserPatientInsurance, long> UserPatientInsurances { get; private set; }
        public IBaseRepository<VInventoryMatrialReleaseItem, int> VInventoryMatrialReleaseItems { get; private set; }
        public IBaseRepository<VInventoryMatrialRequestItem, int> VInventoryMatrialRequestItems { get; private set; }
        public IBaseRepository<Country, int> Countries { get; private set; }
        public IBaseRepository<PosClosingDay, long> PosClosingDays { get; private set; }
        public IBaseRepository<VehiclePerClient, long> VehiclePerClients { get; private set; }
        public IBaseRepository<CompanySerialIdentification, int> CompanySerialIdentifications { get; private set; }
        public IBaseRepository<VehicleMaintenanceJobOrderHistory, long> VehicleMaintenanceJobOrderHistories { get; private set; }
        public IBaseRepository<CompanySpecialty, int> CompanySpecialties { get; private set; }
        public IBaseRepository<VUserRole, long> VUserRoles { get; private set; }
        public IBaseRepository<InventoryMaterialRequestType, long> InventoryMaterialRequestTypes { get; private set; }
        public IBaseRepository<ProjectFabrication, long> ProjectFabrications { get; private set; }
        public IBaseRepository<ProjectFabricationBoq, long> ProjectFabricationBOQs { get; private set; }
        public IBaseRepository<VInventoryMatrialRequest, long> VInventoryMatrialRequests { get; private set; }
        public IBaseRepository<VPurchaseRequestItem, long> VPurchaseRequestItems { get; private set; }
        public IBaseRepository<VInventoryStoreKeeper, long> VInventoryStoreKeepers { get; private set; }
        public IBaseRepository<VProjectFabricationTotalHour, long> VProjectFabricationTotalHours { get; private set; }
        public IBaseRepository<ProjectInstallation, long> ProjectInstallations { get; private set; }
        public IBaseRepository<ProjectInstallationReport, long> ProjectInstallationReports { get; private set; }
        public IBaseRepository<ProjectFinishInstallationAttachment, long> ProjectFinishInstallationAttachments { get; private set; }
        public IBaseRepository<VProjectInstallationTotalHour, long> VProjectInstallationTotalHours { get; private set; }
        public IBaseRepository<Bompartition, long> Bompartitions { get; private set; }
        public IBaseRepository<BompartitionItem, long> BompartitionItems { get; private set; }
        public IBaseRepository<ClientAttachment, long> ClientAttachments { get; private set; }
        //public IBaseRepository<InventoryInternalTransferOrder, long> InventoryInternalTransferOrders { get; private set; }
        public IBaseRepository<InventoryInternalTransferOrderItem, long> InventoryInternalTransferOrderItems { get; private set; }
        public IBaseRepository<VInventoryInternalTransferOrder, long> VInventoryInternalTransferOrders { get; private set; }
        public IBaseRepository<InventoryInternalBackOrderItem, long> InventoryInternalBackOrderItems { get; private set; }
        public IBaseRepository<SalesRentOffer, long> SalesRentOffers { get; private set; }
        public IBaseRepository<VPurchaseRequestItemsPo, long> VPurchaseRequestItemsPos { get; private set; }
        public IBaseRepository<MaintenanceReportExpense, long> MaintenanceReportExpenses { get; private set; }
        public IBaseRepository<VehicleModel, int> VehicleModels { get; private set; }
        public IBaseRepository<VehicleMaintenanceType, long> VehicleMaintenanceTypes { get; private set; }
        public IBaseRepository<VehicleMaintenanceTypeForModel, long> VehicleMaintenanceTypeForModels { get; private set; }
        public IBaseRepository<VMaintenanceForDetail, long> VMaintenanceForDetails { get; private set; }
        public IBaseRepository<VisitsScheduleOfMaintenanceAttachment, long> VisitsScheduleOfMaintenanceAttachments { get; private set; }
        public IBaseRepository<TaskUserMonitor, long> TaskUserMonitors { get; private set; }
        public IBaseRepository<VSalesOfferClient, long> VSalesOfferClients { get; private set; }
        public IBaseRepository<DailyReportExpense, long> DailyReportExpenses { get; private set; }
        public IBaseRepository<CrmcontactType, int> CrmcontactTypes { get; private set; }
        public IBaseRepository<CrmrecievedType, int> CrmrecievedTypes { get; private set; }
        public IBaseRepository<CrmreportReason, int> CrmreportReasons { get; private set; }
        public IBaseRepository<DailyReport, long> DailyReports { get; private set; }
        public IBaseRepository<VSalesOffer, long> VSalesOffers { get; private set; }
        public IBaseRepository<BankChequeTemplate, int> BankChequeTemplates { get; private set; }
        public IBaseRepository<VehicleBrand, int> VehicleBrands { get; private set; }
        public IBaseRepository<VehicleServiceScheduleCategory, long> VehicleServiceScheduleCategories { get; private set; }
        public IBaseRepository<SalesBranchTarget, long> SalesBranchTargets { get; private set; }
        public IBaseRepository<SalesBranchProductTarget, long> SalesBranchProductTargets { get; private set; }
        public IBaseRepository<VehicleColor, int> VehicleColors { get; private set; }
        public IBaseRepository<VehicleTransmission, int> VehicleTransmissions { get; private set; }
        public IBaseRepository<VehicleWheelsDrive, int> VehicleWheelsDrives { get; private set; }
        public IBaseRepository<EinvoiceSetting, int> EinvoiceSettings { get; private set; }
        public IBaseRepository<EinvoiceCompanyActivity, long> EinvoiceCompanyActivities { get; private set; }
        public IBaseRepository<VSalesBranchUserTargetTargetYear, int> VSalesBranchUserTargetTargetYears { get; private set; }
        public IBaseRepository<BundleModule, long> BundleModules { get; private set; }
        public IBaseRepository<SupplierFax, long> SupplierFaxes { get; private set; }
        public IBaseRepository<SupplierSpeciality, long> SupplierSpecialities { get; private set; }
        public IBaseRepository<SupplierPhone, long> SupplierPhones { get; private set; }
        public IBaseRepository<SupplierPaymentTerm, long> SupplierPaymentTerms { get; private set; }
        public IBaseRepository<SupplierBankAccount, long> SupplierBankAccounts { get; private set; }
        public IBaseRepository<VSupplierSpeciality, long> VSupplierSpecialities { get; private set; }
        public IBaseRepository<VSupplierAddress, long> VSupplierAddresses { get; private set; }
        public IBaseRepository<SupplierContactPerson, long> SupplierContactPeople { get; private set; }
        public IBaseRepository<SupplierAttachment, long> SupplierAttachments { get; private set; }
        public IBaseRepository<VSupplier, long> VSuppliers { get; private set; }
        public IBaseRepository<ProjectProgressUser, long> ProjectProgressUsers { get; private set; }
        public IBaseRepository<ProductGroup, long> productGroup { get; private set; }
        public IBaseRepository<Product, long> product { get; private set; } 

        // ---------------------------------------------------Hany------------------------------------------------

        public IBaseRepository<LaboratoryMessagesReport , long> LaboratoryMessagesReports { get; private set; }
        public IBaseRepository<MaritalStatus , int> MaritalStatus { get; private set; }
        public IBaseRepository<ReservationInvoice , int> ReservationInvoice { get; private set; }
        public IBaseRepository<Reservation , int> Reservations { get; private set; }
        public IBaseRepository<InvoiceType , int> InvoiceTypes { get; private set; }
        public IBaseRepository<ClientNational , int> clientNationals { get; private set; }
        public IBaseRepository<ClientInformation , int> clientInformations { get; private set; }
        public IBaseRepository<CostType, long> CostTypes { get; private set; }
        public IBaseRepository<TransportationLine, int> TransportationLines { get; private set; }
        public IBaseRepository<TransportationVehicle, int> TransportationVehicles { get; private set; }
        public IBaseRepository<VehicleType, int> VehicleTypes { get; private set; }
        public IBaseRepository<TransportationVehicleRoute, long> TransportationVehicleRoutes { get; private set; }
        public IBaseRepository<TransportationVehicleRouteDirection, long> TransportationVehicleRouteDirections { get; private set; }
        public IBaseRepository<TransportationVehicleRouteEmployee, long> TransportationVehicleRouteEmployees { get; private set; }
        public IBaseRepository<TransportationLineIncreaseRequest, long> TransportationLineIncreaseRequests { get; private set; }
        public IBaseRepository<TransportationLineIncreaseRequestLine, long> TransportationLineIncreaseRequestLines { get; private set; }
        public IBaseRepository<TransportationVehicleRouteDeduction, long> TransportationVehicleRouteDeductions { get; private set; }
        public IBaseRepository<TransportationVehicleRouteAccount, long> TransportationVehicleRouteAccounts { get; private set; }
        public IBaseRepository<TransprotationUserAttedance, long> TransprotationUserAttedances { get; private set; }
        public IBaseRepository<TransportationVehicleRouteEmployeeException , long> TransportationVehicleRouteEmployeeExceptions { get; private set; }
        public IBaseRepository<TransportationRoundForRoute , int> TransportationRoundForRoutes { get; private set; }
        public IBaseRepository<VBusProgram , int> VBusProgram { get; private set; }
        public IBaseRepository<TransportionLineExceptionPrice , int> TransportionLineExceptionPrice { get; private set; }
        public IBaseRepository<TransportionLineSupplierPayment , int> TransportionLineSupplierPayment { get; private set; }
        public IBaseRepository<DistributionSupplierPayment , int> DistributionSupplierPayment { get; private set; }


        //----------------------------------------------------------------------------------
        // Maintenance Contract
        public IBaseRepository<MaintenanceRequestType, int> MaintenanceRequestTypes { get; }
        public IBaseRepository<MangmentOfMaintenanceRequest, long> MangmentOfMaintenanceRequests { get; }
        public IBaseRepository<MangmentOfMaintenanceRequestReport, long> MangmentOfMaintenanceRequestReports { get; }
        public IBaseRepository<MangmentOfMaintenanceRequestReportAttachment, long> MangmentOfMaintenanceRequestReportAttachments { get; }


        // => throw new NotImplementedException();

        //public IBaseRepository<PurchasePotype, long> PurchasePotypes { get; set; }


        /*public IBaseRepository<HrWeekDay, int> HrWeekDay { get; private set; }
public IBaseRepository<Shift, int> Shift { get; private set; }
public IBaseRepository<Vacation, int> Vacation { get; private set; }
public IBaseRepository<OvertimeRates, int> OvertimeRates { get; private set; }
public IBaseRepository<DeductionRates, int> DeductionRates { get; private set; }
public IBaseRepository<AttendenceNPayrollSettings, int> AttendenceNPayrollSettings { get; private set; }*/
        public UnitOfWork(GarasTestContext context)
        {
            _context = context;
            UserSessions = new BaseRepository<UserSession, long>(_context);
            Users = new BaseRepository<User, long>(_context);
            Salaries = new BaseRepository<Salary,long>(_context);
            HrUsers = new HrUserRepository(_context);
            Contracts = new BaseRepository<ContractDetail, long>(_context);
            Branches = new BaseRepository<Branch,int>(_context);
            Departments = new BaseRepository<Department,int>(_context);
            Teams = new BaseRepository<Team,long>(_context);
            UserTeams = new BaseRepository<UserTeam,int>(_context);
            SystemLogs = new BaseRepository<SystemLog,int>(_context);
            Currencies = new BaseRepository<Currency,int>(_context);
            PaymentStrategies = new BaseRepository<PaymentStrategy,int>(_context);
            JobTitles = new BaseRepository<JobTitle, int>(_context);
            WeekDays = new BaseRepository<WeekDay,int>(_context);
            AllowncesTypes = new BaseRepository<AllowncesType, int>(_context);
            SalaryAllownces = new BaseRepository<SalaryAllownce, int>(_context);
            BranchSchedules = new BaseRepository<BranchSchedule,long>(_context);
            SalaryTaxs = new BaseRepository<SalaryTax, long>(_context);
            ContractLeaveSetting = new BaseRepository<ContractLeaveSetting, int>(context);
            DeductionTypes = new BaseRepository<DeductionType,int>(_context);
            OffDays = new BaseRepository<OffDay,long>(_context);
            Attendances = new BaseRepository<Attendance,long>(_context);
            BranchSetting = new BaseRepository<BranchSetting,long>(_context);
            OverTimeAndDeductionRates = new BaseRepository<OverTimeAndDeductionRate,long>(_context);
            VacationOverTimeAndDeductionRates = new BaseRepository<VacationOverTimeAndDeductionRate,long>(context);
            VacationDays = new BaseRepository<VacationDay,long>(_context);
            SalesOffers = new  BaseRepository<SalesOffer,long>(_context);
            WorkingHoursTrackings = new BaseRepository<WorkingHourseTracking,long>(_context);
            DayTypes = new BaseRepository<DayType, int>(_context);
            SalaryTypes = new BaseRepository<SalaryType,int>(_context);
            TaxTypes = new BaseRepository<TaxType,int>(_context);
            Payrolls = new BaseRepository<Payroll,long>(_context);
            ContractLeaveEmployees = new BaseRepository<ContractLeaveEmployee,long>(_context);
            TaskRequirements = new BaseRepository<TaskRequirement, long>(_context);
            LeaveRequests = new BaseRepository<LeaveRequest,long>(_context);
            ProjectInvoices = new BaseRepository<ProjectInvoice,long>(_context);
            ProjectInvoiceItems = new BaseRepository<ProjectInvoiceItem,long>(_context);
            ContractReportTos = new BaseRepository<ContractReportTo,long>(_context);
            ProjectCheques = new BaseRepository<ProjectCheque,long>(_context);
            ChequeCashingStatuses = new BaseRepository<ChequeCashingStatus, int>(_context);
            ProgressTypes = new BaseRepository<ProgressType, int>(_context);
            DeliveryTypes = new BaseRepository<DeliveryType, int>(_context);
            ProgressStatuses = new BaseRepository<ProgressStatus, int>(_context);
            ProjectProgresses = new BaseRepository<ProjectProgress, long>(_context);
            InsuranceCompanyNames = new BaseRepository<InsuranceCompanyName, long>(_context);
            InventoryStores = new BaseRepository<InventoryStore, int>(_context);
            InventoryItems = new BaseRepository<Entities.InventoryItem, long>(_context);
            InventoryStoreItems = new BaseRepository<InventoryStoreItem,long>(_context);
            InventoryMatrialRequests = new BaseRepository<InventoryMatrialRequest,long>(_context);
            InventoryMatrialReleases = new BaseRepository<InventoryMatrialRelease, long>(_context);
            InventoryMatrialRequestItems = new BaseRepository<InventoryMatrialRequestItem, long>(_context);
            InventoryMatrialReleaseItems = new BaseRepository<InventoryMatrialReleaseItem, long>(_context);
            UserPatients = new BaseRepository<UserPatient, long>(_context);
            InventoryStoreKeepers = new BaseRepository<InventoryStoreKeeper, int>(_context);
            UserPatientInsurances = new BaseRepository<UserPatientInsurance, long>(_context);
            VInventoryMatrialReleaseItems = new BaseRepository<VInventoryMatrialReleaseItem, int>(_context);
            VInventoryMatrialRequestItems = new BaseRepository<VInventoryMatrialRequestItem, int>(_context);
            Countries = new BaseRepository<Country, int>(_context);
            Accounts = new BaseRepository<Account, long>(_context);
            AccountOfJournalEntries = new BaseRepository<AccountOfJournalEntry, long>(_context);
            VProjectSalesOfferClients = new BaseRepository<VProjectSalesOfferClient, long>(_context);
            SupplierAccounts = new BaseRepository<SupplierAccount, long>(_context);
            VPurchasePos = new BaseRepository<VPurchasePo, long>(_context);
            VAccountOfJournalEntryWithDailies = new BaseRepository<VAccountOfJournalEntryWithDaily, long>(_context);
            DailyJournalEntryReverses = new BaseRepository<DailyJournalEntryReverse, long>(_context);
            VDailyJournalEntries = new BaseRepository<VDailyJournalEntry, long>(_context);
            PurchasePos = new BaseRepository<PurchasePo, long>(_context);
            VProjectSalesOffers = new BaseRepository<VProjectSalesOffer, long>(_context);
            DailyAdjustingEntryCostCenters = new BaseRepository<DailyAdjustingEntryCostCenter, long>(_context);
            GeneralActiveCostCenters = new BaseRepository<GeneralActiveCostCenter, long>(_context);
            DailyTranactionBeneficiaryToUsers = new BaseRepository<DailyTranactionBeneficiaryToUser, long>(_context);
            Suppliers = new BaseRepository<Supplier, long>(_context);
            AccountFinancialPeriods = new BaseRepository<AccountFinancialPeriod, int>(_context);
            AccountOfJournalEntryOtherCurrencies = new BaseRepository<AccountOfJournalEntryOtherCurrency, long>(_context);
            InvoiceCnandDns = new BaseRepository<InvoiceCnandDn, long>(_context);
            VAccounts = new BaseRepository<VAccount, long>(_context);
            AccountCategories = new BaseRepository<AccountCategory, long>(_context);
            PurchasePaymentMethods = new BaseRepository<PurchasePaymentMethod, long>(_context);
            AdvanciedTypes = new BaseRepository<AdvanciedType, long>(_context);
            VisitsScheduleOfMaintenances = new BaseRepository<VisitsScheduleOfMaintenance, long>(_context);
            SalesOfferLocations = new BaseRepository<SalesOfferLocation, long>(_context);
            VVisitsMaintenanceReportUsers = new BaseRepository<VVisitsMaintenanceReportUser, long>(_context);
            InventoryItemCategories = new BaseRepository<InventoryItemCategory, int>(_context);
            MaintenanceFors = new BaseRepository<MaintenanceFor, long>(_context);
            SalesOfferProducts = new BaseRepository<SalesOfferProduct, long>(_context);
            SalesMaintenanceOffers = new BaseRepository<SalesMaintenanceOffer, long>(_context);
            ManagementOfMaintenanceOrders = new BaseRepository<ManagementOfMaintenanceOrder, long>(_context);
            SalesOfferDiscounts = new BaseRepository<SalesOfferDiscount, long>(_context);
            VInventoryStoreItems = new BaseRepository<VInventoryStoreItem, long>(_context);
            InvoiceItems = new BaseRepository<InvoiceItem, long>(_context);
            VSalesOfferProducts = new BaseRepository<VSalesOfferProduct, long>(_context);
            SalesOfferProductTaxes = new BaseRepository<SalesOfferProductTax, long>(_context);
            VInventoryItems = new BaseRepository<VInventoryItem, long>(_context);
            PosClosingDays = new BaseRepository<PosClosingDay, long>(_context);
            VehiclePerClients = new BaseRepository<VehiclePerClient, long>(_context);
            CompanySerialIdentifications = new BaseRepository<CompanySerialIdentification, int>(_context);
            VClientMobiles = new BaseRepository<VClientMobile, long>(_context);
            ClientContactPeople = new BaseRepository<ClientContactPerson, long>(_context);
            VClientPhones = new BaseRepository<VClientPhone, long>(_context);
            ClientFaxes = new BaseRepository<ClientFax, long>(_context);
            VClientSpecialities = new BaseRepository<VClientSpeciality, long>(_context);
            VClientAddresses = new BaseRepository<VClientAddress, long>(_context);
            VClientUseers = new BaseRepository<VClientUseer, long>(_context);
            VVehicles = new BaseRepository<VVehicle, long>(_context);
            VClientExpiredes = new BaseRepository<VClientExpired, long>(_context);
            InvoiceExtraModifications = new BaseRepository<InvoiceExtraModification, long>(_context);
            VSalesOfferExtraCosts = new BaseRepository<VSalesOfferExtraCost, long>(_context);
            SalesOfferAttachments = new BaseRepository<SalesOfferAttachment, long>(_context);
            SalesOfferItemAttachments = new BaseRepository<SalesOfferItemAttachment, long>(_context);
            SalesOfferInvoiceTaxes = new BaseRepository<SalesOfferInvoiceTax, long>(_context);
            SalesOfferExtraCosts = new BaseRepository<SalesOfferExtraCost, long>(_context);
            Boms = new BaseRepository<Bom, long>(_context);
            VBompartitionItemsInventoryItems = new BaseRepository<VBompartitionItemsInventoryItem, long>(_context);
            VSalesOfferProductSalesOffers = new BaseRepository<VSalesOfferProductSalesOffer, long>(_context);
            VehicleMaintenanceJobOrderHistories = new BaseRepository<VehicleMaintenanceJobOrderHistory, long>(_context);
            CompanySpecialties = new BaseRepository<CompanySpecialty, int>(_context);
            VUserRoles = new BaseRepository<VUserRole, long>(_context);
            InventoryMaterialRequestTypes = new BaseRepository<InventoryMaterialRequestType, long>(_context);
            ProjectFabrications = new BaseRepository<ProjectFabrication, long>(_context);
            ProjectFabricationBOQs = new BaseRepository<ProjectFabricationBoq, long>(_context);
            VInventoryMatrialRequests = new BaseRepository<VInventoryMatrialRequest, long>(_context);
            VPurchaseRequestItems = new BaseRepository<VPurchaseRequestItem, long>(_context);
            VInventoryStoreKeepers = new BaseRepository<VInventoryStoreKeeper, long>(_context);
            VProjectFabricationTotalHours = new BaseRepository<VProjectFabricationTotalHour, long>(_context);
            ProjectInstallations = new BaseRepository<ProjectInstallation, long>(_context);
            ProjectInstallationReports = new BaseRepository<ProjectInstallationReport, long>(_context);
            ProjectFinishInstallationAttachments = new BaseRepository<ProjectFinishInstallationAttachment, long>(_context);
            VProjectInstallationTotalHours = new BaseRepository<VProjectInstallationTotalHour, long>(_context);
            Bompartitions = new BaseRepository<Bompartition, long>(_context);
            BompartitionItems = new BaseRepository<BompartitionItem, long>(_context);
            ClientAttachments = new BaseRepository<ClientAttachment, long>(_context);
            InventoryInternalTransferOrders = new BaseRepository<InventoryInternalTransferOrder, long>(_context);
            InventoryInternalTransferOrderItems = new BaseRepository<InventoryInternalTransferOrderItem, long>(_context);
            VInventoryInternalTransferOrders = new BaseRepository<VInventoryInternalTransferOrder, long>(_context);
            VInventoryInternalBackOrders = new BaseRepository<VInventoryInternalBackOrder, long>(_context);
            InventoryInternalBackOrderItems = new BaseRepository<InventoryInternalBackOrderItem, long>(_context);
            InventoryInternalBackOrders = new BaseRepository<InventoryInternalBackOrder, long>(_context);
            VInventoryMatrialReleases = new BaseRepository<VInventoryMatrialRelease, long>(_context);
            VInventoryInternalBackOrderItems = new BaseRepository<VInventoryInternalBackOrderItem, long>(_context);
            VInventoryInternalTransferOrderItems = new BaseRepository<VInventoryInternalTransferOrderItem, long>(_context);
            InventoryReportItems = new BaseRepository<InventoryReportItem, long>(_context);
            Hrcustodies = new BaseRepository<Hrcustody, long>(_context);
            PurchasePoinvoices = new BaseRepository<PurchasePoinvoice, long>(_context);
            InventoryMatrialReleasePrintInfoes = new BaseRepository<InventoryMatrialReleasePrintInfo, long>(_context);
            SalesRentOffers = new BaseRepository<SalesRentOffer, long>(_context);
            SalesOfferInternalApprovals = new BaseRepository<SalesOfferInternalApproval, long>(_context);
            VPurchaseRequestItemsPos = new BaseRepository<VPurchaseRequestItemsPo, long>(_context);
            MaintenanceReports = new BaseRepository<MaintenanceReport, long>(_context);
            MaintenanceReportUsers = new BaseRepository<MaintenanceReportUser, long>(_context);
            MaintenanceReportExpenses = new BaseRepository<MaintenanceReportExpense, long>(_context);
            VehicleModels = new BaseRepository<VehicleModel, int>(_context);
            VehicleMaintenanceTypes = new BaseRepository<VehicleMaintenanceType, long>(_context);
            VehicleMaintenanceTypeForModels = new BaseRepository<VehicleMaintenanceTypeForModel, long>(_context);
            VMaintenanceForDetails = new BaseRepository<VMaintenanceForDetail, long>(_context);
            VisitsScheduleOfMaintenanceAttachments = new BaseRepository<VisitsScheduleOfMaintenanceAttachment, long>(_context);
            TaskUserMonitors = new BaseRepository<TaskUserMonitor, long>(_context);
            SalesTargets = new BaseRepository<SalesTarget, int>(_context);
            VSalesOfferClients = new BaseRepository<VSalesOfferClient, long>(_context);
            VDailyReportReportLineThroughApis = new BaseRepository<VDailyReportReportLineThroughApi, long>(_context);
            Crmreports = new BaseRepository<Crmreport, long>(_context);
            DailyReportExpenses = new BaseRepository<DailyReportExpense, long>(_context);
            CrmcontactTypes = new BaseRepository<CrmcontactType, int>(_context);
            CrmrecievedTypes = new BaseRepository<CrmrecievedType, int>(_context);
            CrmreportReasons = new BaseRepository<CrmreportReason, int>(_context);
            DailyReports = new BaseRepository<DailyReport, long>(_context);
            VSalesOffers = new BaseRepository<VSalesOffer, long>(_context);
            BankChequeTemplates = new BaseRepository<BankChequeTemplate, int>(_context);
            CategoryTypes = new BaseRepository<CategoryType, int>(_context);
            VehicleBrands = new BaseRepository<VehicleBrand, int>(_context);
            VehicleBodyTypes = new BaseRepository<VehicleBodyType, int>(_context);
            VehicleServiceScheduleCategories = new BaseRepository<VehicleServiceScheduleCategory, long>(_context);
            SalesBranchTargets = new BaseRepository<SalesBranchTarget, long>(_context);
            SalesBranchProductTargets = new BaseRepository<SalesBranchProductTarget, long>(_context);
            VehicleColors = new BaseRepository<VehicleColor, int>(_context);
            VehicleTransmissions = new BaseRepository<VehicleTransmission, int>(_context);
            VehicleWheelsDrives = new BaseRepository<VehicleWheelsDrive, int>(_context);
            EinvoiceSettings = new BaseRepository<EinvoiceSetting, int>(_context);
            EinvoiceCompanyActivities = new BaseRepository<EinvoiceCompanyActivity, long>(_context);
            EinvoiceAttachments = new BaseRepository<EinvoiceAttachment, long>(_context);
            NotificationProcesses = new BaseRepository<NotificationProcess, int>(_context);
            VBranchProducts = new BaseRepository<VBranchProduct, long>(_context);
            VUserBranchGroups = new BaseRepository<VUserBranchGroup, long>(_context);
            VSalesBranchUserTargetTargetYears = new BaseRepository<VSalesBranchUserTargetTargetYear, int>(_context);
            BundleModules = new BaseRepository<BundleModule, long>(_context);
            SupportedBies = new BaseRepository<SupportedBy, long>(_context);
            PermissionLevels = new BaseRepository<PermissionLevel, int>(_context);
            VMatrialReleaseSalesOffers = new BaseRepository<VMatrialReleaseSalesOffer, long>(_context);
            ClientSpecialities = new BaseRepository<ClientSpeciality, long>(_context);
            InventoryReportItemParents = new BaseRepository<InventoryReportItemParent, long>(_context);
            SupplierMobiles = new BaseRepository<SupplierMobile, long>(_context);
            SupplierFaxes = new BaseRepository<SupplierFax, long>(_context);
            SupplierSpecialities = new BaseRepository<SupplierSpeciality, long>(_context);
            SupplierPhones = new BaseRepository<SupplierPhone, long>(_context);
            SupplierPaymentTerms = new BaseRepository<SupplierPaymentTerm, long>(_context);
            SupplierBankAccounts = new BaseRepository<SupplierBankAccount, long>(_context);
            VSupplierSpecialities = new BaseRepository<VSupplierSpeciality, long>(_context);
            VSupplierAddresses = new BaseRepository<VSupplierAddress, long>(_context);
            SupplierContactPeople = new BaseRepository<SupplierContactPerson, long>(_context);
            SupplierAttachments = new BaseRepository<SupplierAttachment, long>(_context);
            VSuppliers = new BaseRepository<VSupplier, long>(_context);
            PurchasePoitems = new BaseRepository<PurchasePoitem, long>(_context);
            InventoryItemAttachments = new BaseRepository<InventoryItemAttachment, long>(_context);
            VPrsupplierOfferItems = new BaseRepository<VPrsupplierOfferItem, long>(_context);
            VClientConsultantAddresses = new BaseRepository<VClientConsultantAddress, long>(_context);
            ClientConsultantFaxes = new BaseRepository<ClientConsultantFax, long>(_context);
            VNotificationsDetails = new BaseRepository<VNotificationsDetail, long>(_context);
            ProjectProgressUsers = new BaseRepository<ProjectProgressUser, long>(_context);
            CostTypes = new BaseRepository<CostType, long>(_context);

            /*UserDeviceConnections = new BaseRepository<UserDeviceConnection, long>(_context);
            InventoryItems = new InventoryItemRepository(_context);
            DashboardManagement = new DashboardManagementRepository(_context);
            //MatrialRequests = new MatrialRequestRepository(_context);
            ContactInfos = new BaseRepository<HrContactInfo, int>(_context);
            UserAddresses = new BaseRepository<HrUserAddress, int>(_context);
            HrNationalities = new BaseRepository<HrNationality, int>(_context);
            HrMilitaryStatuses = new BaseRepository<HrMilitaryStatus, int>(_context);
            HrMaritalStatuses = new BaseRepository<HrMaritalStatus, int>(_context);
            HrBranches = new BaseRepository<HrBranch, int>(_context);
            BranchContactInfos = new BaseRepository<HrBranchContactInfo, int>(_context);
            BranchAddresses = new BaseRepository<HrBranchAddress, int>(_context);
            Grades = new BaseRepository<HrGrade, int>(_context);
            BranchDepartments = new BaseRepository<HrBranchDepartment, int>(_context);
            Teams = new BaseRepository<HrTeam, long>(_context);
            UserTeams = new BaseRepository<HrUserTeam, long>(_context);
            JobDescription = new BaseRepository<HrJobDescription, int>(_context);
            JobDescriptionAttachment = new BaseRepository<HrJobDescriptionAttachment, int>(_context);
            JobResponsabillity = new BaseRepository<HrJobResponsabillity, int>(_context);
            JobResponsbillityAttachment = new BaseRepository<HrJobResponsbillityAttachment, int>(_context);
            WorkInstruction = new BaseRepository<HrWorkInstruction, int>(_context);
            WorkInstructionAttachment = new BaseRepository<HrWorkInstructionAttachment, int>(_context);
            Skill = new BaseRepository<HrSkill, int>(_context);
            SkillAttachment = new BaseRepository<HrSkillAttachment, int>(_context);
            InterviewKpi = new BaseRepository<HrInterviewKpi, int>(_context);
            OperationKpi = new BaseRepository<HrOperationKpi, int>(_context);
            JobTitleDepartment = new BaseRepository<HrJobTitleDepartment, int>(_context);*/
            Country = new BaseRepository<Country, int>(_context);
            City = new BaseRepository<City, int>(_context);
            Area = new BaseRepository<Area, int>(_context);
            ClientAccounts = new BaseRepository<ClientAccount,long>(_context);
            Invoices = new BaseRepository<Invoice,long>(_context);
            AdvanciedSettingAccounts = new BaseRepository<AdvanciedSettingAccount,long>(_context);
            /*HrWeekDay = new BaseRepository<HrWeekDay, int>(_context);
            Shift = new BaseRepository<Shift, int>(_context);
            Vacation = new BaseRepository<Vacation, int>(_context);
            OvertimeRates = new BaseRepository<OvertimeRates, int>(_context);
            DeductionRates = new BaseRepository<DeductionRates, int>(_context);
            AttendenceNPayrollSettings = new BaseRepository<AttendenceNPayrollSettings, int>(_context);*/





            //-----------------------------------------patrick--------------------------------------------
            Tasks = new BaseRepository<Task, long>(_context);
            ExpensisTypes = new BaseRepository<ExpensisType, long>(_context);
            TaskExpensis = new BaseRepository<TaskExpensi, long>(_context);
            Clients = new BaseRepository<Client, long>(_context);
            ProjectContactPersons = new BaseRepository<ProjectContactPerson, long>(_context);
            Prioritys = new BaseRepository<Priority, int>(_context);
            Projects = new BaseRepository<Project, long>(_context);
            ProjectAttachments = new BaseRepository<ProjectAttachment, long>(_context);
            Governorates = new BaseRepository<Governorate, int>(_context);
            Areas = new BaseRepository<Area ,  long>(_context);
            Workflows = new BaseRepository<ProjectWorkFlow, long>(_context);
            ProjectSprints = new BaseRepository<ProjectSprint, long>(_context);
            ProjectCostTypes = new BaseRepository<ProjectCostingType, int>(_context);
            Billingtypes = new BaseRepository<BillingType, int>(_context);
            UserRoles = new BaseRepository<UserRole, long>(_context);
            ProjectAssignUsers = new BaseRepository<ProjectAssignUser, long>(_context);
            SalaryDeductionTaxs = new BaseRepository<SalaryDeductionTax, long>(_context);
            BankDetails = new BaseRepository<BankDetail, int>(_context);
            PaymentMethods = new BaseRepository<PaymentMethod, int>(_context);
            InventoryUoms = new BaseRepository<InventoryUom, int>(_context);
            TaskUnitRateServices = new BaseRepository<TaskUnitRateService, long>(_context);
            ProjectInvoicesCollected = new BaseRepository<ProjectInvoiceCollected, long>(_context);
            TaskUserReplies = new BaseRepository<TaskUserReply, long>(_context);
            TaskDetails = new BaseRepository<TaskDetail, long>(_context);
            TaskTypes = new BaseRepository<TaskType, int>(_context);
            TaskFlagsOwnerRecievers = new BaseRepository<TaskFlagsOwnerReciever, long>(_context);
            ProjectWorkFlows = new BaseRepository<ProjectWorkFlow, long>(_context);
            TaskPermissons = new BaseRepository<TaskPermission, long>(_context);
            PaymentTerms = new BaseRepository<PaymentTerm, int>(_context);
            ProjectPaymentTerms = new BaseRepository<ProjectPaymentTerm, long>(_context);
            ProjectPaymentJournalEntries = new BaseRepository<ProjectPaymentJournalEntry, long>(_context);
            DailyJournalEntries = new BaseRepository<DailyJournalEntry, long>(_context);
            LetterOfCreditTypies = new BaseRepository<LetterOfCreditType, int>(_context);
            ProjectLetterOfCredits = new BaseRepository<ProjectLetterOfCredit, long>(_context);
            ProjectLetterOfCreditComments = new BaseRepository<ProjectLetterOfCreditComment, long>(_context);
            TaskPermissions = new BaseRepository<TaskPermission,  long>(_context);
            GroupUsers = new BaseRepository<GroupUser, long>(_context);
            Groups = new BaseRepository<Group, long>(_context);
            TaskCategories = new BaseRepository<TaskCategory, long>(_context);
            IncomeTypes = new BaseRepository<IncomeType, long>(_context);
            ShippingMethods = new BaseRepository<ShippingMethod, long>(_context);
            CrmContactTypes = new BaseRepository<CrmcontactType, int>(_context);
            CrmRecievedTypes = new BaseRepository<CrmrecievedType, int>(_context);
            DailyReportThroughs = new BaseRepository<DailyReportThrough, int>(_context);
            DeliveryAndShippingMethods = new BaseRepository<DeliveryAndShippingMethod, int>(_context);
            SalesExtraCostTypes =new BaseRepository<SalesExtraCostType, long>(_context);
            DailyTranactionBeneficiaryToGeneralTypes = new BaseRepository<DailyTranactionBeneficiaryToGeneralType, long>(_context);
            DailyTranactionBeneficiaryToTypes = new BaseRepository<DailyTranactionBeneficiaryToType, long>(_context);
            PurchasePoinvoiceTaxIncludedTypes = new BaseRepository<PurchasePoinvoiceTaxIncludedType, long>(_context);
            PurchasePoinvoiceNotIncludedTaxTypes = new BaseRepository<PurchasePoinvoiceNotIncludedTaxType, long>(_context);
            PurchasePoinvoiceExtraFeesTypes = new BaseRepository<PurchasePoinvoiceExtraFeesType, long>(_context);
            PurchasePoinvoiceDeductionTypes = new BaseRepository<PurchasePoinvoiceDeductionType, long> (_context);
            //PurchasePaymentMethods = new BaseRepository<PurchasePaymentMethod, long>(_context);
            Specialities = new BaseRepository<Speciality, int>(_context);
            SpecialitySuppliers = new BaseRepository<SpecialitySupplier, int>(_context);
            TermsAndConditions = new BaseRepository<TermsAndCondition, long>(_context);
            TermsAndConditionsCategories = new BaseRepository<TermsAndConditionsCategory, int>(_context);
            ClientAddresses = new BaseRepository<ClientAddress, long>(_context);
            Roles = new BaseRepository<Role, int>(_context);
            GroupRoles = new BaseRepository<GroupRole, long>(_context);
            Genders = new BaseRepository<Gender, int>(_context);
            ImportantDates = new BaseRepository<ImportantDate, int>(_context);
            RoleModules = new BaseRepository<RoleModule, long>(_context);
            Modules = new BaseRepository<Module, long>(_context);
            Taxes = new BaseRepository<Tax, long>(_context);
            Notifications = new BaseRepository<Notification,long>(_context);
            VInventoryStoreItemPrices = new BaseRepository<VInventoryStoreItemPrice, long>(_context);
            Emails = new BaseRepository<Email, long>(_context);
            EmailCcs = new BaseRepository<EmailCc, long>(_context);
            EmailAttachments = new BaseRepository<EmailAttachment, long>(_context);
            InventoryReports = new BaseRepository<InventoryReport, long>(_context);
            InventoryAddingOrders = new BaseRepository<InventoryAddingOrder, long>(_context);
            InventoryAddingOrderItems = new BaseRepository<InventoryAddingOrderItem, long>(_context);
            InventoryStoreLocations = new BaseRepository<InventoryStoreLocation, int>(_context);
            VInventoryAddingOrders = new BaseRepository<VInventoryAddingOrder, long>(_context);
            PurchasePOItems = new BaseRepository<PurchasePoitem, long>(_context);
            VInventoryAddingOrderItems = new BaseRepository<VInventoryAddingOrderItem, long>(_context);
            VPurchasePoItems = new BaseRepository<VPurchasePoItem, long>(_context);
            PurchasePOInvoices = new BaseRepository<PurchasePoinvoice, long>(_context);
            VInventoryStoreItemMovements = new BaseRepository<VInventoryStoreItemMovement, long>(_context);
            ClientPhones = new BaseRepository<ClientPhone, long>(_context);
            ClientMobiles = new BaseRepository<ClientMobile, long>(_context);
            ClientContactPersons = new BaseRepository<ClientContactPerson, long>(_context);
            VPurchaseRequestItemsPo = new BaseRepository<VPurchaseRequestItemsPo, long>(_context);
            PRSupplierOfferItems = new BaseRepository<PrsupplierOfferItem, long>(_context);
            VCrmreports = new BaseRepository<VCrmreport, long>(_context);
            DailyReportLines = new BaseRepository<DailyReportLine, long>(_context);
            ClientClassifications = new BaseRepository<ClientClassification, long>(_context);
            VProjectSalesOfferClientAccounts = new BaseRepository<VProjectSalesOfferClientAccount, long>(_context);
            ClientConsultants = new BaseRepository<ClientConsultant, long>(_context);
            ClientConsultantAddressis = new BaseRepository<ClientConsultantAddress, long>(_context);
            ClientConsultantFaxs = new BaseRepository<ClientConsultantFax, long>(_context);
            ClientConsultantEmails = new BaseRepository<ClientConsultantEmail, long>(_context);
            ClientConsultantMobiles = new BaseRepository<ClientConsultantMobile, long>(_context);
            ClientConsultantPhones = new BaseRepository<ClientConsultantPhone, long>(_context);
            ClientConsultantSpecialilties = new BaseRepository<ClientConsultantSpecialilty, long>(_context);
            VPurchaseRequests = new BaseRepository<VPurchaseRequest, long >(_context);
            InventoryInternalTransferOrders = new BaseRepository<InventoryInternalTransferOrder, long>(_context);
            VGroupUsers = new BaseRepository<VGroupUser, long>(_context);
            VUserInfos = new BaseRepository<VUserInfo, long>(_context);
            PurchaseRequests = new BaseRepository<PurchaseRequest, long> (_context);
            PurchaseRequestItems = new BaseRepository<PurchaseRequestItem, long>(_context);
            SalesOfferTermsAndConditions = new BaseRepository<SalesOfferTermsAndCondition, long>(_context);
            PurchasePotypes = new BaseRepository<PurchasePotype, long>(_context);
            PRSupplierOffers = new BaseRepository<PrsupplierOffer, long>(_context);
            PurchasePOes = new BaseRepository<PurchasePo, long>(_context);
            TaskBrowserTabs = new BaseRepository<TaskBrowserTab, long>(_context);
            TaskApplicationsOpen = new BaseRepository<TaskApplicationOpen, long>(_context);
            TaskscreenShots = new BaseRepository<TaskScreenShot, long>(_context);
            Nationalities = new BaseRepository<Nationality, long>(_context);
            VPurchasePoitemInvoicePoSuppliers = new BaseRepository<VPurchasePoitemInvoicePoSupplier, long>(_context);
            InventoryReportAttachments = new BaseRepository<InventoryReportAttachment , long>(_context);
            PurchaseImportPOSettings = new BaseRepository<PurchaseImportPosetting,  long>(_context);
            PurchasePOAttachments = new BaseRepository<PurchasePoattachment, long>(_context);
            PuchasePOShipments = new BaseRepository<PuchasePoshipment, long>(_context);
            PurchasePOShipmentDocuments = new BaseRepository<PurchasePoshipmentDocument, long>(_context);
            PurchasePOShipmentShippingMethodDetails = new BaseRepository<PurchasePoshipmentShippingMethodDetail, long>(_context);
            EmailTypes = new BaseRepository<EmailType, int>(_context);
            EmailCategories = new BaseRepository<EmailCategory, long>(_context);
            EmailCategoryTypes = new BaseRepository<EmailCategoryType, long>(_context);
            NotificationSubscriptions = new BaseRepository<NotificationSubscription, long>(_context);
            ContactThroughs =  new BaseRepository<ContactThrough, int>(_context);
            PofinalSelecteSuppliers = new BaseRepository<PofinalSelecteSupplier, long>(_context);
            PurchasePoinvoiceExtraFees = new BaseRepository<PurchasePoinvoiceExtraFee, long>(_context);
            SupplierAddresses = new BaseRepository<SupplierAddress, long>(_context);
            VPoapprovalStatus = new BaseRepository<VPoapprovalStatus, long>(_context);
            PurchasePoinvoiceTaxIncluds = new BaseRepository<PurchasePoinvoiceTaxIncluded, long>(_context);
            PurchasePoinvoiceTypes = new BaseRepository<PurchasePoinvoiceType, long>(_context);
            SalesBranchUserTargets = new BaseRepository<SalesBranchUserTarget, long>(_context);
            VInvoices = new BaseRepository<VInvoice, long>(_context);
            WorkshopStations = new BaseRepository<WorkshopStation, long>(_context);
            ProjectFabricationWorkshopStationHistories = new BaseRepository<ProjectFabricationWorkshopStationHistory, long>(_context);
            ProjectWorkshopStations = new BaseRepository<ProjectWorkshopStation,  long>(_context);
            VProjectFabricationProjectOfferEntities = new BaseRepository<VProjectFabricationProjectOfferEntity, long>(_context);
            ContractTypes = new BaseRepository<ContractType, long>(_context);
            VInventoryMatrialRequestWithItems = new BaseRepository<VInventoryMatrialRequestWithItem, long>(_context);
            VInventoryStoreItemPriceReports = new BaseRepository<VInventoryStoreItemPriceReport, long>(_context);
            InventoryItemContents = new BaseRepository<InventoryItemContent, long>(_context);
            VehicleIssuers = new BaseRepository<VehicleIssuer, long>(_context);
            VehicleMaintenanceTypeServiceSheduleCategories = new BaseRepository<VehicleMaintenanceTypeServiceSheduleCategory, long>(_context);
            EmailReceivers = new BaseRepository<EmailReceiver, long>(_context);
            HremployeeAttachments = new BaseRepository<HremployeeAttachment, long>(_context);
            AttendancePaySlips = new BaseRepository<AttendancePaySlip, long>(_context);
            WorkingHours = new BaseRepository<WorkingHour, long>(_context);
            WeekEnds = new BaseRepository<WeekEnd, long>(_context);
            VGroupUserBranchs = new BaseRepository<VGroupUserBranch, long>(_context);
            MedicalDoctorScheduleStatus = new BaseRepository<MedicalDoctorScheduleStatus, int>(_context);
            MedicalDoctorPercentageTypes = new BaseRepository<MedicalDoctorPercentageType, int>(_context) ;
            MedicalPatientTypes = new BaseRepository<MedicalPatientType, int>(_context);
            DoctorSchedules = new BaseRepository<DoctorSchedule, long>(_context);
            MedicalExaminationOffers = new BaseRepository<MedicalExaminationOffer, long>(_context);
            MedicalReservations = new BaseRepository<MedicalReservation, long>(_context);
            MedicalDailyTreasuryBalances = new BaseRepository<MedicalDailyTreasuryBalance, long>(_context);
            productGroup = new BaseRepository<ProductGroup, long >(_context);
            product = new BaseRepository<Product,long>(_context);
            DoctorRooms = new BaseRepository<DoctorRoom,long>(_context);
            ClientExtraInfos = new BaseRepository<ClientExtraInfo, long>(_context);
            HREmployeeAttachments = new BaseRepository<HremployeeAttachment, long>(_context);
            //-----------------------------------------------------HANY----------------------------------------------
            LaboratoryMessagesReports =new BaseRepository<LaboratoryMessagesReport , long>(_context);
            MaritalStatus=new BaseRepository<MaritalStatus , int>(_context);
            ReservationInvoice=new BaseRepository<ReservationInvoice , int>(_context);
            clientNationals=new BaseRepository<ClientNational , int>(_context);
            clientInformations=new BaseRepository<ClientInformation , int>(_context);


            VClientSalesPeople = new BaseRepository<VClientSalesPerson, long>(_context);
            ClientSalesPeople = new BaseRepository<ClientSalesPerson, long>(_context);
            ClientPaymentTerms = new BaseRepository<ClientPaymentTerm, long>(_context);
            ClientBankAccounts = new BaseRepository<ClientBankAccount, long>(_context);
            Reservations= new BaseRepository<Reservation, int>(_context);
            InvoiceTypes= new BaseRepository<InvoiceType, int>(_context);
            PosNumbers= new BaseRepository<PosNumber, int>(_context);

            TransportationLines = new BaseRepository<TransportationLine , int>(_context);
            TransportationVehicles= new BaseRepository<TransportationVehicle , int>(_context);
            VehicleTypes= new BaseRepository<VehicleType , int>(_context);
            TransportationVehicleRoutes= new BaseRepository<TransportationVehicleRoute , long>(_context);
            TransportationVehicleRouteDirections= new BaseRepository<TransportationVehicleRouteDirection , long>(_context);
            TransportationVehicleRouteEmployees= new BaseRepository<TransportationVehicleRouteEmployee , long>(_context);
            TransportationLineIncreaseRequests= new BaseRepository<TransportationLineIncreaseRequest , long>(_context);
            TransportationLineIncreaseRequestLines= new BaseRepository<TransportationLineIncreaseRequestLine , long>(_context);
            TransportationVehicleRouteDeductions= new BaseRepository<TransportationVehicleRouteDeduction , long>(_context);
            TransportationVehicleRouteAccounts= new BaseRepository<TransportationVehicleRouteAccount , long>(_context);
            TransprotationUserAttedances= new BaseRepository<TransprotationUserAttedance , long>(_context);
            TransportationVehicleRouteEmployeeExceptions=new BaseRepository<TransportationVehicleRouteEmployeeException , long>(_context);
            TransportationRoundForRoutes=new BaseRepository<TransportationRoundForRoute , int>(_context);
            VBusProgram=new BaseRepository<VBusProgram , int>(_context);
            TransportionLineExceptionPrice=new BaseRepository<TransportionLineExceptionPrice , int>(_context);
            TransportionLineSupplierPayment=new BaseRepository<TransportionLineSupplierPayment , int>(_context);
            DistributionSupplierPayment=new BaseRepository<DistributionSupplierPayment , int>(_context);


            //----------------------------------------------------------------------------------
            // Maintenance Contract
            MaintenanceRequestTypes= new BaseRepository<MaintenanceRequestType, int>(_context);
            MangmentOfMaintenanceRequests  = new BaseRepository<MangmentOfMaintenanceRequest, long>(_context);
            MangmentOfMaintenanceRequestReports  = new BaseRepository<MangmentOfMaintenanceRequestReport, long>(_context);
            MangmentOfMaintenanceRequestReportAttachments  = new BaseRepository<MangmentOfMaintenanceRequestReportAttachment, long>(_context);
            MangmentOfMaintenanceRequestAttachments = new BaseRepository<MangmentOfMaintenanceRequestAttachment, long>(_context);

        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }



        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }
    }
}
