/* ============================================================================
   Drops the ERP tables the transportation module never touches.

   OPTIONAL — the app runs fine without this. It only trims a restored backup
   of the full Garas ERP down to what this project actually uses.

   52 tables, each of which satisfies ALL THREE of:
     1. zero rows
     2. no foreign key points at it
     3. its name appears nowhere in the C# source

   Condition 3 is what makes this safe. An earlier pass that used only the
   repository/DbSet references flagged 202 tables — including UserSession,
   which the code does use through a path that scan missed. Matching against
   every identifier in the source cut the list to these 52.

   Modules covered: BOM, e-invoicing, purchasing, HR, project management.

   BACK UP FIRST:
     BACKUP DATABASE [GARASElWaseem] TO DISK='C:\SqlRestoreefore-cleanup.bak' WITH INIT, COMPRESSION;

   Then set the database name below and run. Re-running is harmless.
   ============================================================================ */

USE [GARASElWaseem];   -- <<< change to your database
GO

SET NOCOUNT ON;
DECLARE @dropped int = 0, @skipped int = 0;

IF OBJECT_ID('dbo.[BOMAttachments]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[BOMAttachments]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[BOMHistory]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[BOMHistory]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[BOMImages]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[BOMImages]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[BOMLibrary]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[BOMLibrary]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[BOMPartitionAttachments]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[BOMPartitionAttachments]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[BOMPartitionHistory]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[BOMPartitionHistory]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[BOMPartitionItemAttachments]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[BOMPartitionItemAttachments]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[BOMProduct]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[BOMProduct]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[ClientNATIONAL]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[ClientNATIONAL]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[EInvoiceAttachment]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[EInvoiceAttachment]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[EInvoiceCompanyActivity]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[EInvoiceCompanyActivity]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[EInvoiceSetting]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[EInvoiceSetting]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[HRCustodyReportAttachment]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[HRCustodyReportAttachment]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[HRDeductionRewarding]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[HRDeductionRewarding]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[HREmployeeAttachment]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[HREmployeeAttachment]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[HRLoan]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[HRLoan]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[HRUserWarning]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[HRUserWarning]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[InventoryItemUOM]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[InventoryItemUOM]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[InvoiceCNAndDN]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[InvoiceCNAndDN]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[POApprovalStatus]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[POApprovalStatus]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[POApprovalUser]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[POApprovalUser]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[POFinalSelecteSupplier]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[POFinalSelecteSupplier]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PricingBOM]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PricingBOM]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[ProjectInstallationBOQ]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[ProjectInstallationBOQ]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[ProjectTMAssignUser]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[ProjectTMAssignUser]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[ProjectTMAttachment]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[ProjectTMAttachment]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[ProjectTMImpDate]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[ProjectTMImpDate]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[ProjectTMRevision]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[ProjectTMRevision]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[ProjectTMSprint]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[ProjectTMSprint]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PRSupplierOfferItem]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PRSupplierOfferItem]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PuchasePOShipment]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PuchasePOShipment]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchaseImportPOSetting]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchaseImportPOSetting]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOAmountPaymentMethod]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOAmountPaymentMethod]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInactiveTask]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInactiveTask]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInvoiceCalculatedShipmentValue]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInvoiceCalculatedShipmentValue]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInvoiceClosedPayment]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInvoiceClosedPayment]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInvoiceDeduction]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInvoiceDeduction]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInvoiceExtraFees]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInvoiceExtraFees]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInvoiceFinalExpensis]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInvoiceFinalExpensis]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInvoiceNotIncludedTax]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInvoiceNotIncludedTax]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInvoiceTaxIncluded]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInvoiceTaxIncluded]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInvoiceTotalOrderCustomFee]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInvoiceTotalOrderCustomFee]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOInvoiceUnloadingFee]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOInvoiceUnloadingFee]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOPaymentSwift]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOPaymentSwift]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOPdf]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOPdf]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOPdfEditHistory]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOPdfEditHistory]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOPdfTemplate]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOPdfTemplate]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[PurchasePOShipmentDocuments]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[PurchasePOShipmentDocuments]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[Region]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[Region]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[ReportCCGroup]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[ReportCCGroup]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[ReportCCUser]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[ReportCCUser]; SET @dropped += 1; END
ELSE SET @skipped += 1;
IF OBJECT_ID('dbo.[Sheet2$]', 'U') IS NOT NULL
    BEGIN DROP TABLE dbo.[Sheet2$]; SET @dropped += 1; END
ELSE SET @skipped += 1;

PRINT CONCAT('dropped: ', @dropped, '   already absent: ', @skipped);
GO

-- Sanity check — these must all still be here:
SELECT name, (SELECT SUM(p.rows) FROM sys.partitions p
              WHERE p.object_id = t.object_id AND p.index_id IN (0,1)) AS [rows]
FROM sys.tables t
WHERE name IN ('TransportationVehicleRoute','TransportationLine','TransportationVehicle',
               'TransportationVehicleRouteEmployee','TransprotationUserAttedance',
               'HrUser','Supplier','User','UserSession','Role')
ORDER BY name;
GO
