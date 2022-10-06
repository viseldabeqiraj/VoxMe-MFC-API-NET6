using MFC_VoxMe_API.Dtos.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace MFC_VoxMe_API.Services.Transactions
{
    public interface ITransactionService
    {
        Task<TransactionDetailsDto> GetDetails(string externalRef);
        Task<CreateTransactionDto> CreateTransaction(CreateTransactionDto createTransactionRequest);
        Task<TransactionSummary> GetSummary(string externalRef);
        Task<UpdateTransactionDto> UpdateTransaction(UpdateTransactionDto updateTransactionRequest);
        Task<TransactionDownloadDetails> GetDownloadDetails(string externalRef);
        Task<bool> AddDocumentToTransaction(IFormFile File, string DocTitle, string externalRef);
        Task<AssignStaffDesignateForemanDto> AssignStaffDesignateForeman(AssignStaffDesignateForemanDto request, string externalRef);
        Task<bool> RemoveResourceFromTransaction(List<string> resourceCodes, string externalRef);
        Task<AssignResourcesToTransactionDto> AssignResourcesToTransaction(AssignResourcesToTransactionDto request, string externalRef);
        Task<AssignMaterialsToTransactionDto> AssignMaterialsToTransaction(AssignMaterialsToTransactionDto request, string externalRef);
        Task<bool> RemoveMaterialsFromTransaction(List<string> resourceCodes, string externalRef);
    }
}
