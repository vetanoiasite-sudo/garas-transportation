using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("User")]
public partial class User
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Password { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(250)]
    public string Email { get; set; }

    [Required]
    [StringLength(20)]
    public string Mobile { get; set; }

    public byte[] Photo { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(50)]
    public string MiddleName { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string Gender { get; set; }

    public long? CreatedBy { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column("DepartmentID")]
    public int? DepartmentId { get; set; }

    [Column("JobTitleID")]
    public int? JobTitleId { get; set; }

    [Column("OldID")]
    public int? OldId { get; set; }

    [Column("PhotoURL")]
    [StringLength(500)]
    public string PhotoUrl { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AccountCategory> AccountCategoryCreatedByNavigations { get; set; } = new List<AccountCategory>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<AccountCategory> AccountCategoryModifiedByNavigations { get; set; } = new List<AccountCategory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Account> AccountCreatedByNavigations { get; set; } = new List<Account>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Account> AccountModifiedByNavigations { get; set; } = new List<Account>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AccountOfAdjustingEntry> AccountOfAdjustingEntryCreatedByNavigations { get; set; } = new List<AccountOfAdjustingEntry>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<AccountOfAdjustingEntry> AccountOfAdjustingEntryModifiedByNavigations { get; set; } = new List<AccountOfAdjustingEntry>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AccountOfJournalEntry> AccountOfJournalEntryCreatedByNavigations { get; set; } = new List<AccountOfJournalEntry>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<AccountOfJournalEntry> AccountOfJournalEntryModifiedByNavigations { get; set; } = new List<AccountOfJournalEntry>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AccountOfJournalEntryOtherCurrency> AccountOfJournalEntryOtherCurrencyCreatedByNavigations { get; set; } = new List<AccountOfJournalEntryOtherCurrency>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<AccountOfJournalEntryOtherCurrency> AccountOfJournalEntryOtherCurrencyModifiedByNavigations { get; set; } = new List<AccountOfJournalEntryOtherCurrency>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AdvanciedSettingAccount> AdvanciedSettingAccountCreatedByNavigations { get; set; } = new List<AdvanciedSettingAccount>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<AdvanciedSettingAccount> AdvanciedSettingAccountModifiedByNavigations { get; set; } = new List<AdvanciedSettingAccount>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AdvanciedType> AdvanciedTypeCreatedByNavigations { get; set; } = new List<AdvanciedType>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<AdvanciedType> AdvanciedTypeModifiedByNavigations { get; set; } = new List<AdvanciedType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Area> AreaCreatedByNavigations { get; set; } = new List<Area>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Area> AreaModifiedByNavigations { get; set; } = new List<Area>();

    [InverseProperty("ApprovedByUser")]
    public virtual ICollection<Attendance> AttendanceApprovedByUsers { get; set; } = new List<Attendance>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Attendance> AttendanceCreatedByNavigations { get; set; } = new List<Attendance>();

    [InverseProperty("Employee")]
    public virtual ICollection<Attendance> AttendanceEmployees { get; set; } = new List<Attendance>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Attendance> AttendanceModifiedByNavigations { get; set; } = new List<Attendance>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BankChequeTemplate> BankChequeTemplateCreatedByNavigations { get; set; } = new List<BankChequeTemplate>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<BankChequeTemplate> BankChequeTemplateModifiedByNavigations { get; set; } = new List<BankChequeTemplate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Bom> BomCreatedByNavigations { get; set; } = new List<Bom>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Bom> BomModifiedByNavigations { get; set; } = new List<Bom>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Bomattachment> BomattachmentCreatedByNavigations { get; set; } = new List<Bomattachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Bomattachment> BomattachmentModifiedByNavigations { get; set; } = new List<Bomattachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Bomhistory> BomhistoryCreatedByNavigations { get; set; } = new List<Bomhistory>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Bomhistory> BomhistoryModifiedByNavigations { get; set; } = new List<Bomhistory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Bomimage> BomimageCreatedByNavigations { get; set; } = new List<Bomimage>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Bomimage> BomimageModifiedByNavigations { get; set; } = new List<Bomimage>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Bomlibrary> BomlibraryCreatedByNavigations { get; set; } = new List<Bomlibrary>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Bomlibrary> BomlibraryModifiedByNavigations { get; set; } = new List<Bomlibrary>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BompartitionAttachment> BompartitionAttachmentCreatedByNavigations { get; set; } = new List<BompartitionAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<BompartitionAttachment> BompartitionAttachmentModifiedByNavigations { get; set; } = new List<BompartitionAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Bompartition> BompartitionCreatedByNavigations { get; set; } = new List<Bompartition>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BompartitionHistory> BompartitionHistoryCreatedByNavigations { get; set; } = new List<BompartitionHistory>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<BompartitionHistory> BompartitionHistoryModifiedByNavigations { get; set; } = new List<BompartitionHistory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BompartitionItemAttachment> BompartitionItemAttachmentCreatedByNavigations { get; set; } = new List<BompartitionItemAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<BompartitionItemAttachment> BompartitionItemAttachmentModifiedByNavigations { get; set; } = new List<BompartitionItemAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BompartitionItem> BompartitionItemCreatedByNavigations { get; set; } = new List<BompartitionItem>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<BompartitionItem> BompartitionItemModifiedByNavigations { get; set; } = new List<BompartitionItem>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Bompartition> BompartitionModifiedByNavigations { get; set; } = new List<Bompartition>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Bomproduct> BomproductCreatedByNavigations { get; set; } = new List<Bomproduct>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Bomproduct> BomproductModifiedByNavigations { get; set; } = new List<Bomproduct>();

    [ForeignKey("BranchId")]
    [InverseProperty("Users")]
    public virtual Branch Branch { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Branch> BranchCreatedByNavigations { get; set; } = new List<Branch>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Branch> BranchModifiedByNavigations { get; set; } = new List<Branch>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BranchProduct> BranchProducts { get; set; } = new List<BranchProduct>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BranchSchedule> BranchScheduleCreatedByNavigations { get; set; } = new List<BranchSchedule>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<BranchSchedule> BranchScheduleModifiedByNavigations { get; set; } = new List<BranchSchedule>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BranchSetting> BranchSettingCreatedByNavigations { get; set; } = new List<BranchSetting>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<BranchSetting> BranchSettingModifiedByNavigations { get; set; } = new List<BranchSetting>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Car> CarCreatedByNavigations { get; set; } = new List<Car>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Car> CarModifiedByNavigations { get; set; } = new List<Car>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CarsAttachment> CarsAttachmentCreatedByNavigations { get; set; } = new List<CarsAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<CarsAttachment> CarsAttachmentModifiedByNavigations { get; set; } = new List<CarsAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CategoryType> CategoryTypeCreatedByNavigations { get; set; } = new List<CategoryType>();

    [InverseProperty("ModifedByNavigation")]
    public virtual ICollection<CategoryType> CategoryTypeModifedByNavigations { get; set; } = new List<CategoryType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientAccount> ClientAccountCreatedByNavigations { get; set; } = new List<ClientAccount>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientAccount> ClientAccountModifiedByNavigations { get; set; } = new List<ClientAccount>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientAddress> ClientAddressCreatedByNavigations { get; set; } = new List<ClientAddress>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientAddress> ClientAddressModifiedByNavigations { get; set; } = new List<ClientAddress>();

    [InverseProperty("ApprovedByNavigation")]
    public virtual ICollection<Client> ClientApprovedByNavigations { get; set; } = new List<Client>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientAttachment> ClientAttachmentCreatedByNavigations { get; set; } = new List<ClientAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientAttachment> ClientAttachmentModifiedByNavigations { get; set; } = new List<ClientAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientConsultantAddress> ClientConsultantAddressCreatedByNavigations { get; set; } = new List<ClientConsultantAddress>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientConsultantAddress> ClientConsultantAddressModifiedByNavigations { get; set; } = new List<ClientConsultantAddress>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientConsultant> ClientConsultantCreatedByNavigations { get; set; } = new List<ClientConsultant>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientConsultantEmail> ClientConsultantEmailCreatedByNavigations { get; set; } = new List<ClientConsultantEmail>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientConsultantEmail> ClientConsultantEmailModifiedByNavigations { get; set; } = new List<ClientConsultantEmail>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientConsultantFax> ClientConsultantFaxCreatedByNavigations { get; set; } = new List<ClientConsultantFax>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientConsultantFax> ClientConsultantFaxModifiedByNavigations { get; set; } = new List<ClientConsultantFax>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientConsultantMobile> ClientConsultantMobileCreatedByNavigations { get; set; } = new List<ClientConsultantMobile>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientConsultantMobile> ClientConsultantMobileModifiedByNavigations { get; set; } = new List<ClientConsultantMobile>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientConsultant> ClientConsultantModifiedByNavigations { get; set; } = new List<ClientConsultant>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientConsultantPhone> ClientConsultantPhoneCreatedByNavigations { get; set; } = new List<ClientConsultantPhone>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientConsultantPhone> ClientConsultantPhoneModifiedByNavigations { get; set; } = new List<ClientConsultantPhone>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientContactPerson> ClientContactPersonCreatedByNavigations { get; set; } = new List<ClientContactPerson>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientContactPerson> ClientContactPersonModifiedByNavigations { get; set; } = new List<ClientContactPerson>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Client> ClientCreatedByNavigations { get; set; } = new List<Client>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientExtraInfo> ClientExtraInfoCreatedByNavigations { get; set; } = new List<ClientExtraInfo>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientExtraInfo> ClientExtraInfoModifiedByNavigations { get; set; } = new List<ClientExtraInfo>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientFax> ClientFaxCreatedByNavigations { get; set; } = new List<ClientFax>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientFax> ClientFaxModifiedByNavigations { get; set; } = new List<ClientFax>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientMobile> ClientMobileCreatedByNavigations { get; set; } = new List<ClientMobile>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientMobile> ClientMobileModifiedByNavigations { get; set; } = new List<ClientMobile>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientPhone> ClientPhoneCreatedByNavigations { get; set; } = new List<ClientPhone>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientPhone> ClientPhoneModifiedByNavigations { get; set; } = new List<ClientPhone>();

    [InverseProperty("SalesPerson")]
    public virtual ICollection<Client> ClientSalesPeople { get; set; } = new List<Client>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientSalesPerson> ClientSalesPersonCreatedByNavigations { get; set; } = new List<ClientSalesPerson>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientSalesPerson> ClientSalesPersonModifiedByNavigations { get; set; } = new List<ClientSalesPerson>();

    [InverseProperty("SalesPerson")]
    public virtual ICollection<ClientSalesPerson> ClientSalesPersonSalesPeople { get; set; } = new List<ClientSalesPerson>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ClientSpeciality> ClientSpecialityCreatedByNavigations { get; set; } = new List<ClientSpeciality>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ClientSpeciality> ClientSpecialityModifiedByNavigations { get; set; } = new List<ClientSpeciality>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ConfirmedRecieveAndReleaseAttachment> ConfirmedRecieveAndReleaseAttachmentCreatedByNavigations { get; set; } = new List<ConfirmedRecieveAndReleaseAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ConfirmedRecieveAndReleaseAttachment> ConfirmedRecieveAndReleaseAttachmentModifiedByNavigations { get; set; } = new List<ConfirmedRecieveAndReleaseAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ConfirmedRecieveAndRelease> ConfirmedRecieveAndReleaseCreatedByNavigations { get; set; } = new List<ConfirmedRecieveAndRelease>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ConfirmedRecieveAndRelease> ConfirmedRecieveAndReleaseModifiedByNavigations { get; set; } = new List<ConfirmedRecieveAndRelease>();

    [InverseProperty("ReceivedByNavigation")]
    public virtual ICollection<ConfirmedRecieveAndRelease> ConfirmedRecieveAndReleaseReceivedByNavigations { get; set; } = new List<ConfirmedRecieveAndRelease>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ContractDetail> ContractDetailCreatedByNavigations { get; set; } = new List<ContractDetail>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ContractDetail> ContractDetailModifiedByNavigations { get; set; } = new List<ContractDetail>();

    [InverseProperty("ReportTo")]
    public virtual ICollection<ContractDetail> ContractDetailReportTos { get; set; } = new List<ContractDetail>();

    [InverseProperty("User")]
    public virtual ICollection<ContractDetail> ContractDetailUsers { get; set; } = new List<ContractDetail>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ContractLeaveEmployee> ContractLeaveEmployeeCreatedByNavigations { get; set; } = new List<ContractLeaveEmployee>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ContractLeaveEmployee> ContractLeaveEmployeeModifiedByNavigations { get; set; } = new List<ContractLeaveEmployee>();

    [InverseProperty("User")]
    public virtual ICollection<ContractLeaveEmployee> ContractLeaveEmployeeUsers { get; set; } = new List<ContractLeaveEmployee>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ContractLeaveSetting> ContractLeaveSettingCreatedByNavigations { get; set; } = new List<ContractLeaveSetting>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ContractLeaveSetting> ContractLeaveSettingModifiedByNavigations { get; set; } = new List<ContractLeaveSetting>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ContractReportTo> ContractReportToCreatedByNavigations { get; set; } = new List<ContractReportTo>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ContractReportTo> ContractReportToModifiedByNavigations { get; set; } = new List<ContractReportTo>();

    [InverseProperty("ReportTo")]
    public virtual ICollection<ContractReportTo> ContractReportToReportTos { get; set; } = new List<ContractReportTo>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CostType> CostTypes { get; set; } = new List<CostType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Country> CountryCreatedByNavigations { get; set; } = new List<Country>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Country> CountryModifiedByNavigations { get; set; } = new List<Country>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("InverseCreatedByNavigation")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CrmcontactType> CrmcontactTypeCreatedByNavigations { get; set; } = new List<CrmcontactType>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<CrmcontactType> CrmcontactTypeModifiedByNavigations { get; set; } = new List<CrmcontactType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CrmrecievedType> CrmrecievedTypeCreatedByNavigations { get; set; } = new List<CrmrecievedType>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<CrmrecievedType> CrmrecievedTypeModifiedByNavigations { get; set; } = new List<CrmrecievedType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Crmreport> CrmreportCreatedByNavigations { get; set; } = new List<Crmreport>();

    [InverseProperty("Crmuser")]
    public virtual ICollection<Crmreport> CrmreportCrmusers { get; set; } = new List<Crmreport>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Crmreport> CrmreportModifiedByNavigations { get; set; } = new List<Crmreport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<CrmreportReason> CrmreportReasonCreatedByNavigations { get; set; } = new List<CrmreportReason>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<CrmreportReason> CrmreportReasonModifiedByNavigations { get; set; } = new List<CrmreportReason>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyAdjustingEntryCostCenter> DailyAdjustingEntryCostCenterCreatedByNavigations { get; set; } = new List<DailyAdjustingEntryCostCenter>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyAdjustingEntryCostCenter> DailyAdjustingEntryCostCenterModifiedByNavigations { get; set; } = new List<DailyAdjustingEntryCostCenter>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyJournalEntry> DailyJournalEntryCreatedByNavigations { get; set; } = new List<DailyJournalEntry>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyJournalEntry> DailyJournalEntryModifiedByNavigations { get; set; } = new List<DailyJournalEntry>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyJournalEntryReverse> DailyJournalEntryReverseCreatedByNavigations { get; set; } = new List<DailyJournalEntryReverse>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyJournalEntryReverse> DailyJournalEntryReverseModifiedByNavigations { get; set; } = new List<DailyJournalEntryReverse>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyReportAttachment> DailyReportAttachmentCreatedByNavigations { get; set; } = new List<DailyReportAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyReportAttachment> DailyReportAttachmentModifiedByNavigations { get; set; } = new List<DailyReportAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyReportExpense> DailyReportExpenseCreatedByNavigations { get; set; } = new List<DailyReportExpense>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyReportExpense> DailyReportExpenseModifiedByNavigations { get; set; } = new List<DailyReportExpense>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyReport> DailyReportModifiedByNavigations { get; set; } = new List<DailyReport>();

    [InverseProperty("ReviewedByNavigation")]
    public virtual ICollection<DailyReport> DailyReportReviewedByNavigations { get; set; } = new List<DailyReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyReportThrough> DailyReportThroughCreatedByNavigations { get; set; } = new List<DailyReportThrough>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyReportThrough> DailyReportThroughModifiedByNavigations { get; set; } = new List<DailyReportThrough>();

    [InverseProperty("User")]
    public virtual ICollection<DailyReport> DailyReportUsers { get; set; } = new List<DailyReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyTranactionAttachment> DailyTranactionAttachmentCreatedByNavigations { get; set; } = new List<DailyTranactionAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyTranactionAttachment> DailyTranactionAttachmentModifiedByNavigations { get; set; } = new List<DailyTranactionAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyTranactionBeneficiaryToGeneralType> DailyTranactionBeneficiaryToGeneralTypeCreatedByNavigations { get; set; } = new List<DailyTranactionBeneficiaryToGeneralType>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyTranactionBeneficiaryToGeneralType> DailyTranactionBeneficiaryToGeneralTypeModifiedByNavigations { get; set; } = new List<DailyTranactionBeneficiaryToGeneralType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyTranactionBeneficiaryToType> DailyTranactionBeneficiaryToTypeCreatedByNavigations { get; set; } = new List<DailyTranactionBeneficiaryToType>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyTranactionBeneficiaryToType> DailyTranactionBeneficiaryToTypeModifiedByNavigations { get; set; } = new List<DailyTranactionBeneficiaryToType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyTranactionBeneficiaryToUser> DailyTranactionBeneficiaryToUserCreatedByNavigations { get; set; } = new List<DailyTranactionBeneficiaryToUser>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyTranactionBeneficiaryToUser> DailyTranactionBeneficiaryToUserModifiedByNavigations { get; set; } = new List<DailyTranactionBeneficiaryToUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyTransactionCostCenter> DailyTransactionCostCenterCreatedByNavigations { get; set; } = new List<DailyTransactionCostCenter>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyTransactionCostCenter> DailyTransactionCostCenterModifiedByNavigations { get; set; } = new List<DailyTransactionCostCenter>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DailyTransaction> DailyTransactionCreatedByNavigations { get; set; } = new List<DailyTransaction>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DailyTransaction> DailyTransactionModifiedByNavigations { get; set; } = new List<DailyTransaction>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DeductionType> DeductionTypes { get; set; } = new List<DeductionType>();

    [ForeignKey("DepartmentId")]
    [InverseProperty("Users")]
    public virtual Department Department { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Department> DepartmentCreatedByNavigations { get; set; } = new List<Department>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Department> DepartmentModifiedByNavigations { get; set; } = new List<Department>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DoctorRoom> DoctorRooms { get; set; } = new List<DoctorRoom>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DoctorSchedule> DoctorScheduleCreatedByNavigations { get; set; } = new List<DoctorSchedule>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DoctorSchedule> DoctorScheduleModifiedByNavigations { get; set; } = new List<DoctorSchedule>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Driver> DriverCreatedByNavigations { get; set; } = new List<Driver>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Driver> DriverModifiedByNavigations { get; set; } = new List<Driver>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<DriversAttachment> DriversAttachmentCreatedByNavigations { get; set; } = new List<DriversAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<DriversAttachment> DriversAttachmentModifiedByNavigations { get; set; } = new List<DriversAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<EmailCategory> EmailCategoryCreatedByNavigations { get; set; } = new List<EmailCategory>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<EmailCategory> EmailCategoryModifiedByNavigations { get; set; } = new List<EmailCategory>();

    [InverseProperty("User")]
    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ExchangeRate> ExchangeRateCreatedByNavigations { get; set; } = new List<ExchangeRate>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ExchangeRate> ExchangeRateModifiedByNavigations { get; set; } = new List<ExchangeRate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ExpensisType> ExpensisTypeCreatedByNavigations { get; set; } = new List<ExpensisType>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ExpensisType> ExpensisTypeModifiedByNavigations { get; set; } = new List<ExpensisType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ExtraCostLibrary> ExtraCostLibraryCreatedByNavigations { get; set; } = new List<ExtraCostLibrary>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ExtraCostLibrary> ExtraCostLibraryModifiedByNavigations { get; set; } = new List<ExtraCostLibrary>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<GarasClientInfo> GarasClientInfoCreatedByNavigations { get; set; } = new List<GarasClientInfo>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<GarasClientInfo> GarasClientInfoModifiedByNavigations { get; set; } = new List<GarasClientInfo>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<GeneralActiveCostCenter> GeneralActiveCostCenterCreatedByNavigations { get; set; } = new List<GeneralActiveCostCenter>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<GeneralActiveCostCenter> GeneralActiveCostCenterModifiedByNavigations { get; set; } = new List<GeneralActiveCostCenter>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Governorate> Governorates { get; set; } = new List<Governorate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Group> GroupCreatedByNavigations { get; set; } = new List<Group>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Group> GroupModifiedByNavigations { get; set; } = new List<Group>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<GroupRole> GroupRoles { get; set; } = new List<GroupRole>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<GroupUser> GroupUserCreatedByNavigations { get; set; } = new List<GroupUser>();

    [InverseProperty("User")]
    public virtual ICollection<GroupUser> GroupUserUsers { get; set; } = new List<GroupUser>();

    [InverseProperty("CreatedBy")]
    public virtual ICollection<HrUser> HrUserCreatedBies { get; set; } = new List<HrUser>();

    [InverseProperty("ModifiedBy")]
    public virtual ICollection<HrUser> HrUserModifiedBies { get; set; } = new List<HrUser>();

    [InverseProperty("User")]
    public virtual ICollection<HrUser> HrUserUsers { get; set; } = new List<HrUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Hrcustody> HrcustodyCreatedByNavigations { get; set; } = new List<Hrcustody>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Hrcustody> HrcustodyModifiedByNavigations { get; set; } = new List<Hrcustody>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<HrcustodyReportAttachment> HrcustodyReportAttachmentCreatedByNavigations { get; set; } = new List<HrcustodyReportAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<HrcustodyReportAttachment> HrcustodyReportAttachmentModifiedByNavigations { get; set; } = new List<HrcustodyReportAttachment>();

    [InverseProperty("User")]
    public virtual ICollection<Hrcustody> HrcustodyUsers { get; set; } = new List<Hrcustody>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<HrdeductionRewarding> HrdeductionRewardingCreatedByUsers { get; set; } = new List<HrdeductionRewarding>();

    [InverseProperty("User")]
    public virtual ICollection<HrdeductionRewarding> HrdeductionRewardingUsers { get; set; } = new List<HrdeductionRewarding>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<HremployeeAttachment> HremployeeAttachmentCreatedByNavigations { get; set; } = new List<HremployeeAttachment>();

    [InverseProperty("EmployeeUser")]
    public virtual ICollection<HremployeeAttachment> HremployeeAttachmentEmployeeUsers { get; set; } = new List<HremployeeAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<HremployeeAttachment> HremployeeAttachmentModifiedByNavigations { get; set; } = new List<HremployeeAttachment>();

    [InverseProperty("ApprovedByUser")]
    public virtual ICollection<Hrloan> HrloanApprovedByUsers { get; set; } = new List<Hrloan>();

    [InverseProperty("User")]
    public virtual ICollection<Hrloan> HrloanUsers { get; set; } = new List<Hrloan>();

    [InverseProperty("User")]
    public virtual ICollection<HruserWarning> HruserWarnings { get; set; } = new List<HruserWarning>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ImportantDate> ImportantDates { get; set; } = new List<ImportantDate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<IncomeType> IncomeTypeCreatedByNavigations { get; set; } = new List<IncomeType>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<IncomeType> IncomeTypeModifiedByNavigations { get; set; } = new List<IncomeType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InsuranceCompanyName> InsuranceCompanyNames { get; set; } = new List<InsuranceCompanyName>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Interview> InterviewCreatedByNavigations { get; set; } = new List<Interview>();

    [InverseProperty("Interviewer")]
    public virtual ICollection<Interview> InterviewInterviewers { get; set; } = new List<Interview>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Interview> InterviewModifiedByNavigations { get; set; } = new List<Interview>();

    [InverseProperty("User")]
    public virtual ICollection<Interview> InterviewUsers { get; set; } = new List<Interview>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryAddingOrder> InventoryAddingOrderCreatedByNavigations { get; set; } = new List<InventoryAddingOrder>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryAddingOrder> InventoryAddingOrderModifiedByNavigations { get; set; } = new List<InventoryAddingOrder>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryInternalBackOrder> InventoryInternalBackOrderCreatedByNavigations { get; set; } = new List<InventoryInternalBackOrder>();

    [InverseProperty("From")]
    public virtual ICollection<InventoryInternalBackOrder> InventoryInternalBackOrderFroms { get; set; } = new List<InventoryInternalBackOrder>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryInternalBackOrder> InventoryInternalBackOrderModifiedByNavigations { get; set; } = new List<InventoryInternalBackOrder>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryInternalTransferOrder> InventoryInternalTransferOrderCreatedByNavigations { get; set; } = new List<InventoryInternalTransferOrder>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryInternalTransferOrder> InventoryInternalTransferOrderModifiedByNavigations { get; set; } = new List<InventoryInternalTransferOrder>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryItemCategory> InventoryItemCategoryCreatedByNavigations { get; set; } = new List<InventoryItemCategory>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryItemCategory> InventoryItemCategoryModifiedByNavigations { get; set; } = new List<InventoryItemCategory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryItemContent> InventoryItemContentCreatedByNavigations { get; set; } = new List<InventoryItemContent>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryItemContent> InventoryItemContentModifiedByNavigations { get; set; } = new List<InventoryItemContent>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryItem> InventoryItemCreatedByNavigations { get; set; } = new List<InventoryItem>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryItem> InventoryItemModifiedByNavigations { get; set; } = new List<InventoryItem>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryItemPrice> InventoryItemPrices { get; set; } = new List<InventoryItemPrice>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryItemUom> InventoryItemUomCreatedByNavigations { get; set; } = new List<InventoryItemUom>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryItemUom> InventoryItemUomModifiedByNavigations { get; set; } = new List<InventoryItemUom>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryMatrialRelease> InventoryMatrialReleaseCreatedByNavigations { get; set; } = new List<InventoryMatrialRelease>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryMatrialRelease> InventoryMatrialReleaseModifiedByNavigations { get; set; } = new List<InventoryMatrialRelease>();

    [InverseProperty("ToUser")]
    public virtual ICollection<InventoryMatrialRelease> InventoryMatrialReleaseToUsers { get; set; } = new List<InventoryMatrialRelease>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryMatrialRequest> InventoryMatrialRequestCreatedByNavigations { get; set; } = new List<InventoryMatrialRequest>();

    [InverseProperty("FromUser")]
    public virtual ICollection<InventoryMatrialRequest> InventoryMatrialRequestFromUsers { get; set; } = new List<InventoryMatrialRequest>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryMatrialRequest> InventoryMatrialRequestModifiedByNavigations { get; set; } = new List<InventoryMatrialRequest>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryReportItem> InventoryReportItemCreatedByNavigations { get; set; } = new List<InventoryReportItem>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryReportItem> InventoryReportItemModifiedByNavigations { get; set; } = new List<InventoryReportItem>();

    [InverseProperty("ByUser")]
    public virtual ICollection<InventoryReport> InventoryReports { get; set; } = new List<InventoryReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryStore> InventoryStoreCreatedByNavigations { get; set; } = new List<InventoryStore>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryStoreItem> InventoryStoreItemCreatedByNavigations { get; set; } = new List<InventoryStoreItem>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryStoreItem> InventoryStoreItemModifiedByNavigations { get; set; } = new List<InventoryStoreItem>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryStoreKeeper> InventoryStoreKeeperCreatedByNavigations { get; set; } = new List<InventoryStoreKeeper>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryStoreKeeper> InventoryStoreKeeperModifiedByNavigations { get; set; } = new List<InventoryStoreKeeper>();

    [InverseProperty("User")]
    public virtual ICollection<InventoryStoreKeeper> InventoryStoreKeeperUsers { get; set; } = new List<InventoryStoreKeeper>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryStore> InventoryStoreModifiedByNavigations { get; set; } = new List<InventoryStore>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryUom> InventoryUomCreatedByNavigations { get; set; } = new List<InventoryUom>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InventoryUom> InventoryUomModifiedByNavigations { get; set; } = new List<InventoryUom>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<User> InverseModifiedByNavigation { get; set; } = new List<User>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Invoice> InvoiceCreatedByNavigations { get; set; } = new List<Invoice>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InvoiceExtraCost> InvoiceExtraCostCreatedByNavigations { get; set; } = new List<InvoiceExtraCost>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InvoiceExtraCost> InvoiceExtraCostModifiedByNavigations { get; set; } = new List<InvoiceExtraCost>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InvoiceExtraModification> InvoiceExtraModificationCreatedByNavigations { get; set; } = new List<InvoiceExtraModification>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InvoiceExtraModification> InvoiceExtraModificationModifiedByNavigations { get; set; } = new List<InvoiceExtraModification>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Invoice> InvoiceModifiedByNavigations { get; set; } = new List<Invoice>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InvoiceNewClient> InvoiceNewClients { get; set; } = new List<InvoiceNewClient>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InvoiceTax> InvoiceTaxCreatedByNavigations { get; set; } = new List<InvoiceTax>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<InvoiceTax> InvoiceTaxModifiedByNavigations { get; set; } = new List<InvoiceTax>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<JobInformation> JobInformationCreatedByNavigations { get; set; } = new List<JobInformation>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<JobInformation> JobInformationModifiedByNavigations { get; set; } = new List<JobInformation>();

    [InverseProperty("ReportTo")]
    public virtual ICollection<JobInformation> JobInformationReportTos { get; set; } = new List<JobInformation>();

    [InverseProperty("User")]
    public virtual ICollection<JobInformation> JobInformationUsers { get; set; } = new List<JobInformation>();

    [ForeignKey("JobTitleId")]
    [InverseProperty("Users")]
    public virtual JobTitle JobTitle { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<JobTitle> JobTitleCreatedByNavigations { get; set; } = new List<JobTitle>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<JobTitle> JobTitleModifiedByNavigations { get; set; } = new List<JobTitle>();

    [InverseProperty("CreateByNavigation")]
    public virtual ICollection<LaboratoryMessagesReport> LaboratoryMessagesReports { get; set; } = new List<LaboratoryMessagesReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<LeaveRequest> LeaveRequestCreatedByNavigations { get; set; } = new List<LeaveRequest>();

    [InverseProperty("FirstApprovedByNavigation")]
    public virtual ICollection<LeaveRequest> LeaveRequestFirstApprovedByNavigations { get; set; } = new List<LeaveRequest>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<LeaveRequest> LeaveRequestModifiedByNavigations { get; set; } = new List<LeaveRequest>();

    [InverseProperty("SecondApprovedByNavigation")]
    public virtual ICollection<LeaveRequest> LeaveRequestSecondApprovedByNavigations { get; set; } = new List<LeaveRequest>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MaintenanceReportAttachment> MaintenanceReportAttachmentCreatedByNavigations { get; set; } = new List<MaintenanceReportAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<MaintenanceReportAttachment> MaintenanceReportAttachmentModifiedByNavigations { get; set; } = new List<MaintenanceReportAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MaintenanceReportClarificationAttachment> MaintenanceReportClarificationAttachmentCreatedByNavigations { get; set; } = new List<MaintenanceReportClarificationAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<MaintenanceReportClarificationAttachment> MaintenanceReportClarificationAttachmentModifiedByNavigations { get; set; } = new List<MaintenanceReportClarificationAttachment>();

    [InverseProperty("ClarificationUser")]
    public virtual ICollection<MaintenanceReportClarification> MaintenanceReportClarificationClarificationUsers { get; set; } = new List<MaintenanceReportClarification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MaintenanceReportClarification> MaintenanceReportClarificationCreatedByNavigations { get; set; } = new List<MaintenanceReportClarification>();

    [InverseProperty("ModifedByNavigation")]
    public virtual ICollection<MaintenanceReportClarification> MaintenanceReportClarificationModifedByNavigations { get; set; } = new List<MaintenanceReportClarification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MaintenanceReport> MaintenanceReportCreatedByNavigations { get; set; } = new List<MaintenanceReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MaintenanceReportExpense> MaintenanceReportExpenses { get; set; } = new List<MaintenanceReportExpense>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<MaintenanceReport> MaintenanceReportModifiedByNavigations { get; set; } = new List<MaintenanceReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MaintenanceReportUser> MaintenanceReportUserCreatedByNavigations { get; set; } = new List<MaintenanceReportUser>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<MaintenanceReportUser> MaintenanceReportUserModifiedByNavigations { get; set; } = new List<MaintenanceReportUser>();

    [InverseProperty("User")]
    public virtual ICollection<MaintenanceReportUser> MaintenanceReportUserUsers { get; set; } = new List<MaintenanceReportUser>();

    [InverseProperty("CreateByNavigation")]
    public virtual ICollection<ManageStage> ManageStageCreateByNavigations { get; set; } = new List<ManageStage>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ManageStage> ManageStageModifiedByNavigations { get; set; } = new List<ManageStage>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ManagementOfMaintenanceOrder> ManagementOfMaintenanceOrderCreatedByNavigations { get; set; } = new List<ManagementOfMaintenanceOrder>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ManagementOfMaintenanceOrder> ManagementOfMaintenanceOrderModifiedByNavigations { get; set; } = new List<ManagementOfMaintenanceOrder>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ManagementOfRentOrderAttachment> ManagementOfRentOrderAttachmentCreatedByNavigations { get; set; } = new List<ManagementOfRentOrderAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ManagementOfRentOrderAttachment> ManagementOfRentOrderAttachmentModifiedByNavigations { get; set; } = new List<ManagementOfRentOrderAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ManagementOfRentOrder> ManagementOfRentOrderCreatedByNavigations { get; set; } = new List<ManagementOfRentOrder>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ManagementOfRentOrder> ManagementOfRentOrderModifiedByNavigations { get; set; } = new List<ManagementOfRentOrder>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MangmentOfMaintenanceRequestAttachment> MangmentOfMaintenanceRequestAttachments { get; set; } = new List<MangmentOfMaintenanceRequestAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MangmentOfMaintenanceRequest> MangmentOfMaintenanceRequestCreatedByNavigations { get; set; } = new List<MangmentOfMaintenanceRequest>();

    [InverseProperty("InstallationAssignToNavigation")]
    public virtual ICollection<MangmentOfMaintenanceRequest> MangmentOfMaintenanceRequestInstallationAssignToNavigations { get; set; } = new List<MangmentOfMaintenanceRequest>();

    [InverseProperty("PreparingUser")]
    public virtual ICollection<MangmentOfMaintenanceRequest> MangmentOfMaintenanceRequestPreparingUsers { get; set; } = new List<MangmentOfMaintenanceRequest>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MangmentOfMaintenanceRequestReportAttachment> MangmentOfMaintenanceRequestReportAttachments { get; set; } = new List<MangmentOfMaintenanceRequestReportAttachment>();

    [InverseProperty("FinishedByNavigation")]
    public virtual ICollection<MangmentOfMaintenanceRequestReport> MangmentOfMaintenanceRequestReportFinishedByNavigations { get; set; } = new List<MangmentOfMaintenanceRequestReport>();

    [InverseProperty("ReportedByNavigation")]
    public virtual ICollection<MangmentOfMaintenanceRequestReport> MangmentOfMaintenanceRequestReportReportedByNavigations { get; set; } = new List<MangmentOfMaintenanceRequestReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MedicalDailyTreasuryBalance> MedicalDailyTreasuryBalanceCreatedByNavigations { get; set; } = new List<MedicalDailyTreasuryBalance>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<MedicalDailyTreasuryBalance> MedicalDailyTreasuryBalanceModifiedByNavigations { get; set; } = new List<MedicalDailyTreasuryBalance>();

    [InverseProperty("ReceivedFromNavigation")]
    public virtual ICollection<MedicalDailyTreasuryBalance> MedicalDailyTreasuryBalanceReceivedFromNavigations { get; set; } = new List<MedicalDailyTreasuryBalance>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MedicalReservation> MedicalReservationCreatedByNavigations { get; set; } = new List<MedicalReservation>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<MedicalReservation> MedicalReservationModifiedByNavigations { get; set; } = new List<MedicalReservation>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InverseModifiedByNavigation")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MovementReportAttachment> MovementReportAttachmentCreatedByNavigations { get; set; } = new List<MovementReportAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<MovementReportAttachment> MovementReportAttachmentModifiedByNavigations { get; set; } = new List<MovementReportAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MovementReport> MovementReportCreatedByNavigations { get; set; } = new List<MovementReport>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<MovementReport> MovementReportModifiedByNavigations { get; set; } = new List<MovementReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<MovementsAndDeliveryOrder> MovementsAndDeliveryOrderCreatedByNavigations { get; set; } = new List<MovementsAndDeliveryOrder>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<MovementsAndDeliveryOrder> MovementsAndDeliveryOrderModifiedByNavigations { get; set; } = new List<MovementsAndDeliveryOrder>();

    [InverseProperty("FromUser")]
    public virtual ICollection<Notification> NotificationFromUsers { get; set; } = new List<Notification>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> NotificationUsers { get; set; } = new List<Notification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<OverTimeAndDeductionRate> OverTimeAndDeductionRateCreatedByNavigations { get; set; } = new List<OverTimeAndDeductionRate>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<OverTimeAndDeductionRate> OverTimeAndDeductionRateModifiedByNavigations { get; set; } = new List<OverTimeAndDeductionRate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Payroll> PayrollCreatedByNavigations { get; set; } = new List<Payroll>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Payroll> PayrollModifiedByNavigations { get; set; } = new List<Payroll>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PermissionLevel> PermissionLevelCreatedByNavigations { get; set; } = new List<PermissionLevel>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PermissionLevel> PermissionLevelModifiedByNavigations { get; set; } = new List<PermissionLevel>();

    [InverseProperty("ApprovalUser")]
    public virtual ICollection<PoapprovalStatus> PoapprovalStatuses { get; set; } = new List<PoapprovalStatus>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PosClosingDay> PosClosingDayCreatedByNavigations { get; set; } = new List<PosClosingDay>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PosClosingDay> PosClosingDayModifiedByNavigations { get; set; } = new List<PosClosingDay>();

    [InverseProperty("User")]
    public virtual ICollection<PosClosingDay> PosClosingDayUsers { get; set; } = new List<PosClosingDay>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PricingBom> PricingBomCreatedByNavigations { get; set; } = new List<PricingBom>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PricingBom> PricingBomModifiedByNavigations { get; set; } = new List<PricingBom>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PricingClarificationAttachment> PricingClarificationAttachmentCreatedByNavigations { get; set; } = new List<PricingClarificationAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PricingClarificationAttachment> PricingClarificationAttachmentModifiedByNavigations { get; set; } = new List<PricingClarificationAttachment>();

    [InverseProperty("CrreatedByNavigation")]
    public virtual ICollection<PricingClearfication> PricingClearficationCrreatedByNavigations { get; set; } = new List<PricingClearfication>();

    [InverseProperty("SentToNavigation")]
    public virtual ICollection<PricingClearfication> PricingClearficationSentToNavigations { get; set; } = new List<PricingClearfication>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Pricing> PricingCreatedByNavigations { get; set; } = new List<Pricing>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PricingExtraCost> PricingExtraCostCreatedByNavigations { get; set; } = new List<PricingExtraCost>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PricingExtraCost> PricingExtraCostModifiedByNavigations { get; set; } = new List<PricingExtraCost>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Pricing> PricingModifiedByNavigations { get; set; } = new List<Pricing>();

    [InverseProperty("PricingPerson")]
    public virtual ICollection<Pricing> PricingPricingPeople { get; set; } = new List<Pricing>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PricingProductAttachment> PricingProductAttachmentCreatedByNavigations { get; set; } = new List<PricingProductAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PricingProductAttachment> PricingProductAttachmentModifiedByNavigations { get; set; } = new List<PricingProductAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PricingProduct> PricingProductCreatedByNavigations { get; set; } = new List<PricingProduct>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PricingProduct> PricingProductModifiedByNavigations { get; set; } = new List<PricingProduct>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PricingTerm> PricingTermCreatedByNavigations { get; set; } = new List<PricingTerm>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PricingTerm> PricingTermModifiedByNavigations { get; set; } = new List<PricingTerm>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProductGroup> ProductGroupCreatedByNavigations { get; set; } = new List<ProductGroup>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProductGroup> ProductGroupModifiedByNavigations { get; set; } = new List<ProductGroup>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Product> ProductModifiedByNavigations { get; set; } = new List<Product>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProgressType> ProgressTypeCreatedByNavigations { get; set; } = new List<ProgressType>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProgressType> ProgressTypeModifiedByNavigations { get; set; } = new List<ProgressType>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<ProjectAssignUser> ProjectAssignUserCreationByNavigations { get; set; } = new List<ProjectAssignUser>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectAssignUser> ProjectAssignUserModifiedByNavigations { get; set; } = new List<ProjectAssignUser>();

    [InverseProperty("User")]
    public virtual ICollection<ProjectAssignUser> ProjectAssignUserUsers { get; set; } = new List<ProjectAssignUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectCheque> ProjectChequeCreatedByNavigations { get; set; } = new List<ProjectCheque>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectCheque> ProjectChequeModifiedByNavigations { get; set; } = new List<ProjectCheque>();

    [InverseProperty("WithdrawedByNavigation")]
    public virtual ICollection<ProjectCheque> ProjectChequeWithdrawedByNavigations { get; set; } = new List<ProjectCheque>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectContactPerson> ProjectContactPersonCreatedByNavigations { get; set; } = new List<ProjectContactPerson>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectContactPerson> ProjectContactPersonModifiedByNavigations { get; set; } = new List<ProjectContactPerson>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Project> ProjectCreatedByNavigations { get; set; } = new List<Project>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationAttachment> ProjectFabricationAttachmentCreatedByNavigations { get; set; } = new List<ProjectFabricationAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFabricationAttachment> ProjectFabricationAttachmentModifiedByNavigations { get; set; } = new List<ProjectFabricationAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationBoq> ProjectFabricationBoqCreatedByNavigations { get; set; } = new List<ProjectFabricationBoq>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFabricationBoq> ProjectFabricationBoqModifiedByNavigations { get; set; } = new List<ProjectFabricationBoq>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabrication> ProjectFabricationCreatedByNavigations { get; set; } = new List<ProjectFabrication>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationJobTitle> ProjectFabricationJobTitleCreatedByNavigations { get; set; } = new List<ProjectFabricationJobTitle>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFabricationJobTitle> ProjectFabricationJobTitleModifiedByNavigations { get; set; } = new List<ProjectFabricationJobTitle>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFabrication> ProjectFabricationModifiedByNavigations { get; set; } = new List<ProjectFabrication>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationOrderUser> ProjectFabricationOrderUserCreatedByNavigations { get; set; } = new List<ProjectFabricationOrderUser>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFabricationOrderUser> ProjectFabricationOrderUserModifiedByNavigations { get; set; } = new List<ProjectFabricationOrderUser>();

    [InverseProperty("User")]
    public virtual ICollection<ProjectFabricationOrderUser> ProjectFabricationOrderUserUsers { get; set; } = new List<ProjectFabricationOrderUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationReportAttachment> ProjectFabricationReportAttachmentCreatedByNavigations { get; set; } = new List<ProjectFabricationReportAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFabricationReportAttachment> ProjectFabricationReportAttachmentModifiedByNavigations { get; set; } = new List<ProjectFabricationReportAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationReportClarificationAttachment> ProjectFabricationReportClarificationAttachmentCreatedByNavigations { get; set; } = new List<ProjectFabricationReportClarificationAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFabricationReportClarificationAttachment> ProjectFabricationReportClarificationAttachmentModifiedByNavigations { get; set; } = new List<ProjectFabricationReportClarificationAttachment>();

    [InverseProperty("ClarificationUser")]
    public virtual ICollection<ProjectFabricationReportClarification> ProjectFabricationReportClarificationClarificationUsers { get; set; } = new List<ProjectFabricationReportClarification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationReportClarification> ProjectFabricationReportClarificationCreatedByNavigations { get; set; } = new List<ProjectFabricationReportClarification>();

    [InverseProperty("ModifedByNavigation")]
    public virtual ICollection<ProjectFabricationReportClarification> ProjectFabricationReportClarificationModifedByNavigations { get; set; } = new List<ProjectFabricationReportClarification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationReport> ProjectFabricationReportCreatedByNavigations { get; set; } = new List<ProjectFabricationReport>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFabricationReport> ProjectFabricationReportModifiedByNavigations { get; set; } = new List<ProjectFabricationReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationReportUser> ProjectFabricationReportUserCreatedByNavigations { get; set; } = new List<ProjectFabricationReportUser>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFabricationReportUser> ProjectFabricationReportUserModifiedByNavigations { get; set; } = new List<ProjectFabricationReportUser>();

    [InverseProperty("User")]
    public virtual ICollection<ProjectFabricationReportUser> ProjectFabricationReportUserUsers { get; set; } = new List<ProjectFabricationReportUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFabricationVersion> ProjectFabricationVersions { get; set; } = new List<ProjectFabricationVersion>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectFinishInstallationAttachment> ProjectFinishInstallationAttachmentCreatedByNavigations { get; set; } = new List<ProjectFinishInstallationAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectFinishInstallationAttachment> ProjectFinishInstallationAttachmentModifiedByNavigations { get; set; } = new List<ProjectFinishInstallationAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallAttachment> ProjectInstallAttachmentCreatedByNavigations { get; set; } = new List<ProjectInstallAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInstallAttachment> ProjectInstallAttachmentModifiedByNavigations { get; set; } = new List<ProjectInstallAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallationAttachment> ProjectInstallationAttachmentCreatedByNavigations { get; set; } = new List<ProjectInstallationAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInstallationAttachment> ProjectInstallationAttachmentModifiedByNavigations { get; set; } = new List<ProjectInstallationAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallationBoq> ProjectInstallationBoqCreatedByNavigations { get; set; } = new List<ProjectInstallationBoq>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInstallationBoq> ProjectInstallationBoqModifiedByNavigations { get; set; } = new List<ProjectInstallationBoq>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallation> ProjectInstallationCreatedByNavigations { get; set; } = new List<ProjectInstallation>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallationJobTitle> ProjectInstallationJobTitleCreatedByNavigations { get; set; } = new List<ProjectInstallationJobTitle>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInstallationJobTitle> ProjectInstallationJobTitleModifiedByNavigations { get; set; } = new List<ProjectInstallationJobTitle>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInstallation> ProjectInstallationModifiedByNavigations { get; set; } = new List<ProjectInstallation>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallationReportAttachment> ProjectInstallationReportAttachmentCreatedByNavigations { get; set; } = new List<ProjectInstallationReportAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInstallationReportAttachment> ProjectInstallationReportAttachmentModifiedByNavigations { get; set; } = new List<ProjectInstallationReportAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallationReportClarificationAttachment> ProjectInstallationReportClarificationAttachmentCreatedByNavigations { get; set; } = new List<ProjectInstallationReportClarificationAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInstallationReportClarificationAttachment> ProjectInstallationReportClarificationAttachmentModifiedByNavigations { get; set; } = new List<ProjectInstallationReportClarificationAttachment>();

    [InverseProperty("ClarificationUser")]
    public virtual ICollection<ProjectInstallationReportClarification> ProjectInstallationReportClarificationClarificationUsers { get; set; } = new List<ProjectInstallationReportClarification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallationReportClarification> ProjectInstallationReportClarificationCreatedByNavigations { get; set; } = new List<ProjectInstallationReportClarification>();

    [InverseProperty("ModifedByNavigation")]
    public virtual ICollection<ProjectInstallationReportClarification> ProjectInstallationReportClarificationModifedByNavigations { get; set; } = new List<ProjectInstallationReportClarification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallationReport> ProjectInstallationReportCreatedByNavigations { get; set; } = new List<ProjectInstallationReport>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInstallationReport> ProjectInstallationReportModifiedByNavigations { get; set; } = new List<ProjectInstallationReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallationReportUser> ProjectInstallationReportUserCreatedByNavigations { get; set; } = new List<ProjectInstallationReportUser>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInstallationReportUser> ProjectInstallationReportUserModifiedByNavigations { get; set; } = new List<ProjectInstallationReportUser>();

    [InverseProperty("User")]
    public virtual ICollection<ProjectInstallationReportUser> ProjectInstallationReportUserUsers { get; set; } = new List<ProjectInstallationReportUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInstallationVersion> ProjectInstallationVersions { get; set; } = new List<ProjectInstallationVersion>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInvoiceCollected> ProjectInvoiceCollectedCreatedByNavigations { get; set; } = new List<ProjectInvoiceCollected>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInvoiceCollected> ProjectInvoiceCollectedModifiedByNavigations { get; set; } = new List<ProjectInvoiceCollected>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInvoice> ProjectInvoiceCreatedByNavigations { get; set; } = new List<ProjectInvoice>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectInvoiceItem> ProjectInvoiceItemCreatedByNavigations { get; set; } = new List<ProjectInvoiceItem>();

    [InverseProperty("ModifedByNavigation")]
    public virtual ICollection<ProjectInvoiceItem> ProjectInvoiceItemModifedByNavigations { get; set; } = new List<ProjectInvoiceItem>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectInvoice> ProjectInvoiceModifiedByNavigations { get; set; } = new List<ProjectInvoice>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectLetterOfCreditComment> ProjectLetterOfCreditCommentCreatedByNavigations { get; set; } = new List<ProjectLetterOfCreditComment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectLetterOfCreditComment> ProjectLetterOfCreditCommentModifiedByNavigations { get; set; } = new List<ProjectLetterOfCreditComment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Project> ProjectModifiedByNavigations { get; set; } = new List<Project>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectPaymentTerm> ProjectPaymentTermCreatedByNavigations { get; set; } = new List<ProjectPaymentTerm>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectPaymentTerm> ProjectPaymentTermModifiedByNavigations { get; set; } = new List<ProjectPaymentTerm>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectProgress> ProjectProgressCreatedByNavigations { get; set; } = new List<ProjectProgress>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectProgress> ProjectProgressModifiedByNavigations { get; set; } = new List<ProjectProgress>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectProgressUser> ProjectProgressUserCreatedByNavigations { get; set; } = new List<ProjectProgressUser>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectProgressUser> ProjectProgressUserModifiedByNavigations { get; set; } = new List<ProjectProgressUser>();

    [InverseProperty("ProjectManager")]
    public virtual ICollection<Project> ProjectProjectManagers { get; set; } = new List<Project>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectSprint> ProjectSprintCreatedByNavigations { get; set; } = new List<ProjectSprint>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectSprint> ProjectSprintModifiedByNavigations { get; set; } = new List<ProjectSprint>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectTm> ProjectTmCreatedByNavigations { get; set; } = new List<ProjectTm>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectTm> ProjectTmModifiedByNavigations { get; set; } = new List<ProjectTm>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectTmassignUser> ProjectTmassignUserCreatedByNavigations { get; set; } = new List<ProjectTmassignUser>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectTmassignUser> ProjectTmassignUserModifiedByNavigations { get; set; } = new List<ProjectTmassignUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectTmattachment> ProjectTmattachmentCreatedByNavigations { get; set; } = new List<ProjectTmattachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectTmattachment> ProjectTmattachmentModifiedByNavigations { get; set; } = new List<ProjectTmattachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectTmimpDate> ProjectTmimpDateCreatedByNavigations { get; set; } = new List<ProjectTmimpDate>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectTmimpDate> ProjectTmimpDateModifiedByNavigations { get; set; } = new List<ProjectTmimpDate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectTmrevision> ProjectTmrevisionCreatedByNavigations { get; set; } = new List<ProjectTmrevision>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectTmrevision> ProjectTmrevisionModifiedByNavigations { get; set; } = new List<ProjectTmrevision>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ProjectTmsprint> ProjectTmsprintCreatedByNavigations { get; set; } = new List<ProjectTmsprint>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectTmsprint> ProjectTmsprintModifiedByNavigations { get; set; } = new List<ProjectTmsprint>();

    [InverseProperty("CreateByNavigation")]
    public virtual ICollection<ProjectWorkFlow> ProjectWorkFlowCreateByNavigations { get; set; } = new List<ProjectWorkFlow>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ProjectWorkFlow> ProjectWorkFlowModifiedByNavigations { get; set; } = new List<ProjectWorkFlow>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PrsupplierOffer> PrsupplierOfferCreatedByNavigations { get; set; } = new List<PrsupplierOffer>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PrsupplierOfferItem> PrsupplierOfferItems { get; set; } = new List<PrsupplierOfferItem>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PrsupplierOffer> PrsupplierOfferModifiedByNavigations { get; set; } = new List<PrsupplierOffer>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PuchasePoshipment> PuchasePoshipmentCreatedByNavigations { get; set; } = new List<PuchasePoshipment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PuchasePoshipment> PuchasePoshipmentModifiedByNavigations { get; set; } = new List<PuchasePoshipment>();

    [InverseProperty("AssignedAccountant")]
    public virtual ICollection<PurchasePo> PurchasePoAssignedAccountants { get; set; } = new List<PurchasePo>();

    [InverseProperty("AssignedPurchasingPerson")]
    public virtual ICollection<PurchasePo> PurchasePoAssignedPurchasingPeople { get; set; } = new List<PurchasePo>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePo> PurchasePoCreatedByNavigations { get; set; } = new List<PurchasePo>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchasePo> PurchasePoModifiedByNavigations { get; set; } = new List<PurchasePo>();

    [InverseProperty("UserIdforFinalApproveNavigation")]
    public virtual ICollection<PurchasePo> PurchasePoUserIdforFinalApproveNavigations { get; set; } = new List<PurchasePo>();

    [InverseProperty("UserIdforTechApproveNavigation")]
    public virtual ICollection<PurchasePo> PurchasePoUserIdforTechApproveNavigations { get; set; } = new List<PurchasePo>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceCalculatedShipmentValue> PurchasePoinvoiceCalculatedShipmentValues { get; set; } = new List<PurchasePoinvoiceCalculatedShipmentValue>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoinvoice> PurchasePoinvoiceCreatedByNavigations { get; set; } = new List<PurchasePoinvoice>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceDeduction> PurchasePoinvoiceDeductionCreatedByNavigations { get; set; } = new List<PurchasePoinvoiceDeduction>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceDeduction> PurchasePoinvoiceDeductionModifiedByNavigations { get; set; } = new List<PurchasePoinvoiceDeduction>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceExtraFee> PurchasePoinvoiceExtraFeeCreatedByNavigations { get; set; } = new List<PurchasePoinvoiceExtraFee>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceExtraFee> PurchasePoinvoiceExtraFeeModifiedByNavigations { get; set; } = new List<PurchasePoinvoiceExtraFee>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceFinalExpensi> PurchasePoinvoiceFinalExpensis { get; set; } = new List<PurchasePoinvoiceFinalExpensi>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchasePoinvoice> PurchasePoinvoiceModifiedByNavigations { get; set; } = new List<PurchasePoinvoice>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceNotIncludedTax> PurchasePoinvoiceNotIncludedTaxCreatedByNavigations { get; set; } = new List<PurchasePoinvoiceNotIncludedTax>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceNotIncludedTax> PurchasePoinvoiceNotIncludedTaxModifiedByNavigations { get; set; } = new List<PurchasePoinvoiceNotIncludedTax>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceTaxIncluded> PurchasePoinvoiceTaxIncludedCreatedByNavigations { get; set; } = new List<PurchasePoinvoiceTaxIncluded>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceTaxIncluded> PurchasePoinvoiceTaxIncludedModifiedByNavigations { get; set; } = new List<PurchasePoinvoiceTaxIncluded>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceTotalOrderCustomFee> PurchasePoinvoiceTotalOrderCustomFees { get; set; } = new List<PurchasePoinvoiceTotalOrderCustomFee>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoinvoiceUnloadingFee> PurchasePoinvoiceUnloadingFees { get; set; } = new List<PurchasePoinvoiceUnloadingFee>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePopaymentSwift> PurchasePopaymentSwiftCreatedByNavigations { get; set; } = new List<PurchasePopaymentSwift>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchasePopaymentSwift> PurchasePopaymentSwiftModifiedByNavigations { get; set; } = new List<PurchasePopaymentSwift>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePopdf> PurchasePopdfCreatedByNavigations { get; set; } = new List<PurchasePopdf>();

    [InverseProperty("EditedByNavigation")]
    public virtual ICollection<PurchasePopdfEditHistory> PurchasePopdfEditHistories { get; set; } = new List<PurchasePopdfEditHistory>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchasePopdf> PurchasePopdfModifiedByNavigations { get; set; } = new List<PurchasePopdf>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePopdfTemplate> PurchasePopdfTemplateCreatedByNavigations { get; set; } = new List<PurchasePopdfTemplate>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchasePopdfTemplate> PurchasePopdfTemplateModifiedByNavigations { get; set; } = new List<PurchasePopdfTemplate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchasePoshipmentDocument> PurchasePoshipmentDocuments { get; set; } = new List<PurchasePoshipmentDocument>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<PurchaseRequest> PurchaseRequestCreatedByNavigations { get; set; } = new List<PurchaseRequest>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<PurchaseRequest> PurchaseRequestModifiedByNavigations { get; set; } = new List<PurchaseRequest>();

    [InverseProperty("ToUser")]
    public virtual ICollection<PurchaseRequest> PurchaseRequestToUsers { get; set; } = new List<PurchaseRequest>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportCcgroup> ReportCcgroups { get; set; } = new List<ReportCcgroup>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportCcuser> ReportCcuserCreatedByNavigations { get; set; } = new List<ReportCcuser>();

    [InverseProperty("User")]
    public virtual ICollection<ReportCcuser> ReportCcuserUsers { get; set; } = new List<ReportCcuser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportGroup> ReportGroups { get; set; } = new List<ReportGroup>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ReportUser> ReportUserCreatedByNavigations { get; set; } = new List<ReportUser>();

    [InverseProperty("User")]
    public virtual ICollection<ReportUser> ReportUserUsers { get; set; } = new List<ReportUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<RequieredCostAttachment> RequieredCostAttachmentCreatedByNavigations { get; set; } = new List<RequieredCostAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<RequieredCostAttachment> RequieredCostAttachmentModifiedByNavigations { get; set; } = new List<RequieredCostAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<RequieredCost> RequieredCostCreatedByNavigations { get; set; } = new List<RequieredCost>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<RequieredCost> RequieredCostModifiedByNavigations { get; set; } = new List<RequieredCost>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Role> RoleCreatedByNavigations { get; set; } = new List<Role>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Role> RoleModifiedByNavigations { get; set; } = new List<Role>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Salary> SalaryCreatedByNavigations { get; set; } = new List<Salary>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalaryDeductionTax> SalaryDeductionTaxCreatedByNavigations { get; set; } = new List<SalaryDeductionTax>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalaryDeductionTax> SalaryDeductionTaxModifiedByNavigations { get; set; } = new List<SalaryDeductionTax>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalaryInsurance> SalaryInsuranceCreatedByNavigations { get; set; } = new List<SalaryInsurance>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalaryInsurance> SalaryInsuranceModifiedByNavigations { get; set; } = new List<SalaryInsurance>();

    [InverseProperty("User")]
    public virtual ICollection<SalaryInsurance> SalaryInsuranceUsers { get; set; } = new List<SalaryInsurance>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Salary> SalaryModifiedByNavigations { get; set; } = new List<Salary>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<SalaryTax> SalaryTaxes { get; set; } = new List<SalaryTax>();

    [InverseProperty("User")]
    public virtual ICollection<Salary> SalaryUsers { get; set; } = new List<Salary>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesBranchProductTarget> SalesBranchProductTargetCreatedByNavigations { get; set; } = new List<SalesBranchProductTarget>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesBranchProductTarget> SalesBranchProductTargetModifiedByNavigations { get; set; } = new List<SalesBranchProductTarget>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesBranchTarget> SalesBranchTargetCreatedByNavigations { get; set; } = new List<SalesBranchTarget>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesBranchTarget> SalesBranchTargetModifiedByNavigations { get; set; } = new List<SalesBranchTarget>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesBranchUserProductTarget> SalesBranchUserProductTargetCreatedByNavigations { get; set; } = new List<SalesBranchUserProductTarget>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesBranchUserProductTarget> SalesBranchUserProductTargetModifiedByNavigations { get; set; } = new List<SalesBranchUserProductTarget>();

    [InverseProperty("User")]
    public virtual ICollection<SalesBranchUserProductTarget> SalesBranchUserProductTargetUsers { get; set; } = new List<SalesBranchUserProductTarget>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesBranchUserTarget> SalesBranchUserTargetCreatedByNavigations { get; set; } = new List<SalesBranchUserTarget>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesBranchUserTarget> SalesBranchUserTargetModifiedByNavigations { get; set; } = new List<SalesBranchUserTarget>();

    [InverseProperty("User")]
    public virtual ICollection<SalesBranchUserTarget> SalesBranchUserTargetUsers { get; set; } = new List<SalesBranchUserTarget>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesMaintenanceOffer> SalesMaintenanceOfferCreatedByNavigations { get; set; } = new List<SalesMaintenanceOffer>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesMaintenanceOffer> SalesMaintenanceOfferModifiedByNavigations { get; set; } = new List<SalesMaintenanceOffer>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferAttachment> SalesOfferAttachmentCreatedByNavigations { get; set; } = new List<SalesOfferAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferAttachmentGroupPermission> SalesOfferAttachmentGroupPermissionCreatedByNavigations { get; set; } = new List<SalesOfferAttachmentGroupPermission>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferAttachmentGroupPermission> SalesOfferAttachmentGroupPermissionModifiedByNavigations { get; set; } = new List<SalesOfferAttachmentGroupPermission>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferAttachment> SalesOfferAttachmentModifiedByNavigations { get; set; } = new List<SalesOfferAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferAttachmentUserPermission> SalesOfferAttachmentUserPermissionCreatedByNavigations { get; set; } = new List<SalesOfferAttachmentUserPermission>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferAttachmentUserPermission> SalesOfferAttachmentUserPermissionModifiedByNavigations { get; set; } = new List<SalesOfferAttachmentUserPermission>();

    [InverseProperty("User")]
    public virtual ICollection<SalesOfferAttachmentUserPermission> SalesOfferAttachmentUserPermissionUsers { get; set; } = new List<SalesOfferAttachmentUserPermission>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOffer> SalesOfferCreatedByNavigations { get; set; } = new List<SalesOffer>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferDiscount> SalesOfferDiscountCreatedByNavigations { get; set; } = new List<SalesOfferDiscount>();

    [InverseProperty("DiscountApprovedByNavigation")]
    public virtual ICollection<SalesOfferDiscount> SalesOfferDiscountDiscountApprovedByNavigations { get; set; } = new List<SalesOfferDiscount>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferDiscount> SalesOfferDiscountModifiedByNavigations { get; set; } = new List<SalesOfferDiscount>();

    [InverseProperty("EditedByNavigation")]
    public virtual ICollection<SalesOfferEditHistory> SalesOfferEditHistories { get; set; } = new List<SalesOfferEditHistory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferExpirationHistory> SalesOfferExpirationHistories { get; set; } = new List<SalesOfferExpirationHistory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferExtraCost> SalesOfferExtraCostCreatedByNavigations { get; set; } = new List<SalesOfferExtraCost>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferExtraCost> SalesOfferExtraCostModifiedByNavigations { get; set; } = new List<SalesOfferExtraCost>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferGroupPermission> SalesOfferGroupPermissionCreatedByNavigations { get; set; } = new List<SalesOfferGroupPermission>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferGroupPermission> SalesOfferGroupPermissionModifiedByNavigations { get; set; } = new List<SalesOfferGroupPermission>();

    [InverseProperty("ByUserNavigation")]
    public virtual ICollection<SalesOfferInternalApproval> SalesOfferInternalApprovalByUserNavigations { get; set; } = new List<SalesOfferInternalApproval>();

    [InverseProperty("User")]
    public virtual ICollection<SalesOfferInternalApproval> SalesOfferInternalApprovalUsers { get; set; } = new List<SalesOfferInternalApproval>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferInvoiceTax> SalesOfferInvoiceTaxCreatedByNavigations { get; set; } = new List<SalesOfferInvoiceTax>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferInvoiceTax> SalesOfferInvoiceTaxModifiedByNavigations { get; set; } = new List<SalesOfferInvoiceTax>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOffer> SalesOfferModifiedByNavigations { get; set; } = new List<SalesOffer>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferPdf> SalesOfferPdfCreatedByNavigations { get; set; } = new List<SalesOfferPdf>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferPdf> SalesOfferPdfModifiedByNavigations { get; set; } = new List<SalesOfferPdf>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferPdfTemplate> SalesOfferPdfTemplateCreatedByNavigations { get; set; } = new List<SalesOfferPdfTemplate>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferPdfTemplate> SalesOfferPdfTemplateModifiedByNavigations { get; set; } = new List<SalesOfferPdfTemplate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferProduct> SalesOfferProductCreatedByNavigations { get; set; } = new List<SalesOfferProduct>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferProduct> SalesOfferProductModifiedByNavigations { get; set; } = new List<SalesOfferProduct>();

    [InverseProperty("SalesPerson")]
    public virtual ICollection<SalesOffer> SalesOfferSalesPeople { get; set; } = new List<SalesOffer>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferTermsAndCondition> SalesOfferTermsAndConditionCreatedByNavigations { get; set; } = new List<SalesOfferTermsAndCondition>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferTermsAndCondition> SalesOfferTermsAndConditionModifiedByNavigations { get; set; } = new List<SalesOfferTermsAndCondition>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesOfferUserPermission> SalesOfferUserPermissionCreatedByNavigations { get; set; } = new List<SalesOfferUserPermission>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesOfferUserPermission> SalesOfferUserPermissionModifiedByNavigations { get; set; } = new List<SalesOfferUserPermission>();

    [InverseProperty("User")]
    public virtual ICollection<SalesOfferUserPermission> SalesOfferUserPermissionUsers { get; set; } = new List<SalesOfferUserPermission>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesRentOffer> SalesRentOfferCreatedByNavigations { get; set; } = new List<SalesRentOffer>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SalesRentOffer> SalesRentOfferModifiedByNavigations { get; set; } = new List<SalesRentOffer>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SalesTarget> SalesTargetCreatedByNavigations { get; set; } = new List<SalesTarget>();

    [InverseProperty("ModifiedNavigation")]
    public virtual ICollection<SalesTarget> SalesTargetModifiedNavigations { get; set; } = new List<SalesTarget>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<ShippingCompanyAttachment> ShippingCompanyAttachmentCreatedByNavigations { get; set; } = new List<ShippingCompanyAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<ShippingCompanyAttachment> ShippingCompanyAttachmentModifiedByNavigations { get; set; } = new List<ShippingCompanyAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Speciality> SpecialityCreatedByNavigations { get; set; } = new List<Speciality>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Speciality> SpecialityModifiedByNavigations { get; set; } = new List<Speciality>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SpecialitySupplier> SpecialitySupplierCreatedByNavigations { get; set; } = new List<SpecialitySupplier>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SpecialitySupplier> SpecialitySupplierModifiedByNavigations { get; set; } = new List<SpecialitySupplier>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Stage> StageCreatedByNavigations { get; set; } = new List<Stage>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Stage> StageModifiedByNavigations { get; set; } = new List<Stage>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SubmittedReport> SubmittedReportCreatedByNavigations { get; set; } = new List<SubmittedReport>();

    [InverseProperty("User")]
    public virtual ICollection<SubmittedReport> SubmittedReportUsers { get; set; } = new List<SubmittedReport>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupplierAccount> SupplierAccountCreatedByNavigations { get; set; } = new List<SupplierAccount>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SupplierAccount> SupplierAccountModifiedByNavigations { get; set; } = new List<SupplierAccount>();

    [InverseProperty("CreatedyByNavigation")]
    public virtual ICollection<SupplierAccountReviewed> SupplierAccountReviewedCreatedyByNavigations { get; set; } = new List<SupplierAccountReviewed>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SupplierAccountReviewed> SupplierAccountReviewedModifiedByNavigations { get; set; } = new List<SupplierAccountReviewed>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupplierAddress> SupplierAddressCreatedByNavigations { get; set; } = new List<SupplierAddress>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SupplierAddress> SupplierAddressModifiedByNavigations { get; set; } = new List<SupplierAddress>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupplierAttachment> SupplierAttachmentCreatedByNavigations { get; set; } = new List<SupplierAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SupplierAttachment> SupplierAttachmentModifiedByNavigations { get; set; } = new List<SupplierAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupplierContactPerson> SupplierContactPersonCreatedByNavigations { get; set; } = new List<SupplierContactPerson>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SupplierContactPerson> SupplierContactPersonModifiedByNavigations { get; set; } = new List<SupplierContactPerson>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Supplier> SupplierCreatedByNavigations { get; set; } = new List<Supplier>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupplierFax> SupplierFaxCreatedByNavigations { get; set; } = new List<SupplierFax>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SupplierFax> SupplierFaxModifiedByNavigations { get; set; } = new List<SupplierFax>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupplierMobile> SupplierMobileCreatedByNavigations { get; set; } = new List<SupplierMobile>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SupplierMobile> SupplierMobileModifiedByNavigations { get; set; } = new List<SupplierMobile>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Supplier> SupplierModifiedByNavigations { get; set; } = new List<Supplier>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupplierPhone> SupplierPhoneCreatedByNavigations { get; set; } = new List<SupplierPhone>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SupplierPhone> SupplierPhoneModifiedByNavigations { get; set; } = new List<SupplierPhone>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupplierSpeciality> SupplierSpecialityCreatedByNavigations { get; set; } = new List<SupplierSpeciality>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<SupplierSpeciality> SupplierSpecialityModifiedByNavigations { get; set; } = new List<SupplierSpeciality>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();

    [InverseProperty("User")]
    public virtual ICollection<TaskApplicationOpen> TaskApplicationOpens { get; set; } = new List<TaskApplicationOpen>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskAssignUser> TaskAssignUserCreatedByNavigations { get; set; } = new List<TaskAssignUser>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskAssignUser> TaskAssignUserModifiedByNavigations { get; set; } = new List<TaskAssignUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskAttachment> TaskAttachmentCreatedByNavigations { get; set; } = new List<TaskAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskAttachment> TaskAttachmentModifiedByNavigations { get; set; } = new List<TaskAttachment>();

    [InverseProperty("User")]
    public virtual ICollection<TaskBrowserTab> TaskBrowserTabs { get; set; } = new List<TaskBrowserTab>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskClosureLog> TaskClosureLogCreatedByNavigations { get; set; } = new List<TaskClosureLog>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskClosureLog> TaskClosureLogModifiedByNavigations { get; set; } = new List<TaskClosureLog>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskCommentAttachment> TaskCommentAttachmentCreatedByNavigations { get; set; } = new List<TaskCommentAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskCommentAttachment> TaskCommentAttachmentModifiedByNavigations { get; set; } = new List<TaskCommentAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskComment> TaskCommentCreatedByNavigations { get; set; } = new List<TaskComment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskComment> TaskCommentModifiedByNavigations { get; set; } = new List<TaskComment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Task> TaskCreatedByNavigations { get; set; } = new List<Task>();

    [InverseProperty("ApprovedByNavigation")]
    public virtual ICollection<TaskExpensi> TaskExpensiApprovedByNavigations { get; set; } = new List<TaskExpensi>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskExpensi> TaskExpensiCreatedByNavigations { get; set; } = new List<TaskExpensi>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskExpensi> TaskExpensiModifiedByNavigations { get; set; } = new List<TaskExpensi>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskInfo> TaskInfoCreatedByNavigations { get; set; } = new List<TaskInfo>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskInfo> TaskInfoModifiedByNavigations { get; set; } = new List<TaskInfo>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskInfoRevision> TaskInfoRevisionCreatedByNavigations { get; set; } = new List<TaskInfoRevision>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskInfoRevision> TaskInfoRevisionModifiedByNavigations { get; set; } = new List<TaskInfoRevision>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Task> TaskModifiedByNavigations { get; set; } = new List<Task>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskPrimarySubCategory> TaskPrimarySubCategoryCreatedByNavigations { get; set; } = new List<TaskPrimarySubCategory>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskPrimarySubCategory> TaskPrimarySubCategoryModifiedByNavigations { get; set; } = new List<TaskPrimarySubCategory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskRequirement> TaskRequirementCreatedByNavigations { get; set; } = new List<TaskRequirement>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskRequirement> TaskRequirementModifiedByNavigations { get; set; } = new List<TaskRequirement>();

    [InverseProperty("User")]
    public virtual ICollection<TaskScreenShot> TaskScreenShots { get; set; } = new List<TaskScreenShot>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskStageHistory> TaskStageHistoryCreatedByNavigations { get; set; } = new List<TaskStageHistory>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskStageHistory> TaskStageHistoryModifiedByNavigations { get; set; } = new List<TaskStageHistory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskType> TaskTypeCreatedByNavigations { get; set; } = new List<TaskType>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskType> TaskTypeModifiedByNavigations { get; set; } = new List<TaskType>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskUnitRateService> TaskUnitRateServiceCreatedByNavigations { get; set; } = new List<TaskUnitRateService>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskUnitRateService> TaskUnitRateServiceModifiedByNavigations { get; set; } = new List<TaskUnitRateService>();

    [InverseProperty("User")]
    public virtual ICollection<TaskUserMonitor> TaskUserMonitors { get; set; } = new List<TaskUserMonitor>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TaskUserReply> TaskUserReplyCreatedByNavigations { get; set; } = new List<TaskUserReply>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TaskUserReply> TaskUserReplyModifiedByNavigations { get; set; } = new List<TaskUserReply>();

    [InverseProperty("RecieverUser")]
    public virtual ICollection<TaskUserReply> TaskUserReplyRecieverUsers { get; set; } = new List<TaskUserReply>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Tax> TaxCreatedByNavigations { get; set; } = new List<Tax>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Tax> TaxModifiedByNavigations { get; set; } = new List<Tax>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Team> TeamCreatedByNavigations { get; set; } = new List<Team>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<Team> TeamModifiedByNavigations { get; set; } = new List<Team>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TermsAndCondition> TermsAndConditionCreatedByNavigations { get; set; } = new List<TermsAndCondition>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TermsAndCondition> TermsAndConditionModifiedByNavigations { get; set; } = new List<TermsAndCondition>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TermsAndConditionsCategory> TermsAndConditionsCategoryCreatedByNavigations { get; set; } = new List<TermsAndConditionsCategory>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TermsAndConditionsCategory> TermsAndConditionsCategoryModifiedByNavigations { get; set; } = new List<TermsAndConditionsCategory>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TermsGroup> TermsGroupCreatedByNavigations { get; set; } = new List<TermsGroup>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TermsGroup> TermsGroupModifiedByNavigations { get; set; } = new List<TermsGroup>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TermsLibrary> TermsLibraryCreatedByNavigations { get; set; } = new List<TermsLibrary>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TermsLibrary> TermsLibraryModifiedByNavigations { get; set; } = new List<TermsLibrary>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransportationLine> TransportationLineCreationByNavigations { get; set; } = new List<TransportationLine>();

    [InverseProperty("ApprovedByNavigation")]
    public virtual ICollection<TransportationLineIncreaseRequest> TransportationLineIncreaseRequestApprovedByNavigations { get; set; } = new List<TransportationLineIncreaseRequest>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransportationLineIncreaseRequest> TransportationLineIncreaseRequestCreationByNavigations { get; set; } = new List<TransportationLineIncreaseRequest>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TransportationLine> TransportationLineModifiedByNavigations { get; set; } = new List<TransportationLine>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransportationVehicle> TransportationVehicleCreationByNavigations { get; set; } = new List<TransportationVehicle>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TransportationVehicle> TransportationVehicleModifiedByNavigations { get; set; } = new List<TransportationVehicle>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransportationVehicleRouteAccount> TransportationVehicleRouteAccountCreationByNavigations { get; set; } = new List<TransportationVehicleRouteAccount>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TransportationVehicleRouteAccount> TransportationVehicleRouteAccountModifiedByNavigations { get; set; } = new List<TransportationVehicleRouteAccount>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransportationVehicleRoute> TransportationVehicleRouteCreationByNavigations { get; set; } = new List<TransportationVehicleRoute>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransportationVehicleRouteDeduction> TransportationVehicleRouteDeductions { get; set; } = new List<TransportationVehicleRouteDeduction>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransportationVehicleRouteDirection> TransportationVehicleRouteDirections { get; set; } = new List<TransportationVehicleRouteDirection>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransportationVehicleRouteEmployeeException> TransportationVehicleRouteEmployeeExceptions { get; set; } = new List<TransportationVehicleRouteEmployeeException>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransportationVehicleRouteEmployee> TransportationVehicleRouteEmployees { get; set; } = new List<TransportationVehicleRouteEmployee>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<TransportationVehicleRoute> TransportationVehicleRouteModifiedByNavigations { get; set; } = new List<TransportationVehicleRoute>();

    [InverseProperty("Supervisor")]
    public virtual ICollection<TransportationVehicleRoute> TransportationVehicleRouteSupervisors { get; set; } = new List<TransportationVehicleRoute>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<TransprotationUserAttedance> TransprotationUserAttedances { get; set; } = new List<TransprotationUserAttedance>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UserPatient> UserPatientCreatedByNavigations { get; set; } = new List<UserPatient>();

    [InverseProperty("CreationByNavigation")]
    public virtual ICollection<UserPatientInsurance> UserPatientInsuranceCreationByNavigations { get; set; } = new List<UserPatientInsurance>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<UserPatientInsurance> UserPatientInsuranceModifiedByNavigations { get; set; } = new List<UserPatientInsurance>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<UserPatient> UserPatientModifiedByNavigations { get; set; } = new List<UserPatient>();

    [InverseProperty("User")]
    public virtual ICollection<UserPatient> UserPatientUsers { get; set; } = new List<UserPatient>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UserRole> UserRoleCreatedByNavigations { get; set; } = new List<UserRole>();

    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoleUsers { get; set; } = new List<UserRole>();

    [InverseProperty("User")]
    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UserTeam> UserTeamCreatedByNavigations { get; set; } = new List<UserTeam>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<UserTeam> UserTeamModifiedByNavigations { get; set; } = new List<UserTeam>();

    [InverseProperty("User")]
    public virtual ICollection<UserTeam> UserTeamUsers { get; set; } = new List<UserTeam>();

    [InverseProperty("User")]
    public virtual ICollection<UserTimer> UserTimers { get; set; } = new List<UserTimer>();

    [InverseProperty("User")]
    public virtual ICollection<UserViewNotification> UserViewNotifications { get; set; } = new List<UserViewNotification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<VacationDay> VacationDayCreatedByNavigations { get; set; } = new List<VacationDay>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<VacationDay> VacationDayModifiedByNavigations { get; set; } = new List<VacationDay>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<VacationOverTimeAndDeductionRate> VacationOverTimeAndDeductionRateCreatedByNavigations { get; set; } = new List<VacationOverTimeAndDeductionRate>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<VacationOverTimeAndDeductionRate> VacationOverTimeAndDeductionRateModifiedByNavigations { get; set; } = new List<VacationOverTimeAndDeductionRate>();

    [InverseProperty("AssignedTo")]
    public virtual ICollection<VisitsScheduleOfMaintenance> VisitsScheduleOfMaintenanceAssignedTos { get; set; } = new List<VisitsScheduleOfMaintenance>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<VisitsScheduleOfMaintenanceAttachment> VisitsScheduleOfMaintenanceAttachmentCreatedByNavigations { get; set; } = new List<VisitsScheduleOfMaintenanceAttachment>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<VisitsScheduleOfMaintenanceAttachment> VisitsScheduleOfMaintenanceAttachmentModifiedByNavigations { get; set; } = new List<VisitsScheduleOfMaintenanceAttachment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<VisitsScheduleOfMaintenance> VisitsScheduleOfMaintenanceCreatedByNavigations { get; set; } = new List<VisitsScheduleOfMaintenance>();

    [InverseProperty("ModifiedByNavigation")]
    public virtual ICollection<VisitsScheduleOfMaintenance> VisitsScheduleOfMaintenanceModifiedByNavigations { get; set; } = new List<VisitsScheduleOfMaintenance>();

    [InverseProperty("ApprovedByNavigation")]
    public virtual ICollection<WorkingHourseTracking> WorkingHourseTrackingApprovedByNavigations { get; set; } = new List<WorkingHourseTracking>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<WorkingHourseTracking> WorkingHourseTrackingCreatedByNavigations { get; set; } = new List<WorkingHourseTracking>();
}
