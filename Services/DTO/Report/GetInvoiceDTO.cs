namespace EBISX_POS.API.Services.DTO.Report
{
    public class GetInvoiceDTO
    {
        // Business Details
        public required string RegisteredName { get; set; }
        public required string Address { get; set; }
        public required string VatTinNumber { get; set; }
        public required string MinNumber { get; set; }

        // Invoice Details
        public required string InvoiceNum { get; set; }
        public required string InvoiceDate { get; set; }
        public required string OrderType { get; set; }
        public required string CashierName { get; set; }

        // Items
        public required List<ItemDTO> Items { get; set; } = new List<ItemDTO>();

        // Totals
        public required string TotalAmount { get; set; }
        public required string DiscountAmount { get; set; }
        public required string DueAmount { get; set; }
        public List<OtherPaymentDTO>? OtherPayments { get; set; } = new List<OtherPaymentDTO>();
        public required string CashTenderAmount { get; set; }
        public required string TotalTenderAmount { get; set; }
        public required string ChangeAmount { get; set; }
        public required string VatExemptSales { get; set; }
        public required string VatSales { get; set; }
        public required string VatAmount { get; set; }
        
        public List<string> ElligiblePeopleDiscounts { get; set; } = new List<string>();

        // POS Details
        public required string PosSerialNumber { get; set; } // serial number of the POS machine.
        public required string DateIssued { get; set; } 
        public required string ValidUntil { get; set; } 
    }

    public class ItemDTO
    {
        public required int Qty { get; set; }
        public List<ItemInfoDTO> itemInfos { get; set; } = new List<ItemInfoDTO>();

    }

    public class ItemInfoDTO
    {
        public required string Description { get; set; }
        public required string Amount { get; set; }
    }

    public class OtherPaymentDTO
    {
        public required string SaleTypeName { get; set; }
        public required string Amount { get; set; }
    }
}
