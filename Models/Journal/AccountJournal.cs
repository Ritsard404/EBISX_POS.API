using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBISX_POS.API.Models.Journal
{
    [Table("accountjournal")]
    public class AccountJournal
    {
        [Key]
        [Column("unique_id")]
        public long UniqueId { get; set; }  // NOT NULL

        [Required]
        [Column("Entry_Type")]
        public string EntryType { get; set; } = "INVOICE";  // NOT NULL

        [Column("Entry_No")]
        public long? EntryNo { get; set; }  // DEFAULT NULL

        [Column("Entry_Line_No")]
        public int? EntryLineNo { get; set; }  // DEFAULT '0', but nullable because NOT NULL isn’t specified

        [Required]
        [Column("Entry_Date")]
        public DateTime EntryDate { get; set; }  // NOT NULL

        [Required]
        [Column("Entry_Name")]
        public string EntryName { get; set; }  // NOT NULL

        [Required]
        [Column("group_id")]
        public string GroupId { get; set; }  // NOT NULL

        [Required]
        [Column("AccountName")]
        public string AccountName { get; set; }  // NOT NULL

        [Required]
        [Column("Description")]
        public string Description { get; set; }  // NOT NULL

        [Required]
        [Column("Reference")]
        public string Reference { get; set; }  // NOT NULL

        [Required]
        [Column("Branch")]
        public string Branch { get; set; }  // NOT NULL

        [Column("TerminalNo")]
        public int? TerminalNo { get; set; }  // DEFAULT '0'

        [Column("Debit")]
        public double? Debit { get; set; }  // DEFAULT '0'

        [Column("Credit")]
        public double? Credit { get; set; }  // DEFAULT '0'

        [Column("AccountBalance")]
        public double? AccountBalance { get; set; }  // DEFAULT '0'

        [Required]
        [Column("Status")]
        public string Status { get; set; }  // NOT NULL

        [Required]
        [Column("cleared")]
        public string Cleared { get; set; }  // NOT NULL

        [Column("clearingref")]
        public int? ClearingRef { get; set; }  // DEFAULT '0'

        [Required]
        [Column("costcenter")]
        public string CostCenter { get; set; }  // NOT NULL

        [Required]
        [Column("accountno")]
        public string AccountNo { get; set; }  // NOT NULL

        [Required]
        [Column("costcenterdesc")]
        public string CostCenterDesc { get; set; }  // NOT NULL

        [Required]
        [Column("linetype")]
        public string LineType { get; set; }  // NOT NULL

        [Column("linetype_transno")]
        public int? LineTypeTransNo { get; set; }  // DEFAULT '0'

        [Required]
        [Column("ItemID")]
        public string ItemID { get; set; }  // NOT NULL

        [Column("ItemDesc")]
        public string ItemDesc { get; set; }  // TEXT, nullable

        [Required]
        [Column("Unit")]
        public string Unit { get; set; }  // NOT NULL

        [Column("QtyIn")]
        public double? QtyIn { get; set; }  // DEFAULT '0'

        [Column("QtyOut")]
        public double? QtyOut { get; set; }  // DEFAULT '0'

        [Column("QtyPerBaseUnit")]
        public double? QtyPerBaseUnit { get; set; }  // DEFAULT '1'

        [Column("QtyBalanceInBaseUnit")]
        public double? QtyBalanceInBaseUnit { get; set; }  // DEFAULT '0'

        [Column("Cost")]
        public double? Cost { get; set; }  // DEFAULT '0'

        [Column("Price")]
        public double? Price { get; set; }  // DEFAULT '0'

        [Column("Discrate")]
        public double? DiscRate { get; set; }  // DEFAULT '0'

        [Column("Discamt")]
        public double? DiscAmt { get; set; }  // DEFAULT '0'

        [Column("TotalCost")]
        public double? TotalCost { get; set; }  // DEFAULT '0'

        [Column("TotalPrice")]
        public double? TotalPrice { get; set; }  // DEFAULT '0'

        [Column("received")]
        public double? Received { get; set; }  // DEFAULT '0'

        [Required]
        [Column("delivered", TypeName = "decimal(10,0)")]
        public decimal Delivered { get; set; }  // NOT NULL

        [Required]
        [Column("tax_id")]
        public string TaxId { get; set; }  // NOT NULL

        [Required]
        [Column("tax_account")]
        public string TaxAccount { get; set; }  // NOT NULL

        [Required]
        [Column("tax_type")]
        public string TaxType { get; set; }  // NOT NULL

        [Column("tax_rate")]
        public double? TaxRate { get; set; }  // DEFAULT '0'

        [Column("tax_total")]
        public double? TaxTotal { get; set; }  // DEFAULT '0'

        [Column("sub_total")]
        public double? SubTotal { get; set; }  // DEFAULT '0'

        [Column("serial")]
        public string Serial { get; set; }  // TEXT, nullable

        [Required]
        [Column("chassis")]
        public string Chassis { get; set; }  // NOT NULL

        [Required]
        [Column("engine")]
        public string Engine { get; set; }  // NOT NULL

        [Required]
        [Column("itemtype")]
        public string ItemType { get; set; }  // NOT NULL

        [Column("serialstatus")]
        public int? SerialStatus { get; set; }  // DEFAULT '0'

        [Required]
        [Column("expirydate")]
        public DateTime ExpiryDate { get; set; }  // NOT NULL

        [Required]
        [Column("batchno")]
        public string BatchNo { get; set; }  // NOT NULL

        [Required]
        [Column("itemcolor")]
        public string ItemColor { get; set; }  // NOT NULL

        [Column("converted")]
        public int? Converted { get; set; }  // DEFAULT '0'

        [Required]
        [Column("vattype")]
        public string VatType { get; set; }  // NOT NULL

        [Column("vatable")]
        public double? Vatable { get; set; }  // DEFAULT '0'

        [Column("exempt")]
        public double? Exempt { get; set; }  // DEFAULT '0'

        [Column("nonvatable")]
        public double? NonVatable { get; set; }  // DEFAULT '0'

        [Column("zerorated")]
        public double? ZeroRated { get; set; }  // DEFAULT '0'

        [Required]
        [Column("income_account")]
        public string IncomeAccount { get; set; }  // NOT NULL

        [Required]
        [Column("cogs_account")]
        public string CogsAccount { get; set; }  // NOT NULL

        [Required]
        [Column("inventory_account")]
        public string InventoryAccount { get; set; }  // NOT NULL

        [Column("job_no")]
        public string JobNo { get; set; }  // DEFAULT NULL

        [Required]
        [Column("job_desc")]
        public string JobDesc { get; set; }  // NOT NULL

        [Required]
        [Column("name_type")]
        public string NameType { get; set; }  // NOT NULL

        [Required]
        [Column("docref")]
        public string DocRef { get; set; }  // NOT NULL

        [Required]
        [Column("name_desc")]
        public string NameDesc { get; set; }  // NOT NULL

        [Column("length", TypeName = "decimal(12,2)")]
        public decimal? Length { get; set; }  // DEFAULT '0.00'

        [Column("width", TypeName = "decimal(12,2)")]
        public decimal? Width { get; set; }  // DEFAULT '0.00'

        [Column("area", TypeName = "decimal(12,2)")]
        public decimal? Area { get; set; }  // DEFAULT '0.00'

        [Column("perimeter", TypeName = "decimal(12,2)")]
        public decimal? Perimeter { get; set; }  // DEFAULT '0.00'

        [Required]
        [Column("fgid")]
        public string Fgid { get; set; }  // NOT NULL

        [Required]
        [Column("illumination")]
        public string Illumination { get; set; }  // NOT NULL

        [Required]
        [Column("size")]
        public string Size { get; set; }  // NOT NULL

        [Required]
        [Column("face")]
        public string Face { get; set; }  // NOT NULL

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }  // NOT NULL

        [Required]
        [Column("location")]
        public string Location { get; set; }  // NOT NULL

        [Column("principal")]
        public double? Principal { get; set; }  // DEFAULT '0'

        [Column("interest")]
        public double? Interest { get; set; }  // DEFAULT '0'

        [Column("penalty")]
        public double? Penalty { get; set; }  // DEFAULT '0'

        [Column("total_loan_amount")]
        public double? TotalLoanAmount { get; set; }  // DEFAULT '0'

        [Column("penalty_rate")]
        public double? PenaltyRate { get; set; }  // DEFAULT '0'

        [Column("penalty_term")]
        public int? PenaltyTerm { get; set; }  // DEFAULT '0'

        [Required]
        [Column("penalty_period")]
        public string PenaltyPeriod { get; set; }  // NOT NULL

        [Required]
        [Column("bank")]
        public string Bank { get; set; }  // NOT NULL

        [Required]
        [Column("check_number")]
        public string CheckNumber { get; set; }  // NOT NULL

        [Required]
        [Column("amountinwords")]
        public string AmountInWords { get; set; }  // NOT NULL

        [Required]
        [Column("voucher_date")]
        public DateTime VoucherDate { get; set; }  // NOT NULL

        [Required]
        [Column("ship_to")]
        public string ShipTo { get; set; }  // NOT NULL

        [Required]
        [Column("ship_to_name")]
        public string ShipToName { get; set; }  // NOT NULL

        [Required]
        [Column("clerk")]
        public string Clerk { get; set; }  // NOT NULL

        [Column("requested", TypeName = "decimal(12,2)")]
        public decimal? Requested { get; set; }  // DEFAULT '0.00'

        [Column("entry_time")]
        public TimeSpan? EntryTime { get; set; }  // DEFAULT '00:00:00'

        [Required]
        [Column("prodno")]
        public string ProdNo { get; set; }  // NOT NULL

        [Required]
        [Column("dispensed")]
        public string Dispensed { get; set; }  // NOT NULL

        [Required]
        [Column("prev_reading")]
        public int PrevReading { get; set; }  // NOT NULL

        [Required]
        [Column("curr_reading")]
        public int CurrReading { get; set; }  // NOT NULL

        [Required]
        [Column("consumption")]
        public int Consumption { get; set; }  // NOT NULL

        [Required]
        [Column("memo")]
        public string Memo { get; set; }  // NOT NULL

        [Required]
        [Column("mobilization")]
        public string Mobilization { get; set; }  // NOT NULL

        [Required]
        [Column("room_id")]
        public string RoomId { get; set; }  // NOT NULL

        [Required]
        [Column("date_start")]
        public DateTime DateStart { get; set; }  // NOT NULL

        [Required]
        [Column("wtax_rate", TypeName = "decimal(12,2)")]
        public decimal WTaxRate { get; set; }  // NOT NULL

        [Required]
        [Column("wtax_amount", TypeName = "decimal(12,2)")]
        public decimal WTaxAmount { get; set; }  // NOT NULL

        [Required]
        [Column("block_no")]
        public string BlockNo { get; set; }  // NOT NULL

        [Required]
        [Column("lot_no")]
        public string LotNo { get; set; }  // NOT NULL

        [Required]
        [Column("interest_days")]
        public int InterestDays { get; set; }  // NOT NULL

        [Required]
        [Column("daily_interest", TypeName = "decimal(12,2)")]
        public decimal DailyInterest { get; set; }  // NOT NULL

        [Required]
        [Column("interest_rate", TypeName = "decimal(12,2)")]
        public decimal InterestRate { get; set; }  // NOT NULL

        [Required]
        [Column("interest_period_start")]
        public int InterestPeriodStart { get; set; }  // NOT NULL

        [Required]
        [Column("loan_no_of_payments")]
        public int LoanNoOfPayments { get; set; }  // NOT NULL

        [Required]
        [Column("loan_payments_interval")]
        public int LoanPaymentsInterval { get; set; }  // NOT NULL

        [Required]
        [Column("loan_payments_term")]
        public string LoanPaymentsTerm { get; set; }  // NOT NULL

        [Required]
        [Column("savings", TypeName = "decimal(12,2)")]
        public decimal Savings { get; set; }  // NOT NULL

        [Required]
        [Column("cbu", TypeName = "decimal(12,2)")]
        public decimal Cbu { get; set; }  // NOT NULL

        [Required]
        [Column("collector")]
        public string Collector { get; set; }  // NOT NULL

        [Required]
        [Column("insurance", TypeName = "decimal(12,2)")]
        public decimal Insurance { get; set; }  // NOT NULL

        [Required]
        [Column("commission", TypeName = "decimal(12,2)")]
        public decimal Commission { get; set; }  // NOT NULL

        [Required]
        [Column("collector_fee", TypeName = "decimal(12,2)")]
        public decimal CollectorFee { get; set; }  // NOT NULL

        [Required]
        [Column("company_id")]
        public string CompanyId { get; set; }  // NOT NULL
    }
}
