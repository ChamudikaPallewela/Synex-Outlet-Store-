using System;

namespace Client.DTO
{
    public class BillReportItemDto
    {
        public int BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CashTendered { get; set; }
        public decimal ChangeDue { get; set; }
    }
}