namespace EBISX_POS.API.Services.DTO.Report
{
    public class ZInvoiceReportDTO
    {
        // Business Info
        public string BusinessName { get; set; } = string.Empty;
        public string OperatorName { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string VatRegTin { get; set; } = string.Empty;
        public string Min { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;

        // Report Info
        public string ReportDate { get; set; } = string.Empty;
        public string ReportTime { get; set; } = string.Empty;
        public string StartDateTime { get; set; } = string.Empty;
        public string EndDateTime { get; set; } = string.Empty;

        // Serial/Reference Numbers
        public string BeginningSiNumber { get; set; } = string.Empty;
        public string EndingSiNumber { get; set; } = string.Empty;
        public string BeginningVoidNumber { get; set; } = string.Empty;
        public string EndingVoidNumber { get; set; } = string.Empty;
        public string BeginningReturnNumber { get; set; } = string.Empty;
        public string EndingReturnNumber { get; set; } = string.Empty;

        // Counters
        public string ResetCounterNo { get; set; } = string.Empty;
        public string ZCounterNo { get; set; } = string.Empty;

        // Accumulated Sales
        public string PresentAccumulatedSales { get; set; } = string.Empty;
        public string PreviousAccumulatedSales { get; set; } = string.Empty;
        public string SalesForTheDay { get; set; } = string.Empty;

        // Breakdown of Sales
        public string VatableSales { get; set; } = string.Empty;
        public string VatAmount { get; set; } = string.Empty;
        public string VatExemptSales { get; set; } = string.Empty;
        public string ZeroRatedSales { get; set; } = string.Empty;
        public string GrossAmount { get; set; } = string.Empty;
        public string Discount { get; set; } = string.Empty;
        public string Return { get; set; } = string.Empty;
        public string Void { get; set; } = string.Empty;
        public string VatAdjustment { get; set; } = string.Empty;
        public string NetAmount { get; set; } = string.Empty;

        // Discount Summary
        public string ScDiscount { get; set; } = string.Empty;
        public string PwdDiscount { get; set; } = string.Empty;
        public string NaacDiscount { get; set; } = string.Empty;
        public string SoloParentDiscount { get; set; } = string.Empty;
        public string OtherDiscount { get; set; } = string.Empty;

        // VAT Adjustments
        public string ScVatTrans { get; set; } = string.Empty;
        public string PwdVatTrans { get; set; } = string.Empty;
        public string RegDiscVatTrans { get; set; } = string.Empty;
        public string ZeroRatedTrans { get; set; } = string.Empty;
        public string VatOnReturn { get; set; } = string.Empty;
        public string OtherVatAdjustments { get; set; } = string.Empty;

        // Transaction Summary
        public string CashInDrawer { get; set; } = string.Empty;
        public List<PaymentItemString> OtherPayments { get; set; } = new();
        public string OpeningFund { get; set; } = string.Empty;
        public string Withdrawal { get; set; } = string.Empty;
        public string PaymentsReceived { get; set; } = string.Empty;

        // Short/Over
        public string ShortOver { get; set; } = string.Empty;
    }

    public class PaymentItemString
    {
        public string Name { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
    }

}
