namespace DataLibrary.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        
        public int PaymentTypeId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string CashierName { get; set; }

        public string RegisterNumber { get; set; }

        public decimal Total { get; set; }

        public string Description { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}
