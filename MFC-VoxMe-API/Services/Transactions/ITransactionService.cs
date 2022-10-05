using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe_API.Services.Transactions
{
    public interface ITransactionService
    {
        Task<TransactionDetailsDto> GetDetails(string externalRef);
        Task<CreateTransactionDto> CreateTransaction(CreateTransactionDto createTransactionRequest);
        Task<TransactionSummary> GetSummary(string externalRef);
        Task<UpdateTransactionDto> UpdateJob(UpdateTransactionDto updateTransactionRequest);
        Task<TransactionDownloadDetails> GetDownloadDetails(string externalRef);
    }
}
