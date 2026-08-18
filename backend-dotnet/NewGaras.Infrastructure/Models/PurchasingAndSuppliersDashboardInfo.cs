namespace NewGaras.Infrastructure.Models
{
    public class PurchasingAndSuppliersDashboardInfo
    {
        public decimal PurchasedMaterialTotalAmount { get; set; }
        public int PurchasedMaterialPONo { get; set; }
        public int PurchasedMaterialPOItemNo { get; set; }
        public int PurchasedMaterialPOSupplierNo { get; set; }
        //------PR
        public int PROpenNo { get; set; }
        public int PROpenFromStoreNo { get; set; }
        public int PRItemOpenNo { get; set; }
        public int PRClosedNo { get; set; }
        public int PRClosedFromStoreNo { get; set; }
        public int PRItemClosedNo { get; set; }

        // PR  Assign
        public int TotalPRItemNo { get; set; }
        public int AssignedPRItemNo { get; set; }
        public float AssignedPRItemPercent { get; set; }
        public int NoPersonAssignedPRItem {  get; set; }
        public List<PersonAssignedPO> PersonAssingedPOList { get; set; }
        // PR Not Assigned
        public int NOTAssignedPRItemNo { get; set; }
        public float NOTAssignedPRItemPercent { get; set; }

        //-----PO
        public int PoOpenNo { get; set; }
        public int PoItemOpenNo { get; set; }
        public int PoOpenFromSupplierNo { get; set; }
        public int PoClosedNo { get; set; }
        public int PoItemClosedNo { get; set; }
        public int PoItemClosedFromSupplierNo { get; set; }
        // -----PO Invoices 

        public int NOPOInvoiceForOpenPO { get; set; }
        public int NOPOMissedInvoiceForOpenPO { get; set; }
        public decimal TotalCostPOInvoiceForOpenPO { get; set; }

        public int NOPOInvoiceForClosedPO { get; set; }
        public int NOPOMissedInvoiceForClosedPO { get; set; }
        public decimal TotalCostPOInvoiceForClosedPO { get; set; }

        public decimal TotalInvoicesAmountForPO { get; set; }
        // Inventory Data Expired Itmes - low Stocks
        public int ExpiredItemsNo { get; set; }
        public decimal ExpiredItemsTotalAmount { get; set; }
        public long LowStockItemsNo { get; set; }
        public decimal TotalLowStockItems {  get; set; }

        // External Back Order
        public int ExternalBackOrdersNO { get; set; }
        public int ExternalBackOrdersItemsNO { get; set; }
        public int ExternalBackOrdersReturnedItemsNO { get; set; }
        public int ExternalBackOrdersToSupplierNO { get; set; }
        public decimal ExternalBackOrdersTotalAmountCost {  get; set; }

        // Supplier
        public int NoOfSupplier {  get; set; }
        // Supplier Accounts
        public decimal SupplierAccountsPercentPaid { get; set; }
        public decimal SupplierAccountsPercentRemain { get; set; }
        public decimal SupplierAccountsTotalAmountPaid { get; set; }
        public decimal SupplierAccountsTotalAmountRemain { get; set; }
    }
}