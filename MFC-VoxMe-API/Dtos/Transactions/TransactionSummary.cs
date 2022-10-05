namespace MFC_VoxMe_API.Dtos.Transactions
{
    public class TransactionSummary
    {
        public string externalRef { get; set; }
        public string onsiteStatus { get; set; }
        public string transactionStatus { get; set; }
        public string transactionType { get; set; }
        public string uniqueId { get; set; }
        public string updateDate { get; set; }
    }
}
