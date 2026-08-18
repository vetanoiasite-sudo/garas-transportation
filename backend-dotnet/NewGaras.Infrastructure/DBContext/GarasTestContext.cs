using Microsoft.EntityFrameworkCore;
using NewGaras.Infrastructure.Entities;
using NewGaras.Infrastructure.Helper.TenantService;
using System;
using System.Collections.Generic;
using Task = NewGaras.Infrastructure.Entities.Task;

namespace NewGaras.Infrastructure.DBContext;

public partial class GarasTestContext : DbContext
{
    public string TenantId { get; set; }
    private readonly ITenantService _tenantService;
    public GarasTestContext(ITenantService tenantService)
    {
        _tenantService = tenantService;
        TenantId = _tenantService.GetTenant()?.TID;
    }

    public GarasTestContext(DbContextOptions options, ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
        TenantId = _tenantService.GetTenant()?.TID;
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountCategory> AccountCategories { get; set; }

    public virtual DbSet<AccountFinancialPeriod> AccountFinancialPeriods { get; set; }

    public virtual DbSet<AccountFinancialPeriodAttachment> AccountFinancialPeriodAttachments { get; set; }

    public virtual DbSet<AccountOfAdjustingEntry> AccountOfAdjustingEntries { get; set; }

    public virtual DbSet<AccountOfJournalEntry> AccountOfJournalEntries { get; set; }

    public virtual DbSet<AccountOfJournalEntryOtherCurrency> AccountOfJournalEntryOtherCurrencies { get; set; }

    public virtual DbSet<AdvanciedSettingAccount> AdvanciedSettingAccounts { get; set; }

    public virtual DbSet<AdvanciedType> AdvanciedTypes { get; set; }

    public virtual DbSet<AdvanciedTypeSettingCsv> AdvanciedTypeSettingCsvs { get; set; }

    public virtual DbSet<AllowncesType> AllowncesTypes { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<AttachmentCategory> AttachmentCategories { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<AttendancePaySlip> AttendancePaySlips { get; set; }

    public virtual DbSet<BankChequeTemplate> BankChequeTemplates { get; set; }

    public virtual DbSet<BankDetail> BankDetails { get; set; }

    public virtual DbSet<BillingType> BillingTypes { get; set; }

    public virtual DbSet<Bom> Boms { get; set; }

    public virtual DbSet<Bomattachment> Bomattachments { get; set; }

    public virtual DbSet<Bomhistory> Bomhistories { get; set; }

    public virtual DbSet<Bomimage> Bomimages { get; set; }

    public virtual DbSet<Bomlibrary> Bomlibraries { get; set; }

    public virtual DbSet<Bompartition> Bompartitions { get; set; }

    public virtual DbSet<BompartitionAttachment> BompartitionAttachments { get; set; }

    public virtual DbSet<BompartitionHistory> BompartitionHistories { get; set; }

    public virtual DbSet<BompartitionItem> BompartitionItems { get; set; }

    public virtual DbSet<BompartitionItemAttachment> BompartitionItemAttachments { get; set; }

    public virtual DbSet<Bomproduct> Bomproducts { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<BranchProduct> BranchProducts { get; set; }

    public virtual DbSet<BranchSchedule> BranchSchedules { get; set; }

    public virtual DbSet<BranchSetting> BranchSettings { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<BundleModule> BundleModules { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarsAttachment> CarsAttachments { get; set; }

    public virtual DbSet<CategoryType> CategoryTypes { get; set; }

    public virtual DbSet<ChequeCashingStatus> ChequeCashingStatuses { get; set; }

    public virtual DbSet<Childern> Childerns { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientAccount> ClientAccounts { get; set; }

    public virtual DbSet<ClientAddress> ClientAddresses { get; set; }

    public virtual DbSet<ClientAttachment> ClientAttachments { get; set; }

    public virtual DbSet<ClientBankAccount> ClientBankAccounts { get; set; }

    public virtual DbSet<ClientClassification> ClientClassifications { get; set; }

    public virtual DbSet<ClientConsultant> ClientConsultants { get; set; }

    public virtual DbSet<ClientConsultantAddress> ClientConsultantAddresses { get; set; }

    public virtual DbSet<ClientConsultantEmail> ClientConsultantEmails { get; set; }

    public virtual DbSet<ClientConsultantFax> ClientConsultantFaxes { get; set; }

    public virtual DbSet<ClientConsultantMobile> ClientConsultantMobiles { get; set; }

    public virtual DbSet<ClientConsultantPhone> ClientConsultantPhones { get; set; }

    public virtual DbSet<ClientConsultantSpecialilty> ClientConsultantSpecialilties { get; set; }

    public virtual DbSet<ClientContactPerson> ClientContactPeople { get; set; }

    public virtual DbSet<ClientExtraInfo> ClientExtraInfos { get; set; }

    public virtual DbSet<ClientFax> ClientFaxes { get; set; }

    public virtual DbSet<ClientInformation> ClientInformations { get; set; }

    public virtual DbSet<ClientLanguagee> ClientLanguagees { get; set; }

    public virtual DbSet<ClientMobile> ClientMobiles { get; set; }

    public virtual DbSet<ClientNational> ClientNationals { get; set; }

    public virtual DbSet<ClientPaymentTerm> ClientPaymentTerms { get; set; }

    public virtual DbSet<ClientPhone> ClientPhones { get; set; }

    public virtual DbSet<ClientSalesPerson> ClientSalesPeople { get; set; }

    public virtual DbSet<ClientSession> ClientSessions { get; set; }

    public virtual DbSet<ClientSpeciality> ClientSpecialities { get; set; }

    public virtual DbSet<Collect> Collects { get; set; }

    public virtual DbSet<CompanySerialIdentification> CompanySerialIdentifications { get; set; }

    public virtual DbSet<CompanySpecialty> CompanySpecialties { get; set; }

    public virtual DbSet<ConfirmedRecieveAndRelease> ConfirmedRecieveAndReleases { get; set; }

    public virtual DbSet<ConfirmedRecieveAndReleaseAttachment> ConfirmedRecieveAndReleaseAttachments { get; set; }

    public virtual DbSet<ContactThrough> ContactThroughs { get; set; }

    public virtual DbSet<ContractDetail> ContractDetails { get; set; }

    public virtual DbSet<ContractLeaveEmployee> ContractLeaveEmployees { get; set; }

    public virtual DbSet<ContractLeaveSetting> ContractLeaveSettings { get; set; }

    public virtual DbSet<ContractReportTo> ContractReportTos { get; set; }

    public virtual DbSet<ContractType> ContractTypes { get; set; }

    public virtual DbSet<CostType> CostTypes { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CrmcontactType> CrmcontactTypes { get; set; }

    public virtual DbSet<CrmrecievedType> CrmrecievedTypes { get; set; }

    public virtual DbSet<Crmreport> Crmreports { get; set; }

    public virtual DbSet<CrmreportReason> CrmreportReasons { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<DailyAdjustingEntry> DailyAdjustingEntries { get; set; }

    public virtual DbSet<DailyAdjustingEntryCostCenter> DailyAdjustingEntryCostCenters { get; set; }

    public virtual DbSet<DailyJournalEntry> DailyJournalEntries { get; set; }

    public virtual DbSet<DailyJournalEntryReverse> DailyJournalEntryReverses { get; set; }

    public virtual DbSet<DailyReport> DailyReports { get; set; }

    public virtual DbSet<DailyReportAttachment> DailyReportAttachments { get; set; }

    public virtual DbSet<DailyReportExpense> DailyReportExpenses { get; set; }

    public virtual DbSet<DailyReportLine> DailyReportLines { get; set; }

    public virtual DbSet<DailyReportThrough> DailyReportThroughs { get; set; }

    public virtual DbSet<DailyTranactionAttachment> DailyTranactionAttachments { get; set; }

    public virtual DbSet<DailyTranactionBeneficiaryToGeneralType> DailyTranactionBeneficiaryToGeneralTypes { get; set; }

    public virtual DbSet<DailyTranactionBeneficiaryToType> DailyTranactionBeneficiaryToTypes { get; set; }

    public virtual DbSet<DailyTranactionBeneficiaryToUser> DailyTranactionBeneficiaryToUsers { get; set; }

    public virtual DbSet<DailyTransaction> DailyTransactions { get; set; }

    public virtual DbSet<DailyTransactionCostCenter> DailyTransactionCostCenters { get; set; }

    public virtual DbSet<DayType> DayTypes { get; set; }

    public virtual DbSet<DeductionType> DeductionTypes { get; set; }

    public virtual DbSet<DeliveryAndShippingMethod> DeliveryAndShippingMethods { get; set; }

    public virtual DbSet<DeliveryType> DeliveryTypes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DistributionSupplierPayment> DistributionSupplierPayments { get; set; }

    public virtual DbSet<DoctorRoom> DoctorRooms { get; set; }

    public virtual DbSet<DoctorSchedule> DoctorSchedules { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<DriversAttachment> DriversAttachments { get; set; }

    public virtual DbSet<EinvoiceAttachment> EinvoiceAttachments { get; set; }

    public virtual DbSet<EinvoiceCompanyActivity> EinvoiceCompanyActivities { get; set; }

    public virtual DbSet<EinvoiceSetting> EinvoiceSettings { get; set; }

    public virtual DbSet<Email> Emails { get; set; }

    public virtual DbSet<EmailAttachment> EmailAttachments { get; set; }

    public virtual DbSet<EmailCategory> EmailCategories { get; set; }

    public virtual DbSet<EmailCategoryType> EmailCategoryTypes { get; set; }

    public virtual DbSet<EmailCc> EmailCcs { get; set; }

    public virtual DbSet<EmailReceiver> EmailReceivers { get; set; }

    public virtual DbSet<EmailType> EmailTypes { get; set; }

    public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }

    public virtual DbSet<ExpensessStatus> ExpensessStatuses { get; set; }

    public virtual DbSet<ExpensisType> ExpensisTypes { get; set; }

    public virtual DbSet<Extra> Extras { get; set; }

    public virtual DbSet<ExtraCostLibrary> ExtraCostLibraries { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<GarasClientInfo> GarasClientInfos { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<GeneralActiveCostCenter> GeneralActiveCostCenters { get; set; }

    public virtual DbSet<GfUser> GfUsers { get; set; }

    public virtual DbSet<Governorate> Governorates { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupRole> GroupRoles { get; set; }

    public virtual DbSet<GroupUser> GroupUsers { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<HrUser> HrUsers { get; set; }

    public virtual DbSet<Hrcustody> Hrcustodies { get; set; }

    public virtual DbSet<HrcustodyReportAttachment> HrcustodyReportAttachments { get; set; }

    public virtual DbSet<HrcustodyStatus> HrcustodyStatuses { get; set; }

    public virtual DbSet<HrdeductionRewarding> HrdeductionRewardings { get; set; }

    public virtual DbSet<HremployeeAttachment> HremployeeAttachments { get; set; }

    public virtual DbSet<Hrloan> Hrloans { get; set; }

    public virtual DbSet<HrloanApprovalStatus> HrloanApprovalStatuses { get; set; }

    public virtual DbSet<HrloanRefundStrategy> HrloanRefundStrategies { get; set; }

    public virtual DbSet<HrloanStatus> HrloanStatuses { get; set; }

    public virtual DbSet<HruserWarning> HruserWarnings { get; set; }

    public virtual DbSet<HruserWarningActionPlanApproval> HruserWarningActionPlanApprovals { get; set; }

    public virtual DbSet<HruserWarningStatus> HruserWarningStatuses { get; set; }

    public virtual DbSet<ImportantDate> ImportantDates { get; set; }

    public virtual DbSet<IncomeType> IncomeTypes { get; set; }

    public virtual DbSet<InsuranceCompanyName> InsuranceCompanyNames { get; set; }

    public virtual DbSet<Interview> Interviews { get; set; }

    public virtual DbSet<InventoryAddingOrder> InventoryAddingOrders { get; set; }

    public virtual DbSet<InventoryAddingOrderItem> InventoryAddingOrderItems { get; set; }

    public virtual DbSet<InventoryInternalBackOrder> InventoryInternalBackOrders { get; set; }

    public virtual DbSet<InventoryInternalBackOrderItem> InventoryInternalBackOrderItems { get; set; }

    public virtual DbSet<InventoryInternalTransferOrder> InventoryInternalTransferOrders { get; set; }

    public virtual DbSet<InventoryInternalTransferOrderItem> InventoryInternalTransferOrderItems { get; set; }

    public virtual DbSet<InventoryItem> InventoryItems { get; set; }

    public virtual DbSet<InventoryItemAttachment> InventoryItemAttachments { get; set; }

    public virtual DbSet<InventoryItemCategory> InventoryItemCategories { get; set; }

    public virtual DbSet<InventoryItemContent> InventoryItemContents { get; set; }

    public virtual DbSet<InventoryItemPrice> InventoryItemPrices { get; set; }

    public virtual DbSet<InventoryItemUom> InventoryItemUoms { get; set; }

    public virtual DbSet<InventoryMaterialRequestType> InventoryMaterialRequestTypes { get; set; }

    public virtual DbSet<InventoryMatrialRelease> InventoryMatrialReleases { get; set; }

    public virtual DbSet<InventoryMatrialReleaseItem> InventoryMatrialReleaseItems { get; set; }

    public virtual DbSet<InventoryMatrialReleasePrintInfo> InventoryMatrialReleasePrintInfos { get; set; }

    public virtual DbSet<InventoryMatrialRequest> InventoryMatrialRequests { get; set; }

    public virtual DbSet<InventoryMatrialRequestItem> InventoryMatrialRequestItems { get; set; }

    public virtual DbSet<InventoryReport> InventoryReports { get; set; }

    public virtual DbSet<InventoryReportAttachment> InventoryReportAttachments { get; set; }

    public virtual DbSet<InventoryReportItem> InventoryReportItems { get; set; }

    public virtual DbSet<InventoryReportItemParent> InventoryReportItemParents { get; set; }

    public virtual DbSet<InventoryStore> InventoryStores { get; set; }

    public virtual DbSet<InventoryStoreItem> InventoryStoreItems { get; set; }

    public virtual DbSet<InventoryStoreKeeper> InventoryStoreKeepers { get; set; }

    public virtual DbSet<InventoryStoreLocation> InventoryStoreLocations { get; set; }

    public virtual DbSet<InventoryUom> InventoryUoms { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceCnandDn> InvoiceCnandDns { get; set; }

    public virtual DbSet<InvoiceExtraCost> InvoiceExtraCosts { get; set; }

    public virtual DbSet<InvoiceExtraModification> InvoiceExtraModifications { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<InvoiceNewClient> InvoiceNewClients { get; set; }

    public virtual DbSet<InvoiceOfProject> InvoiceOfProjects { get; set; }

    public virtual DbSet<InvoiceTax> InvoiceTaxes { get; set; }

    public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }

    public virtual DbSet<JobInformation> JobInformations { get; set; }

    public virtual DbSet<JobTitle> JobTitles { get; set; }

    public virtual DbSet<LaboratoryMessagesReport> LaboratoryMessagesReports { get; set; }

    public virtual DbSet<Languagee> Languagees { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<LetterOfCreditType> LetterOfCreditTypes { get; set; }

    public virtual DbSet<MaintenanceFor> MaintenanceFors { get; set; }

    public virtual DbSet<MaintenanceReport> MaintenanceReports { get; set; }

    public virtual DbSet<MaintenanceReportAttachment> MaintenanceReportAttachments { get; set; }

    public virtual DbSet<MaintenanceReportClarification> MaintenanceReportClarifications { get; set; }

    public virtual DbSet<MaintenanceReportClarificationAttachment> MaintenanceReportClarificationAttachments { get; set; }

    public virtual DbSet<MaintenanceReportExpense> MaintenanceReportExpenses { get; set; }

    public virtual DbSet<MaintenanceReportUser> MaintenanceReportUsers { get; set; }

    public virtual DbSet<MaintenanceRequestType> MaintenanceRequestTypes { get; set; }

    public virtual DbSet<ManageStage> ManageStages { get; set; }

    public virtual DbSet<ManagementOfMaintenanceOrder> ManagementOfMaintenanceOrders { get; set; }

    public virtual DbSet<ManagementOfRentOrder> ManagementOfRentOrders { get; set; }

    public virtual DbSet<ManagementOfRentOrderAttachment> ManagementOfRentOrderAttachments { get; set; }

    public virtual DbSet<MangmentOfMaintenanceRequest> MangmentOfMaintenanceRequests { get; set; }

    public virtual DbSet<MangmentOfMaintenanceRequestAttachment> MangmentOfMaintenanceRequestAttachments { get; set; }

    public virtual DbSet<MangmentOfMaintenanceRequestReport> MangmentOfMaintenanceRequestReports { get; set; }

    public virtual DbSet<MangmentOfMaintenanceRequestReportAttachment> MangmentOfMaintenanceRequestReportAttachments { get; set; }

    public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }

    public virtual DbSet<MealType> MealTypes { get; set; }

    public virtual DbSet<MedicalDailyTreasuryBalance> MedicalDailyTreasuryBalances { get; set; }

    public virtual DbSet<MedicalDoctorPercentageType> MedicalDoctorPercentageTypes { get; set; }

    public virtual DbSet<MedicalDoctorScheduleStatus> MedicalDoctorScheduleStatuses { get; set; }

    public virtual DbSet<MedicalExaminationOffer> MedicalExaminationOffers { get; set; }

    public virtual DbSet<MedicalPatientType> MedicalPatientTypes { get; set; }

    public virtual DbSet<MedicalReservation> MedicalReservations { get; set; }

    public virtual DbSet<MilitaryStatus> MilitaryStatuses { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<MovementReport> MovementReports { get; set; }

    public virtual DbSet<MovementReportAttachment> MovementReportAttachments { get; set; }

    public virtual DbSet<MovementsAndDeliveryOrder> MovementsAndDeliveryOrders { get; set; }

    public virtual DbSet<National> Nationals { get; set; }

    public virtual DbSet<Nationality> Nationalities { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationProcess> NotificationProcesses { get; set; }

    public virtual DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

    public virtual DbSet<OffDay> OffDays { get; set; }

    public virtual DbSet<OverTimeAndDeductionRate> OverTimeAndDeductionRates { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentStrategy> PaymentStrategies { get; set; }

    public virtual DbSet<PaymentTerm> PaymentTerms { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<PermissionLevel> PermissionLevels { get; set; }

    public virtual DbSet<PoapprovalSetting> PoapprovalSettings { get; set; }

    public virtual DbSet<PoapprovalStatus> PoapprovalStatuses { get; set; }

    public virtual DbSet<PoapprovalUser> PoapprovalUsers { get; set; }

    public virtual DbSet<PofinalSelecteSupplier> PofinalSelecteSuppliers { get; set; }

    public virtual DbSet<PosClosingDay> PosClosingDays { get; set; }

    public virtual DbSet<PosNumber> PosNumbers { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Pricing> Pricings { get; set; }

    public virtual DbSet<PricingBom> PricingBoms { get; set; }

    public virtual DbSet<PricingClarificationAttachment> PricingClarificationAttachments { get; set; }

    public virtual DbSet<PricingClearfication> PricingClearfications { get; set; }

    public virtual DbSet<PricingExtraCost> PricingExtraCosts { get; set; }

    public virtual DbSet<PricingProduct> PricingProducts { get; set; }

    public virtual DbSet<PricingProductAttachment> PricingProductAttachments { get; set; }

    public virtual DbSet<PricingTerm> PricingTerms { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    public virtual DbSet<ProAutoAccountTreeTemp> ProAutoAccountTreeTemps { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductGroup> ProductGroups { get; set; }

    public virtual DbSet<ProgressStatus> ProgressStatuses { get; set; }

    public virtual DbSet<ProgressType> ProgressTypes { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectAssignUser> ProjectAssignUsers { get; set; }

    public virtual DbSet<ProjectAttachment> ProjectAttachments { get; set; }

    public virtual DbSet<ProjectCheque> ProjectCheques { get; set; }

    public virtual DbSet<ProjectContactPerson> ProjectContactPeople { get; set; }

    public virtual DbSet<ProjectCostingType> ProjectCostingTypes { get; set; }

    public virtual DbSet<ProjectFabrication> ProjectFabrications { get; set; }

    public virtual DbSet<ProjectFabricationAttachment> ProjectFabricationAttachments { get; set; }

    public virtual DbSet<ProjectFabricationBoq> ProjectFabricationBoqs { get; set; }

    public virtual DbSet<ProjectFabricationJobTitle> ProjectFabricationJobTitles { get; set; }

    public virtual DbSet<ProjectFabricationOrderUser> ProjectFabricationOrderUsers { get; set; }

    public virtual DbSet<ProjectFabricationReport> ProjectFabricationReports { get; set; }

    public virtual DbSet<ProjectFabricationReportAttachment> ProjectFabricationReportAttachments { get; set; }

    public virtual DbSet<ProjectFabricationReportClarification> ProjectFabricationReportClarifications { get; set; }

    public virtual DbSet<ProjectFabricationReportClarificationAttachment> ProjectFabricationReportClarificationAttachments { get; set; }

    public virtual DbSet<ProjectFabricationReportUser> ProjectFabricationReportUsers { get; set; }

    public virtual DbSet<ProjectFabricationVersion> ProjectFabricationVersions { get; set; }

    public virtual DbSet<ProjectFabricationWorkshopStationHistory> ProjectFabricationWorkshopStationHistories { get; set; }

    public virtual DbSet<ProjectFinishInstallationAttachment> ProjectFinishInstallationAttachments { get; set; }

    public virtual DbSet<ProjectInstallAttachment> ProjectInstallAttachments { get; set; }

    public virtual DbSet<ProjectInstallation> ProjectInstallations { get; set; }

    public virtual DbSet<ProjectInstallationAttachment> ProjectInstallationAttachments { get; set; }

    public virtual DbSet<ProjectInstallationBoq> ProjectInstallationBoqs { get; set; }

    public virtual DbSet<ProjectInstallationJobTitle> ProjectInstallationJobTitles { get; set; }

    public virtual DbSet<ProjectInstallationReport> ProjectInstallationReports { get; set; }

    public virtual DbSet<ProjectInstallationReportAttachment> ProjectInstallationReportAttachments { get; set; }

    public virtual DbSet<ProjectInstallationReportClarification> ProjectInstallationReportClarifications { get; set; }

    public virtual DbSet<ProjectInstallationReportClarificationAttachment> ProjectInstallationReportClarificationAttachments { get; set; }

    public virtual DbSet<ProjectInstallationReportUser> ProjectInstallationReportUsers { get; set; }

    public virtual DbSet<ProjectInstallationVersion> ProjectInstallationVersions { get; set; }

    public virtual DbSet<ProjectInvoice> ProjectInvoices { get; set; }

    public virtual DbSet<ProjectInvoiceCollected> ProjectInvoiceCollecteds { get; set; }

    public virtual DbSet<ProjectInvoiceItem> ProjectInvoiceItems { get; set; }

    public virtual DbSet<ProjectLetterOfCredit> ProjectLetterOfCredits { get; set; }

    public virtual DbSet<ProjectLetterOfCreditComment> ProjectLetterOfCreditComments { get; set; }

    public virtual DbSet<ProjectPaymentJournalEntry> ProjectPaymentJournalEntries { get; set; }

    public virtual DbSet<ProjectPaymentTerm> ProjectPaymentTerms { get; set; }

    public virtual DbSet<ProjectProgress> ProjectProgresses { get; set; }

    public virtual DbSet<ProjectProgressUser> ProjectProgressUsers { get; set; }

    public virtual DbSet<ProjectSprint> ProjectSprints { get; set; }

    public virtual DbSet<ProjectTm> ProjectTms { get; set; }

    public virtual DbSet<ProjectTmassignUser> ProjectTmassignUsers { get; set; }

    public virtual DbSet<ProjectTmattachment> ProjectTmattachments { get; set; }

    public virtual DbSet<ProjectTmimpDate> ProjectTmimpDates { get; set; }

    public virtual DbSet<ProjectTmrevision> ProjectTmrevisions { get; set; }

    public virtual DbSet<ProjectTmsprint> ProjectTmsprints { get; set; }

    public virtual DbSet<ProjectWorkFlow> ProjectWorkFlows { get; set; }

    public virtual DbSet<ProjectWorkshopStation> ProjectWorkshopStations { get; set; }

    public virtual DbSet<PrsupplierOffer> PrsupplierOffers { get; set; }

    public virtual DbSet<PrsupplierOfferItem> PrsupplierOfferItems { get; set; }

    public virtual DbSet<PuchasePoshipment> PuchasePoshipments { get; set; }

    public virtual DbSet<PurchaseImportPosetting> PurchaseImportPosettings { get; set; }

    public virtual DbSet<PurchasePaymentMethod> PurchasePaymentMethods { get; set; }

    public virtual DbSet<PurchasePo> PurchasePos { get; set; }

    public virtual DbSet<PurchasePoamountPaymentMethod> PurchasePoamountPaymentMethods { get; set; }

    public virtual DbSet<PurchasePoattachment> PurchasePoattachments { get; set; }

    public virtual DbSet<PurchasePoinactiveTask> PurchasePoinactiveTasks { get; set; }

    public virtual DbSet<PurchasePoinvoice> PurchasePoinvoices { get; set; }

    public virtual DbSet<PurchasePoinvoiceAttachment> PurchasePoinvoiceAttachments { get; set; }

    public virtual DbSet<PurchasePoinvoiceCalculatedShipmentValue> PurchasePoinvoiceCalculatedShipmentValues { get; set; }

    public virtual DbSet<PurchasePoinvoiceClosedPayment> PurchasePoinvoiceClosedPayments { get; set; }

    public virtual DbSet<PurchasePoinvoiceDeduction> PurchasePoinvoiceDeductions { get; set; }

    public virtual DbSet<PurchasePoinvoiceDeductionType> PurchasePoinvoiceDeductionTypes { get; set; }

    public virtual DbSet<PurchasePoinvoiceExtraFee> PurchasePoinvoiceExtraFees { get; set; }

    public virtual DbSet<PurchasePoinvoiceExtraFeesType> PurchasePoinvoiceExtraFeesTypes { get; set; }

    public virtual DbSet<PurchasePoinvoiceFinalExpensi> PurchasePoinvoiceFinalExpenses { get; set; }

    public virtual DbSet<PurchasePoinvoiceNotIncludedTax> PurchasePoinvoiceNotIncludedTaxes { get; set; }

    public virtual DbSet<PurchasePoinvoiceNotIncludedTaxType> PurchasePoinvoiceNotIncludedTaxTypes { get; set; }

    public virtual DbSet<PurchasePoinvoiceTaxIncluded> PurchasePoinvoiceTaxIncludeds { get; set; }

    public virtual DbSet<PurchasePoinvoiceTaxIncludedType> PurchasePoinvoiceTaxIncludedTypes { get; set; }

    public virtual DbSet<PurchasePoinvoiceTotalOrderCustomFee> PurchasePoinvoiceTotalOrderCustomFees { get; set; }

    public virtual DbSet<PurchasePoinvoiceType> PurchasePoinvoiceTypes { get; set; }

    public virtual DbSet<PurchasePoinvoiceUnloadingFee> PurchasePoinvoiceUnloadingFees { get; set; }

    public virtual DbSet<PurchasePoitem> PurchasePoitems { get; set; }

    public virtual DbSet<PurchasePopaymentSwift> PurchasePopaymentSwifts { get; set; }

    public virtual DbSet<PurchasePopdf> PurchasePopdfs { get; set; }

    public virtual DbSet<PurchasePopdfEditHistory> PurchasePopdfEditHistories { get; set; }

    public virtual DbSet<PurchasePopdfTemplate> PurchasePopdfTemplates { get; set; }

    public virtual DbSet<PurchasePoshipmentDocument> PurchasePoshipmentDocuments { get; set; }

    public virtual DbSet<PurchasePoshipmentShippingMethodDetail> PurchasePoshipmentShippingMethodDetails { get; set; }

    public virtual DbSet<PurchasePotype> PurchasePotypes { get; set; }

    public virtual DbSet<PurchaseRequest> PurchaseRequests { get; set; }

    public virtual DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportCcgroup> ReportCcgroups { get; set; }

    public virtual DbSet<ReportCcuser> ReportCcusers { get; set; }

    public virtual DbSet<ReportGroup> ReportGroups { get; set; }

    public virtual DbSet<ReportUser> ReportUsers { get; set; }

    public virtual DbSet<RequieredCost> RequieredCosts { get; set; }

    public virtual DbSet<RequieredCostAttachment> RequieredCostAttachments { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<ReservationInvoice> ReservationInvoices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleModule> RoleModules { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomFacility> RoomFacilities { get; set; }

    public virtual DbSet<RoomService> RoomServices { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<RoomView> RoomViews { get; set; }

    public virtual DbSet<RoomsReservation> RoomsReservations { get; set; }

    public virtual DbSet<RoomsReservationChildern> RoomsReservationChilderns { get; set; }

    public virtual DbSet<RoomsReservationMeal> RoomsReservationMeals { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<SalaryAllownce> SalaryAllownces { get; set; }

    public virtual DbSet<SalaryDeductionTax> SalaryDeductionTaxes { get; set; }

    public virtual DbSet<SalaryInsurance> SalaryInsurances { get; set; }

    public virtual DbSet<SalaryTax> SalaryTaxes { get; set; }

    public virtual DbSet<SalaryType> SalaryTypes { get; set; }

    public virtual DbSet<SalesBranchProductTarget> SalesBranchProductTargets { get; set; }

    public virtual DbSet<SalesBranchTarget> SalesBranchTargets { get; set; }

    public virtual DbSet<SalesBranchUserProductTarget> SalesBranchUserProductTargets { get; set; }

    public virtual DbSet<SalesBranchUserTarget> SalesBranchUserTargets { get; set; }

    public virtual DbSet<SalesExtraCostType> SalesExtraCostTypes { get; set; }

    public virtual DbSet<SalesMaintenanceOffer> SalesMaintenanceOffers { get; set; }

    public virtual DbSet<SalesOffer> SalesOffers { get; set; }

    public virtual DbSet<SalesOfferAttachment> SalesOfferAttachments { get; set; }

    public virtual DbSet<SalesOfferAttachmentGroupPermission> SalesOfferAttachmentGroupPermissions { get; set; }

    public virtual DbSet<SalesOfferAttachmentUserPermission> SalesOfferAttachmentUserPermissions { get; set; }

    public virtual DbSet<SalesOfferDiscount> SalesOfferDiscounts { get; set; }

    public virtual DbSet<SalesOfferEditHistory> SalesOfferEditHistories { get; set; }

    public virtual DbSet<SalesOfferExpirationHistory> SalesOfferExpirationHistories { get; set; }

    public virtual DbSet<SalesOfferExtraCost> SalesOfferExtraCosts { get; set; }

    public virtual DbSet<SalesOfferGroupPermission> SalesOfferGroupPermissions { get; set; }

    public virtual DbSet<SalesOfferInternalApproval> SalesOfferInternalApprovals { get; set; }

    public virtual DbSet<SalesOfferInvoiceTax> SalesOfferInvoiceTaxes { get; set; }

    public virtual DbSet<SalesOfferItemAttachment> SalesOfferItemAttachments { get; set; }

    public virtual DbSet<SalesOfferLocation> SalesOfferLocations { get; set; }

    public virtual DbSet<SalesOfferPdf> SalesOfferPdfs { get; set; }

    public virtual DbSet<SalesOfferPdfTemplate> SalesOfferPdfTemplates { get; set; }

    public virtual DbSet<SalesOfferProduct> SalesOfferProducts { get; set; }

    public virtual DbSet<SalesOfferProductTax> SalesOfferProductTaxes { get; set; }

    public virtual DbSet<SalesOfferTermsAndCondition> SalesOfferTermsAndConditions { get; set; }

    public virtual DbSet<SalesOfferUserPermission> SalesOfferUserPermissions { get; set; }

    public virtual DbSet<SalesRentOffer> SalesRentOffers { get; set; }

    public virtual DbSet<SalesTarget> SalesTargets { get; set; }

    public virtual DbSet<Sheet2> Sheet2s { get; set; }

    public virtual DbSet<ShippingCompany> ShippingCompanies { get; set; }

    public virtual DbSet<ShippingCompanyAttachment> ShippingCompanyAttachments { get; set; }

    public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }

    public virtual DbSet<Speciality> Specialities { get; set; }

    public virtual DbSet<SpecialitySupplier> SpecialitySuppliers { get; set; }

    public virtual DbSet<Stage> Stages { get; set; }

    public virtual DbSet<StatusReservation> StatusReservations { get; set; }

    public virtual DbSet<StpTaxTypeV> StpTaxTypeVs { get; set; }

    public virtual DbSet<SubmittedReport> SubmittedReports { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierAccount> SupplierAccounts { get; set; }

    public virtual DbSet<SupplierAccountReviewed> SupplierAccountRevieweds { get; set; }

    public virtual DbSet<SupplierAddress> SupplierAddresses { get; set; }

    public virtual DbSet<SupplierAttachment> SupplierAttachments { get; set; }

    public virtual DbSet<SupplierBankAccount> SupplierBankAccounts { get; set; }

    public virtual DbSet<SupplierContactPerson> SupplierContactPeople { get; set; }

    public virtual DbSet<SupplierFax> SupplierFaxes { get; set; }

    public virtual DbSet<SupplierMobile> SupplierMobiles { get; set; }

    public virtual DbSet<SupplierPaymentTerm> SupplierPaymentTerms { get; set; }

    public virtual DbSet<SupplierPhone> SupplierPhones { get; set; }

    public virtual DbSet<SupplierSpeciality> SupplierSpecialities { get; set; }

    public virtual DbSet<SupportedBy> SupportedBies { get; set; }

    public virtual DbSet<SystemLog> SystemLogs { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskApplicationOpen> TaskApplicationOpens { get; set; }

    public virtual DbSet<TaskAssignUser> TaskAssignUsers { get; set; }

    public virtual DbSet<TaskAttachment> TaskAttachments { get; set; }

    public virtual DbSet<TaskBrowserTab> TaskBrowserTabs { get; set; }

    public virtual DbSet<TaskCategory> TaskCategories { get; set; }

    public virtual DbSet<TaskClosureLog> TaskClosureLogs { get; set; }

    public virtual DbSet<TaskComment> TaskComments { get; set; }

    public virtual DbSet<TaskCommentAttachment> TaskCommentAttachments { get; set; }

    public virtual DbSet<TaskDetail> TaskDetails { get; set; }

    public virtual DbSet<TaskExpensi> TaskExpenses { get; set; }

    public virtual DbSet<TaskFlagsOwnerReciever> TaskFlagsOwnerRecievers { get; set; }

    public virtual DbSet<TaskHistory> TaskHistories { get; set; }

    public virtual DbSet<TaskInfo> TaskInfos { get; set; }

    public virtual DbSet<TaskInfoRevision> TaskInfoRevisions { get; set; }

    public virtual DbSet<TaskPermission> TaskPermissions { get; set; }

    public virtual DbSet<TaskPrimarySubCategory> TaskPrimarySubCategories { get; set; }

    public virtual DbSet<TaskRequirement> TaskRequirements { get; set; }

    public virtual DbSet<TaskScreenShot> TaskScreenShots { get; set; }

    public virtual DbSet<TaskSecondarySubCategory> TaskSecondarySubCategories { get; set; }

    public virtual DbSet<TaskStageHistory> TaskStageHistories { get; set; }

    public virtual DbSet<TaskType> TaskTypes { get; set; }

    public virtual DbSet<TaskUnitRateService> TaskUnitRateServices { get; set; }

    public virtual DbSet<TaskUserMonitor> TaskUserMonitors { get; set; }

    public virtual DbSet<TaskUserReply> TaskUserReplies { get; set; }

    public virtual DbSet<Tax> Taxes { get; set; }

    public virtual DbSet<TaxType> TaxTypes { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TermsAndCondition> TermsAndConditions { get; set; }

    public virtual DbSet<TermsAndConditionsCategory> TermsAndConditionsCategories { get; set; }

    public virtual DbSet<TermsGroup> TermsGroups { get; set; }

    public virtual DbSet<TermsLibrary> TermsLibraries { get; set; }

    public virtual DbSet<TransportationLine> TransportationLines { get; set; }

    public virtual DbSet<TransportationLineIncreaseRequest> TransportationLineIncreaseRequests { get; set; }

    public virtual DbSet<TransportationLineIncreaseRequestLine> TransportationLineIncreaseRequestLines { get; set; }

    public virtual DbSet<TransportationRoundForRoute> TransportationRoundForRoutes { get; set; }

    public virtual DbSet<TransportationVehicle> TransportationVehicles { get; set; }

    public virtual DbSet<TransportationVehicleRoute> TransportationVehicleRoutes { get; set; }

    public virtual DbSet<TransportationVehicleRouteAccount> TransportationVehicleRouteAccounts { get; set; }

    public virtual DbSet<TransportationVehicleRouteDeduction> TransportationVehicleRouteDeductions { get; set; }

    public virtual DbSet<TransportationVehicleRouteDirection> TransportationVehicleRouteDirections { get; set; }

    public virtual DbSet<TransportationVehicleRouteEmployee> TransportationVehicleRouteEmployees { get; set; }

    public virtual DbSet<TransportationVehicleRouteEmployeeException> TransportationVehicleRouteEmployeeExceptions { get; set; }

    public virtual DbSet<TransportionLineExceptionPrice> TransportionLineExceptionPrices { get; set; }

    public virtual DbSet<TransportionLineSupplierPayment> TransportionLineSupplierPayments { get; set; }

    public virtual DbSet<TransprotationUserAttedance> TransprotationUserAttedances { get; set; }

    public virtual DbSet<TypeService> TypeServices { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPatient> UserPatients { get; set; }

    public virtual DbSet<UserPatientInsurance> UserPatientInsurances { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    public virtual DbSet<UserTeam> UserTeams { get; set; }

    public virtual DbSet<UserTimer> UserTimers { get; set; }

    public virtual DbSet<UserViewNotification> UserViewNotifications { get; set; }

    public virtual DbSet<VAccount> VAccounts { get; set; }

    public virtual DbSet<VAccountOfJournalEntryWithDaily> VAccountOfJournalEntryWithDailies { get; set; }

    public virtual DbSet<VAccountsAdvanciedSettingAccount> VAccountsAdvanciedSettingAccounts { get; set; }

    public virtual DbSet<VBompartitionItemsInventoryItem> VBompartitionItemsInventoryItems { get; set; }

    public virtual DbSet<VBranchProduct> VBranchProducts { get; set; }

    public virtual DbSet<VBranchUser> VBranchUsers { get; set; }

    public virtual DbSet<VBusProgram> VBusPrograms { get; set; }

    public virtual DbSet<VClientAccountsProject> VClientAccountsProjects { get; set; }

    public virtual DbSet<VClientAddress> VClientAddresses { get; set; }

    public virtual DbSet<VClientConsultantAddress> VClientConsultantAddresses { get; set; }

    public virtual DbSet<VClientExpired> VClientExpireds { get; set; }

    public virtual DbSet<VClientMobile> VClientMobiles { get; set; }

    public virtual DbSet<VClientPhone> VClientPhones { get; set; }

    public virtual DbSet<VClientSalesPerson> VClientSalesPeople { get; set; }

    public virtual DbSet<VClientSpeciality> VClientSpecialities { get; set; }

    public virtual DbSet<VClientUseer> VClientUseers { get; set; }

    public virtual DbSet<VCountry> VCountries { get; set; }

    public virtual DbSet<VCrmreport> VCrmreports { get; set; }

    public virtual DbSet<VCustodyRequestReleaseBackOrder> VCustodyRequestReleaseBackOrders { get; set; }

    public virtual DbSet<VDailyJournalEntry> VDailyJournalEntries { get; set; }

    public virtual DbSet<VDailyReport> VDailyReports { get; set; }

    public virtual DbSet<VDailyReportLineThrough> VDailyReportLineThroughs { get; set; }

    public virtual DbSet<VDailyReportReportLineThrough> VDailyReportReportLineThroughs { get; set; }

    public virtual DbSet<VDailyReportReportLineThroughApi> VDailyReportReportLineThroughApis { get; set; }

    public virtual DbSet<VGroupRoleUser> VGroupRoleUsers { get; set; }

    public virtual DbSet<VGroupUser> VGroupUsers { get; set; }

    public virtual DbSet<VGroupUserBranch> VGroupUserBranches { get; set; }

    public virtual DbSet<VInventoryAddingOrder> VInventoryAddingOrders { get; set; }

    public virtual DbSet<VInventoryAddingOrderItem> VInventoryAddingOrderItems { get; set; }

    public virtual DbSet<VInventoryAddingOrderItemsDetail> VInventoryAddingOrderItemsDetails { get; set; }

    public virtual DbSet<VInventoryCategory> VInventoryCategories { get; set; }

    public virtual DbSet<VInventoryInternalBackOrder> VInventoryInternalBackOrders { get; set; }

    public virtual DbSet<VInventoryInternalBackOrderItem> VInventoryInternalBackOrderItems { get; set; }

    public virtual DbSet<VInventoryInternalTransferOrder> VInventoryInternalTransferOrders { get; set; }

    public virtual DbSet<VInventoryInternalTransferOrderItem> VInventoryInternalTransferOrderItems { get; set; }

    public virtual DbSet<VInventoryItem> VInventoryItems { get; set; }

    public virtual DbSet<VInventoryItemInventoryItemCategory> VInventoryItemInventoryItemCategories { get; set; }

    public virtual DbSet<VInventoryMatrialRelease> VInventoryMatrialReleases { get; set; }

    public virtual DbSet<VInventoryMatrialReleaseItem> VInventoryMatrialReleaseItems { get; set; }

    public virtual DbSet<VInventoryMatrialReleaseUnionInternalBackOrderItem> VInventoryMatrialReleaseUnionInternalBackOrderItems { get; set; }

    public virtual DbSet<VInventoryMatrialRequest> VInventoryMatrialRequests { get; set; }

    public virtual DbSet<VInventoryMatrialRequestItem> VInventoryMatrialRequestItems { get; set; }

    public virtual DbSet<VInventoryMatrialRequestWithItem> VInventoryMatrialRequestWithItems { get; set; }

    public virtual DbSet<VInventoryStoreItem> VInventoryStoreItems { get; set; }

    public virtual DbSet<VInventoryStoreItemMinBalance> VInventoryStoreItemMinBalances { get; set; }

    public virtual DbSet<VInventoryStoreItemMovement> VInventoryStoreItemMovements { get; set; }

    public virtual DbSet<VInventoryStoreItemPrice> VInventoryStoreItemPrices { get; set; }

    public virtual DbSet<VInventoryStoreItemPriceReport> VInventoryStoreItemPriceReports { get; set; }

    public virtual DbSet<VInventoryStoreKeeper> VInventoryStoreKeepers { get; set; }

    public virtual DbSet<VInvoice> VInvoices { get; set; }

    public virtual DbSet<VMainFabInsReport> VMainFabInsReports { get; set; }

    public virtual DbSet<VMaintenanceForDetail> VMaintenanceForDetails { get; set; }

    public virtual DbSet<VMaintenanceHoursReport> VMaintenanceHoursReports { get; set; }

    public virtual DbSet<VMatrialReleaseSalesOffer> VMatrialReleaseSalesOffers { get; set; }

    public virtual DbSet<VNotificationsDetail> VNotificationsDetails { get; set; }

    public virtual DbSet<VPoapprovalStatus> VPoapprovalStatuses { get; set; }

    public virtual DbSet<VPricingFullDatum> VPricingFullData { get; set; }

    public virtual DbSet<VPricingProduct> VPricingProducts { get; set; }

    public virtual DbSet<VProductTargetChart> VProductTargetCharts { get; set; }

    public virtual DbSet<VProjectFabricationProjectOffer> VProjectFabricationProjectOffers { get; set; }

    public virtual DbSet<VProjectFabricationProjectOfferEntity> VProjectFabricationProjectOfferEntities { get; set; }

    public virtual DbSet<VProjectFabricationTotalHour> VProjectFabricationTotalHours { get; set; }

    public virtual DbSet<VProjectInstallationProjectOffer> VProjectInstallationProjectOffers { get; set; }

    public virtual DbSet<VProjectInstallationTotalHour> VProjectInstallationTotalHours { get; set; }

    public virtual DbSet<VProjectReturnSalesOffer> VProjectReturnSalesOffers { get; set; }

    public virtual DbSet<VProjectSalesOffer> VProjectSalesOffers { get; set; }

    public virtual DbSet<VProjectSalesOfferClient> VProjectSalesOfferClients { get; set; }

    public virtual DbSet<VProjectSalesOfferClientAccount> VProjectSalesOfferClientAccounts { get; set; }

    public virtual DbSet<VProjectSalesOfferEntity> VProjectSalesOfferEntities { get; set; }

    public virtual DbSet<VPrsupplierOfferItem> VPrsupplierOfferItems { get; set; }

    public virtual DbSet<VPurchasePo> VPurchasePos { get; set; }

    public virtual DbSet<VPurchasePoItem> VPurchasePoItems { get; set; }

    public virtual DbSet<VPurchasePoItemPo> VPurchasePoItemPos { get; set; }

    public virtual DbSet<VPurchasePoSupplierAccountAmount> VPurchasePoSupplierAccountAmounts { get; set; }

    public virtual DbSet<VPurchasePoinactiveTask> VPurchasePoinactiveTasks { get; set; }

    public virtual DbSet<VPurchasePoitemInvoicePo> VPurchasePoitemInvoicePos { get; set; }

    public virtual DbSet<VPurchasePoitemInvoicePoSupplier> VPurchasePoitemInvoicePoSuppliers { get; set; }

    public virtual DbSet<VPurchaseRequest> VPurchaseRequests { get; set; }

    public virtual DbSet<VPurchaseRequestItem> VPurchaseRequestItems { get; set; }

    public virtual DbSet<VPurchaseRequestItemsPo> VPurchaseRequestItemsPos { get; set; }

    public virtual DbSet<VPurchaseRequestItemsPoAo> VPurchaseRequestItemsPoAos { get; set; }

    public virtual DbSet<VSalesBranchUserTargetTargetYear> VSalesBranchUserTargetTargetYears { get; set; }

    public virtual DbSet<VSalesMaintenanceOfferReport> VSalesMaintenanceOfferReports { get; set; }

    public virtual DbSet<VSalesOffer> VSalesOffers { get; set; }

    public virtual DbSet<VSalesOfferClient> VSalesOfferClients { get; set; }

    public virtual DbSet<VSalesOfferClientFullDatum> VSalesOfferClientFullData { get; set; }

    public virtual DbSet<VSalesOfferDiff> VSalesOfferDiffs { get; set; }

    public virtual DbSet<VSalesOfferExtraCost> VSalesOfferExtraCosts { get; set; }

    public virtual DbSet<VSalesOfferProduct> VSalesOfferProducts { get; set; }

    public virtual DbSet<VSalesOfferProductSalesOffer> VSalesOfferProductSalesOffers { get; set; }

    public virtual DbSet<VSalesOfferUser> VSalesOfferUsers { get; set; }

    public virtual DbSet<VSupplier> VSuppliers { get; set; }

    public virtual DbSet<VSupplierAddress> VSupplierAddresses { get; set; }

    public virtual DbSet<VSupplierSpeciality> VSupplierSpecialities { get; set; }

    public virtual DbSet<VTaskMangerProject> VTaskMangerProjects { get; set; }

    public virtual DbSet<VUserBranchGroup> VUserBranchGroups { get; set; }

    public virtual DbSet<VUserDetail> VUserDetails { get; set; }

    public virtual DbSet<VUserInfo> VUserInfos { get; set; }

    public virtual DbSet<VUserRole> VUserRoles { get; set; }

    public virtual DbSet<VVehicle> VVehicles { get; set; }

    public virtual DbSet<VVisitsMaintenanceReportUser> VVisitsMaintenanceReportUsers { get; set; }

    public virtual DbSet<VacationDay> VacationDays { get; set; }

    public virtual DbSet<VacationOverTimeAndDeductionRate> VacationOverTimeAndDeductionRates { get; set; }

    public virtual DbSet<VacationPaymentStrategy> VacationPaymentStrategies { get; set; }

    public virtual DbSet<VehicleBodyType> VehicleBodyTypes { get; set; }

    public virtual DbSet<VehicleBrand> VehicleBrands { get; set; }

    public virtual DbSet<VehicleColor> VehicleColors { get; set; }

    public virtual DbSet<VehicleIssuer> VehicleIssuers { get; set; }

    public virtual DbSet<VehicleMaintenanceJobOrderHistory> VehicleMaintenanceJobOrderHistories { get; set; }

    public virtual DbSet<VehicleMaintenanceType> VehicleMaintenanceTypes { get; set; }

    public virtual DbSet<VehicleMaintenanceTypeForModel> VehicleMaintenanceTypeForModels { get; set; }

    public virtual DbSet<VehicleMaintenanceTypePriorityLevel> VehicleMaintenanceTypePriorityLevels { get; set; }

    public virtual DbSet<VehicleMaintenanceTypeRate> VehicleMaintenanceTypeRates { get; set; }

    public virtual DbSet<VehicleMaintenanceTypeServiceSheduleCategory> VehicleMaintenanceTypeServiceSheduleCategories { get; set; }

    public virtual DbSet<VehicleModel> VehicleModels { get; set; }

    public virtual DbSet<VehiclePerClient> VehiclePerClients { get; set; }

    public virtual DbSet<VehicleServiceScheduleCategory> VehicleServiceScheduleCategories { get; set; }

    public virtual DbSet<VehicleTransmission> VehicleTransmissions { get; set; }

    public virtual DbSet<VehicleType> VehicleTypes { get; set; }

    public virtual DbSet<VehicleWheelsDrive> VehicleWheelsDrives { get; set; }

    public virtual DbSet<VisitsScheduleOfMaintenance> VisitsScheduleOfMaintenances { get; set; }

    public virtual DbSet<VisitsScheduleOfMaintenanceAttachment> VisitsScheduleOfMaintenanceAttachments { get; set; }

    public virtual DbSet<WeekDay> WeekDays { get; set; }

    public virtual DbSet<WeekEnd> WeekEnds { get; set; }

    public virtual DbSet<WorkingHour> WorkingHours { get; set; }

    public virtual DbSet<WorkingHourseTracking> WorkingHourseTrackings { get; set; }

    public virtual DbSet<WorkshopStation> WorkshopStations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenantConnectionString = _tenantService.GetConnectionString();
        if (!string.IsNullOrEmpty(tenantConnectionString))
        {
            var DBProvider = _tenantService.GetDatabaseProvider();
            if (DBProvider.ToLower() == "mssql")
            {
                optionsBuilder.UseSqlServer(tenantConnectionString);
            }
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AS");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasOne(d => d.AccountCategory).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_AccountCategory");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AccountCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.Accounts).HasConstraintName("FK_Accounts_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AccountModifiedByNavigations).HasConstraintName("FK_Accounts_User1");

            entity.HasOne(d => d.Tax).WithMany(p => p.Accounts).HasConstraintName("FK_Accounts_Tax");
        });

        modelBuilder.Entity<AccountCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AccountType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AccountCategoryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AccountCategoryModifiedByNavigations).HasConstraintName("FK_AccountType_User1");
        });

        modelBuilder.Entity<AccountFinancialPeriodAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<AccountOfAdjustingEntry>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.AccountOfAdjustingEntries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfAdjustingEntry_Accounts");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AccountOfAdjustingEntryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfAdjustingEntry_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AccountOfAdjustingEntryModifiedByNavigations).HasConstraintName("FK_AccountOfAdjustingEntry_User1");
        });

        modelBuilder.Entity<AccountOfJournalEntry>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.AccountOfJournalEntries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfJournalEntry_Accounts");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AccountOfJournalEntryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfJournalEntry_User");

            entity.HasOne(d => d.Entry).WithMany(p => p.AccountOfJournalEntries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfJournalEntry_DailyJournalEntry");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AccountOfJournalEntryModifiedByNavigations).HasConstraintName("FK_AccountOfJournalEntry_User1");
        });

        modelBuilder.Entity<AccountOfJournalEntryOtherCurrency>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Account).WithMany(p => p.AccountOfJournalEntryOtherCurrencies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfJournalEntryOtherCurrency_Accounts");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AccountOfJournalEntryOtherCurrencyCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfJournalEntryOtherCurrency_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.AccountOfJournalEntryOtherCurrencies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfJournalEntryOtherCurrency_Currency");

            entity.HasOne(d => d.Entry).WithMany(p => p.AccountOfJournalEntryOtherCurrencies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfJournalEntryOtherCurrency_DailyJournalEntry");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AccountOfJournalEntryOtherCurrencyModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountOfJournalEntryOtherCurrency_User1");
        });

        modelBuilder.Entity<AdvanciedSettingAccount>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.AdvanciedSettingAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdvanciedSettingAccount_Accounts");

            entity.HasOne(d => d.AdvanciedType).WithMany(p => p.AdvanciedSettingAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdvanciedSettingAccount_AdvanciedType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AdvanciedSettingAccountCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdvanciedSettingAccount_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AdvanciedSettingAccountModifiedByNavigations).HasConstraintName("FK_AdvanciedSettingAccount_User1");
        });

        modelBuilder.Entity<AdvanciedType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AdvanciedTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdvanciedType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AdvanciedTypeModifiedByNavigations).HasConstraintName("FK_AdvanciedType_User1");
        });

        modelBuilder.Entity<AllowncesType>(entity =>
        {
            entity.HasOne(d => d.Currency).WithMany(p => p.AllowncesTypes).HasConstraintName("FK_AllowncesType_Currency");

            entity.HasOne(d => d.SalaryType).WithMany(p => p.AllowncesTypes).HasConstraintName("FK_AllowncesType_SalaryType");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AreaCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Area_User");

            entity.HasOne(d => d.Governorate).WithMany(p => p.Areas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Area_Governorate");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AreaModifiedByNavigations).HasConstraintName("FK_Area_User1");
        });

        modelBuilder.Entity<AttachmentCategory>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.AbsenceType).WithMany(p => p.Attendances)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Attendance_ContractLeaveSetting");

            entity.HasOne(d => d.ApprovedByUser).WithMany(p => p.AttendanceApprovedByUsers).HasConstraintName("FK_Attendance_User1");

            entity.HasOne(d => d.Branch).WithMany(p => p.Attendances)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Attendance_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AttendanceCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_User2");

            entity.HasOne(d => d.DayType).WithMany(p => p.Attendances).HasConstraintName("FK_Attendance_DayType");

            entity.HasOne(d => d.Department).WithMany(p => p.Attendances).HasConstraintName("FK_Attendance_Department");

            entity.HasOne(d => d.Employee).WithMany(p => p.AttendanceEmployees).HasConstraintName("FK_Attendance_User");

            entity.HasOne(d => d.HrUser).WithMany(p => p.Attendances).HasConstraintName("FK_Attendance_HrUser");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AttendanceModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_User3");

            entity.HasOne(d => d.Team).WithMany(p => p.Attendances).HasConstraintName("FK_Attendance_Team");
        });

        modelBuilder.Entity<AttendancePaySlip>(entity =>
        {
            entity.Property(e => e.IsCompleted).HasDefaultValue(true);
        });

        modelBuilder.Entity<BankChequeTemplate>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BankChequeTemplateCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankChequeTemplate_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BankChequeTemplateModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankChequeTemplate_User1");
        });

        modelBuilder.Entity<BankDetail>(entity =>
        {
            entity.HasOne(d => d.HrUser).WithMany(p => p.BankDetails).HasConstraintName("FK_BankDetail_HrUser");
        });

        modelBuilder.Entity<BillingType>(entity =>
        {
            entity.Property(e => e.BillingTypeName).IsFixedLength();
        });

        modelBuilder.Entity<Bom>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BomCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOM_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BomModifiedByNavigations).HasConstraintName("FK_BOM_User1");

            entity.HasOne(d => d.ProductGroup).WithMany(p => p.Boms).HasConstraintName("FK_BOM_InventoryItemCategory");

            entity.HasOne(d => d.Product).WithMany(p => p.Boms).HasConstraintName("FK_BOM_InventoryItem");
        });

        modelBuilder.Entity<Bomattachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Bom).WithMany(p => p.Bomattachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMAttachments_BOM");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BomattachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMAttachments_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BomattachmentModifiedByNavigations).HasConstraintName("FK_BOMAttachments_User1");
        });

        modelBuilder.Entity<Bomhistory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Bom).WithMany(p => p.Bomhistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMHistory_BOM");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BomhistoryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMHistory_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BomhistoryModifiedByNavigations).HasConstraintName("FK_BOMHistory_User1");
        });

        modelBuilder.Entity<Bomimage>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Bom).WithMany(p => p.Bomimages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMImages_BOM");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BomimageCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMImages_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BomimageModifiedByNavigations).HasConstraintName("FK_BOMImages_User1");
        });

        modelBuilder.Entity<Bomlibrary>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Global).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BomlibraryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMLibrary_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.Bomlibraries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMLibrary_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BomlibraryModifiedByNavigations).HasConstraintName("FK_BOMLibrary_User1");

            entity.HasOne(d => d.Product).WithMany(p => p.Bomlibraries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMLibrary_Product");
        });

        modelBuilder.Entity<Bompartition>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Bom).WithMany(p => p.Bompartitions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitions_BOM");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BompartitionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitions_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BompartitionModifiedByNavigations).HasConstraintName("FK_BOMPartitions_User1");
        });

        modelBuilder.Entity<BompartitionAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Bompartition).WithMany(p => p.BompartitionAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitionAttachments_BOMPartitions");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BompartitionAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitionAttachments_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BompartitionAttachmentModifiedByNavigations).HasConstraintName("FK_BOMPartitionAttachments_User1");
        });

        modelBuilder.Entity<BompartitionHistory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Bompartition).WithMany(p => p.BompartitionHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitionHistory_BOMPartitions");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BompartitionHistoryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitionHistory_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BompartitionHistoryModifiedByNavigations).HasConstraintName("FK_BOMPartitionHistory_User1");
        });

        modelBuilder.Entity<BompartitionItem>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.ActiveToUse).HasDefaultValue(true);
            entity.Property(e => e.AlternativeItem).HasDefaultValue(1L);
            entity.Property(e => e.IsAlternative).HasDefaultValue(true);

            entity.HasOne(d => d.Bompartition).WithMany(p => p.BompartitionItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitionItems_BOMPartitions");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BompartitionItemCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitionItems_User");

            entity.HasOne(d => d.Item).WithMany(p => p.BompartitionItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitionItems_InventoryItem");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BompartitionItemModifiedByNavigations).HasConstraintName("FK_BOMPartitionItems_User1");
        });

        modelBuilder.Entity<BompartitionItemAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.BompartitionItem).WithMany(p => p.BompartitionItemAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitionItemAttachments_BOMPartitionItems");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BompartitionItemAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMPartitionItemAttachments_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BompartitionItemAttachmentModifiedByNavigations).HasConstraintName("FK_BOMPartitionItemAttachments_User1");
        });

        modelBuilder.Entity<Bomproduct>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Bom).WithMany(p => p.Bomproducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMProduct_BOM");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BomproductCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMProduct_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BomproductModifiedByNavigations).HasConstraintName("FK_BOMProduct_User1");

            entity.HasOne(d => d.ProductGroup).WithMany(p => p.Bomproducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMProduct_InventoryItemCategory");

            entity.HasOne(d => d.Product).WithMany(p => p.Bomproducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOMProduct_InventoryItem");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Branches");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Area).WithMany(p => p.Branches).HasConstraintName("FK_Branch_AreaId");

            entity.HasOne(d => d.Country).WithMany(p => p.Branches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branch_Country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BranchCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branch_User");

            entity.HasOne(d => d.Governorate).WithMany(p => p.Branches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branch_Governorate");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BranchModifiedByNavigations).HasConstraintName("FK_Branch_User1");
        });

        modelBuilder.Entity<BranchProduct>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.BranchProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchProduct_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BranchProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchProduct_User");

            entity.HasOne(d => d.Product).WithMany(p => p.BranchProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchProduct_Product");
        });

        modelBuilder.Entity<BranchSchedule>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.BranchSchedules).HasConstraintName("FK_BranchSchedule_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BranchScheduleCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchSchedule_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BranchScheduleModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchSchedule_User1");

            entity.HasOne(d => d.WeekDay).WithMany(p => p.BranchSchedules).HasConstraintName("FK_BranchSchedule_WeekDays");
        });

        modelBuilder.Entity<BranchSetting>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.BranchSettings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchSettings_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BranchSettingCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchSettings_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BranchSettingModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchSettings_User1");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CarCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_User");

            entity.HasOne(d => d.Driver).WithMany(p => p.Cars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_Drivers");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CarModifiedByNavigations).HasConstraintName("FK_Cars_User1");
        });

        modelBuilder.Entity<CarsAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Car).WithMany(p => p.CarsAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarsAttachment_Cars");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CarsAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarsAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CarsAttachmentModifiedByNavigations).HasConstraintName("FK_CarsAttachment_User1");
        });

        modelBuilder.Entity<CategoryType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryType_User");

            entity.HasOne(d => d.ModifedByNavigation).WithMany(p => p.CategoryTypeModifedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryType_User1");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.HasLogo).HasDefaultValue(false);
            entity.Property(e => e.OtherDelivaryAndShippingMethodName).IsFixedLength();
            entity.Property(e => e.Rate).HasDefaultValue(0);

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.ClientApprovedByNavigations).HasConstraintName("FK_Client_User3");

            entity.HasOne(d => d.Branch).WithMany(p => p.Clients).HasConstraintName("FK_Client_Branch");

            entity.HasOne(d => d.ClientClassification).WithMany(p => p.Clients).HasConstraintName("FK_Client_ClientClassification");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_User");

            entity.HasOne(d => d.DefaultDelivaryAndShippingMethod).WithMany(p => p.Clients).HasConstraintName("FK_Client_DeliveryAndShippingMethod");

            entity.HasOne(d => d.OpeningBalanceCurrency).WithMany(p => p.Clients).HasConstraintName("FK_Client_Currency");

            entity.HasOne(d => d.SalesPerson).WithMany(p => p.ClientSalesPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_User1");
        });

        modelBuilder.Entity<ClientAccount>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Account).WithMany(p => p.ClientAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAccounts_Accounts");

            entity.HasOne(d => d.AccountOfJe).WithMany(p => p.ClientAccounts).HasConstraintName("FK_ClientAccounts_AccountOfJournalEntry");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAccounts_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientAccountCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAccounts_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientAccountModifiedByNavigations).HasConstraintName("FK_ClientAccounts_User1");

            entity.HasOne(d => d.Offer).WithMany(p => p.ClientAccounts).HasConstraintName("FK_ClientAccounts_SalesOffer");

            entity.HasOne(d => d.Project).WithMany(p => p.ClientAccounts).HasConstraintName("FK_ClientAccounts_Project");
        });

        modelBuilder.Entity<ClientAddress>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.BuildingNumber).HasDefaultValueSql("((0))");
            entity.Property(e => e.Floor).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Area).WithMany(p => p.ClientAddresses).HasConstraintName("FK_ClientAddress_Area");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAddress_Client");

            entity.HasOne(d => d.Country).WithMany(p => p.ClientAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAddress_Country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientAddressCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAddress_User");

            entity.HasOne(d => d.Governorate).WithMany(p => p.ClientAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAddress_Governorate");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientAddressModifiedByNavigations).HasConstraintName("FK_ClientAddress_User1");
        });

        modelBuilder.Entity<ClientAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Client).WithMany(p => p.ClientAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAttachment_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientAttachmentModifiedByNavigations).HasConstraintName("FK_ClientAttachment_User1");
        });

        modelBuilder.Entity<ClientBankAccount>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Client).WithMany(p => p.ClientBankAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientBankAccount_Client");
        });

        modelBuilder.Entity<ClientConsultant>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientConsultantCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultant_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientConsultantModifiedByNavigations).HasConstraintName("FK_ClientConsultant_User1");
        });

        modelBuilder.Entity<ClientConsultantAddress>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Country).WithMany(p => p.ClientConsultantAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantAddress_Country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientConsultantAddressCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantAddress_User");

            entity.HasOne(d => d.Governorate).WithMany(p => p.ClientConsultantAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantAddress_Governorate");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientConsultantAddressModifiedByNavigations).HasConstraintName("FK_ClientConsultantAddress_User1");
        });

        modelBuilder.Entity<ClientConsultantEmail>(entity =>
        {
            entity.HasOne(d => d.Consultant).WithMany(p => p.ClientConsultantEmails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantEmail_ClientConsultant");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientConsultantEmailCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantEmail_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientConsultantEmailModifiedByNavigations).HasConstraintName("FK_ClientConsultantEmail_User1");
        });

        modelBuilder.Entity<ClientConsultantFax>(entity =>
        {
            entity.HasOne(d => d.Consultant).WithMany(p => p.ClientConsultantFaxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantFax_ClientConsultant");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientConsultantFaxCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantFax_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientConsultantFaxModifiedByNavigations).HasConstraintName("FK_ClientConsultantFax_User1");
        });

        modelBuilder.Entity<ClientConsultantMobile>(entity =>
        {
            entity.HasOne(d => d.Consultant).WithMany(p => p.ClientConsultantMobiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantMobile_ClientConsultant");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientConsultantMobileCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantMobile_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientConsultantMobileModifiedByNavigations).HasConstraintName("FK_ClientConsultantMobile_User1");
        });

        modelBuilder.Entity<ClientConsultantPhone>(entity =>
        {
            entity.HasOne(d => d.Consultant).WithMany(p => p.ClientConsultantPhones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantPhone_ClientConsultant");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientConsultantPhoneCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantPhone_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientConsultantPhoneModifiedByNavigations).HasConstraintName("FK_ClientConsultantPhone_User1");
        });

        modelBuilder.Entity<ClientConsultantSpecialilty>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Consultant).WithMany(p => p.ClientConsultantSpecialilties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantSpecialilty_ClientConsultant");

            entity.HasOne(d => d.Speciality).WithMany(p => p.ClientConsultantSpecialilties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientConsultantSpecialilty_Speciality");
        });

        modelBuilder.Entity<ClientContactPerson>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.ClientContactPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientContactPerson_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientContactPersonCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientContactPerson_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientContactPersonModifiedByNavigations).HasConstraintName("FK_ClientContactPerson_User1");
        });

        modelBuilder.Entity<ClientExtraInfo>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.ClientExtraInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientExtraInfo_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientExtraInfoCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientExtraInfo_User");

            entity.HasOne(d => d.MaritalStatus).WithMany(p => p.ClientExtraInfos).HasConstraintName("FK_ClientExtraInfo_MaritalStatus");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientExtraInfoModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientExtraInfo_User1");

            entity.HasOne(d => d.Nationality).WithMany(p => p.ClientExtraInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientExtraInfo_Nationality");
        });

        modelBuilder.Entity<ClientFax>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientFaxCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientFax_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientFaxModifiedByNavigations).HasConstraintName("FK_ClientFax_User1");
        });

        modelBuilder.Entity<ClientMobile>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.ClientMobiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientMobile_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientMobileCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientMobile_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientMobileModifiedByNavigations).HasConstraintName("FK_ClientMobile_User1");
        });

        modelBuilder.Entity<ClientNational>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClientNA__3214EC07DF6DFAC9");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientNationals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientNAT__Clien__313C38C2");

            entity.HasOne(d => d.National).WithMany(p => p.ClientNationals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientNAT__Natio__32305CFB");
        });

        modelBuilder.Entity<ClientPaymentTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ClienPaymentTerm");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Client).WithMany(p => p.ClientPaymentTerms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientPaymentTerm_Client");
        });

        modelBuilder.Entity<ClientPhone>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.ClientPhones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientPhone_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientPhoneCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientPhone_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientPhoneModifiedByNavigations).HasConstraintName("FK_ClientPhone_User1");
        });

        modelBuilder.Entity<ClientSalesPerson>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.ClientSalesPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientSalesPerson_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientSalesPersonCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientSalesPerson_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientSalesPersonModifiedByNavigations).HasConstraintName("FK_ClientSalesPerson_User2");

            entity.HasOne(d => d.SalesPerson).WithMany(p => p.ClientSalesPersonSalesPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientSalesPerson_User");
        });

        modelBuilder.Entity<ClientSession>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Client).WithMany(p => p.ClientSessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientSession_Client");
        });

        modelBuilder.Entity<ClientSpeciality>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.ClientSpecialities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientSpeciality_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClientSpecialityCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientSpeciality_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ClientSpecialityModifiedByNavigations).HasConstraintName("FK_ClientSpeciality_User1");

            entity.HasOne(d => d.Speciality).WithMany(p => p.ClientSpecialities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientSpeciality_Speciality");
        });

        modelBuilder.Entity<CompanySerialIdentification>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<ConfirmedRecieveAndRelease>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.ConfirmedRecieveAndReleases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfirmedRecieveAndRelease_Accounts");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConfirmedRecieveAndReleaseCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfirmedRecieveAndRelease_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ConfirmedRecieveAndReleaseModifiedByNavigations).HasConstraintName("FK_ConfirmedRecieveAndRelease_User2");

            entity.HasOne(d => d.ReceivedByNavigation).WithMany(p => p.ConfirmedRecieveAndReleaseReceivedByNavigations).HasConstraintName("FK_ConfirmedRecieveAndRelease_User");
        });

        modelBuilder.Entity<ConfirmedRecieveAndReleaseAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.ConfirmedRecieveAndRelease).WithMany(p => p.ConfirmedRecieveAndReleaseAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfirmedRecieveAndReleaseAttachment_ConfirmedRecieveAndRelease");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConfirmedRecieveAndReleaseAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfirmedRecieveAndReleaseAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ConfirmedRecieveAndReleaseAttachmentModifiedByNavigations).HasConstraintName("FK_ConfirmedRecieveAndReleaseAttachment_User1");
        });

        modelBuilder.Entity<ContactThrough>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<ContractDetail>(entity =>
        {
            entity.Property(e => e.Isautomatic).HasDefaultValue(true);

            entity.HasOne(d => d.ContactType).WithMany(p => p.ContractDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractDetails_ContractType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ContractDetailCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractDetails_User1");

            entity.HasOne(d => d.HrUser).WithMany(p => p.ContractDetails).HasConstraintName("FK_ContractDetails_HrUserId");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ContractDetailModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractDetails_User2");

            entity.HasOne(d => d.ReportTo).WithMany(p => p.ContractDetailReportTos).HasConstraintName("FK_ContractDetails_User3");

            entity.HasOne(d => d.User).WithMany(p => p.ContractDetailUsers).HasConstraintName("FK_ContractDetails_User");
        });

        modelBuilder.Entity<ContractLeaveEmployee>(entity =>
        {
            entity.HasOne(d => d.Contract).WithMany(p => p.ContractLeaveEmployees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractLeaveEmployee_ContractDetails");

            entity.HasOne(d => d.ContractLeaveSetting).WithMany(p => p.ContractLeaveEmployees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractLeaveEmployee_ContractLeaveSetting");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ContractLeaveEmployeeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractLeaveEmployee_User1");

            entity.HasOne(d => d.HrUser).WithMany(p => p.ContractLeaveEmployees).HasConstraintName("FK_ContractLeaveEmployee_HrUser");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ContractLeaveEmployeeModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractLeaveEmployee_User2");

            entity.HasOne(d => d.User).WithMany(p => p.ContractLeaveEmployeeUsers).HasConstraintName("FK_ContractLeaveEmployee_User");
        });

        modelBuilder.Entity<ContractLeaveSetting>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ContractLeaveSettingCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractLeaveSetting_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ContractLeaveSettingModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractLeaveSetting_User1");
        });

        modelBuilder.Entity<ContractReportTo>(entity =>
        {
            entity.HasOne(d => d.Contract).WithMany(p => p.ContractReportTos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractReportTo_ContractDetails");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ContractReportToCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractReportTo_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ContractReportToModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractReportTo_User2");

            entity.HasOne(d => d.ReportTo).WithMany(p => p.ContractReportToReportTos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractReportTo_User");
        });

        modelBuilder.Entity<ContractType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ContractID");
        });

        modelBuilder.Entity<CostType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CostTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CostType_User");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CountryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Country_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CountryModifiedByNavigations).HasConstraintName("FK_Country_User1");
        });

        modelBuilder.Entity<CrmcontactType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CrmcontactTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMContactType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CrmcontactTypeModifiedByNavigations).HasConstraintName("FK_CRMContactType_User1");
        });

        modelBuilder.Entity<CrmrecievedType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CrmrecievedTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMRecievedType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CrmrecievedTypeModifiedByNavigations).HasConstraintName("FK_CRMRecievedType_User1");
        });

        modelBuilder.Entity<Crmreport>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.Crmreports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMReport_Branch");

            entity.HasOne(d => d.ClientContactPerson).WithMany(p => p.Crmreports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMReport_ClientContactPerson");

            entity.HasOne(d => d.Client).WithMany(p => p.Crmreports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMReport_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CrmreportCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMReport_User1");

            entity.HasOne(d => d.CrmcontactType).WithMany(p => p.Crmreports).HasConstraintName("FK_CRMReport_CRMContactType");

            entity.HasOne(d => d.CrmrecievedType).WithMany(p => p.Crmreports).HasConstraintName("FK_CRMReport_CRMRecievedType");

            entity.HasOne(d => d.CrmreportReason).WithMany(p => p.Crmreports).HasConstraintName("FK_CRMReport_CRMReportReason");

            entity.HasOne(d => d.Crmuser).WithMany(p => p.CrmreportCrmusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMReport_User");

            entity.HasOne(d => d.DailyReport).WithMany(p => p.Crmreports).HasConstraintName("FK_DailyReport_CRMReport");

            entity.HasOne(d => d.DailyReportLine).WithMany(p => p.Crmreports).HasConstraintName("FK_DailyReportLine_CRMReport");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CrmreportModifiedByNavigations).HasConstraintName("FK_CRMReport_User2");

            entity.HasOne(d => d.RelatedToInventoryItem).WithMany(p => p.Crmreports).HasConstraintName("FK_InventoryItem_CRMReport");

            entity.HasOne(d => d.RelatedToSalesOffer).WithMany(p => p.Crmreports).HasConstraintName("FK_CRMReport_SalesOffer");

            entity.HasOne(d => d.RelatedToSalesOfferProduct).WithMany(p => p.Crmreports).HasConstraintName("FK_SalesOfferProduct_CRMReport");
        });

        modelBuilder.Entity<CrmreportReason>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CrmreportReasonCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMReportReason_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CrmreportReasonModifiedByNavigations).HasConstraintName("FK_CRMReportReason_User1");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.IsLocal).HasDefaultValue(false);
        });

        modelBuilder.Entity<DailyAdjustingEntryCostCenter>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyAdjustingEntryCostCenterCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyAdjustingEntryCostCenter_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyAdjustingEntryCostCenterModifiedByNavigations).HasConstraintName("FK_DailyAdjustingEntryCostCenter_User1");
        });

        modelBuilder.Entity<DailyJournalEntry>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.DailyJournalEntries).HasConstraintName("FK_DailyJournalEntry_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyJournalEntryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyJournalEntry_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyJournalEntryModifiedByNavigations).HasConstraintName("FK_DailyJournalEntry_User1");
        });

        modelBuilder.Entity<DailyJournalEntryReverse>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyJournalEntryReverseCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyJournalEntryReverse_User");

            entity.HasOne(d => d.Djentry).WithMany(p => p.DailyJournalEntryReverseDjentries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyJournalEntryReverse_DailyJournalEntry");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyJournalEntryReverseModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyJournalEntryReverse_User1");

            entity.HasOne(d => d.ParentDjentry).WithMany(p => p.DailyJournalEntryReverseParentDjentries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyJournalEntryReverse_DailyJournalEntry1");
        });

        modelBuilder.Entity<DailyReport>(entity =>
        {
            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyReportModifiedByNavigations).HasConstraintName("FK_DailyReport_User1");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.DailyReportReviewedByNavigations).HasConstraintName("FK_DailyReport_User2");

            entity.HasOne(d => d.User).WithMany(p => p.DailyReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyReport_User");
        });

        modelBuilder.Entity<DailyReportAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyReportAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyReportAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyReportAttachmentModifiedByNavigations).HasConstraintName("FK_DailyReportAttachment_User1");
        });

        modelBuilder.Entity<DailyReportExpense>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyReportExpenseCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyReportExpense_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.DailyReportExpenses).HasConstraintName("FK_DailyReportExpense_DailyReportExpense");

            entity.HasOne(d => d.DailyReport).WithMany(p => p.DailyReportExpenses).HasConstraintName("FK_DailyReportExpense_DailyReport");

            entity.HasOne(d => d.DailyReportLine).WithMany(p => p.DailyReportExpenses).HasConstraintName("FK_DailyReportExpense_DailyReportLine");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyReportExpenseModifiedByNavigations).HasConstraintName("FK_DailyReportExpense_User1");
        });

        modelBuilder.Entity<DailyReportLine>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.DailyReportLines).HasConstraintName("FK_DailyReportLine_Client");

            entity.HasOne(d => d.DailyReport).WithMany(p => p.DailyReportLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyReportLine_DailyReport");

            entity.HasOne(d => d.DailyReportThrough).WithMany(p => p.DailyReportLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyReportLine_DailyReportThrough");

            entity.HasOne(d => d.ReasonType).WithMany(p => p.DailyReportLines).HasConstraintName("FK_DailyReportLine_CRMReportReason");

            entity.HasOne(d => d.RelatedToInventoryItem).WithMany(p => p.DailyReportLines).HasConstraintName("FK_InventoryItem_DailyReportLine");

            entity.HasOne(d => d.RelatedToSalesOffer).WithMany(p => p.DailyReportLines).HasConstraintName("FK_DailyReportLine_SalesOffer");

            entity.HasOne(d => d.RelatedToSalesOfferProduct).WithMany(p => p.DailyReportLines).HasConstraintName("FK_SalesOfferProduct_DailyReportLine");
        });

        modelBuilder.Entity<DailyReportThrough>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyReportThroughCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyReportThrough_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyReportThroughModifiedByNavigations).HasConstraintName("FK_DailyReportThrough_User1");
        });

        modelBuilder.Entity<DailyTranactionAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyTranactionAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyTranactionAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyTranactionAttachmentModifiedByNavigations).HasConstraintName("FK_DailyTranactionAttachment_User1");
        });

        modelBuilder.Entity<DailyTranactionBeneficiaryToGeneralType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyTranactionBeneficiaryToGeneralTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyTranactionBeneficiaryToGeneralType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyTranactionBeneficiaryToGeneralTypeModifiedByNavigations).HasConstraintName("FK_DailyTranactionBeneficiaryToGeneralType_User1");
        });

        modelBuilder.Entity<DailyTranactionBeneficiaryToType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyTranactionBeneficiaryToTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyTranactionBeneficiaryToType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyTranactionBeneficiaryToTypeModifiedByNavigations).HasConstraintName("FK_DailyTranactionBeneficiaryToType_User1");
        });

        modelBuilder.Entity<DailyTranactionBeneficiaryToUser>(entity =>
        {
            entity.HasOne(d => d.BeneficiaryType).WithMany(p => p.DailyTranactionBeneficiaryToUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyTranactionBeneficiaryToUser_DailyTranactionBeneficiaryToType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyTranactionBeneficiaryToUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyTranactionBeneficiaryToUser_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyTranactionBeneficiaryToUserModifiedByNavigations).HasConstraintName("FK_DailyTranactionBeneficiaryToUser_User1");
        });

        modelBuilder.Entity<DailyTransaction>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyTransactionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyTransaction_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyTransactionModifiedByNavigations).HasConstraintName("FK_DailyTransaction_User1");
        });

        modelBuilder.Entity<DailyTransactionCostCenter>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyTransactionCostCenterCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyTransactionCostCenter_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DailyTransactionCostCenterModifiedByNavigations).HasConstraintName("FK_DailyTransactionCostCenter_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.DailyTransactionCostCenters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyTransactionCostCenter_Project");
        });

        modelBuilder.Entity<DeductionType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DeductionTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeductionType_User");
        });

        modelBuilder.Entity<DeliveryAndShippingMethod>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Department_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DepartmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Department_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DepartmentModifiedByNavigations).HasConstraintName("FK_Department_User1");
        });

        modelBuilder.Entity<DistributionSupplierPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Distribu__3214EC072E0F93D2");

            entity.HasOne(d => d.Supplier).WithMany(p => p.DistributionSupplierPayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DistributionSupplierPayment_Supplier");

            entity.HasOne(d => d.SupplierPayment).WithMany(p => p.DistributionSupplierPayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DistributionSupplierPayment_TransportionLineSupplierPayment");
        });

        modelBuilder.Entity<DoctorRoom>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.DoctorRooms).HasConstraintName("FK_DoctorRoom_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DoctorRooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorRoom_User");
        });

        modelBuilder.Entity<DoctorSchedule>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.DoctorSchedules).HasConstraintName("FK_DoctorSchedule_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DoctorScheduleCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorSchedule_User");

            entity.HasOne(d => d.Doctor).WithMany(p => p.DoctorSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorSchedule_HrUser");

            entity.HasOne(d => d.DoctorSpeciality).WithMany(p => p.DoctorSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorSchedule_Team");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DoctorScheduleModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorSchedule_User1");

            entity.HasOne(d => d.PercentageType).WithMany(p => p.DoctorSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorSchedule_MedicalDoctorPercentageType");

            entity.HasOne(d => d.Room).WithMany(p => p.DoctorSchedules).HasConstraintName("FK_DoctorSchedule_DoctorRoom");

            entity.HasOne(d => d.Status).WithMany(p => p.DoctorSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorSchedule_MedicalDoctorScheduleStatus");

            entity.HasOne(d => d.WeekDay).WithMany(p => p.DoctorSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorSchedule_WeekDays");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DriverCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drivers_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DriverModifiedByNavigations).HasConstraintName("FK_Drivers_User1");
        });

        modelBuilder.Entity<DriversAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DriversAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DriversAttachment_User");

            entity.HasOne(d => d.Driver).WithMany(p => p.DriversAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DriversAttachment_Drivers");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.DriversAttachmentModifiedByNavigations).HasConstraintName("FK_DriversAttachment_User1");
        });

        modelBuilder.Entity<EinvoiceSetting>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<Email>(entity =>
        {
            entity.HasOne(d => d.EmailTypeNavigation).WithMany(p => p.Emails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Email_EmailType");

            entity.HasOne(d => d.User).WithMany(p => p.Emails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Email_User");
        });

        modelBuilder.Entity<EmailAttachment>(entity =>
        {
            entity.HasOne(d => d.Email).WithMany(p => p.EmailAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailAttachment_Email");
        });

        modelBuilder.Entity<EmailCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EmailCategory_1");

            entity.HasOne(d => d.CategoryType).WithMany(p => p.EmailCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailCategory_EmailCategoryType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmailCategoryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailCategory_User");

            entity.HasOne(d => d.Email).WithMany(p => p.EmailCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailCategory_Email");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.EmailCategoryModifiedByNavigations).HasConstraintName("FK_EmailCategory_User1");
        });

        modelBuilder.Entity<EmailCategoryType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EmailCategory");
        });

        modelBuilder.Entity<EmailCc>(entity =>
        {
            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.EmailCcs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailCc_Email");
        });

        modelBuilder.Entity<EmailReceiver>(entity =>
        {
            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.EmailReceivers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailReceivers_Email");
        });

        modelBuilder.Entity<ExchangeRate>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ExchangeRateCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExchangeRate_User");

            entity.HasOne(d => d.FromCurrency).WithMany(p => p.ExchangeRateFromCurrencies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExchangeRate_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ExchangeRateModifiedByNavigations).HasConstraintName("FK_ExchangeRate_User1");

            entity.HasOne(d => d.ToCurrency).WithMany(p => p.ExchangeRateToCurrencies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExchangeRate_Currency1");
        });

        modelBuilder.Entity<ExpensisType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ExpensisTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpensisType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ExpensisTypeModifiedByNavigations).HasConstraintName("FK_ExpensisType_User1");
        });

        modelBuilder.Entity<ExtraCostLibrary>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ExtraCostLibraryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExtraCostLibrary_User");

            entity.HasOne(d => d.DefaultCurrency).WithMany(p => p.ExtraCostLibraries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExtraCostLibrary_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ExtraCostLibraryModifiedByNavigations).HasConstraintName("FK_ExtraCostLibrary_User1");
        });

        modelBuilder.Entity<GarasClientInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CompanyProfile");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GarasClientInfoCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyProfile_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.GarasClientInfoModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyProfile_User1");
        });

        modelBuilder.Entity<GeneralActiveCostCenter>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GeneralActiveCostCenterCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GeneralActiveCostCenters_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.GeneralActiveCostCenterModifiedByNavigations).HasConstraintName("FK_GeneralActiveCostCenters_User1");
        });

        modelBuilder.Entity<Governorate>(entity =>
        {
            entity.HasOne(d => d.Country).WithMany(p => p.Governorates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Governorate_Country");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.Governorates).HasConstraintName("FK_Governorate_User");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GroupCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.GroupModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_User1");
        });

        modelBuilder.Entity<GroupRole>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GroupRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupRole_User");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupRole_Group");

            entity.HasOne(d => d.Role).WithMany(p => p.GroupRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupRole_Role");
        });

        modelBuilder.Entity<GroupUser>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GroupUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_User_User1");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_User_Group");

            entity.HasOne(d => d.HrUser).WithMany(p => p.GroupUsers).HasConstraintName("FK_GroupUser_HrUserId");

            entity.HasOne(d => d.User).WithMany(p => p.GroupUserUsers).HasConstraintName("FK_Group_User_User");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<HrUser>(entity =>
        {
            entity.ToTable("HrUser", tb => tb.HasTrigger("Trig_log"));

            entity.Property(e => e.ArmiddleName).HasDefaultValue("");
            entity.Property(e => e.NationalityId).HasDefaultValue(0L);

            entity.HasOne(d => d.Branch).WithMany(p => p.HrUsers).HasConstraintName("FK_HrUser_BranchID");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.HrUserCreatedBies).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Department).WithMany(p => p.HrUsers).HasConstraintName("FK_HrUser_DepartmentID");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.HrUsers).HasConstraintName("FK_HrUser_JobTitleID");

            entity.HasOne(d => d.MaritalStatus).WithMany(p => p.HrUsers).HasConstraintName("FK_HrUser_MaritalStatus_HrMaritalStatusId");

            entity.HasOne(d => d.ModifiedBy).WithMany(p => p.HrUserModifiedBies).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.HrUserUsers).HasConstraintName("FK_HrUser_HrUser_UserID");
        });

        modelBuilder.Entity<Hrcustody>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HrcustodyCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustody_User");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.Hrcustodies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustody_InventoryItem");

            entity.HasOne(d => d.MaterialRequest).WithMany(p => p.Hrcustodies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustody_InventoryMatrialRequest");

            entity.HasOne(d => d.MaterialRequestItem).WithMany(p => p.Hrcustodies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustody_InventoryMatrialRequestItems");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.HrcustodyModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustody_User1");

            entity.HasOne(d => d.Status).WithMany(p => p.Hrcustodies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustody_HRCustodyStatus1");

            entity.HasOne(d => d.User).WithMany(p => p.HrcustodyUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustody_HRCustodyStatus");
        });

        modelBuilder.Entity<HrcustodyReportAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HrcustodyReportAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustodyReportAttachment_User");

            entity.HasOne(d => d.Custody).WithMany(p => p.HrcustodyReportAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustodyReportAttachment_HRCustody");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.HrcustodyReportAttachmentModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRCustodyReportAttachment_User1");
        });

        modelBuilder.Entity<HrdeductionRewarding>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.IsDeduction).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.HrdeductionRewardingCreatedByUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRDeductionRewarding_User");

            entity.HasOne(d => d.User).WithMany(p => p.HrdeductionRewardingUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRDeductionRewarding_User1");
        });

        modelBuilder.Entity<HremployeeAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HremployeeAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HREmployeeAttachment_User1");

            entity.HasOne(d => d.EmployeeUser).WithMany(p => p.HremployeeAttachmentEmployeeUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HREmployeeAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.HremployeeAttachmentModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HREmployeeAttachment_User2");
        });

        modelBuilder.Entity<Hrloan>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.ApprovalStatus).WithMany(p => p.Hrloans).HasConstraintName("FK_HRLoan_HRLoanApprovalStatus");

            entity.HasOne(d => d.ApprovedByUser).WithMany(p => p.HrloanApprovedByUsers).HasConstraintName("FK_HRLoan_User1");

            entity.HasOne(d => d.LoanStatus).WithMany(p => p.Hrloans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRLoan_HRLoanStatus");

            entity.HasOne(d => d.RefundStrategy).WithMany(p => p.Hrloans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRLoan_HRLoanRefundStrategy");

            entity.HasOne(d => d.User).WithMany(p => p.HrloanUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRLoan_User");
        });

        modelBuilder.Entity<HrloanApprovalStatus>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<HrloanRefundStrategy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HRLoanRefundStatus");

            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<HrloanStatus>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<HruserWarning>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.ActionPlanApproval).WithMany(p => p.HruserWarnings).HasConstraintName("FK_HRUserWarning_HRUserWarningActionPlanApproval");

            entity.HasOne(d => d.Status).WithMany(p => p.HruserWarnings).HasConstraintName("FK_HRUserWarning_HRUserWarningStatus");

            entity.HasOne(d => d.User).WithMany(p => p.HruserWarnings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HRUserWarning_User");
        });

        modelBuilder.Entity<HruserWarningActionPlanApproval>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<HruserWarningStatus>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<ImportantDate>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ImportantDates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportantDate_User");
        });

        modelBuilder.Entity<IncomeType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.IncomeTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IncomeType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.IncomeTypeModifiedByNavigations).HasConstraintName("FK_IncomeType_User1");
        });

        modelBuilder.Entity<InsuranceCompanyName>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InsuranceCompanyNames).HasConstraintName("FK_InsuranceCompanyNames_User");
        });

        modelBuilder.Entity<Interview>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InterviewCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Interview_User2");

            entity.HasOne(d => d.Interviewer).WithMany(p => p.InterviewInterviewers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Interview_User");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.Interviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Interview_JobTitle");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InterviewModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Interview_User3");

            entity.HasOne(d => d.User).WithMany(p => p.InterviewUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Interview_User1");
        });

        modelBuilder.Entity<InventoryAddingOrder>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryAddingOrderCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryAddingOrder_User");

            entity.HasOne(d => d.InventoryStore).WithMany(p => p.InventoryAddingOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryAddingOrder_InventoryStore");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryAddingOrderModifiedByNavigations).HasConstraintName("FK_InventoryAddingOrder_User1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.InventoryAddingOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryAddingOrder_Supplier");
        });

        modelBuilder.Entity<InventoryAddingOrderItem>(entity =>
        {
            entity.HasOne(d => d.InventoryAddingOrder).WithMany(p => p.InventoryAddingOrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryAddingOrderItems_InventoryAddingOrder");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryAddingOrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryAddingOrderItems_InventoryItem");

            entity.HasOne(d => d.Po).WithMany(p => p.InventoryAddingOrderItems)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_InventoryAddingOrderItems_PurchasePO");

            entity.HasOne(d => d.Uom).WithMany(p => p.InventoryAddingOrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryAddingOrderItems_InventoryUOM");
        });

        modelBuilder.Entity<InventoryInternalBackOrder>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryInternalBackOrderCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalBackOrder_User1");

            entity.HasOne(d => d.From).WithMany(p => p.InventoryInternalBackOrderFroms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalBackOrder_User");

            entity.HasOne(d => d.InventoryStore).WithMany(p => p.InventoryInternalBackOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalBackOrder_InventoryStore");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryInternalBackOrderModifiedByNavigations).HasConstraintName("FK_InventoryInternalBackOrder_User2");
        });

        modelBuilder.Entity<InventoryInternalBackOrderItem>(entity =>
        {
            entity.HasOne(d => d.InventoryInternalBackOrder).WithMany(p => p.InventoryInternalBackOrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalBackOrderItems_InventoryInternalBackOrder");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryInternalBackOrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalBackOrderItems_InventoryItem");

            entity.HasOne(d => d.InventoryMatrialReleaseItem).WithMany(p => p.InventoryInternalBackOrderItems).HasConstraintName("FK_InventoryInternalBackOrderItems_InventoryMatrialReleaseItems");

            entity.HasOne(d => d.Uom).WithMany(p => p.InventoryInternalBackOrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalBackOrderItems_InventoryUOM");
        });

        modelBuilder.Entity<InventoryInternalTransferOrder>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryInternalTransferOrderCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalTransferOrder_User");

            entity.HasOne(d => d.FromInventoryStore).WithMany(p => p.InventoryInternalTransferOrderFromInventoryStores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalTransferOrder_InventoryStore");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryInternalTransferOrderModifiedByNavigations).HasConstraintName("FK_InventoryInternalTransferOrder_User1");

            entity.HasOne(d => d.ToInventoryStore).WithMany(p => p.InventoryInternalTransferOrderToInventoryStores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalTransferOrder_InventoryStore1");
        });

        modelBuilder.Entity<InventoryInternalTransferOrderItem>(entity =>
        {
            entity.HasOne(d => d.InventoryInternalTransferOrder).WithMany(p => p.InventoryInternalTransferOrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalTransferOrderItems_InventoryInternalTransferOrder");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryInternalTransferOrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalTransferOrderItems_InventoryItem");

            entity.HasOne(d => d.Uom).WithMany(p => p.InventoryInternalTransferOrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryInternalTransferOrderItems_InventoryUOM");
        });

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.MaxBalance).HasDefaultValue(0.0);
            entity.Property(e => e.MaxBalance1).HasDefaultValue(0m);
            entity.Property(e => e.MinBalance).HasDefaultValue(0.0);
            entity.Property(e => e.MinBalance1).HasDefaultValue(0m);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryItemCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItem_User");

            entity.HasOne(d => d.InventoryItemCategory).WithMany(p => p.InventoryItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItem_InventoryItemCategory");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryItemModifiedByNavigations).HasConstraintName("FK_InventoryItem_User1");

            entity.HasOne(d => d.Priority).WithMany(p => p.InventoryItems).HasConstraintName("FK_InventoryItem_Priority");

            entity.HasOne(d => d.PurchasingUom).WithMany(p => p.InventoryItemPurchasingUoms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItem_InventoryUOM");

            entity.HasOne(d => d.RequstionUom).WithMany(p => p.InventoryItemRequstionUoms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItem_InventoryUOM1");
        });

        modelBuilder.Entity<InventoryItemAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryItemAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemAttachment_InventoryItemAttachment");
        });

        modelBuilder.Entity<InventoryItemCategory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CategoryType).WithMany(p => p.InventoryItemCategories).HasConstraintName("FK_InventoryItemCategory_CategoryType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryItemCategoryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemCategory_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryItemCategoryModifiedByNavigations).HasConstraintName("FK_InventoryItemCategory_User1");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory).HasConstraintName("FK_InventoryItemCategory_InventoryItemCategory");
        });

        modelBuilder.Entity<InventoryItemContent>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryItemContentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemContent_User");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryItemContents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemContent_InventoryItem");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryItemContentModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemContent_User1");

            entity.HasOne(d => d.ParentContent).WithMany(p => p.InverseParentContent).HasConstraintName("FK_InventoryItemContent_InventoryItemContent1");
        });

        modelBuilder.Entity<InventoryItemPrice>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryItemPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemPrice_User");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryItemPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemPrice_InventoryItem");

            entity.HasOne(d => d.Supplier).WithMany(p => p.InventoryItemPrices).HasConstraintName("FK_InventoryItemPrice_Supplier");
        });

        modelBuilder.Entity<InventoryItemUom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_InventoryItemUOM_1");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CanEdit).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryItemUomCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemUOM_User");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryItemUoms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemUOM_InventoryItem");

            entity.HasOne(d => d.InventoryUom).WithMany(p => p.InventoryItemUoms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryItemUOM_InventoryUOM");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryItemUomModifiedByNavigations).HasConstraintName("FK_InventoryItemUOM_User1");
        });

        modelBuilder.Entity<InventoryMatrialRelease>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryMatrialReleaseCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRelease_User1");

            entity.HasOne(d => d.FromInventoryStore).WithMany(p => p.InventoryMatrialReleases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRelease_InventoryStore");

            entity.HasOne(d => d.MatrialRequest).WithMany(p => p.InventoryMatrialReleases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRelease_InventoryMatrialRequest");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryMatrialReleaseModifiedByNavigations).HasConstraintName("FK_InventoryMatrialRelease_User2");

            entity.HasOne(d => d.ToUser).WithMany(p => p.InventoryMatrialReleaseToUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRelease_User");
        });

        modelBuilder.Entity<InventoryMatrialReleaseItem>(entity =>
        {
            entity.HasOne(d => d.InventoryMatrialReleaset).WithMany(p => p.InventoryMatrialReleaseItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialReleaseItems_InventoryMatrialRelease");

            entity.HasOne(d => d.InventoryMatrialRequestItem).WithMany(p => p.InventoryMatrialReleaseItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialReleaseItems_InventoryMatrialRequestItems");
        });

        modelBuilder.Entity<InventoryMatrialReleasePrintInfo>(entity =>
        {
            entity.HasOne(d => d.InventoryMatrialRelease).WithMany(p => p.InventoryMatrialReleasePrintInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialReleasePrintInfo_InventoryMatrialRelease");

            entity.HasOne(d => d.Project).WithMany(p => p.InventoryMatrialReleasePrintInfos).HasConstraintName("FK_InventoryMatrialReleasePrintInfo_Project");
        });

        modelBuilder.Entity<InventoryMatrialRequest>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryMatrialRequestCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRequest_User1");

            entity.HasOne(d => d.FromUser).WithMany(p => p.InventoryMatrialRequestFromUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRequest_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryMatrialRequestModifiedByNavigations).HasConstraintName("FK_InventoryMatrialRequest_User2");

            entity.HasOne(d => d.RequestType).WithMany(p => p.InventoryMatrialRequests).HasConstraintName("FK_InventoryMatrialRequest_InventoryMaterialRequestType");

            entity.HasOne(d => d.ToInventoryStore).WithMany(p => p.InventoryMatrialRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRequest_InventoryStore");
        });

        modelBuilder.Entity<InventoryMatrialRequestItem>(entity =>
        {
            entity.Property(e => e.FromBom).HasDefaultValue(false);
            entity.Property(e => e.PurchaseQuantity).HasDefaultValue(0.0);
            entity.Property(e => e.PurchaseQuantity1).HasDefaultValue(0m);

            entity.HasOne(d => d.FabricationOrder).WithMany(p => p.InventoryMatrialRequestItems).HasConstraintName("FK_InventoryMatrialRequestItems_ProjectFabrication");

            entity.HasOne(d => d.FabricationOrderItem).WithMany(p => p.InventoryMatrialRequestItems).HasConstraintName("FK_InventoryMatrialRequestItems_ProjectFabricationBOQ");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryMatrialRequestItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRequestItems_InventoryItem");

            entity.HasOne(d => d.InventoryMatrialRequest).WithMany(p => p.InventoryMatrialRequestItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRequestItems_InventoryMatrialRequest");

            entity.HasOne(d => d.Project).WithMany(p => p.InventoryMatrialRequestItems).HasConstraintName("FK_InventoryMatrialRequestItems_Project");

            entity.HasOne(d => d.Uom).WithMany(p => p.InventoryMatrialRequestItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMatrialRequestItems_InventoryUOM");
        });

        modelBuilder.Entity<InventoryReport>(entity =>
        {
            entity.HasOne(d => d.ByUser).WithMany(p => p.InventoryReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryReport_User");

            entity.HasOne(d => d.InventoryStore).WithMany(p => p.InventoryReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryReport_InventoryStore");
        });

        modelBuilder.Entity<InventoryReportAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.InventoryReport).WithMany(p => p.InventoryReportAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryReportAttachment_InventoryReport");
        });

        modelBuilder.Entity<InventoryReportItem>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryReportItemCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryReportItems_User");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryReportItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryReportItems_InventoryItem");

            entity.HasOne(d => d.InventoryReport).WithMany(p => p.InventoryReportItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryReportItems_InventoryReport");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryReportItemModifiedByNavigations).HasConstraintName("FK_InventoryReportItems_User1");
        });

        modelBuilder.Entity<InventoryReportItemParent>(entity =>
        {
            entity.HasOne(d => d.InvReportItem).WithMany(p => p.InventoryReportItemParents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryReportItemParent_InventoryReportItems");

            entity.HasOne(d => d.InvStoreItem).WithMany(p => p.InventoryReportItemParents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryReportItemParent_InventoryStoreItem");
        });

        modelBuilder.Entity<InventoryStore>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryStoreCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryStore_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryStoreModifiedByNavigations).HasConstraintName("FK_InventoryStore_User1");
        });

        modelBuilder.Entity<InventoryStoreItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_InvneotryStoreItem");

            entity.Property(e => e.HoldQty).HasDefaultValue(0m);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryStoreItemCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvneotryStoreItem_User");

            entity.HasOne(d => d.InvenoryStoreLocation).WithMany(p => p.InventoryStoreItems).HasConstraintName("FK_InventoryStoreItem_InventoryStoreLocation");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryStoreItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvneotryStoreItem_InventoryItem");

            entity.HasOne(d => d.InventoryStore).WithMany(p => p.InventoryStoreItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvneotryStoreItem_InventoryStore");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryStoreItemModifiedByNavigations).HasConstraintName("FK_InvneotryStoreItem_User1");
        });

        modelBuilder.Entity<InventoryStoreKeeper>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryStoreKeeperCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryStoreKeeper_User1");

            entity.HasOne(d => d.InventoryStore).WithMany(p => p.InventoryStoreKeepers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryStoreKeeper_InventoryStore");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryStoreKeeperModifiedByNavigations).HasConstraintName("FK_InventoryStoreKeeper_User2");

            entity.HasOne(d => d.User).WithMany(p => p.InventoryStoreKeeperUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryStoreKeeper_User");
        });

        modelBuilder.Entity<InventoryStoreLocation>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.InventoryStore).WithMany(p => p.InventoryStoreLocations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryStoreLocation_InventoryStore");
        });

        modelBuilder.Entity<InventoryUom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_InventoryItemUOM");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryUomCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryUOM_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InventoryUomModifiedByNavigations).HasConstraintName("FK_InventoryUOM_User1");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InvoiceCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoices_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InvoiceModifiedByNavigations).HasConstraintName("FK_Invoices_User1");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.Invoices).HasConstraintName("FK_Invoices_SalesOffer");
        });

        modelBuilder.Entity<InvoiceCnandDn>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.ParentInvoice).WithMany(p => p.InvoiceCnandDns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceCNAndDN_Invoices");

            entity.HasOne(d => d.ParentSalesOffer).WithMany(p => p.InvoiceCnandDnParentSalesOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceCNAndDN_SalesOffer1");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.InvoiceCnandDnSalesOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceCNAndDN_SalesOffer");
        });

        modelBuilder.Entity<InvoiceExtraCost>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InvoiceExtraCostCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceExtraCost_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InvoiceExtraCostModifiedByNavigations).HasConstraintName("FK_InvoiceExtraCost_User1");
        });

        modelBuilder.Entity<InvoiceExtraModification>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InvoiceExtraModificationCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceExtraModification_User");

            entity.HasOne(d => d.FabricationOrder).WithMany(p => p.InvoiceExtraModifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceExtraModification_ProjectFabrication");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceExtraModifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceExtraModification_Invoices");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InvoiceExtraModificationModifiedByNavigations).HasConstraintName("FK_InvoiceExtraModification_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.InvoiceExtraModifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceExtraModification_Project");
        });

        modelBuilder.Entity<InvoiceNewClient>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InvoiceNewClients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceNewClient_User");
        });

        modelBuilder.Entity<InvoiceOfProject>(entity =>
        {
            entity.HasOne(d => d.Project).WithMany(p => p.InvoiceOfProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceOfProject_Project");
        });

        modelBuilder.Entity<InvoiceTax>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InvoiceTaxCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceTax_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InvoiceTaxModifiedByNavigations).HasConstraintName("FK_InvoiceTax_User1");
        });

        modelBuilder.Entity<InvoiceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceT__3214EC2715E4BA9D");
        });

        modelBuilder.Entity<JobInformation>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.JobInformations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobInformation_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.JobInformationCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobInformation_User2");

            entity.HasOne(d => d.Department).WithMany(p => p.JobInformations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobInformation_Department");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.JobInformations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobInformation_JobTitle");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.JobInformationModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobInformation_User3");

            entity.HasOne(d => d.ReportTo).WithMany(p => p.JobInformationReportTos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobInformation_User1");

            entity.HasOne(d => d.User).WithMany(p => p.JobInformationUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobInformation_User");
        });

        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.JobTitleCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobTitle_User");

            entity.HasOne(d => d.CurrencyNavigation).WithMany(p => p.JobTitles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_jobtitle_currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.JobTitleModifiedByNavigations).HasConstraintName("FK_JobTitle_User1");
        });

        modelBuilder.Entity<LaboratoryMessagesReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Laborato__3214EC07EA76564C");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.LaboratoryMessagesReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Laborator__Creat__5D034A62");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveRequestCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveRequest_User2");

            entity.HasOne(d => d.FirstApprovedByNavigation).WithMany(p => p.LeaveRequestFirstApprovedByNavigations).HasConstraintName("FK_LeaveRequest_User");

            entity.HasOne(d => d.HrUser).WithMany(p => p.LeaveRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveRequest_HrUser");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.LeaveRequestModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveRequest_User3");

            entity.HasOne(d => d.SecondApprovedByNavigation).WithMany(p => p.LeaveRequestSecondApprovedByNavigations).HasConstraintName("FK_LeaveRequest_User1");

            entity.HasOne(d => d.VacationType).WithMany(p => p.LeaveRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveRequest_ContractLeaveSetting");
        });

        modelBuilder.Entity<MaintenanceFor>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.MaintenanceFors).HasConstraintName("FK_MaintenanceFor_InventoryItemCategory");

            entity.HasOne(d => d.Client).WithMany(p => p.MaintenanceFors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceFor_Client");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.MaintenanceFors).HasConstraintName("FK_MaintenanceFor_InventoryItem");

            entity.HasOne(d => d.Project).WithMany(p => p.MaintenanceFors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceFor_Project");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.MaintenanceFors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceFor_SalesOffer");
        });

        modelBuilder.Entity<MaintenanceReport>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaintenanceReportCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReport_User");

            entity.HasOne(d => d.MaintVisit).WithMany(p => p.MaintenanceReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReport_VisitsScheduleOfMaintenance");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MaintenanceReportModifiedByNavigations).HasConstraintName("FK_MaintenanceReport_User1");
        });

        modelBuilder.Entity<MaintenanceReportAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaintenanceReportAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportAttachment_User");

            entity.HasOne(d => d.MaintenanceReport).WithMany(p => p.MaintenanceReportAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportAttachment_MaintenanceReport");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MaintenanceReportAttachmentModifiedByNavigations).HasConstraintName("FK_MaintenanceReportAttachment_User1");
        });

        modelBuilder.Entity<MaintenanceReportClarification>(entity =>
        {
            entity.HasOne(d => d.ClarificationUser).WithMany(p => p.MaintenanceReportClarificationClarificationUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportClarification_User");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaintenanceReportClarificationCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportClarification_User1");

            entity.HasOne(d => d.MaintenanceReport).WithMany(p => p.MaintenanceReportClarifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportClarification_MaintenanceReport");

            entity.HasOne(d => d.ModifedByNavigation).WithMany(p => p.MaintenanceReportClarificationModifedByNavigations).HasConstraintName("FK_MaintenanceReportClarification_User2");
        });

        modelBuilder.Entity<MaintenanceReportClarificationAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaintenanceReportClarificationAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportClarificationAttachment_User");

            entity.HasOne(d => d.MaintenanceReportClarification).WithMany(p => p.MaintenanceReportClarificationAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportClarificationAttachment_MaintenanceReportClarification");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MaintenanceReportClarificationAttachmentModifiedByNavigations).HasConstraintName("FK_MaintenanceReportClarificationAttachment_User1");
        });

        modelBuilder.Entity<MaintenanceReportExpense>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaintenanceReportExpenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportExpenses_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.MaintenanceReportExpenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportExpenses_Currency");

            entity.HasOne(d => d.ExpensisType).WithMany(p => p.MaintenanceReportExpenses).HasConstraintName("FK_MaintenanceReportExpenses_ExpensisType");

            entity.HasOne(d => d.MaintenanceReport).WithMany(p => p.MaintenanceReportExpenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportExpenses_MaintenanceReport");
        });

        modelBuilder.Entity<MaintenanceReportUser>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.MaintenanceReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportUsers_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaintenanceReportUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportUsers_User");

            entity.HasOne(d => d.Department).WithMany(p => p.MaintenanceReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportUsers_Department");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.MaintenanceReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportUsers_JobTitle");

            entity.HasOne(d => d.MaintenanceReport).WithMany(p => p.MaintenanceReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportUsers_MaintenanceReport");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MaintenanceReportUserModifiedByNavigations).HasConstraintName("FK_MaintenanceReportUsers_User1");

            entity.HasOne(d => d.User).WithMany(p => p.MaintenanceReportUserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceReportUsers_User2");
        });

        modelBuilder.Entity<ManageStage>(entity =>
        {
            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.ManageStageCreateByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManageStages_ManageStages");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ManageStageModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManageStages_User");

            entity.HasOne(d => d.ProjectTm).WithMany(p => p.ManageStages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManageStages_ProjectTM");

            entity.HasOne(d => d.Stage).WithMany(p => p.ManageStages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManageStages_Stages");
        });

        modelBuilder.Entity<ManagementOfMaintenanceOrder>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ManagementOfMaintenanceOrderCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManagementOfMaintenanceOrder_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.ManagementOfMaintenanceOrders).HasConstraintName("FK_ManagementOfMaintenanceOrder_Currency");

            entity.HasOne(d => d.MaintenanceFor).WithMany(p => p.ManagementOfMaintenanceOrders).HasConstraintName("FK_ManagementOfMaintenanceOrder_MaintenanceFor");

            entity.HasOne(d => d.MaintenanceOffer).WithMany(p => p.ManagementOfMaintenanceOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManagementOfMaintenanceOrder_SalesOffer");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ManagementOfMaintenanceOrderModifiedByNavigations).HasConstraintName("FK_ManagementOfMaintenanceOrder_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ManagementOfMaintenanceOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManagementOfMaintenanceOrder_Project");
        });

        modelBuilder.Entity<ManagementOfRentOrder>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ManagementOfRentOrderCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManagementOfRentOrder_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ManagementOfRentOrderModifiedByNavigations).HasConstraintName("FK_ManagementOfRentOrder_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ManagementOfRentOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManagementOfRentOrder_Project");

            entity.HasOne(d => d.RentOffer).WithMany(p => p.ManagementOfRentOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManagementOfRentOrder_SalesOffer");
        });

        modelBuilder.Entity<ManagementOfRentOrderAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ManagementOfRentOrderAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManagementOfRentOrderAttachment_User");

            entity.HasOne(d => d.ManagementOfRentOrder).WithMany(p => p.ManagementOfRentOrderAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ManagementOfRentOrderAttachment_ManagementOfRentOrder");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ManagementOfRentOrderAttachmentModifiedByNavigations).HasConstraintName("FK_ManagementOfRentOrderAttachment_User1");
        });

        modelBuilder.Entity<MangmentOfMaintenanceRequest>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.MangmentOfMaintenanceRequests).HasConstraintName("FK_MangmentOfMaintenanceRequest_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MangmentOfMaintenanceRequestCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MangmentOfMaintenanceRequest_User");

            entity.HasOne(d => d.InstallationAssignToNavigation).WithMany(p => p.MangmentOfMaintenanceRequestInstallationAssignToNavigations).HasConstraintName("FK_MangmentOfMaintenanceRequest_User1");

            entity.HasOne(d => d.MaintenanceFor).WithMany(p => p.MangmentOfMaintenanceRequests).HasConstraintName("FK_MangmentOfMaintenanceRequest_MaintenanceFor");

            entity.HasOne(d => d.MaintenanceRequestType).WithMany(p => p.MangmentOfMaintenanceRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MangmentOfMaintenanceRequest_MaintenanceRequestType");

            entity.HasOne(d => d.ManagementOfMaintenanceOrder).WithMany(p => p.MangmentOfMaintenanceRequests).HasConstraintName("FK_MangmentOfMaintenanceRequest_ManagementOfMaintenanceOrder");

            entity.HasOne(d => d.PreparingUser).WithMany(p => p.MangmentOfMaintenanceRequestPreparingUsers).HasConstraintName("FK_MangmentOfMaintenanceRequest_User2");
        });

        modelBuilder.Entity<MangmentOfMaintenanceRequestAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MangmentOfMaintenanceRequestAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MangmentOfMaintenanceRequestAttachments_User");

            entity.HasOne(d => d.MangmentOfMaintenanceRequest).WithMany(p => p.MangmentOfMaintenanceRequestAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MangmentOfMaintenanceRequestAttachments_MangmentOfMaintenanceRequest");
        });

        modelBuilder.Entity<MangmentOfMaintenanceRequestReport>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.MangmentOfMaintenanceRequestReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MangmentOfMaintenanceRequestReport_Client");

            entity.HasOne(d => d.FinishedByNavigation).WithMany(p => p.MangmentOfMaintenanceRequestReportFinishedByNavigations).HasConstraintName("FK_MangmentOfMaintenanceRequestReport_User1");

            entity.HasOne(d => d.MangmentOfMaintenanceRequest).WithMany(p => p.MangmentOfMaintenanceRequestReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MangmentOfMaintenanceRequestReport_MangmentOfMaintenanceRequest");

            entity.HasOne(d => d.ReportedByNavigation).WithMany(p => p.MangmentOfMaintenanceRequestReportReportedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MangmentOfMaintenanceRequestReport_User");
        });

        modelBuilder.Entity<MangmentOfMaintenanceRequestReportAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MangmentOfMaintenanceRequestReportAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MangmentOfMaintenanceRequestReportAttachments_User");

            entity.HasOne(d => d.MangmentOfMaintenanceRequestReport).WithMany(p => p.MangmentOfMaintenanceRequestReportAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MangmentOfMaintenanceRequestReportAttachments_MangmentOfMaintenanceRequestReport");
        });

        modelBuilder.Entity<MedicalDailyTreasuryBalance>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.MedicalDailyTreasuryBalances).HasConstraintName("FK_MedicalDailyTreasuryBalance_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MedicalDailyTreasuryBalanceCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalDailyTreasuryBalance_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MedicalDailyTreasuryBalanceModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalDailyTreasuryBalance_User1");

            entity.HasOne(d => d.PosNumber).WithMany(p => p.MedicalDailyTreasuryBalances).HasConstraintName("FK_MedicalDailyTreasuryBalance_PosNumber");

            entity.HasOne(d => d.ReceivedFromNavigation).WithMany(p => p.MedicalDailyTreasuryBalanceReceivedFromNavigations).HasConstraintName("FK_MedicalDailyTreasuryBalance_User2");
        });

        modelBuilder.Entity<MedicalExaminationOffer>(entity =>
        {
            entity.HasOne(d => d.Doctor).WithMany(p => p.MedicalExaminationOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalExaminationOffer_HrUser");
        });

        modelBuilder.Entity<MedicalReservation>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MedicalReservationCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalReservation_User");

            entity.HasOne(d => d.Doctor).WithMany(p => p.MedicalReservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalReservation_HrUser");

            entity.HasOne(d => d.DoctorSchedule).WithMany(p => p.MedicalReservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalReservation_DoctorSchedule");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MedicalReservationModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalReservation_User1");

            entity.HasOne(d => d.Offer).WithMany(p => p.MedicalReservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalReservation_SalesOffer");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_MedicalReservation_MedicalReservation");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalReservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalReservation_Client");

            entity.HasOne(d => d.PatientType).WithMany(p => p.MedicalReservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalReservation_MedicalPatientType");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.MedicalReservations).HasConstraintName("FK_MedicalReservation_PaymentMethod");

            entity.HasOne(d => d.Room).WithMany(p => p.MedicalReservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalReservation_DoctorRoom");

            entity.HasOne(d => d.Team).WithMany(p => p.MedicalReservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalReservation_Team");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MovementReport>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MovementReportCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovementReport_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MovementReportModifiedByNavigations).HasConstraintName("FK_MovementReport_User1");

            entity.HasOne(d => d.MovementAndDeliveryOrder).WithMany(p => p.MovementReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovementReport_MovmentsAndDeliveryOrder");
        });

        modelBuilder.Entity<MovementReportAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MovementReportAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovementReportAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MovementReportAttachmentModifiedByNavigations).HasConstraintName("FK_MovementReportAttachment_User1");

            entity.HasOne(d => d.MovementReport).WithMany(p => p.MovementReportAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovementReportAttachment_MovementReport");
        });

        modelBuilder.Entity<MovementsAndDeliveryOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MovmentsAndDeliveryOrder");

            entity.HasOne(d => d.Car).WithMany(p => p.MovementsAndDeliveryOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovmentsAndDeliveryOrder_Cars");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MovementsAndDeliveryOrderCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovmentsAndDeliveryOrder_User");

            entity.HasOne(d => d.Driver).WithMany(p => p.MovementsAndDeliveryOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovmentsAndDeliveryOrder_Drivers");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MovementsAndDeliveryOrderModifiedByNavigations).HasConstraintName("FK_MovmentsAndDeliveryOrder_User1");
        });

        modelBuilder.Entity<Nationality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__National__3214EC27224522B5");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(e => e.New).HasDefaultValue(true);

            entity.HasOne(d => d.FromUser).WithMany(p => p.NotificationFromUsers).HasConstraintName("FK_Notification_User1");

            entity.HasOne(d => d.NotificationProcess).WithMany(p => p.Notifications).HasConstraintName("FK_Notification_NotificationProcess");

            entity.HasOne(d => d.User).WithMany(p => p.NotificationUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_User");
        });

        modelBuilder.Entity<NotificationProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC2738C9647A");
        });

        modelBuilder.Entity<OffDay>(entity =>
        {
            entity.HasOne(d => d.Holiday).WithMany(p => p.OffDays).HasConstraintName("FK_OffDay_Holiday");

            entity.HasOne(d => d.VacationPaymentStrategy).WithMany(p => p.OffDays).HasConstraintName("FK_OffDay_VacationPaymentStrategy");
        });

        modelBuilder.Entity<OverTimeAndDeductionRate>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.OverTimeAndDeductionRates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OverTimeAndDeductionRate_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OverTimeAndDeductionRateCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OverTimeAndDeductionRate_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.OverTimeAndDeductionRateModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OverTimeAndDeductionRate_User1");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.Payrolls).HasConstraintName("FK_Payroll_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PayrollCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payroll_User");

            entity.HasOne(d => d.HrUser).WithMany(p => p.Payrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payroll_HrUser");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PayrollModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payroll_User1");
        });

        modelBuilder.Entity<PermissionLevel>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PermissionLevelCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PermissionLevel_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PermissionLevelModifiedByNavigations).HasConstraintName("FK_PermissionLevel_User1");
        });

        modelBuilder.Entity<PoapprovalSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_POApproval");

            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<PoapprovalStatus>(entity =>
        {
            entity.HasOne(d => d.ApprovalUser).WithMany(p => p.PoapprovalStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POApprovalStatus_User");

            entity.HasOne(d => d.PoapprovalSetting).WithMany(p => p.PoapprovalStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POApprovalStatus_POApprovalSetting");

            entity.HasOne(d => d.Po).WithMany(p => p.PoapprovalStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POApprovalStatus_POApprovalStatus");
        });

        modelBuilder.Entity<PoapprovalUser>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.PoapprovalSetting).WithMany(p => p.PoapprovalUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POApprovalUser_POApproval");
        });

        modelBuilder.Entity<PofinalSelecteSupplier>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.ContactThrough).WithMany(p => p.PofinalSelecteSuppliers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POFinalSelecteSupplier_ContactThrough");

            entity.HasOne(d => d.Po).WithMany(p => p.PofinalSelecteSuppliers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POFinalSelecteSupplier_PurchasePO");

            entity.HasOne(d => d.SupplierContactPerson).WithMany(p => p.PofinalSelecteSuppliers).HasConstraintName("FK_POFinalSelecteSupplier_SupplierContactPerson");
        });

        modelBuilder.Entity<PosClosingDay>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PosClosingDayCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PosClosingDay_User1");

            entity.HasOne(d => d.JournalEntry).WithMany(p => p.PosClosingDays).HasConstraintName("FK_PosClosingDay_DailyJournalEntry");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PosClosingDayModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PosClosingDay_User2");

            entity.HasOne(d => d.Store).WithMany(p => p.PosClosingDays)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PosClosingDay_InventoryStore");

            entity.HasOne(d => d.User).WithMany(p => p.PosClosingDayUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PosClosingDay_User");
        });

        modelBuilder.Entity<PosNumber>(entity =>
        {
            entity.HasOne(d => d.Store).WithMany(p => p.PosNumbers).HasConstraintName("FK_PosNumber_InventoryStore");
        });

        modelBuilder.Entity<Pricing>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.BompriceTotal).HasDefaultValue(0m);
            entity.Property(e => e.QuantityTotal).HasDefaultValue(0.0);
            entity.Property(e => e.Total).HasDefaultValue(0m);

            entity.HasOne(d => d.Branch).WithMany(p => p.Pricings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pricing_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PricingCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pricing_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PricingModifiedByNavigations).HasConstraintName("FK_Pricing_User2");

            entity.HasOne(d => d.PricingPerson).WithMany(p => p.PricingPricingPeople).HasConstraintName("FK_Pricing_User");
        });

        modelBuilder.Entity<PricingBom>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PricingBomCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingBOM_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PricingBoms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingBOM_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PricingBomModifiedByNavigations).HasConstraintName("FK_PricingBOM_User1");

            entity.HasOne(d => d.PricingProduct).WithMany(p => p.PricingBoms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingBOM_Pricing");
        });

        modelBuilder.Entity<PricingClarificationAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PricingClarificationAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingClarificationAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PricingClarificationAttachmentModifiedByNavigations).HasConstraintName("FK_PricingClarificationAttachment_User1");

            entity.HasOne(d => d.PricingClarification).WithMany(p => p.PricingClarificationAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingClarificationAttachment_PricingClearfication");
        });

        modelBuilder.Entity<PricingClearfication>(entity =>
        {
            entity.HasOne(d => d.CrreatedByNavigation).WithMany(p => p.PricingClearficationCrreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingClearfication_User");

            entity.HasOne(d => d.Pricing).WithMany(p => p.PricingClearfications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingClearfication_Pricing");

            entity.HasOne(d => d.SentToNavigation).WithMany(p => p.PricingClearficationSentToNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingClearfication_User1");
        });

        modelBuilder.Entity<PricingExtraCost>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PricingExtraCostCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingExtraCost_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PricingExtraCostCurrencies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingExtraCost_Currency");

            entity.HasOne(d => d.LocalCurrency).WithMany(p => p.PricingExtraCostLocalCurrencies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingExtraCost_Currency1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PricingExtraCostModifiedByNavigations).HasConstraintName("FK_PricingExtraCost_User1");

            entity.HasOne(d => d.Pricing).WithMany(p => p.PricingExtraCosts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingExtraCost_Pricing");
        });

        modelBuilder.Entity<PricingProduct>(entity =>
        {
            entity.Property(e => e.SalesManagerAddPrice).HasDefaultValue(0m);
            entity.Property(e => e.SalesManagerAddPricePercentage).HasDefaultValue(0.0);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PricingProductCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingProduct_User");

            entity.HasOne(d => d.InventoryItemCategory).WithMany(p => p.PricingProducts).HasConstraintName("FK_PricingProduct_InventoryItemCategory");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.PricingProducts).HasConstraintName("FK_PricingProduct_InventoryItem");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PricingProductModifiedByNavigations).HasConstraintName("FK_PricingProduct_User1");

            entity.HasOne(d => d.Pricing).WithMany(p => p.PricingProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingProduct_Pricing");

            entity.HasOne(d => d.ProductGroup).WithMany(p => p.PricingProducts).HasConstraintName("FK_PricingProduct_ProductGroup");

            entity.HasOne(d => d.Product).WithMany(p => p.PricingProducts).HasConstraintName("FK_PricingProduct_SalesOfferProduct");
        });

        modelBuilder.Entity<PricingProductAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PricingProductAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingProductAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PricingProductAttachmentModifiedByNavigations).HasConstraintName("FK_PricingProductAttachment_User1");

            entity.HasOne(d => d.PricingProduct).WithMany(p => p.PricingProductAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingProductAttachment_PricingProduct");
        });

        modelBuilder.Entity<PricingTerm>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PricingTermCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingTerm_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PricingTermModifiedByNavigations).HasConstraintName("FK_PricingTerm_User1");

            entity.HasOne(d => d.Pricing).WithMany(p => p.PricingTerms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingTerm_Pricing");

            entity.HasOne(d => d.TermGroup).WithMany(p => p.PricingTerms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PricingTerm_TermsGroups");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProductModifiedByNavigations).HasConstraintName("FK_Product_User1");

            entity.HasOne(d => d.ProductGroup).WithMany(p => p.Products).HasConstraintName("FK_Product_ProductGroup");
        });

        modelBuilder.Entity<ProductGroup>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductGroupCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductGroup_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProductGroupModifiedByNavigations).HasConstraintName("FK_ProductGroup_User1");
        });

        modelBuilder.Entity<ProgressType>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProgressTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProgressType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProgressTypeModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProgressType_User1");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project", tb => tb.HasTrigger("Trig_Project_log"));

            entity.HasOne(d => d.BillingType).WithMany(p => p.Projects).HasConstraintName("FK_Project_BillingType");

            entity.HasOne(d => d.Branch).WithMany(p => p.Projects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Project_Branch");

            entity.HasOne(d => d.CostType).WithMany(p => p.Projects).HasConstraintName("FK_Project_ProjectCostingType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Project_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.Projects).HasConstraintName("FK_Project_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectModifiedByNavigations).HasConstraintName("FK_Project_User1");

            entity.HasOne(d => d.Priorty).WithMany(p => p.Projects).HasConstraintName("FK_Project_Priority");

            entity.HasOne(d => d.ProjectManager).WithMany(p => p.ProjectProjectManagers).HasConstraintName("FK_Project_User2");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.Projects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Project_SalesOffer");
        });

        modelBuilder.Entity<ProjectAssignUser>(entity =>
        {
            entity.ToTable("ProjectAssignUser", tb =>
                {
                    tb.HasTrigger("Trig_ProjectAssignUser_Delete_log");
                    tb.HasTrigger("Trig_ProjectAssignUser_log");
                });

            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.ProjectAssignUserCreationByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectAssignUser_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectAssignUserModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectAssignUser_User2");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectAssignUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectAssignUser_Project");

            entity.HasOne(d => d.Role).WithMany(p => p.ProjectAssignUsers).HasConstraintName("FK_ProjectAssignUser_Role");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectAssignUserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectAssignUser_User");
        });

        modelBuilder.Entity<ProjectAttachment>(entity =>
        {
            entity.Property(e => e.FileExtenssion).IsFixedLength();

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectAttachment_Project");
        });

        modelBuilder.Entity<ProjectCheque>(entity =>
        {
            entity.HasOne(d => d.ChequeChashingStatus).WithMany(p => p.ProjectCheques).HasConstraintName("FK_ProjectCheque_ChequeCashingStatus");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectChequeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectCheque_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.ProjectCheques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectCheque_Currency");

            entity.HasOne(d => d.MaintenanceFor).WithMany(p => p.ProjectCheques).HasConstraintName("FK_ProjectCheque_MaintenanceFor");

            entity.HasOne(d => d.MaintenanceOrder).WithMany(p => p.ProjectCheques).HasConstraintName("FK_ProjectCheque_ManagementOfMaintenanceOrder");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectChequeModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectCheque_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectCheques).HasConstraintName("FK_ProjectCheque_Project");

            entity.HasOne(d => d.WithdrawedByNavigation).WithMany(p => p.ProjectChequeWithdrawedByNavigations).HasConstraintName("FK_ProjectCheque_User2");
        });

        modelBuilder.Entity<ProjectContactPerson>(entity =>
        {
            entity.HasOne(d => d.Area).WithMany(p => p.ProjectContactPeople).HasConstraintName("FK_ProjectContactPerson_Area");

            entity.HasOne(d => d.Country).WithMany(p => p.ProjectContactPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectContactPerson_Country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectContactPersonCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectContactPerson_User");

            entity.HasOne(d => d.Governorate).WithMany(p => p.ProjectContactPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectContactPerson_Governorate");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectContactPersonModifiedByNavigations).HasConstraintName("FK_ProjectContactPerson_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectContactPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectContactPerson_Project");
        });

        modelBuilder.Entity<ProjectFabrication>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabrication_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFabricationModifiedByNavigations).HasConstraintName("FK_ProjectFabrication_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectFabrications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabrication_Project");
        });

        modelBuilder.Entity<ProjectFabricationAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFabricationAttachmentModifiedByNavigations).HasConstraintName("FK_ProjectFabricationAttachment_User1");

            entity.HasOne(d => d.ProjectFabrication).WithMany(p => p.ProjectFabricationAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationAttachment_ProjectFabrication");
        });

        modelBuilder.Entity<ProjectFabricationBoq>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationBoqCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationBOQ_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFabricationBoqModifiedByNavigations).HasConstraintName("FK_ProjectFabricationBOQ_User1");

            entity.HasOne(d => d.ProjectFabrication).WithMany(p => p.ProjectFabricationBoqs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationBOQ_ProjectFabrication");

            entity.HasOne(d => d.SalesOfferProduct).WithMany(p => p.ProjectFabricationBoqs).HasConstraintName("FK__ProjectFa__Sales__60A8F2BD");
        });

        modelBuilder.Entity<ProjectFabricationJobTitle>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationJobTitleCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationJobTitle_User");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.ProjectFabricationJobTitles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationJobTitle_JobTitle");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFabricationJobTitleModifiedByNavigations).HasConstraintName("FK_ProjectFabricationJobTitle_User1");
        });

        modelBuilder.Entity<ProjectFabricationOrderUser>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationOrderUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationOrderUsers_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFabricationOrderUserModifiedByNavigations).HasConstraintName("FK_ProjectFabricationOrderUsers_User2");

            entity.HasOne(d => d.ProjectFabrication).WithMany(p => p.ProjectFabricationOrderUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationOrderUsers_ProjectFabrication");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectFabricationOrderUserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationOrderUsers_User");
        });

        modelBuilder.Entity<ProjectFabricationReport>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationReportCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReport_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFabricationReportModifiedByNavigations).HasConstraintName("FK_ProjectFabricationReport_User1");

            entity.HasOne(d => d.ProjectFabrication).WithMany(p => p.ProjectFabricationReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReport_ProjectFabrication");
        });

        modelBuilder.Entity<ProjectFabricationReportAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationReportAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFabricationReportAttachmentModifiedByNavigations).HasConstraintName("FK_ProjectFabricationReportAttachment_User1");

            entity.HasOne(d => d.ProjectFabricationReport).WithMany(p => p.ProjectFabricationReportAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportAttachment_ProjectFabricationReport");
        });

        modelBuilder.Entity<ProjectFabricationReportClarification>(entity =>
        {
            entity.HasOne(d => d.ClarificationUser).WithMany(p => p.ProjectFabricationReportClarificationClarificationUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportClarification_User");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationReportClarificationCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportClarification_User1");

            entity.HasOne(d => d.ModifedByNavigation).WithMany(p => p.ProjectFabricationReportClarificationModifedByNavigations).HasConstraintName("FK_ProjectFabricationReportClarification_User2");

            entity.HasOne(d => d.ProjectFabricationReport).WithMany(p => p.ProjectFabricationReportClarifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportClarification_ProjectFabricationReport");
        });

        modelBuilder.Entity<ProjectFabricationReportClarificationAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationReportClarificationAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportClarificationAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFabricationReportClarificationAttachmentModifiedByNavigations).HasConstraintName("FK_ProjectFabricationReportClarificationAttachment_User1");

            entity.HasOne(d => d.ProjectFabricationReportClarification).WithMany(p => p.ProjectFabricationReportClarificationAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportClarificationAttachment_ProjectFabricationReportClarification");
        });

        modelBuilder.Entity<ProjectFabricationReportUser>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.ProjectFabricationReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportUsers_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationReportUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportUsers_User1");

            entity.HasOne(d => d.Department).WithMany(p => p.ProjectFabricationReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportUsers_Department");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.ProjectFabricationReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportUsers_JobTitle");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFabricationReportUserModifiedByNavigations).HasConstraintName("FK_ProjectFabricationReportUsers_User2");

            entity.HasOne(d => d.ProjectFabricationReport).WithMany(p => p.ProjectFabricationReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportUsers_ProjectFabricationReport");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectFabricationReportUserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationReportUsers_User");
        });

        modelBuilder.Entity<ProjectFabricationVersion>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFabricationVersions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationVersion_User");

            entity.HasOne(d => d.ProjectFabrication).WithMany(p => p.ProjectFabricationVersions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationVersion_ProjectFabrication");
        });

        modelBuilder.Entity<ProjectFabricationWorkshopStationHistory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.IsCurrent).HasDefaultValue(true);

            entity.HasOne(d => d.FabricationOrder).WithMany(p => p.ProjectFabricationWorkshopStationHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationWorkshopStationHistory_ProjectFabrication");

            entity.HasOne(d => d.ProjectWorkshopStation).WithMany(p => p.ProjectFabricationWorkshopStationHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFabricationWorkshopStationHistory_ProjectWorkshopStation");
        });

        modelBuilder.Entity<ProjectFinishInstallationAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectFinishInstallationAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFinishInstallationAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectFinishInstallationAttachmentModifiedByNavigations).HasConstraintName("FK_ProjectFinishInstallationAttachment_User1");

            entity.HasOne(d => d.ProjectInstallationReport).WithMany(p => p.ProjectFinishInstallationAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectFinishInstallationAttachment_ProjectInstallation");
        });

        modelBuilder.Entity<ProjectInstallAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInstallAttachmentModifiedByNavigations).HasConstraintName("FK_ProjectInstallAttachment_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectInstallAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallAttachment_Project");
        });

        modelBuilder.Entity<ProjectInstallation>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallation_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInstallationModifiedByNavigations).HasConstraintName("FK_ProjectInstallation_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectInstallations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Project_ProjectInstallation");
        });

        modelBuilder.Entity<ProjectInstallationAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInstallationAttachmentModifiedByNavigations).HasConstraintName("FK_ProjectInstallationAttachment_User1");

            entity.HasOne(d => d.ProjectInstallation).WithMany(p => p.ProjectInstallationAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationAttachment_ProjectInstallation");
        });

        modelBuilder.Entity<ProjectInstallationBoq>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationBoqCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationBOQ_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInstallationBoqModifiedByNavigations).HasConstraintName("FK_ProjectInstallationBOQ_User1");

            entity.HasOne(d => d.ProjectInstallation).WithMany(p => p.ProjectInstallationBoqs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationBOQ_ProjectInstallation");

            entity.HasOne(d => d.SalesOfferProduct).WithMany(p => p.ProjectInstallationBoqs).HasConstraintName("FK__ProjectIn__Sales__619D16F6");
        });

        modelBuilder.Entity<ProjectInstallationJobTitle>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationJobTitleCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationJobTitle_User");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.ProjectInstallationJobTitles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationJobTitle_JobTitle");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInstallationJobTitleModifiedByNavigations).HasConstraintName("FK_ProjectInstallationJobTitle_User1");
        });

        modelBuilder.Entity<ProjectInstallationReport>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationReportCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReport_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInstallationReportModifiedByNavigations).HasConstraintName("FK_ProjectInstallationReport_User1");

            entity.HasOne(d => d.ProjectInstallation).WithMany(p => p.ProjectInstallationReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReport_ProjectInstallation");
        });

        modelBuilder.Entity<ProjectInstallationReportAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationReportAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInstallationReportAttachmentModifiedByNavigations).HasConstraintName("FK_ProjectInstallationReportAttachment_User1");

            entity.HasOne(d => d.ProjectInstallationReport).WithMany(p => p.ProjectInstallationReportAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportAttachment_ProjectInstallationReport");
        });

        modelBuilder.Entity<ProjectInstallationReportClarification>(entity =>
        {
            entity.HasOne(d => d.ClarificationUser).WithMany(p => p.ProjectInstallationReportClarificationClarificationUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportClarification_User");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationReportClarificationCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportClarification_User1");

            entity.HasOne(d => d.ModifedByNavigation).WithMany(p => p.ProjectInstallationReportClarificationModifedByNavigations).HasConstraintName("FK_ProjectInstallationReportClarification_User2");

            entity.HasOne(d => d.ProjectInstallationReport).WithMany(p => p.ProjectInstallationReportClarifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportClarification_ProjectInstallationReport");
        });

        modelBuilder.Entity<ProjectInstallationReportClarificationAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationReportClarificationAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportClarificationAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInstallationReportClarificationAttachmentModifiedByNavigations).HasConstraintName("FK_ProjectInstallationReportClarificationAttachment_User1");

            entity.HasOne(d => d.ProjectInstallationReportClarification).WithMany(p => p.ProjectInstallationReportClarificationAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportClarificationAttachment_ProjectInstallationReportClarification");
        });

        modelBuilder.Entity<ProjectInstallationReportUser>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.ProjectInstallationReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportUsers_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationReportUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportUsers_User1");

            entity.HasOne(d => d.Department).WithMany(p => p.ProjectInstallationReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportUsers_Department");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.ProjectInstallationReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportUsers_JobTitle");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInstallationReportUserModifiedByNavigations).HasConstraintName("FK_ProjectInstallationReportUsers_User2");

            entity.HasOne(d => d.ProjectInstallationReport).WithMany(p => p.ProjectInstallationReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportUsers_ProjectInstallationReport");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectInstallationReportUserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationReportUsers_User");
        });

        modelBuilder.Entity<ProjectInstallationVersion>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInstallationVersions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationVersion_User");

            entity.HasOne(d => d.ProjectInstallation).WithMany(p => p.ProjectInstallationVersions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInstallationVersion_ProjectInstallation");
        });

        modelBuilder.Entity<ProjectInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Invoice");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInvoiceCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInvoice_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInvoiceModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInvoice_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInvoice_Project");
        });

        modelBuilder.Entity<ProjectInvoiceCollected>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInvoiceCollectedCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInvoiceCollected_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectInvoiceCollectedModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInvoiceCollected_User1");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.ProjectInvoiceCollecteds).HasConstraintName("FK_ProjectInvoiceCollected_PaymentMethod");

            entity.HasOne(d => d.ProjectInvoice).WithMany(p => p.ProjectInvoiceCollecteds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInvoiceCollected_ProjectInvoice");
        });

        modelBuilder.Entity<ProjectInvoiceItem>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectInvoiceItemCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInvoiceItem_User");

            entity.HasOne(d => d.ModifedByNavigation).WithMany(p => p.ProjectInvoiceItemModifedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInvoiceItem_User1");

            entity.HasOne(d => d.ProjectInvoice).WithMany(p => p.ProjectInvoiceItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectInvoiceItem_ProjectInvoice");

            entity.HasOne(d => d.Uom).WithMany(p => p.ProjectInvoiceItems).HasConstraintName("FK_ProjectInvoiceItem_InventoryUOM");
        });

        modelBuilder.Entity<ProjectLetterOfCredit>(entity =>
        {
            entity.HasOne(d => d.Currency).WithMany(p => p.ProjectLetterOfCredits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectLetterOfCredit_Currency");

            entity.HasOne(d => d.LetterOfCreditType).WithMany(p => p.ProjectLetterOfCredits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectLetterOfCredit_LetterOfCreditType");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectLetterOfCredits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectLetterOfCredit_Project");
        });

        modelBuilder.Entity<ProjectLetterOfCreditComment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectLetterOfCreditCommentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectLetterOfCreditComment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectLetterOfCreditCommentModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectLetterOfCreditComment_User1");

            entity.HasOne(d => d.ProjectLetterOfCredit).WithMany(p => p.ProjectLetterOfCreditComments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectLetterOfCreditComment_ProjectLetterOfCredit");
        });

        modelBuilder.Entity<ProjectPaymentJournalEntry>(entity =>
        {
            entity.HasOne(d => d.DailyJournalEntry).WithMany(p => p.ProjectPaymentJournalEntries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectPaymentJournalEntry_DailyJournalEntry");
        });

        modelBuilder.Entity<ProjectPaymentTerm>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectPaymentTermCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectPaymentTerms_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.ProjectPaymentTerms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectPaymentTerms_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectPaymentTermModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectPaymentTerms_User1");

            entity.HasOne(d => d.PaymentTerm).WithMany(p => p.ProjectPaymentTerms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectPaymentTerms_PaymentTerms");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectPaymentTerms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectPaymentTerms_Project");
        });

        modelBuilder.Entity<ProjectProgress>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectProgressCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgress_User");

            entity.HasOne(d => d.DeliveryType).WithMany(p => p.ProjectProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgress_DeliveryType");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectProgressModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgress_User1");

            entity.HasOne(d => d.ProgressStatus).WithMany(p => p.ProjectProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgress_ProgressStatus");

            entity.HasOne(d => d.ProgressType).WithMany(p => p.ProjectProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgress_ProgressType");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgress_Project");
        });

        modelBuilder.Entity<ProjectProgressUser>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectProgressUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgressUsers_ProjectProgressUsers");

            entity.HasOne(d => d.HrUser).WithMany(p => p.ProjectProgressUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgressUsers_HrUser");

            entity.HasOne(d => d.InventoryItemCategory).WithMany(p => p.ProjectProgressUsers).HasConstraintName("FK_ProjectProgressUsers_InventoryItemCategory");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectProgressUserModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgressUsers_User");

            entity.HasOne(d => d.ProjectProgress).WithMany(p => p.ProjectProgressUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectProgressUsers_ProjectProgress");
        });

        modelBuilder.Entity<ProjectSprint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProjectSprinit");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectSprintCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectSprint_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectSprintModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectSprint_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectSprints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectSprint_Project");
        });

        modelBuilder.Entity<ProjectTm>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TimeTracking).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.ProjectTms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTM_Branch");

            entity.HasOne(d => d.Client).WithMany(p => p.ProjectTms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTM_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectTmCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTM_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectTmModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTM_User1");

            entity.HasOne(d => d.Priorty).WithMany(p => p.ProjectTms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTM_Priority");
        });

        modelBuilder.Entity<ProjectTmassignUser>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectTmassignUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMAssignUser_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectTmassignUserModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMAssignUser_User1");

            entity.HasOne(d => d.ProjectTm).WithMany(p => p.ProjectTmassignUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMAssignUser_ProjectTM");
        });

        modelBuilder.Entity<ProjectTmattachment>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.ProjectTmattachments).HasConstraintName("FK_ProjectTMAttachment_AttachmentCategory");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectTmattachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectTmattachmentModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMAttachment_User1");

            entity.HasOne(d => d.ProjectTm).WithMany(p => p.ProjectTmattachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMAttachment_ProjectTM");
        });

        modelBuilder.Entity<ProjectTmimpDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ImpDate");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectTmimpDateCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMImpDate_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectTmimpDateModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMImpDate_User1");

            entity.HasOne(d => d.ProjectTm).WithMany(p => p.ProjectTmimpDates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImpDate_ProjectTM");
        });

        modelBuilder.Entity<ProjectTmrevision>(entity =>
        {
            entity.Property(e => e.Billable).HasDefaultValue(false);
            entity.Property(e => e.TimeTracking).HasDefaultValue(false);

            entity.HasOne(d => d.Client).WithMany(p => p.ProjectTmrevisions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMRevision_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectTmrevisionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMRevision_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectTmrevisionModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMRevision_User1");

            entity.HasOne(d => d.Priority).WithMany(p => p.ProjectTmrevisions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMRevision_Priority");
        });

        modelBuilder.Entity<ProjectTmsprint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProjectTMSprinit");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProjectTmsprintCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMSprint_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectTmsprintModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMSprint_User1");

            entity.HasOne(d => d.ProjectTm).WithMany(p => p.ProjectTmsprints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTMSprint_ProjectTM");
        });

        modelBuilder.Entity<ProjectWorkFlow>(entity =>
        {
            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.ProjectWorkFlowCreateByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectWorkFlow_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ProjectWorkFlowModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectWorkFlow_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectWorkFlows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectWorkFlow_Project");
        });

        modelBuilder.Entity<ProjectWorkshopStation>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectWorkshopStations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectWorkshopStation_Project");

            entity.HasOne(d => d.WorkshopStation).WithMany(p => p.ProjectWorkshopStations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectWorkshopStation_WorkshopStation");
        });

        modelBuilder.Entity<PrsupplierOffer>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PrsupplierOfferCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOffer_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PrsupplierOfferModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOffer_User1");

            entity.HasOne(d => d.Po).WithMany(p => p.PrsupplierOffers).HasConstraintName("FK_PRSupplierOffer_PurchasePO");

            entity.HasOne(d => d.Pr).WithMany(p => p.PrsupplierOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOffer_PurchaseRequest");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PrsupplierOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOffer_Supplier");
        });

        modelBuilder.Entity<PrsupplierOfferItem>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PrsupplierOfferItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOfferItem_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PrsupplierOfferItems).HasConstraintName("FK__PRSupplie__Curre__0758AB8A");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.PrsupplierOfferItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOfferItem_InventoryItem");

            entity.HasOne(d => d.Mritem).WithMany(p => p.PrsupplierOfferItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOfferItem_InventoryMatrialRequestItems");

            entity.HasOne(d => d.Po).WithMany(p => p.PrsupplierOfferItems).HasConstraintName("FK_PRSupplierOfferItem_PurchasePO");

            entity.HasOne(d => d.Poitem).WithMany(p => p.PrsupplierOfferItems).HasConstraintName("FK_PRSupplierOfferItem_PurchasePOItem");

            entity.HasOne(d => d.Pr).WithMany(p => p.PrsupplierOfferItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOfferItem_PurchaseRequestItems");

            entity.HasOne(d => d.PrsupplierOffer).WithMany(p => p.PrsupplierOfferItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOfferItem_PRSupplierOffer");

            entity.HasOne(d => d.Uom).WithMany(p => p.PrsupplierOfferItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRSupplierOfferItem_InventoryUOM");
        });

        modelBuilder.Entity<PuchasePoshipment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PuchasePoshipmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PuchasePOShipment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PuchasePoshipmentModifiedByNavigations).HasConstraintName("FK_PuchasePOShipment_User1");

            entity.HasOne(d => d.Po).WithMany(p => p.PuchasePoshipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PuchasePOShipment_PurchasePO");

            entity.HasOne(d => d.ShipmentShippingMethodDetails).WithMany(p => p.PuchasePoshipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PuchasePOShipment_PurchasePOShipmentShippingMethodDetails");
        });

        modelBuilder.Entity<PurchaseImportPosetting>(entity =>
        {
            entity.HasOne(d => d.Po).WithMany(p => p.PurchaseImportPosettings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseImportPOSetting_PurchasePO");
        });

        modelBuilder.Entity<PurchasePaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Inventory_PaymentMethod");
        });

        modelBuilder.Entity<PurchasePo>(entity =>
        {
            entity.HasOne(d => d.AssignedAccountant).WithMany(p => p.PurchasePoAssignedAccountants).HasConstraintName("FK__PurchaseP__Assig__33B5855E");

            entity.HasOne(d => d.AssignedPurchasingPerson).WithMany(p => p.PurchasePoAssignedPurchasingPeople).HasConstraintName("FK__PurchaseP__Assig__40DA7652");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePO_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchasePoModifiedByNavigations).HasConstraintName("FK_PurchasePO_User1");

            entity.HasOne(d => d.Potype).WithMany(p => p.PurchasePos).HasConstraintName("FK__PurchaseP__POTyp__2DFCAC08");

            entity.HasOne(d => d.SentToSupplierContactPerson).WithMany(p => p.PurchasePos).HasConstraintName("FK__PurchaseP__SentT__41CE9A8B");

            entity.HasOne(d => d.ToSupplier).WithMany(p => p.PurchasePos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePO_Supplier");

            entity.HasOne(d => d.UserIdforFinalApproveNavigation).WithMany(p => p.PurchasePoUserIdforFinalApproveNavigations).HasConstraintName("FK__PurchaseP__UserI__359DCDD0");

            entity.HasOne(d => d.UserIdforTechApproveNavigation).WithMany(p => p.PurchasePoUserIdforTechApproveNavigations).HasConstraintName("FK__PurchaseP__UserI__34A9A997");
        });

        modelBuilder.Entity<PurchasePoamountPaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PurchasePOPaymentMethod");

            entity.HasOne(d => d.Po).WithMany(p => p.PurchasePoamountPaymentMethods)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPaymentMethod_PurchasePO");

            entity.HasOne(d => d.PurchasePaymentMethod).WithMany(p => p.PurchasePoamountPaymentMethods)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPaymentMethod_PurchasePaymentMethod");
        });

        modelBuilder.Entity<PurchasePoattachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Po).WithMany(p => p.PurchasePoattachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOAttachment_PurchasePO");
        });

        modelBuilder.Entity<PurchasePoinactiveTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PurchasePOInactiveTasks");

            entity.HasOne(d => d.Po).WithMany(p => p.PurchasePoinactiveTasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInactiveTasks_PurchasePO");
        });

        modelBuilder.Entity<PurchasePoinvoice>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoinvoiceCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoice_User");

            entity.HasOne(d => d.InvoiceAttachement).WithMany(p => p.PurchasePoinvoices).HasConstraintName("FK_PurchasePOInvoice_PurchasePOInvoiceAttachment");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchasePoinvoiceModifiedByNavigations).HasConstraintName("FK_PurchasePOInvoice_User1");

            entity.HasOne(d => d.Po).WithMany(p => p.PurchasePoinvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoice_PurchasePO");

            entity.HasOne(d => d.PurchasePoinvoiceType).WithMany(p => p.PurchasePoinvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoice_PurchasePOInvoiceType");
        });

        modelBuilder.Entity<PurchasePoinvoiceAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.PurchasePoinvoice).WithMany(p => p.PurchasePoinvoiceAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceAttachment_PurchasePOInvoice");
        });

        modelBuilder.Entity<PurchasePoinvoiceCalculatedShipmentValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PurchasePOInvoiceCalculatedShipment");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoinvoiceCalculatedShipmentValues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceCalculatedShipment_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoinvoiceCalculatedShipmentValues).HasConstraintName("FK_PurchasePOInvoiceCalculatedShipment_Currency");

            entity.HasOne(d => d.PurchasePoinvoice).WithMany(p => p.PurchasePoinvoiceCalculatedShipmentValues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceCalculatedShipment_PurchasePOInvoice");
        });

        modelBuilder.Entity<PurchasePoinvoiceClosedPayment>(entity =>
        {
            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoinvoiceClosedPayments).HasConstraintName("FK_PurchasePOInvoiceClosedPayment_Currency");

            entity.HasOne(d => d.Poinvoice).WithMany(p => p.PurchasePoinvoiceClosedPayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceClosedPayment_PurchasePOInvoice");

            entity.HasOne(d => d.PurchasePaymentMethod).WithMany(p => p.PurchasePoinvoiceClosedPayments).HasConstraintName("FK_PurchasePOInvoiceClosedPayment_PurchasePaymentMethod");

            entity.HasOne(d => d.PurchasePoinvoiceAttachment).WithMany(p => p.PurchasePoinvoiceClosedPayments).HasConstraintName("FK_PurchasePOInvoiceClosedPayment_PurchasePOInvoiceAttachment");
        });

        modelBuilder.Entity<PurchasePoinvoiceDeduction>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoinvoiceDeductionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceDeduction_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoinvoiceDeductions).HasConstraintName("FK_PurchasePOInvoiceDeduction_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchasePoinvoiceDeductionModifiedByNavigations).HasConstraintName("FK_PurchasePOInvoiceDeduction_User1");

            entity.HasOne(d => d.PodeductionType).WithMany(p => p.PurchasePoinvoiceDeductions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceDeduction_PurchasePOInvoiceDeductionType");
        });

        modelBuilder.Entity<PurchasePoinvoiceDeductionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PurchasePOInvoiceDeductions");
        });

        modelBuilder.Entity<PurchasePoinvoiceExtraFee>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoinvoiceExtraFeeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceExtraFees_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoinvoiceExtraFees).HasConstraintName("FK_PurchasePOInvoiceExtraFees_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchasePoinvoiceExtraFeeModifiedByNavigations).HasConstraintName("FK_PurchasePOInvoiceExtraFees_User1");

            entity.HasOne(d => d.PoinvoiceExtraFeesType).WithMany(p => p.PurchasePoinvoiceExtraFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceExtraFees_PurchasePOInvoiceExtraFeesType");

            entity.HasOne(d => d.Poitem).WithMany(p => p.PurchasePoinvoiceExtraFees).HasConstraintName("FK_PurchasePOInvoiceExtraFees_PurchasePOItem");
        });

        modelBuilder.Entity<PurchasePoinvoiceFinalExpensi>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoinvoiceFinalExpensis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceFinalExpensis_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoinvoiceFinalExpensis).HasConstraintName("FK_PurchasePOInvoiceFinalExpensis_Currency");

            entity.HasOne(d => d.PurchasePoinvoice).WithMany(p => p.PurchasePoinvoiceFinalExpensis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceFinalExpensis_PurchasePOInvoice");
        });

        modelBuilder.Entity<PurchasePoinvoiceNotIncludedTax>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PurchasePOInvoiceNotIncludedTax_1");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoinvoiceNotIncludedTaxCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceNotIncludedTax_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoinvoiceNotIncludedTaxes).HasConstraintName("FK_PurchasePOInvoiceNotIncludedTax_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchasePoinvoiceNotIncludedTaxModifiedByNavigations).HasConstraintName("FK_PurchasePOInvoiceNotIncludedTax_User1");

            entity.HasOne(d => d.PonotIncludedTaxType).WithMany(p => p.PurchasePoinvoiceNotIncludedTaxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceNotIncludedTax_PurchasePOInvoiceNotIncludedTaxType");
        });

        modelBuilder.Entity<PurchasePoinvoiceNotIncludedTaxType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PurchasePOInvoiceNotIncludedTax");
        });

        modelBuilder.Entity<PurchasePoinvoiceTaxIncluded>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoinvoiceTaxIncludedCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceTaxIncluded_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoinvoiceTaxIncludeds).HasConstraintName("FK_PurchasePOInvoiceTaxIncluded_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchasePoinvoiceTaxIncludedModifiedByNavigations).HasConstraintName("FK_PurchasePOInvoiceTaxIncluded_User1");

            entity.HasOne(d => d.Poinvoice).WithMany(p => p.PurchasePoinvoiceTaxIncludeds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceTaxIncluded_PurchasePOInvoice");

            entity.HasOne(d => d.PotaxIncludedType).WithMany(p => p.PurchasePoinvoiceTaxIncludeds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceTaxIncluded_PurchasePOInvoiceTaxIncludedType");
        });

        modelBuilder.Entity<PurchasePoinvoiceTaxIncludedType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PurchasePOTaxIncludedType");
        });

        modelBuilder.Entity<PurchasePoinvoiceTotalOrderCustomFee>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoinvoiceTotalOrderCustomFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceTotalOrderCustomFee_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoinvoiceTotalOrderCustomFees).HasConstraintName("FK_PurchasePOInvoiceTotalOrderCustomFee_Currency");

            entity.HasOne(d => d.PurchasePoinvoice).WithMany(p => p.PurchasePoinvoiceTotalOrderCustomFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceTotalOrderCustomFee_PurchasePOInvoice");
        });

        modelBuilder.Entity<PurchasePoinvoiceUnloadingFee>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoinvoiceUnloadingFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceUnloadingFee_User");

            entity.HasOne(d => d.PurchasePoinvoice).WithMany(p => p.PurchasePoinvoiceUnloadingFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOInvoiceUnloadingFee_PurchasePOInvoice");
        });

        modelBuilder.Entity<PurchasePoitem>(entity =>
        {
            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoitems).HasConstraintName("FK__PurchaseP__Curre__5AEF4E10");

            entity.HasOne(d => d.FabricationOrder).WithMany(p => p.PurchasePoitems).HasConstraintName("FK_PurchasePOItem_ProjectFabrication");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.PurchasePoitems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOItem_InventoryItem");

            entity.HasOne(d => d.InventoryMatrialRequestItem).WithMany(p => p.PurchasePoitems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOItem_InventoryMatrialRequestItems");

            entity.HasOne(d => d.Project).WithMany(p => p.PurchasePoitems).HasConstraintName("FK_PurchasePOItem_Project");

            entity.HasOne(d => d.PurchasePo).WithMany(p => p.PurchasePoitems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOItem_PurchasePO");

            entity.HasOne(d => d.PurchaseRequestItem).WithMany(p => p.PurchasePoitems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOItem_PurchaseRequestItems");

            entity.HasOne(d => d.Uom).WithMany(p => p.PurchasePoitems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOItem_InventoryUOM");
        });

        modelBuilder.Entity<PurchasePopaymentSwift>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePopaymentSwiftCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPaymentSwift_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchasePopaymentSwiftModifiedByNavigations).HasConstraintName("FK_PurchasePOPaymentSwift_User1");

            entity.HasOne(d => d.Po).WithMany(p => p.PurchasePopaymentSwifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPaymentSwift_PurchasePO");

            entity.HasOne(d => d.PurchasePoattachment).WithMany(p => p.PurchasePopaymentSwifts).HasConstraintName("FK_PurchasePOPaymentSwift_PurchasePOAttachment");
        });

        modelBuilder.Entity<PurchasePopdf>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePopdfCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPdf_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchasePopdfModifiedByNavigations).HasConstraintName("FK_PurchasePOPdf_User1");

            entity.HasOne(d => d.Po).WithMany(p => p.PurchasePopdfs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPdf_PurchasePO");
        });

        modelBuilder.Entity<PurchasePopdfEditHistory>(entity =>
        {
            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.PurchasePopdfEditHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPdfEditHistory_User");

            entity.HasOne(d => d.Po).WithMany(p => p.PurchasePopdfEditHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPdfEditHistory_PurchasePO");
        });

        modelBuilder.Entity<PurchasePopdfTemplate>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePopdfTemplateCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPdfTemplate_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchasePopdfTemplateModifiedByNavigations).HasConstraintName("FK_PurchasePOPdfTemplate_User1");

            entity.HasOne(d => d.Po).WithMany(p => p.PurchasePopdfTemplates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOPdfTemplate_PurchasePO");
        });

        modelBuilder.Entity<PurchasePoshipmentDocument>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchasePoshipmentDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOShipmentDocuments_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoshipmentDocuments).HasConstraintName("FK_PurchasePOShipmentDocuments_Currency");
        });

        modelBuilder.Entity<PurchasePoshipmentShippingMethodDetail>(entity =>
        {
            entity.HasOne(d => d.Currency).WithMany(p => p.PurchasePoshipmentShippingMethodDetails).HasConstraintName("FK_PurchasePOShipmentShippingMethodDetails_Currency");

            entity.HasOne(d => d.ShippingMethod).WithMany(p => p.PurchasePoshipmentShippingMethodDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePOShipmentShippingMethodDetails_ShippingMethod");
        });

        modelBuilder.Entity<PurchasePotype>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<PurchaseRequest>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchaseRequestCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseRequest_User1");

            entity.HasOne(d => d.FromInventoryStore).WithMany(p => p.PurchaseRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseRequest_InventoryStore");

            entity.HasOne(d => d.MatrialRequest).WithMany(p => p.PurchaseRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseRequest_InventoryMatrialRequest");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchaseRequestModifiedByNavigations).HasConstraintName("FK_PurchaseRequest_User2");

            entity.HasOne(d => d.ToUser).WithMany(p => p.PurchaseRequestToUsers).HasConstraintName("FK_PurchaseRequest_User");
        });

        modelBuilder.Entity<PurchaseRequestItem>(entity =>
        {
            entity.HasOne(d => d.InventoryMatrialRequestItem).WithMany(p => p.PurchaseRequestItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseRequestItems_InventoryMatrialRequestItems");

            entity.HasOne(d => d.PurchaseRequest).WithMany(p => p.PurchaseRequestItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseRequestItems_PurchaseRequest");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<ReportCcgroup>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportCcgroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportCCGroup_User");

            entity.HasOne(d => d.Group).WithMany(p => p.ReportCcgroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportCCGroup_Group");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportCcgroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportCCGroup_Report");
        });

        modelBuilder.Entity<ReportCcuser>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportCcuserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportCCUser_User1");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportCcusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportCCUser_Report");

            entity.HasOne(d => d.User).WithMany(p => p.ReportCcuserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportCCUser_User");
        });

        modelBuilder.Entity<ReportGroup>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportGroup_User");

            entity.HasOne(d => d.Group).WithMany(p => p.ReportGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportGroup_Group");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportGroup_Report");
        });

        modelBuilder.Entity<ReportUser>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReportUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportUser_User1");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportUser_Report");

            entity.HasOne(d => d.User).WithMany(p => p.ReportUserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportUser_User");
        });

        modelBuilder.Entity<RequieredCost>(entity =>
        {
            entity.HasOne(d => d.CostType).WithMany(p => p.RequieredCosts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequieredCost_CostType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RequieredCostCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequieredCost_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.RequieredCostModifiedByNavigations).HasConstraintName("FK_RequieredCost_User1");

            entity.HasOne(d => d.MovementAndDeliveryOrder).WithMany(p => p.RequieredCosts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequieredCost_MovmentsAndDeliveryOrder");
        });

        modelBuilder.Entity<RequieredCostAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RequieredCostAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequieredCostAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.RequieredCostAttachmentModifiedByNavigations).HasConstraintName("FK_RequieredCostAttachment_User1");

            entity.HasOne(d => d.RequieredCost).WithMany(p => p.RequieredCostAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequieredCostAttachment_RequieredCost");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasOne(d => d.Offer).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_SalesOffer");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_Reservations_Reservations");
        });

        modelBuilder.Entity<ReservationInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3214EC276071060E");

            entity.HasOne(d => d.Client).WithMany(p => p.ReservationInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservationInvoice_Client");

            entity.HasOne(d => d.Currency).WithMany(p => p.ReservationInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservationInvoice_Currency");

            entity.HasOne(d => d.InvoiceType).WithMany(p => p.ReservationInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservationInvoice_InvoiceType");

            entity.HasOne(d => d.Reservation).WithMany(p => p.ReservationInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservationInvoice_Reservation");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RoleCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.RoleModifiedByNavigations).HasConstraintName("FK_Role_User1");
        });

        modelBuilder.Entity<RoleModule>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.RoleModules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModule_Role");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasOne(d => d.Contract).WithMany(p => p.Salaries).HasConstraintName("FK_Salary_ContractID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalaryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salary_User1");

            entity.HasOne(d => d.Currency).WithMany(p => p.Salaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salary_Currency");

            entity.HasOne(d => d.HrUser).WithMany(p => p.Salaries).HasConstraintName("FK_Salary_HrUserId");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalaryModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salary_User2");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Salaries).HasConstraintName("FK_Salary_PaymentMethod");

            entity.HasOne(d => d.PaymentStrategy).WithMany(p => p.Salaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salary_PaymentStrategy");

            entity.HasOne(d => d.User).WithMany(p => p.SalaryUsers).HasConstraintName("FK_Salary_User");
        });

        modelBuilder.Entity<SalaryAllownce>(entity =>
        {
            entity.HasOne(d => d.AllowncesType).WithMany(p => p.SalaryAllownces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryAllownces_AllowncesType");

            entity.HasOne(d => d.Salary).WithMany(p => p.SalaryAllownces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryAllownces_SalaryAllownces");
        });

        modelBuilder.Entity<SalaryDeductionTax>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalaryDeductionTaxCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryDeductionTax_User");

            entity.HasOne(d => d.DeductionType).WithMany(p => p.SalaryDeductionTaxes).HasConstraintName("FK_SalaryDeductionTax_DeductionType");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalaryDeductionTaxModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryDeductionTax_User1");

            entity.HasOne(d => d.Salary).WithMany(p => p.SalaryDeductionTaxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryDeductionTax_Salary");

            entity.HasOne(d => d.SalaryTax).WithMany(p => p.SalaryDeductionTaxes).HasConstraintName("FK_SalaryDeductionTax_SalaryTax");
        });

        modelBuilder.Entity<SalaryInsurance>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalaryInsuranceCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryInsurance_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalaryInsuranceModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryInsurance_User1");

            entity.HasOne(d => d.Salary).WithMany(p => p.SalaryInsurances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryInsurance_Salary");

            entity.HasOne(d => d.User).WithMany(p => p.SalaryInsuranceUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryInsurance_User2");
        });

        modelBuilder.Entity<SalaryTax>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.SalaryTaxes).HasConstraintName("FK_SalaryTax_Branch");

            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.SalaryTaxes).HasConstraintName("FK_SalaryTax_User");

            entity.HasOne(d => d.SalaryType).WithMany(p => p.SalaryTaxes).HasConstraintName("FK_SalaryTax_SalaryType");

            entity.HasOne(d => d.TaxType).WithMany(p => p.SalaryTaxes).HasConstraintName("FK_SalaryTax_TaxType");
        });

        modelBuilder.Entity<SalesBranchProductTarget>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.SalesBranchProductTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchProductTarget_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesBranchProductTargetCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchProductTarget_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.SalesBranchProductTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchProductTarget_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesBranchProductTargetModifiedByNavigations).HasConstraintName("FK_SalesBranchProductTarget_User1");

            entity.HasOne(d => d.Product).WithMany(p => p.SalesBranchProductTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchProductTarget_InventoryItem");

            entity.HasOne(d => d.Target).WithMany(p => p.SalesBranchProductTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchProductTarget_SalesTarget");
        });

        modelBuilder.Entity<SalesBranchTarget>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.SalesBranchTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchTarget_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesBranchTargetCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchTarget_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.SalesBranchTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchTarget_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesBranchTargetModifiedByNavigations).HasConstraintName("FK_SalesBranchTarget_User1");

            entity.HasOne(d => d.Target).WithMany(p => p.SalesBranchTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchTarget_SalesTarget");
        });

        modelBuilder.Entity<SalesBranchUserProductTarget>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.SalesBranchUserProductTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserProductTarget_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesBranchUserProductTargetCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserProductTarget_User1");

            entity.HasOne(d => d.Currency).WithMany(p => p.SalesBranchUserProductTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserProductTarget_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesBranchUserProductTargetModifiedByNavigations).HasConstraintName("FK_SalesBranchUserProductTarget_User2");

            entity.HasOne(d => d.Product).WithMany(p => p.SalesBranchUserProductTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserProductTarget_InventoryItem");

            entity.HasOne(d => d.Target).WithMany(p => p.SalesBranchUserProductTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserProductTarget_SalesTarget");

            entity.HasOne(d => d.User).WithMany(p => p.SalesBranchUserProductTargetUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserProductTarget_User");
        });

        modelBuilder.Entity<SalesBranchUserTarget>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.SalesBranchUserTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserTarget_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesBranchUserTargetCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserTarget_User1");

            entity.HasOne(d => d.Currency).WithMany(p => p.SalesBranchUserTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserTarget_Currency");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesBranchUserTargetModifiedByNavigations).HasConstraintName("FK_SalesBranchUserTarget_User2");

            entity.HasOne(d => d.Target).WithMany(p => p.SalesBranchUserTargets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserTarget_SalesTarget");

            entity.HasOne(d => d.User).WithMany(p => p.SalesBranchUserTargetUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesBranchUserTarget_User");
        });

        modelBuilder.Entity<SalesMaintenanceOffer>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesMaintenanceOfferCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesMaintenanceOffer_User");

            entity.HasOne(d => d.LinkedSalesOffer).WithMany(p => p.SalesMaintenanceOfferLinkedSalesOffers).HasConstraintName("FK_SalesMaintenanceOffer_SalesOffer1");

            entity.HasOne(d => d.MaintenanceSalesOffer).WithMany(p => p.SalesMaintenanceOfferMaintenanceSalesOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesMaintenanceOffer_SalesOffer");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesMaintenanceOfferModifiedByNavigations).HasConstraintName("FK_SalesMaintenanceOffer_User1");
        });

        modelBuilder.Entity<SalesOffer>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.ClientApprove).HasDefaultValue(false);

            entity.HasOne(d => d.Branch).WithMany(p => p.SalesOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOffer_SalesOffer1");

            entity.HasOne(d => d.Client).WithMany(p => p.SalesOffers).HasConstraintName("FK_SalesOffer_Client");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOffer_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferModifiedByNavigations).HasConstraintName("FK_SalesOffer_User2");

            entity.HasOne(d => d.SalesPerson).WithMany(p => p.SalesOfferSalesPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOffer_User");
        });

        modelBuilder.Entity<SalesOfferAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferAttachmentModifiedByNavigations).HasConstraintName("FK_SalesOfferAttachment_User1");

            entity.HasOne(d => d.Offer).WithMany(p => p.SalesOfferAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachment_SalesOffer");
        });

        modelBuilder.Entity<SalesOfferAttachmentGroupPermission>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferAttachmentGroupPermissionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachmentGroupPermission_User");

            entity.HasOne(d => d.Group).WithMany(p => p.SalesOfferAttachmentGroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachmentGroupPermission_Group");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferAttachmentGroupPermissionModifiedByNavigations).HasConstraintName("FK_SalesOfferAttachmentGroupPermission_User1");

            entity.HasOne(d => d.OfferAttachment).WithMany(p => p.SalesOfferAttachmentGroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachmentGroupPermission_SalesOfferAttachment");

            entity.HasOne(d => d.Permission).WithMany(p => p.SalesOfferAttachmentGroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachmentGroupPermission_PermissionLevel");
        });

        modelBuilder.Entity<SalesOfferAttachmentUserPermission>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferAttachmentUserPermissionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachmentUserPermission_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferAttachmentUserPermissionModifiedByNavigations).HasConstraintName("FK_SalesOfferAttachmentUserPermission_User2");

            entity.HasOne(d => d.OfferAttachment).WithMany(p => p.SalesOfferAttachmentUserPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachmentUserPermission_SalesOfferAttachment");

            entity.HasOne(d => d.Permission).WithMany(p => p.SalesOfferAttachmentUserPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachmentUserPermission_PermissionLevel");

            entity.HasOne(d => d.User).WithMany(p => p.SalesOfferAttachmentUserPermissionUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferAttachmentUserPermission_User");
        });

        modelBuilder.Entity<SalesOfferDiscount>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferDiscountCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferDiscount_User");

            entity.HasOne(d => d.DiscountApprovedByNavigation).WithMany(p => p.SalesOfferDiscountDiscountApprovedByNavigations).HasConstraintName("FK_SalesOfferDiscount_User1");

            entity.HasOne(d => d.InvoicePayerClient).WithMany(p => p.SalesOfferDiscounts).HasConstraintName("FK_SalesOfferDiscount_Client");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferDiscountModifiedByNavigations).HasConstraintName("FK_SalesOfferDiscount_User2");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.SalesOfferDiscounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferDiscount_SalesOffer");
        });

        modelBuilder.Entity<SalesOfferEditHistory>(entity =>
        {
            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.SalesOfferEditHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferEditHistory_User");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.SalesOfferEditHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferEditHistory_SalesOffer");
        });

        modelBuilder.Entity<SalesOfferExpirationHistory>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferExpirationHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferExpirationHistory_User");
        });

        modelBuilder.Entity<SalesOfferExtraCost>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferExtraCostCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferExtraCosts_User");

            entity.HasOne(d => d.ExtraCostType).WithMany(p => p.SalesOfferExtraCosts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferExtraCosts_SalesExtraCostType");

            entity.HasOne(d => d.InvoicePayerClient).WithMany(p => p.SalesOfferExtraCosts).HasConstraintName("FK_SalesOfferExtraCosts_Client");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferExtraCostModifiedByNavigations).HasConstraintName("FK_SalesOfferExtraCosts_User1");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.SalesOfferExtraCosts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferExtraCosts_SalesOffer");
        });

        modelBuilder.Entity<SalesOfferGroupPermission>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferGroupPermissionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferGroupPermission_User");

            entity.HasOne(d => d.Group).WithMany(p => p.SalesOfferGroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferGroupPermission_Group");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferGroupPermissionModifiedByNavigations).HasConstraintName("FK_SalesOfferGroupPermission_User1");

            entity.HasOne(d => d.Offer).WithMany(p => p.SalesOfferGroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferGroupPermission_SalesOffer");

            entity.HasOne(d => d.Permission).WithMany(p => p.SalesOfferGroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferGroupPermission_PermissionLevel");
        });

        modelBuilder.Entity<SalesOfferInternalApproval>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Group).WithMany(p => p.SalesOfferInternalApprovals).HasConstraintName("FK_SalesOfferInternalApproval_Group");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.SalesOfferInternalApprovals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferInternalApproval_SalesOffer");
        });

        modelBuilder.Entity<SalesOfferInvoiceTax>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferInvoiceTaxCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferInvoiceTax_User");

            entity.HasOne(d => d.InvoicePayerClient).WithMany(p => p.SalesOfferInvoiceTaxes).HasConstraintName("FK_SalesOfferInvoiceTax_Client");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferInvoiceTaxModifiedByNavigations).HasConstraintName("FK_SalesOfferInvoiceTax_User1");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.SalesOfferInvoiceTaxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferInvoiceTax_SalesOffer");
        });

        modelBuilder.Entity<SalesOfferItemAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.SalesOfferItemAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferItemAttachment_InventoryItem");

            entity.HasOne(d => d.Offer).WithMany(p => p.SalesOfferItemAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferItemAttachment_SalesOffer");

            entity.HasOne(d => d.SalesOfferProduct).WithMany(p => p.SalesOfferItemAttachments).HasConstraintName("FK_SalesOfferItemAttachment_SalesOfferProduct");
        });

        modelBuilder.Entity<SalesOfferLocation>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Area).WithMany(p => p.SalesOfferLocations).HasConstraintName("FK_SalesOfferLocation_Area");

            entity.HasOne(d => d.Country).WithMany(p => p.SalesOfferLocations).HasConstraintName("FK_SalesOfferLocation_Country");

            entity.HasOne(d => d.Governorate).WithMany(p => p.SalesOfferLocations).HasConstraintName("FK_SalesOfferLocation_Governorate");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.SalesOfferLocations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferLocation_SalesOffer");
        });

        modelBuilder.Entity<SalesOfferPdf>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferPdfCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferPdf_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferPdfModifiedByNavigations).HasConstraintName("FK_SalesOfferPdf_User1");
        });

        modelBuilder.Entity<SalesOfferPdfTemplate>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferPdfTemplateCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferPdfTemplate_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferPdfTemplateModifiedByNavigations).HasConstraintName("FK_SalesOfferPdfTemplate_User1");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.SalesOfferPdfTemplates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferPdfTemplate_SalesOffer");
        });

        modelBuilder.Entity<SalesOfferProduct>(entity =>
        {
            entity.Property(e => e.ProfitPercentage).HasComment("Profit Percentage From Average Price");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferProductCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferProduct_User");

            entity.HasOne(d => d.InventoryItemCategory).WithMany(p => p.SalesOfferProducts).HasConstraintName("FK__SalesOffe__Inven__076CEECC");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.SalesOfferProducts).HasConstraintName("FK__SalesOffe__Inven__0678CA93");

            entity.HasOne(d => d.InvoicePayerClient).WithMany(p => p.SalesOfferProducts).HasConstraintName("FK_SalesOfferProduct_Client");

            entity.HasOne(d => d.MaintenanceFor).WithMany(p => p.SalesOfferProducts)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_SalesOfferProduct_MaintenanceFor");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferProductModifiedByNavigations).HasConstraintName("FK_SalesOfferProduct_User1");

            entity.HasOne(d => d.Offer).WithMany(p => p.SalesOfferProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferProduct_SalesOffer");

            entity.HasOne(d => d.ProductGroup).WithMany(p => p.SalesOfferProducts).HasConstraintName("FK_SalesOfferProduct_ProductGroup");

            entity.HasOne(d => d.Product).WithMany(p => p.SalesOfferProducts).HasConstraintName("FK_SalesOfferProduct_Product");
        });

        modelBuilder.Entity<SalesOfferProductTax>(entity =>
        {
            entity.HasOne(d => d.SalesOfferProduct).WithMany(p => p.SalesOfferProductTaxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferProductTax_SalesOfferProduct");

            entity.HasOne(d => d.Tax).WithMany(p => p.SalesOfferProductTaxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferProductTax_Tax");
        });

        modelBuilder.Entity<SalesOfferTermsAndCondition>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferTermsAndConditionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferTermsAndConditions_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferTermsAndConditionModifiedByNavigations).HasConstraintName("FK_SalesOfferTermsAndConditions_User1");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.SalesOfferTermsAndConditions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferTermsAndConditions_SalesOffer");
        });

        modelBuilder.Entity<SalesOfferUserPermission>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesOfferUserPermissionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferUserPermission_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesOfferUserPermissionModifiedByNavigations).HasConstraintName("FK_SalesOfferUserPermission_User2");

            entity.HasOne(d => d.Offer).WithMany(p => p.SalesOfferUserPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferUserPermission_SalesOffer");

            entity.HasOne(d => d.Permission).WithMany(p => p.SalesOfferUserPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferUserPermission_PermissionLevel");

            entity.HasOne(d => d.User).WithMany(p => p.SalesOfferUserPermissionUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOfferUserPermission_User");
        });

        modelBuilder.Entity<SalesRentOffer>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesRentOfferCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesRentOffer_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SalesRentOfferModifiedByNavigations).HasConstraintName("FK_SalesRentOffer_User1");
        });

        modelBuilder.Entity<SalesTarget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SalesTargetYear");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CanEdit).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalesTargetCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesTarget_User1");

            entity.HasOne(d => d.ModifiedNavigation).WithMany(p => p.SalesTargetModifiedNavigations).HasConstraintName("FK_SalesTarget_User");
        });

        modelBuilder.Entity<ShippingCompany>(entity =>
        {
            entity.HasOne(d => d.Attachement).WithMany(p => p.ShippingCompanies).HasConstraintName("FK_ShippingCompany_ShippingCompanyAttachment");

            entity.HasOne(d => d.PurchasePoshipmentShippingMethodDetails).WithMany(p => p.ShippingCompanies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShippingCompany_PurchasePOShipmentShippingMethodDetails");
        });

        modelBuilder.Entity<ShippingCompanyAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ShippingCompanyAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShippingCompanyAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ShippingCompanyAttachmentModifiedByNavigations).HasConstraintName("FK_ShippingCompanyAttachment_User1");
        });

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SpecialityCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Speciality_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SpecialityModifiedByNavigations).HasConstraintName("FK_Speciality_User1");
        });

        modelBuilder.Entity<SpecialitySupplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SupplierSpeciality");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SpecialitySupplierCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierSpeciality_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SpecialitySupplierModifiedByNavigations).HasConstraintName("FK_SupplierSpeciality_User1");
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StageCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stages_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.StageModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stages_User1");
        });

        modelBuilder.Entity<StpTaxTypeV>(entity =>
        {
            entity.ToView("STP_TaxType_v");
        });

        modelBuilder.Entity<SubmittedReport>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SubmittedReportCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubmittedReport_User1");

            entity.HasOne(d => d.Report).WithMany(p => p.SubmittedReports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubmittedReport_Report");

            entity.HasOne(d => d.User).WithMany(p => p.SubmittedReportUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubmittedReport_User");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.HasLogo).HasDefaultValue(false);
            entity.Property(e => e.OtherDelivaryAndShippingMethodName).IsFixedLength();
            entity.Property(e => e.Rate).HasDefaultValue(0);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplier_User");

            entity.HasOne(d => d.DefaultDelivaryAndShippingMethod).WithMany(p => p.Suppliers).HasConstraintName("FK_Supplier_DeliveryAndShippingMethod");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierModifiedByNavigations).HasConstraintName("FK_Supplier_User1");
        });

        modelBuilder.Entity<SupplierAccount>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Account).WithMany(p => p.SupplierAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAccounts_Accounts");

            entity.HasOne(d => d.AccountOfJe).WithMany(p => p.SupplierAccounts).HasConstraintName("FK_SupplierAccounts_AccountOfJournalEntry");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierAccountCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAccounts_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierAccountModifiedByNavigations).HasConstraintName("FK_SupplierAccounts_User1");

            entity.HasOne(d => d.PurchasePo).WithMany(p => p.SupplierAccounts).HasConstraintName("FK_SupplierAccounts_PurchasePO");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAccounts_Supplier");
        });

        modelBuilder.Entity<SupplierAccountReviewed>(entity =>
        {
            entity.HasOne(d => d.CreatedyByNavigation).WithMany(p => p.SupplierAccountReviewedCreatedyByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAccountReviewed_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierAccountReviewedModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAccountReviewed_User1");

            entity.HasOne(d => d.Po).WithMany(p => p.SupplierAccountRevieweds).HasConstraintName("FK_SupplierAccountReviewed_PurchasePO");

            entity.HasOne(d => d.SupplierAccount).WithMany(p => p.SupplierAccountRevieweds).HasConstraintName("FK_SupplierAccountReviewed_SupplierAccounts");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierAccountRevieweds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAccountReviewed_Supplier");
        });

        modelBuilder.Entity<SupplierAddress>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Area).WithMany(p => p.SupplierAddresses).HasConstraintName("FK_SupplierAddress_Area");

            entity.HasOne(d => d.Country).WithMany(p => p.SupplierAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAddress_Country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierAddressCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAddress_User");

            entity.HasOne(d => d.Governorate).WithMany(p => p.SupplierAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAddress_Governorate");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierAddressModifiedByNavigations).HasConstraintName("FK_SupplierAddress_User1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAddress_Supplier");
        });

        modelBuilder.Entity<SupplierAttachment>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierAttachmentModifiedByNavigations).HasConstraintName("FK_SupplierAttachment_User1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAttachment_Supplier");
        });

        modelBuilder.Entity<SupplierBankAccount>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierBankAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierBankAccount_Client");
        });

        modelBuilder.Entity<SupplierContactPerson>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierContactPersonCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierContactPerson_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierContactPersonModifiedByNavigations).HasConstraintName("FK_SupplierContactPerson_User1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierContactPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierContactPerson_Supplier");
        });

        modelBuilder.Entity<SupplierFax>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierFaxCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierFax_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierFaxModifiedByNavigations).HasConstraintName("FK_SupplierFax_User1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierFaxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierFax_Supplier");
        });

        modelBuilder.Entity<SupplierMobile>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierMobileCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierMobile_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierMobileModifiedByNavigations).HasConstraintName("FK_SupplierMobile_User1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierMobiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierMobile_Supplier");
        });

        modelBuilder.Entity<SupplierPaymentTerm>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierPaymentTerms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierPaymentTerm_Client");
        });

        modelBuilder.Entity<SupplierPhone>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierPhoneCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierPhone_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierPhoneModifiedByNavigations).HasConstraintName("FK_SupplierPhone_User1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierPhones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierPhone_Supplier");
        });

        modelBuilder.Entity<SupplierSpeciality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SupplierSpeciality_1");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierSpecialityCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierSpeciality_User2");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.SupplierSpecialityModifiedByNavigations).HasConstraintName("FK_SupplierSpeciality_User3");

            entity.HasOne(d => d.Speciality).WithMany(p => p.SupplierSpecialities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierSpeciality_SpecialitySupplier");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierSpecialities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierSpeciality_Supplier");
        });

        modelBuilder.Entity<SupportedBy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supporte__3214EC27F76CE4C4");
        });

        modelBuilder.Entity<SystemLog>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SystemLogs).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("Task", tb => tb.HasTrigger("Trig_Task_log"));

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.Tasks).HasConstraintName("FK_Task_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskModifiedByNavigations).HasConstraintName("FK_Task_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks).HasConstraintName("FK_Task_Project");

            entity.HasOne(d => d.ProjectSprint).WithMany(p => p.Tasks).HasConstraintName("FK_Task_ProjectSprint");

            entity.HasOne(d => d.ProjectWorkFlow).WithMany(p => p.Tasks).HasConstraintName("FK_Task_ProjectWorkFlow");

            entity.HasOne(d => d.TaskType).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_TaskType");
        });

        modelBuilder.Entity<TaskApplicationOpen>(entity =>
        {
            entity.HasOne(d => d.Task).WithMany(p => p.TaskApplicationOpens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskApplicationOpen_Task");

            entity.HasOne(d => d.User).WithMany(p => p.TaskApplicationOpens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskApplicationOpen_User");
        });

        modelBuilder.Entity<TaskAssignUser>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskAssignUserCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAssignUser_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskAssignUserModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAssignUser_User");

            entity.HasOne(d => d.TaskInfo).WithMany(p => p.TaskAssignUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAssignUser_TaskInfo");
        });

        modelBuilder.Entity<TaskAttachment>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.TaskAttachments).HasConstraintName("FK_TaskAttachment_AttachmentCategory");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskAttachmentModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAttachment_User1");

            entity.HasOne(d => d.TaskInfo).WithMany(p => p.TaskAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAttachment_TaskInfo");
        });

        modelBuilder.Entity<TaskBrowserTab>(entity =>
        {
            entity.HasOne(d => d.Task).WithMany(p => p.TaskBrowserTabs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskBrowserTab_Task");

            entity.HasOne(d => d.User).WithMany(p => p.TaskBrowserTabs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskBrowserTab_TaskBrowserTab");
        });

        modelBuilder.Entity<TaskCategory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<TaskClosureLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TaskLog");

            entity.Property(e => e.ClosureDate).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskClosureLogCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskLog_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskClosureLogModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskLog_User1");

            entity.HasOne(d => d.TaskInfo).WithMany(p => p.TaskClosureLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskLog_TaskInfo");
        });

        modelBuilder.Entity<TaskComment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskCommentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskComment_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskCommentModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskComment_User");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment).HasConstraintName("FK_TaskComment_TaskComment");

            entity.HasOne(d => d.TaskInfo).WithMany(p => p.TaskComments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskComment_TaskInfo");
        });

        modelBuilder.Entity<TaskCommentAttachment>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.TaskCommentAttachments).HasConstraintName("FK_TaskCommentAttachment_AttachmentCategory");

            entity.HasOne(d => d.Comment).WithMany(p => p.TaskCommentAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskCommentAttachment_TaskComment");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskCommentAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskCommentAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskCommentAttachmentModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskCommentAttachment_User1");
        });

        modelBuilder.Entity<TaskDetail>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Trig_TaskDetails_log"));

            entity.HasOne(d => d.Task).WithMany(p => p.TaskDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_TaskDetails");
        });

        modelBuilder.Entity<TaskExpensi>(entity =>
        {
            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.TaskExpensiApprovedByNavigations).HasConstraintName("FK_TaskExpensis_User2");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskExpensiCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskExpensis_User");

            entity.HasOne(d => d.ExpensisType).WithMany(p => p.TaskExpensis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskExpensis_ExpensisType");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskExpensiModifiedByNavigations).HasConstraintName("FK_TaskExpensis_User1");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskExpensis).HasConstraintName("FK_TaskExpensis_Task");
        });

        modelBuilder.Entity<TaskFlagsOwnerReciever>(entity =>
        {
            entity.HasOne(d => d.Task).WithMany(p => p.TaskFlagsOwnerRecievers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_TaskFlagsOwnerReciever");
        });

        modelBuilder.Entity<TaskHistory>(entity =>
        {
            entity.HasOne(d => d.Task).WithMany(p => p.TaskHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskHistory_Task");
        });

        modelBuilder.Entity<TaskInfo>(entity =>
        {
            entity.Property(e => e.Eapprove).HasDefaultValue(true);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TimeTracking).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskInfoCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskInfo_User");

            entity.HasOne(d => d.ManageStage).WithMany(p => p.TaskInfos).HasConstraintName("FK_TaskInfo_ManageStages");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskInfoModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskInfo_User1");

            entity.HasOne(d => d.TaskCategory).WithMany(p => p.TaskInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskInfo_TaskCategory");
        });

        modelBuilder.Entity<TaskInfoRevision>(entity =>
        {
            entity.Property(e => e.Billable).HasDefaultValue(false);
            entity.Property(e => e.Eapproval).HasDefaultValue(false);
            entity.Property(e => e.MangageStageId).IsFixedLength();
            entity.Property(e => e.TimeTracking).HasDefaultValue(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskInfoRevisionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskInfoRevision_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskInfoRevisionModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskInfoRevision_User1");

            entity.HasOne(d => d.Proiority).WithMany(p => p.TaskInfoRevisions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskInfoRevision_Priority");

            entity.HasOne(d => d.TaskCategory).WithMany(p => p.TaskInfoRevisions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskInfoRevision_TaskCategory");
        });

        modelBuilder.Entity<TaskPermission>(entity =>
        {
            entity.HasOne(d => d.PermissionLevel).WithMany(p => p.TaskPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskPermission_PermissionLevel");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskPermission_Task");
        });

        modelBuilder.Entity<TaskPrimarySubCategory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskPrimarySubCategoryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskPrimarySubCategory_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskPrimarySubCategoryModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskPrimarySubCategory_User1");

            entity.HasOne(d => d.TaskCategory).WithMany(p => p.TaskPrimarySubCategories).HasConstraintName("FK_TaskPrimarySubCategory_TaskCategory");
        });

        modelBuilder.Entity<TaskRequirement>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskRequirementCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskRequirement_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskRequirementModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskRequirement_User1");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskRequirements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskRequirement_Task");

            entity.HasOne(d => d.WorkingHourTracking).WithMany(p => p.TaskRequirements).HasConstraintName("FK_TaskRequirement_WorkingHourseTracking");
        });

        modelBuilder.Entity<TaskScreenShot>(entity =>
        {
            entity.HasOne(d => d.Task).WithMany(p => p.TaskScreenShots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskScreenShot_Task");

            entity.HasOne(d => d.User).WithMany(p => p.TaskScreenShots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskScreenShot_User");
        });

        modelBuilder.Entity<TaskSecondarySubCategory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.TaskPrimarySubCategory).WithMany(p => p.TaskSecondarySubCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskSecondarySubCategory_TaskPrimarySubCategory");
        });

        modelBuilder.Entity<TaskStageHistory>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskStageHistoryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskStageHistory_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskStageHistoryModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskStageHistory_User1");

            entity.HasOne(d => d.Stage).WithMany(p => p.TaskStageHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskStageHistory_Stages");

            entity.HasOne(d => d.TaskInfo).WithMany(p => p.TaskStageHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskStageHistory_TaskInfo");
        });

        modelBuilder.Entity<TaskType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CanEdit).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskTypeCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskType_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskTypeModifiedByNavigations).HasConstraintName("FK_TaskType_User1");
        });

        modelBuilder.Entity<TaskUnitRateService>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskUnitRateServiceCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskUnitRateService_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskUnitRateServiceModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskUnitRateService_User1");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskUnitRateServices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskUnitRateService_Task");

            entity.HasOne(d => d.Uom).WithMany(p => p.TaskUnitRateServices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskUnitRateService_InventoryUOM");
        });

        modelBuilder.Entity<TaskUserMonitor>(entity =>
        {
            entity.HasOne(d => d.Task).WithMany(p => p.TaskUserMonitors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskUserMonitor_Task");

            entity.HasOne(d => d.User).WithMany(p => p.TaskUserMonitors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskUserMonitor_User");
        });

        modelBuilder.Entity<TaskUserReply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TaskUserReplya");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskUserReplyCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskUserReply_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaskUserReplyModifiedByNavigations).HasConstraintName("FK_TaskUserReply_User1");

            entity.HasOne(d => d.RecieverUser).WithMany(p => p.TaskUserReplyRecieverUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskUserReply_User2");
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaxCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tax_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TaxModifiedByNavigations).HasConstraintName("FK_Tax_User1");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TeamCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Team_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TeamModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Team_User1");
        });

        modelBuilder.Entity<TermsAndCondition>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TermsAndConditionCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TermsAndConditions_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TermsAndConditionModifiedByNavigations).HasConstraintName("FK_TermsAndConditions_User1");

            entity.HasOne(d => d.TermsCategory).WithMany(p => p.TermsAndConditions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TermsAndConditions_TermsAndConditionsCategory");
        });

        modelBuilder.Entity<TermsAndConditionsCategory>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TermsAndConditionsCategoryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TermsAndConditionsCategory_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TermsAndConditionsCategoryModifiedByNavigations).HasConstraintName("FK_TermsAndConditionsCategory_User1");
        });

        modelBuilder.Entity<TermsGroup>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TermsGroupCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TermsGroups_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TermsGroupModifiedByNavigations).HasConstraintName("FK_TermsGroups_User1");
        });

        modelBuilder.Entity<TermsLibrary>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TermsLibraryCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TermsLibrary_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TermsLibraryModifiedByNavigations).HasConstraintName("FK_TermsLibrary_User1");

            entity.HasOne(d => d.TermGroup).WithMany(p => p.TermsLibraries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TermsLibrary_TermsGroups");
        });

        modelBuilder.Entity<TransportationLine>(entity =>
        {
            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransportationLineCreationByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationLine_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TransportationLineModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationLine_User1");
        });

        modelBuilder.Entity<TransportationLineIncreaseRequest>(entity =>
        {
            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.TransportationLineIncreaseRequestApprovedByNavigations).HasConstraintName("FK_TransportationLineIncreaseRequest_User1");

            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransportationLineIncreaseRequestCreationByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationLineIncreaseRequest_User");
        });

        modelBuilder.Entity<TransportationLineIncreaseRequestLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Table_1");

            entity.HasOne(d => d.Route).WithMany(p => p.TransportationLineIncreaseRequestLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationLineIncreaseRequestLines_TransportationVehicleRoute");

            entity.HasOne(d => d.TransportationLineIncreaseRequest).WithMany(p => p.TransportationLineIncreaseRequestLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationLineIncreaseRequestLines_TransportationLineIncreaseRequest");
        });

        modelBuilder.Entity<TransportationRoundForRoute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transpor__3214EC070DC7FE52");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TransportationRoundForRoutes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationRoundForRoute_Supplier");

            entity.HasOne(d => d.TransportationVehicleRoute).WithMany(p => p.TransportationRoundForRoutes).HasConstraintName("FK_TransportationRoundForRoute_TransportationVehicleRoute");
        });

        modelBuilder.Entity<TransportationVehicle>(entity =>
        {
            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransportationVehicleCreationByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicle_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TransportationVehicleModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicle_User1");

            entity.HasOne(d => d.VehicleType).WithMany(p => p.TransportationVehicles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicle_VehicleType");
        });

        modelBuilder.Entity<TransportationVehicleRoute>(entity =>
        {
            entity.HasOne(d => d.BranchSchedule).WithMany(p => p.TransportationVehicleRoutes).HasConstraintName("FK_TransportationVehicleRoute_BranchSchedule");

            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransportationVehicleRouteCreationByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRoute_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TransportationVehicleRouteModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRoute_User1");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.TransportationVehicleRouteSupervisors).HasConstraintName("FK_TransportationVehicleRoute_User2");

            entity.HasOne(d => d.SupplierContactPerson).WithMany(p => p.TransportationVehicleRoutes).HasConstraintName("FK_TransportationVehicleRoute_SupplierContactPerson");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TransportationVehicleRoutes).HasConstraintName("FK_TransportationVehicleRoute_Supplier");

            entity.HasOne(d => d.TransportationLine).WithMany(p => p.TransportationVehicleRoutes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRoute_TransportationLine");

            entity.HasOne(d => d.TransportationVehicle).WithMany(p => p.TransportationVehicleRoutes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRoute_TransportationVehicle");
        });

        modelBuilder.Entity<TransportationVehicleRouteAccount>(entity =>
        {
            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransportationVehicleRouteAccountCreationByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteAccounts_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.TransportationVehicleRouteAccountModifiedByNavigations).HasConstraintName("FK_TransportationVehicleRouteAccounts_User1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TransportationVehicleRouteAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteAccounts_Supplier");

            entity.HasOne(d => d.TransportationVehicleRoute).WithMany(p => p.TransportationVehicleRouteAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteAccounts_TransportationVehicleRoute");
        });

        modelBuilder.Entity<TransportationVehicleRouteDeduction>(entity =>
        {
            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransportationVehicleRouteDeductions).HasConstraintName("FK_TransportationVehicleRouteDeduction_User");

            entity.HasOne(d => d.TransportationVehicleRoute).WithMany(p => p.TransportationVehicleRouteDeductions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteDeduction_TransportationVehicleRoute");
        });

        modelBuilder.Entity<TransportationVehicleRouteDirection>(entity =>
        {
            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransportationVehicleRouteDirections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteDirection_User");

            entity.HasOne(d => d.TransportationVehicleRoute).WithMany(p => p.TransportationVehicleRouteDirections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteDirection_TransportationVehicleRoute");
        });

        modelBuilder.Entity<TransportationVehicleRouteEmployee>(entity =>
        {
            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransportationVehicleRouteEmployees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteEmployee_User");

            entity.HasOne(d => d.HrUser).WithMany(p => p.TransportationVehicleRouteEmployees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRoute_HrUser");

            entity.HasOne(d => d.TransportationVehicleRouteDirection).WithMany(p => p.TransportationVehicleRouteEmployees).HasConstraintName("FK_TransportationVehicleRouteEmployee_TransportationVehicleRouteDirection");

            entity.HasOne(d => d.TransportationVehicleRoute).WithMany(p => p.TransportationVehicleRouteEmployees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteEmployee_TransportationVehicleRoute");
        });

        modelBuilder.Entity<TransportationVehicleRouteEmployeeException>(entity =>
        {
            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransportationVehicleRouteEmployeeExceptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteEmployeeException_User");

            entity.HasOne(d => d.HrUser).WithMany(p => p.TransportationVehicleRouteEmployeeExceptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteEmployeeException_HrUser");

            entity.HasOne(d => d.TransportationVehicleRouteDirection).WithMany(p => p.TransportationVehicleRouteEmployeeExceptions).HasConstraintName("FK_TransportationVehicleRouteEmployeeException_TransportationVehicleRouteDirection");

            entity.HasOne(d => d.TransportationVehicleRoute).WithMany(p => p.TransportationVehicleRouteEmployeeExceptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportationVehicleRouteEmployeeException_TransportationVehicleRoute");
        });

        modelBuilder.Entity<TransportionLineExceptionPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transpor__3214EC073FBDF247");
        });

        modelBuilder.Entity<TransportionLineSupplierPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transpor__3214EC07C7DB4463");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TransportionLineSupplierPayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransportionLineSupplierPayment_Supplier");
        });

        modelBuilder.Entity<TransprotationUserAttedance>(entity =>
        {
            entity.HasOne(d => d.CheckInRouteDirection).WithMany(p => p.TransprotationUserAttedanceCheckInRouteDirections).HasConstraintName("FK_TransprotationUserAttedance_TransportationVehicleRouteDirection");

            entity.HasOne(d => d.CheckInRoute).WithMany(p => p.TransprotationUserAttedanceCheckInRoutes).HasConstraintName("FK_TransprotationUserAttedance_TransportationVehicleRoute");

            entity.HasOne(d => d.CheckOutRouteDirection).WithMany(p => p.TransprotationUserAttedanceCheckOutRouteDirections).HasConstraintName("FK_TransprotationUserAttedance_TransportationVehicleRouteDirection1");

            entity.HasOne(d => d.CheckOutRoute).WithMany(p => p.TransprotationUserAttedanceCheckOutRoutes).HasConstraintName("FK_TUA_CheckOutRoute");

            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.TransprotationUserAttedances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransprotationUserAttedance_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.Users).HasConstraintName("FK_User_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation).HasConstraintName("FK_User_User");

            entity.HasOne(d => d.Department).WithMany(p => p.Users).HasConstraintName("FK_User_Department");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.Users).HasConstraintName("FK_User_JobTitle");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InverseModifiedByNavigation).HasConstraintName("FK_User_User1");
        });

        modelBuilder.Entity<UserPatient>(entity =>
        {
            entity.HasOne(d => d.Area).WithMany(p => p.UserPatients).HasConstraintName("FK_UserPatient_Area");

            entity.HasOne(d => d.City).WithMany(p => p.UserPatients).HasConstraintName("FK_UserPatient_Governorate");

            entity.HasOne(d => d.Country).WithMany(p => p.UserPatients).HasConstraintName("FK_UserPatient_Country");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UserPatientCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPatient_User1");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.UserPatientModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPatient_User2");

            entity.HasOne(d => d.User).WithMany(p => p.UserPatientUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPatient_User");
        });

        modelBuilder.Entity<UserPatientInsurance>(entity =>
        {
            entity.HasOne(d => d.CreationByNavigation).WithMany(p => p.UserPatientInsuranceCreationByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPatientInsurance_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.UserPatientInsuranceModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPatientInsurance_User1");

            entity.HasOne(d => d.UserPatient).WithMany(p => p.UserPatientInsurances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPatientInsurance_UserPatient");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UserRoleCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_User1");

            entity.HasOne(d => d.HrUser).WithMany(p => p.UserRoles).HasConstraintName("FK_UserRole_HrUserId");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoleUsers).HasConstraintName("FK_UserRole_User");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSession_User");
        });

        modelBuilder.Entity<UserTeam>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UserTeamCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTeam_User2");

            entity.HasOne(d => d.HrUser).WithMany(p => p.UserTeams).HasConstraintName("FK_UserTeam_HrUserId");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.UserTeamModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTeam_User1");

            entity.HasOne(d => d.Team).WithMany(p => p.UserTeams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTeam_Team");

            entity.HasOne(d => d.User).WithMany(p => p.UserTeamUsers).HasConstraintName("FK_UserTeam_User");
        });

        modelBuilder.Entity<UserTimer>(entity =>
        {
            entity.Property(e => e.AtWork).HasDefaultValue(true);

            entity.HasOne(d => d.TaskInfo).WithMany(p => p.UserTimers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTimer_TaskInfo");

            entity.HasOne(d => d.User).WithMany(p => p.UserTimers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTimer_User");
        });

        modelBuilder.Entity<UserViewNotification>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.UserViewNotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserViewNotification_User");
        });

        modelBuilder.Entity<VAccount>(entity =>
        {
            entity.ToView("V_Account");
        });

        modelBuilder.Entity<VAccountOfJournalEntryWithDaily>(entity =>
        {
            entity.ToView("V_AccountOfJournalEntryWithDaily");
        });

        modelBuilder.Entity<VAccountsAdvanciedSettingAccount>(entity =>
        {
            entity.ToView("V_Accounts_AdvanciedSettingAccount");
        });

        modelBuilder.Entity<VBompartitionItemsInventoryItem>(entity =>
        {
            entity.ToView("V_BOMPartitionItems_InventoryItem");
        });

        modelBuilder.Entity<VBranchProduct>(entity =>
        {
            entity.ToView("V_BranchProduct");
        });

        modelBuilder.Entity<VBranchUser>(entity =>
        {
            entity.ToView("V_BranchUser");
        });

        modelBuilder.Entity<VBusProgram>(entity =>
        {
            entity.ToView("v_Bus_Program");

            entity.Property(e => e.BusType).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CheckDate).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CheckTime).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.EmpNameAra).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.EmployeeNumber).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Type).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<VClientAccountsProject>(entity =>
        {
            entity.ToView("V_ClientAccounts_Project");
        });

        modelBuilder.Entity<VClientAddress>(entity =>
        {
            entity.ToView("V_ClientAddress");
        });

        modelBuilder.Entity<VClientConsultantAddress>(entity =>
        {
            entity.ToView("V_ClientConsultantAddress");
        });

        modelBuilder.Entity<VClientExpired>(entity =>
        {
            entity.ToView("V_Client_Expired");
        });

        modelBuilder.Entity<VClientMobile>(entity =>
        {
            entity.ToView("V_Client_Mobile");
        });

        modelBuilder.Entity<VClientPhone>(entity =>
        {
            entity.ToView("V_ClientPhones");
        });

        modelBuilder.Entity<VClientSalesPerson>(entity =>
        {
            entity.ToView("V_ClientSalesPerson");
        });

        modelBuilder.Entity<VClientSpeciality>(entity =>
        {
            entity.ToView("V_ClientSpeciality");
        });

        modelBuilder.Entity<VClientUseer>(entity =>
        {
            entity.ToView("V_Client_Useer");

            entity.Property(e => e.OtherDelivaryAndShippingMethodName).IsFixedLength();
        });

        modelBuilder.Entity<VCountry>(entity =>
        {
            entity.ToView("V_Country");
        });

        modelBuilder.Entity<VCrmreport>(entity =>
        {
            entity.ToView("V_CRMReport");
        });

        modelBuilder.Entity<VCustodyRequestReleaseBackOrder>(entity =>
        {
            entity.ToView("V_Custody_Request_Release_BackOrder");
        });

        modelBuilder.Entity<VDailyJournalEntry>(entity =>
        {
            entity.ToView("V_DailyJournalEntry");
        });

        modelBuilder.Entity<VDailyReport>(entity =>
        {
            entity.ToView("V_DailyReport");
        });

        modelBuilder.Entity<VDailyReportLineThrough>(entity =>
        {
            entity.ToView("V_DailyReportLine_Through");
        });

        modelBuilder.Entity<VDailyReportReportLineThrough>(entity =>
        {
            entity.ToView("V_DailyReport_ReportLine_Through");
        });

        modelBuilder.Entity<VDailyReportReportLineThroughApi>(entity =>
        {
            entity.ToView("V_DailyReport_ReportLine_Through_API");
        });

        modelBuilder.Entity<VGroupRoleUser>(entity =>
        {
            entity.ToView("V_GroupRoleUser");
        });

        modelBuilder.Entity<VGroupUser>(entity =>
        {
            entity.ToView("V_Group_Users");
        });

        modelBuilder.Entity<VGroupUserBranch>(entity =>
        {
            entity.ToView("V_GroupUser_Branch");
        });

        modelBuilder.Entity<VInventoryAddingOrder>(entity =>
        {
            entity.ToView("V_InventoryAddingOrder");
        });

        modelBuilder.Entity<VInventoryAddingOrderItem>(entity =>
        {
            entity.ToView("V_InventoryAddingOrderItems");
        });

        modelBuilder.Entity<VInventoryAddingOrderItemsDetail>(entity =>
        {
            entity.ToView("V_InventoryAddingOrderItemsDetails");
        });

        modelBuilder.Entity<VInventoryCategory>(entity =>
        {
            entity.ToView("V_InventoryCategory");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<VInventoryInternalBackOrder>(entity =>
        {
            entity.ToView("V_InventoryInternalBackOrder");
        });

        modelBuilder.Entity<VInventoryInternalBackOrderItem>(entity =>
        {
            entity.ToView("V_InventoryInternalBackOrderItem");
        });

        modelBuilder.Entity<VInventoryInternalTransferOrder>(entity =>
        {
            entity.ToView("V_InventoryInternalTransferOrder");
        });

        modelBuilder.Entity<VInventoryInternalTransferOrderItem>(entity =>
        {
            entity.ToView("V_InventoryInternalTransferOrderItem");
        });

        modelBuilder.Entity<VInventoryItem>(entity =>
        {
            entity.ToView("V_InventoryItem");
        });

        modelBuilder.Entity<VInventoryItemInventoryItemCategory>(entity =>
        {
            entity.ToView("V_InventoryItem_InventoryItemCategory");
        });

        modelBuilder.Entity<VInventoryMatrialRelease>(entity =>
        {
            entity.ToView("V_InventoryMatrialRelease");
        });

        modelBuilder.Entity<VInventoryMatrialReleaseItem>(entity =>
        {
            entity.ToView("V_InventoryMatrialReleaseItems");
        });

        modelBuilder.Entity<VInventoryMatrialReleaseUnionInternalBackOrderItem>(entity =>
        {
            entity.ToView("V_InventoryMatrialReleaseUnionInternalBackOrderItems");
        });

        modelBuilder.Entity<VInventoryMatrialRequest>(entity =>
        {
            entity.ToView("V_InventoryMatrialRequest");
        });

        modelBuilder.Entity<VInventoryMatrialRequestItem>(entity =>
        {
            entity.ToView("V_InventoryMatrialRequestItems");
        });

        modelBuilder.Entity<VInventoryMatrialRequestWithItem>(entity =>
        {
            entity.ToView("V_InventoryMatrialRequestWithItems");
        });

        modelBuilder.Entity<VInventoryStoreItem>(entity =>
        {
            entity.ToView("V_InventoryStoreItem");
        });

        modelBuilder.Entity<VInventoryStoreItemMinBalance>(entity =>
        {
            entity.ToView("V_InventoryStoreItemMinBalance");
        });

        modelBuilder.Entity<VInventoryStoreItemMovement>(entity =>
        {
            entity.ToView("V_InventoryStoreItemMovement");
        });

        modelBuilder.Entity<VInventoryStoreItemPrice>(entity =>
        {
            entity.ToView("V_InventoryStoreItemPrice");
        });

        modelBuilder.Entity<VInventoryStoreItemPriceReport>(entity =>
        {
            entity.ToView("V_InventoryStoreItemPriceReport");
        });

        modelBuilder.Entity<VInventoryStoreKeeper>(entity =>
        {
            entity.ToView("V_inventoryStoreKeeper");
        });

        modelBuilder.Entity<VInvoice>(entity =>
        {
            entity.ToView("V_Invoice");
        });

        modelBuilder.Entity<VMainFabInsReport>(entity =>
        {
            entity.ToView("V_MainFabInsReports");
        });

        modelBuilder.Entity<VMaintenanceForDetail>(entity =>
        {
            entity.ToView("V_MaintenanceForDetails");
        });

        modelBuilder.Entity<VMaintenanceHoursReport>(entity =>
        {
            entity.ToView("V_MaintenanceHoursReport");
        });

        modelBuilder.Entity<VMatrialReleaseSalesOffer>(entity =>
        {
            entity.ToView("V_MatrialRelease_SalesOffer");
        });

        modelBuilder.Entity<VNotificationsDetail>(entity =>
        {
            entity.ToView("V_NotificationsDetails");
        });

        modelBuilder.Entity<VPoapprovalStatus>(entity =>
        {
            entity.ToView("V_POApprovalStatus");
        });

        modelBuilder.Entity<VPricingFullDatum>(entity =>
        {
            entity.ToView("V_Pricing_FullData");
        });

        modelBuilder.Entity<VPricingProduct>(entity =>
        {
            entity.ToView("V_PricingProduct");
        });

        modelBuilder.Entity<VProductTargetChart>(entity =>
        {
            entity.ToView("V_ProductTargetChart");
        });

        modelBuilder.Entity<VProjectFabricationProjectOffer>(entity =>
        {
            entity.ToView("V_ProjectFabrication_Project_Offer");
        });

        modelBuilder.Entity<VProjectFabricationProjectOfferEntity>(entity =>
        {
            entity.ToView("V_ProjectFabrication_Project_Offer_Entity");
        });

        modelBuilder.Entity<VProjectFabricationTotalHour>(entity =>
        {
            entity.ToView("V_ProjectFabricationTotalHours");
        });

        modelBuilder.Entity<VProjectInstallationProjectOffer>(entity =>
        {
            entity.ToView("V_ProjectInstallation_Project_Offer");
        });

        modelBuilder.Entity<VProjectInstallationTotalHour>(entity =>
        {
            entity.ToView("V_ProjectInstallationTotalHours");
        });

        modelBuilder.Entity<VProjectReturnSalesOffer>(entity =>
        {
            entity.ToView("V_Project_ReturnSalesOffer");
        });

        modelBuilder.Entity<VProjectSalesOffer>(entity =>
        {
            entity.ToView("V_Project_SalesOffer");
        });

        modelBuilder.Entity<VProjectSalesOfferClient>(entity =>
        {
            entity.ToView("V_Project_SalesOffer_Client");
        });

        modelBuilder.Entity<VProjectSalesOfferClientAccount>(entity =>
        {
            entity.ToView("V_Project_SalesOffer_ClientAccounts");
        });

        modelBuilder.Entity<VProjectSalesOfferEntity>(entity =>
        {
            entity.ToView("V_Project_SalesOffer_Entity");
        });

        modelBuilder.Entity<VPrsupplierOfferItem>(entity =>
        {
            entity.ToView("V_PRSupplierOfferItem");
        });

        modelBuilder.Entity<VPurchasePo>(entity =>
        {
            entity.ToView("V_PurchasePO");
        });

        modelBuilder.Entity<VPurchasePoItem>(entity =>
        {
            entity.ToView("V_PurchasePoItem");
        });

        modelBuilder.Entity<VPurchasePoItemPo>(entity =>
        {
            entity.ToView("V_PurchasePoItem_PO");
        });

        modelBuilder.Entity<VPurchasePoSupplierAccountAmount>(entity =>
        {
            entity.ToView("V_PurchasePO_SupplierAccountAmount");
        });

        modelBuilder.Entity<VPurchasePoinactiveTask>(entity =>
        {
            entity.ToView("V_PurchasePOInactiveTask");
        });

        modelBuilder.Entity<VPurchasePoitemInvoicePo>(entity =>
        {
            entity.ToView("V_PurchasePOItem_InvoicePO");
        });

        modelBuilder.Entity<VPurchasePoitemInvoicePoSupplier>(entity =>
        {
            entity.ToView("V_PurchasePOItem_InvoicePO_Supplier");
        });

        modelBuilder.Entity<VPurchaseRequest>(entity =>
        {
            entity.ToView("V_PurchaseRequest");
        });

        modelBuilder.Entity<VPurchaseRequestItem>(entity =>
        {
            entity.ToView("V_PurchaseRequestItems");
        });

        modelBuilder.Entity<VPurchaseRequestItemsPo>(entity =>
        {
            entity.ToView("V_PurchaseRequestItems_PO");
        });

        modelBuilder.Entity<VPurchaseRequestItemsPoAo>(entity =>
        {
            entity.ToView("V_PurchaseRequestItems_PO_AO");
        });

        modelBuilder.Entity<VSalesBranchUserTargetTargetYear>(entity =>
        {
            entity.ToView("V_SalesBranchUserTarget_TargetYear");
        });

        modelBuilder.Entity<VSalesMaintenanceOfferReport>(entity =>
        {
            entity.ToView("V_SalesMaintenanceOfferReport");
        });

        modelBuilder.Entity<VSalesOffer>(entity =>
        {
            entity.ToView("V_SalesOffer");
        });

        modelBuilder.Entity<VSalesOfferClient>(entity =>
        {
            entity.ToView("V_SalesOffer_Client");
        });

        modelBuilder.Entity<VSalesOfferClientFullDatum>(entity =>
        {
            entity.ToView("V_SalesOffer_Client_FullData");
        });

        modelBuilder.Entity<VSalesOfferDiff>(entity =>
        {
            entity.ToView("V_SalesOffer_diff");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<VSalesOfferExtraCost>(entity =>
        {
            entity.ToView("V_SalesOfferExtraCosts");
        });

        modelBuilder.Entity<VSalesOfferProduct>(entity =>
        {
            entity.ToView("V_SalesOfferProduct");
        });

        modelBuilder.Entity<VSalesOfferProductSalesOffer>(entity =>
        {
            entity.ToView("V_SalesOfferProduct_SalesOffer");
        });

        modelBuilder.Entity<VSalesOfferUser>(entity =>
        {
            entity.ToView("V_SalesOffer_User");
        });

        modelBuilder.Entity<VSupplier>(entity =>
        {
            entity.ToView("V_Supplier");

            entity.Property(e => e.OtherDelivaryAndShippingMethodName).IsFixedLength();
        });

        modelBuilder.Entity<VSupplierAddress>(entity =>
        {
            entity.ToView("V_SupplierAddress");
        });

        modelBuilder.Entity<VSupplierSpeciality>(entity =>
        {
            entity.ToView("V_SupplierSpeciality");
        });

        modelBuilder.Entity<VTaskMangerProject>(entity =>
        {
            entity.ToView("V_TaskMangerProject");
        });

        modelBuilder.Entity<VUserBranchGroup>(entity =>
        {
            entity.ToView("V_User_Branch_Group");
        });

        modelBuilder.Entity<VUserDetail>(entity =>
        {
            entity.ToView("V_UserDetails");
        });

        modelBuilder.Entity<VUserInfo>(entity =>
        {
            entity.ToView("V_UserInfo");
        });

        modelBuilder.Entity<VUserRole>(entity =>
        {
            entity.ToView("V_UserRole");
        });

        modelBuilder.Entity<VVehicle>(entity =>
        {
            entity.ToView("V_Vehicle");
        });

        modelBuilder.Entity<VVisitsMaintenanceReportUser>(entity =>
        {
            entity.ToView("V_VisitsMaintenance_Report_Users");
        });

        modelBuilder.Entity<VacationDay>(entity =>
        {
            entity.HasOne(d => d.Branch).WithMany(p => p.VacationDays)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VacationDay_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VacationDayCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VacationDay_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.VacationDayModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VacationDay_User1");
        });

        modelBuilder.Entity<VacationOverTimeAndDeductionRate>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VacationOverTimeAndDeductionRateCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VacationOverTimeAndDeductionRates_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.VacationOverTimeAndDeductionRateModifiedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VacationOverTimeAndDeductionRates_User1");

            entity.HasOne(d => d.VacationDay).WithMany(p => p.VacationOverTimeAndDeductionRates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VacationOverTimeAndDeductionRates_VacationDay");
        });

        modelBuilder.Entity<VacationPaymentStrategy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_aymentStrategy");

            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<VehicleBodyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VehicleSubModel");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.VehicleModel).WithMany(p => p.VehicleBodyTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleSubModel_VehicleModel");
        });

        modelBuilder.Entity<VehicleBrand>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<VehicleColor>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<VehicleIssuer>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<VehicleMaintenanceJobOrderHistory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.JobOrderProject).WithMany(p => p.VehicleMaintenanceJobOrderHistories).HasConstraintName("FK_VehicleMaintenanceJobOrderHistory_Project");

            entity.HasOne(d => d.NextVisitFor).WithMany(p => p.VehicleMaintenanceJobOrderHistoryNextVisitFors).HasConstraintName("FK_VehicleMaintenanceJobOrderHistory_VehicleMaintenanceType1");

            entity.HasOne(d => d.SalesOffer).WithMany(p => p.VehicleMaintenanceJobOrderHistories).HasConstraintName("FK_VehicleMaintenanceJobOrderHistory_SalesOffer");

            entity.HasOne(d => d.VehicleMaintenanceType).WithMany(p => p.VehicleMaintenanceJobOrderHistoryVehicleMaintenanceTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleMaintenanceJobOrderHistory_VehicleMaintenanceType");

            entity.HasOne(d => d.VehiclePerClient).WithMany(p => p.VehicleMaintenanceJobOrderHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleMaintenanceJobOrderHistory_VehiclePerClient");
        });

        modelBuilder.Entity<VehicleMaintenanceType>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Bom).WithMany(p => p.VehicleMaintenanceTypes).HasConstraintName("FK_VehicleMaintenanceType_BOM");

            entity.HasOne(d => d.VehiclePriorityLevel).WithMany(p => p.VehicleMaintenanceTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleMaintenanceType_VehicleMaintenanceTypePriorityLevel");

            entity.HasOne(d => d.VehicleRate).WithMany(p => p.VehicleMaintenanceTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleMaintenanceType_VehicleMaintenanceTypeRate");
        });

        modelBuilder.Entity<VehicleMaintenanceTypeForModel>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Brand).WithMany(p => p.VehicleMaintenanceTypeForModels).HasConstraintName("FK_VehicleMaintenanceTypeForModel_VehicleBrand");

            entity.HasOne(d => d.Model).WithMany(p => p.VehicleMaintenanceTypeForModels).HasConstraintName("FK_VehicleMaintenanceTypeForModel_VehicleModel");

            entity.HasOne(d => d.VehicleMaintenanceType).WithMany(p => p.VehicleMaintenanceTypeForModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleMaintenanceTypeForModel_VehicleMaintenanceType");
        });

        modelBuilder.Entity<VehicleMaintenanceTypePriorityLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VehicleMaintenanceTypePriority");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.PriorityLevelName).IsFixedLength();
        });

        modelBuilder.Entity<VehicleMaintenanceTypeRate>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.RateName).IsFixedLength();
        });

        modelBuilder.Entity<VehicleMaintenanceTypeServiceSheduleCategory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.VehicleMaintenanceType).WithMany(p => p.VehicleMaintenanceTypeServiceSheduleCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleMaintenanceTypeServiceSheduleCategory_VehicleMaintenanceType");

            entity.HasOne(d => d.VehicleServiceScheduleCategory).WithMany(p => p.VehicleMaintenanceTypeServiceSheduleCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleMaintenanceTypeServiceSheduleCategory_VehicleServiceScheduleCategory");
        });

        modelBuilder.Entity<VehicleModel>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.VehicleBrand).WithMany(p => p.VehicleModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleModel_VehicleBrand");
        });

        modelBuilder.Entity<VehiclePerClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ClientVehicle");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.BodyType).WithMany(p => p.VehiclePerClients).HasConstraintName("FK_VehiclePerClient_VehicleBodyType");

            entity.HasOne(d => d.Brand).WithMany(p => p.VehiclePerClients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehiclePerClient_VehicleBrand");

            entity.HasOne(d => d.City).WithMany(p => p.VehiclePerClients).HasConstraintName("FK_ClientVehicle_Governorate");

            entity.HasOne(d => d.Client).WithMany(p => p.VehiclePerClients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehiclePerClient_Client");

            entity.HasOne(d => d.Color).WithMany(p => p.VehiclePerClients).HasConstraintName("FK_ClientVehicle_VehicleColor");

            entity.HasOne(d => d.Country).WithMany(p => p.VehiclePerClients).HasConstraintName("FK_ClientVehicle_Country");

            entity.HasOne(d => d.Issuer).WithMany(p => p.VehiclePerClients).HasConstraintName("FK_ClientVehicle_VehicleIssuer");

            entity.HasOne(d => d.Model).WithMany(p => p.VehiclePerClients).HasConstraintName("FK_VehiclePerClient_VehicleModel");

            entity.HasOne(d => d.Transmission).WithMany(p => p.VehiclePerClients).HasConstraintName("FK_ClientVehicle_VehicleTransmission");

            entity.HasOne(d => d.WheelsDrive).WithMany(p => p.VehiclePerClients).HasConstraintName("FK_ClientVehicle_VehicleWheelsDrive");
        });

        modelBuilder.Entity<VehicleServiceScheduleCategory>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_VehicleServiceScheduleCategory_VehicleServiceScheduleCategory");
        });

        modelBuilder.Entity<VehicleTransmission>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<VehicleWheelsDrive>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<VisitsScheduleOfMaintenance>(entity =>
        {
            entity.HasOne(d => d.AssignedTo).WithMany(p => p.VisitsScheduleOfMaintenanceAssignedTos).HasConstraintName("FK_VisitsScheduleOfMaintenance_User2");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VisitsScheduleOfMaintenanceCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VisitsScheduleOfMaintenance_User");

            entity.HasOne(d => d.MaintenanceFor).WithMany(p => p.VisitsScheduleOfMaintenances).HasConstraintName("FK_VisitsScheduleOfMaintenance_MaintenanceFor");

            entity.HasOne(d => d.ManagementOfMaintenanceOrder).WithMany(p => p.VisitsScheduleOfMaintenances).HasConstraintName("FK_VisitsScheduleOfMaintenance_ManagementOfMaintenanceOrder");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.VisitsScheduleOfMaintenanceModifiedByNavigations).HasConstraintName("FK_VisitsScheduleOfMaintenance_User1");

            entity.HasOne(d => d.Offer).WithMany(p => p.VisitsScheduleOfMaintenances).HasConstraintName("FK_VisitsScheduleOfMaintenance_SalesOffer");
        });

        modelBuilder.Entity<VisitsScheduleOfMaintenanceAttachment>(entity =>
        {
            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VisitsScheduleOfMaintenanceAttachmentCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VisitsScheduleOfMaintenanceAttachment_User");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.VisitsScheduleOfMaintenanceAttachmentModifiedByNavigations).HasConstraintName("FK_VisitsScheduleOfMaintenanceAttachment_User1");

            entity.HasOne(d => d.VisitsScheduleOfMaintenance).WithMany(p => p.VisitsScheduleOfMaintenanceAttachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VisitsScheduleOfMaintenanceAttachment_VisitsScheduleOfMaintenance");
        });

        modelBuilder.Entity<WeekDay>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.WeekDays).HasConstraintName("FK_WeekDays_Branch");
        });

        modelBuilder.Entity<WorkingHour>(entity =>
        {
            entity.HasOne(d => d.DayNavigation).WithMany(p => p.WorkingHours)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkingHours_WeekDays");
        });

        modelBuilder.Entity<WorkingHourseTracking>(entity =>
        {
            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.WorkingHourseTrackingApprovedByNavigations).HasConstraintName("FK_WorkingHourseTracking_User");

            entity.HasOne(d => d.Branch).WithMany(p => p.WorkingHourseTrackings).HasConstraintName("FK_WorkingHourseTracking_Branch");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkingHourseTrackingCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkingHourseTracking_User1");

            entity.HasOne(d => d.DayType).WithMany(p => p.WorkingHourseTrackings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkingHourseTracking_DayType");

            entity.HasOne(d => d.HrUser).WithMany(p => p.WorkingHourseTrackings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkingHourseTracking_HrUser");

            entity.HasOne(d => d.Invoice).WithMany(p => p.WorkingHourseTrackings).HasConstraintName("FK_WorkingHourseTracking_Invoices");

            entity.HasOne(d => d.Project).WithMany(p => p.WorkingHourseTrackings).HasConstraintName("FK_WorkingHourseTracking_Project");

            entity.HasOne(d => d.Shift).WithMany(p => p.WorkingHourseTrackings).HasConstraintName("FK_WorkingHourseTracking_BranchSchedule");

            entity.HasOne(d => d.Task).WithMany(p => p.WorkingHourseTrackings).HasConstraintName("FK_WorkingHourseTracking_Task");
        });

        modelBuilder.Entity<WorkshopStation>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.WorkshopStations).HasConstraintName("FK_WorkshopStation_Branch");

            entity.HasOne(d => d.Team).WithMany(p => p.WorkshopStations).HasConstraintName("FK_WorkshopStation_Team");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
