# جداول غير مستخدمة في مشروع النقل

قاعدة البيانات `GARASElWaseem` فيها **492** جدول لأنها نسخة من الـ ERP الكامل
(مخازن، مشتريات، حسابات، فنادق، عيادات…) بينما المشروع ده بيستخدم موديول النقل فقط.

**الطريقة:** استخرجت كل `_unitOfWork.X` و `_context.X` من كود الخدمات والكنترولرات،
حوّلتها للكيانات ومنها لأسماء الجداول، وقارنتها بجداول قاعدة البيانات + عدد الصفوف +
المفاتيح الأجنبية اللي بتشاور على كل جدول.

| التصنيف | العدد |
|---|---|
| يستخدمها الكود | **64** |
| غير مستخدمة · فاضية · مفيش FK — **آمنة للحذف** | **202** |
| غير مستخدمة · فاضية · عليها FK — تحتاج ترتيب | **87** |
| غير مستخدمة · **فيها بيانات** — يُفضّل تركها | **139** |

> ⚠️ لم أحذف أي شيء. القوائم للعرض فقط.

---

## 1) آمنة للحذف — فاضية ومفيش حاجة بتشاور عليها (202 جدول)

```
AccountFinancialPeriod
AccountFinancialPeriodAttachment
AccountOfAdjustingEntry
AccountOfJournalEntryOtherCurrency
BankChequeTemplate
BankDetail
BOMAttachments
BOMHistory
BOMImages
BOMLibrary
BOMPartitionAttachments
BOMPartitionHistory
BOMPartitionItemAttachments
BOMProduct
BranchSettings
BundleModule
CarsAttachment
Childerns
City
ClientBankAccount
ClientConsultantEmail
ClientConsultantSpecialilty
ClientExtraInfo
ClientInformations
ClientLanguagees
ClientNATIONAL
ClientPaymentTerm
Collects
ConfirmedRecieveAndReleaseAttachment
ContractReportTo
DailyAdjustingEntry
DailyReportAttachment
DailyReportExpense
DailyTranactionAttachment
DailyTranactionBeneficiaryToUser
DailyTransaction
DailyTransactionCostCenter
DriversAttachment
EInvoiceAttachment
EInvoiceCompanyActivity
EInvoiceSetting
EmailAttachment
EmailCategory
EmailCc
EmailReceivers
ExchangeRate
ExpensessStatuses
ExtraCostLibrary
Extras
GarasClientInfo
GF_Users
GroupRole
HRCustodyReportAttachment
HRDeductionRewarding
HREmployeeAttachment
HRLoan
HRUserWarning
ImportantDate
InsuranceCompanyNames
InventoryAddingOrderItems
InventoryInternalTransferOrderItems
InventoryItemAttachment
InventoryItemUOM
InvoiceCNAndDN
InvoiceExtraCost
InvoiceExtraModification
InvoiceNewClient
InvoiceTax
LaboratoryMessagesReport
MaintenanceReportAttachment
MaintenanceReportClarificationAttachment
MaintenanceReportExpenses
MaintenanceReportUsers
ManagementOfRentOrderAttachment
MedicalDailyTreasuryBalance
MedicalExaminationOffer
Module
MovementReportAttachment
NotificationSubscription
OverTimeAndDeductionRate
Payroll
POApprovalStatus
POApprovalUser
POFinalSelecteSupplier
PosClosingDay
PricingBOM
PricingClarificationAttachment
PricingExtraCost
PricingProductAttachment
PricingTerm
ProjectAttachment
ProjectCheque
ProjectContactPerson
ProjectFabricationAttachment
ProjectFabricationJobTitle
ProjectFabricationOrderUsers
ProjectFabricationReportAttachment
ProjectFabricationReportClarificationAttachment
ProjectFabricationReportUsers
ProjectFinishInstallationAttachment
ProjectInstallationAttachment
ProjectInstallationBOQ
ProjectInstallationJobTitle
ProjectInstallationReportAttachment
ProjectInstallationReportClarificationAttachment
ProjectInstallationReportUsers
ProjectInstallationVersion
ProjectInstallAttachment
ProjectInvoiceCollected
ProjectInvoiceItem
ProjectLetterOfCreditComment
ProjectPaymentJournalEntry
ProjectPaymentTerms
ProjectProgressUsers
ProjectTMAssignUser
ProjectTMAttachment
ProjectTMImpDate
ProjectTMRevision
ProjectTMSprint
PRSupplierOfferItem
PuchasePOShipment
PurchaseImportPOSetting
PurchasePOAmountPaymentMethod
PurchasePOInactiveTask
PurchasePOInvoiceCalculatedShipmentValue
PurchasePOInvoiceClosedPayment
PurchasePOInvoiceDeduction
PurchasePOInvoiceExtraFees
PurchasePOInvoiceFinalExpensis
PurchasePOInvoiceNotIncludedTax
PurchasePOInvoiceTaxIncluded
PurchasePOInvoiceTotalOrderCustomFee
PurchasePOInvoiceUnloadingFee
PurchasePOPaymentSwift
PurchasePOPdf
PurchasePOPdfEditHistory
PurchasePOPdfTemplate
PurchasePOShipmentDocuments
Rates
Region
ReportCCGroup
ReportCCUser
ReportGroup
ReportUser
RequieredCostAttachment
ReservationInvoice
RoleModule
RoomFacilities
RoomServices
RoomsReservationChilderns
RoomsReservationMeals
RoomsReservations
SalaryAllownces
SalaryDeductionTax
SalaryInsurance
SalesBranchProductTarget
SalesBranchUserProductTarget
SalesMaintenanceOffer
SalesOfferAttachmentGroupPermission
SalesOfferAttachmentUserPermission
SalesOfferDiscount
SalesOfferEditHistory
SalesOfferExpirationHistory
SalesOfferExtraCosts
SalesOfferGroupPermission
SalesOfferInvoiceTax
SalesOfferItemAttachment
SalesOfferLocation
SalesOfferPdf
SalesOfferPdfTemplate
SalesOfferTermsAndConditions
SalesOfferUserPermission
SalesRentOffer
Sheet2$
StatusReservations
SubmittedReport
SupplierAccountReviewed
TaskApplicationOpen
TaskAssignUser
TaskAttachment
TaskBrowserTab
TaskClosureLog
TaskCommentAttachment
TaskDetails
TaskExpensis
TaskFlagsOwnerReciever
TaskHistory
TaskInfoRevision
TaskRequirement
TaskScreenShot
TaskSecondarySubCategory
TaskStageHistory
TaskUnitRateService
TaskUserMonitor
TermsAndConditions
TermsLibrary
TransportationLineIncreaseRequestLines
UserPatientInsurance
UserTimer
VacationOverTimeAndDeductionRates
VisitsScheduleOfMaintenanceAttachment
WorkingHours
```

## 2) فاضية لكن عليها مفاتيح أجنبية (87 جدول)

فاضية برضه، بس جداول تانية بتشاور عليها — لازم تتحذف بترتيب أو تتشال الـ FK الأول.

| الجدول | جداول بتشاور عليه |
|---|---|
| AttendancePaySlip | 1 |
| BillingType | 1 |
| Buildings | 2 |
| Cars | 2 |
| CategoryType | 1 |
| ConfirmedRecieveAndRelease | 1 |
| DayType | 2 |
| DeductionType | 1 |
| DeliveryAndShippingMethod | 2 |
| DoctorRoom | 2 |
| DoctorSchedule | 2 |
| Drivers | 4 |
| Email | 4 |
| Facilities | 1 |
| Holiday | 1 |
| HRCustody | 1 |
| HRCustodyStatus | 1 |
| HRLoanApprovalStatus | 1 |
| HRLoanRefundStrategy | 1 |
| HRLoanStatus | 1 |
| HRUserWarningActionPlanApproval | 1 |
| HRUserWarningStatus | 1 |
| InventoryAddingOrder | 1 |
| InventoryInternalTransferOrder | 1 |
| InventoryItemContent | 1 |
| Languagees | 1 |
| LetterOfCreditType | 1 |
| MaintenanceReport | 4 |
| MaintenanceReportClarification | 1 |
| ManagementOfMaintenanceOrder | 2 |
| ManagementOfRentOrder | 1 |
| ManageStages | 1 |
| MealTypes | 1 |
| MedicalDoctorPercentageType | 1 |
| MedicalDoctorScheduleStatus | 1 |
| MedicalPatientType | 1 |
| MedicalReservation | 1 |
| MovementReport | 1 |
| MovementsAndDeliveryOrder | 2 |
| National | 1 |
| PaymentTerms | 1 |
| POApprovalSetting | 3 |
| PosNumber | 1 |
| PricingClearfication | 1 |
| PricingProduct | 2 |
| ProjectCostingType | 1 |
| ProjectFabricationBOQ | 1 |
| ProjectFabricationReport | 3 |
| ProjectFabricationReportClarification | 1 |
| ProjectInstallation | 4 |
| ProjectInstallationReport | 4 |
| ProjectInstallationReportClarification | 1 |
| ProjectInvoice | 2 |
| ProjectLetterOfCredit | 1 |
| ProjectSprint | 1 |
| ProjectTM | 5 |
| ProjectWorkFlow | 1 |
| PRSupplierOffer | 1 |
| PurchasePO | 18 |
| PurchasePOAttachment | 1 |
| PurchasePOInvoice | 7 |
| PurchasePOInvoiceAttachment | 2 |
| PurchasePOItem | 2 |
| Report | 5 |
| RequieredCost | 1 |
| Reservations | 9 |
| Rooms | 6 |
| RoomTypes | 2 |
| RoomViews | 2 |
| Salary | 4 |
| SalaryType | 2 |
| SalesOfferAttachment | 2 |
| SpecialitySupplier | 1 |
| Stages | 2 |
| SupplierAccounts | 1 |
| TaskComment | 2 |
| TaskInfo | 6 |
| TaskUserReply | 1 |
| TaxType | 1 |
| TermsAndConditionsCategory | 1 |
| TermsGroups | 2 |
| TypeServices | 1 |
| UserPatient | 1 |
| VacationDay | 1 |
| VacationPaymentStrategy | 1 |
| VisitsScheduleOfMaintenance | 2 |
| WeekDays | 3 |

## 3) غير مستخدمة لكن فيها بيانات (139 جدول)

موديولات تانية من الـ ERP. الحذف مش هيأثر على النقل لكن هتخسر البيانات — الأفضل تركها.

| الجدول | عدد الصفوف |
|---|---|
| InvoiceItems | 30455 |
| AccountOfJournalEntry | 21500 |
| InventoryMatrialRequestItems | 19192 |
| InventoryStoreItem | 13810 |
| SalesOfferProduct | 12759 |
| SalesOfferInternalApproval | 8064 |
| InventoryMatrialReleaseItems | 7004 |
| DailyJournalEntry | 6661 |
| ClientAccounts | 5534 |
| PurchaseRequestItems | 5159 |
| UserSession | 5026 |
| InventoryMatrialRequest | 4727 |
| InventoryItem | 3718 |
| GeneralActiveCostCenters | 3121 |
| SalesOffer | 2694 |
| SalesOfferProductTax | 2542 |
| VehicleMaintenanceJobOrderHistory | 2362 |
| InventoryMatrialRelease | 1931 |
| InventoryReportItemParent | 1861 |
| Invoices | 1772 |
| InventoryReportItems | 1688 |
| PurchaseRequest | 1586 |
| VehicleModel | 1413 |
| VehiclePerClient | 1358 |
| VehicleBodyType | 1309 |
| ClientSalesPerson | 786 |
| InventoryItemPrice | 623 |
| ClientSpeciality | 514 |
| Accounts | 397 |
| AdvanciedSettingAccount | 307 |
| TransportationVehicleRouteAccounts | 210 |
| VehicleMaintenanceTypeServiceSheduleCategory | 203 |
| VehicleBrand | 172 |
| CRMReport | 166 |
| DailyJournalEntryReverse | 162 |
| Product | 144 |
| ClientAttachment | 141 |
| InventoryUOM | 109 |
| ProAuto_AccountTreeTEMP | 99 |
| AdvanciedTypeSettingCSV | 83 |
| … | باقي 99 جدول |


---

## ما تم حذفه فعليًا (2026-08-18)

حُذف **52** جدول فقط — وهي التي اجتازت **ثلاثة** شروط معًا:

1. فاضية (صفر صفوف)
2. مفيش أي مفتاح أجنبي بيشاور عليها
3. **اسمها مش موجود في أي مكان في كود C#** — ده الشرط الإضافي اللي ضيّق القائمة
   من 202 إلى 52. أُضيف بعد ما اكتشفنا إن `UserSession` ظهر "غير مستخدم"
   في التحليل الأول رغم إن الكود بيستخدمه فعلًا.

كلها من موديولات مالهاش علاقة بالنقل: BOM، الفواتير الإلكترونية، المشتريات،
شؤون الموظفين، المشاريع.

```
﻿BOMAttachments
BOMHistory
BOMImages
BOMLibrary
BOMPartitionAttachments
BOMPartitionHistory
BOMPartitionItemAttachments
BOMProduct
ClientNATIONAL
EInvoiceAttachment
EInvoiceCompanyActivity
EInvoiceSetting
HRCustodyReportAttachment
HRDeductionRewarding
HREmployeeAttachment
HRLoan
HRUserWarning
InventoryItemUOM
InvoiceCNAndDN
POApprovalStatus
POApprovalUser
POFinalSelecteSupplier
PricingBOM
ProjectInstallationBOQ
ProjectTMAssignUser
ProjectTMAttachment
ProjectTMImpDate
ProjectTMRevision
ProjectTMSprint
PRSupplierOfferItem
PuchasePOShipment
PurchaseImportPOSetting
PurchasePOAmountPaymentMethod
PurchasePOInactiveTask
PurchasePOInvoiceCalculatedShipmentValue
PurchasePOInvoiceClosedPayment
PurchasePOInvoiceDeduction
PurchasePOInvoiceExtraFees
PurchasePOInvoiceFinalExpensis
PurchasePOInvoiceNotIncludedTax
PurchasePOInvoiceTaxIncluded
PurchasePOInvoiceTotalOrderCustomFee
PurchasePOInvoiceUnloadingFee
PurchasePOPaymentSwift
PurchasePOPdf
PurchasePOPdfEditHistory
PurchasePOPdfTemplate
PurchasePOShipmentDocuments
Region
ReportCCGroup
ReportCCUser
Sheet2$
```

**نسخة احتياطية قبل الحذف:** `C:\SqlRestore\GARASElWaseem-before-cleanup.bak`

**التحقق بعد الحذف:** تسجيل الدخول + 9 endpoints رئيسية (لوحة المعلومات، الخطوط،
محطات التجمع، المركبات، أنواع المركبات، الخصومات، إعادة التسعير، تقرير التكاليف،
دفعات الموردين) كلها رجّعت `Result: true`، ولوحة المعلومات بنفس الأرقام.

### لم يُحذف

- **150 جدول فاضي** اسمه بيظهر في الكود أو عليه مفاتيح أجنبية — الحذف ممكن يكسر حاجة.
- **139 جدول فيها بيانات حقيقية** (فواتير، مخازن، قيود يومية…) — الحذف هنا خسارة بيانات.
