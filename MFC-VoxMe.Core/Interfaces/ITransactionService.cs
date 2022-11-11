using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;
using Microsoft.AspNetCore.Http;

namespace MFC_VoxMe_API.Services.Transactions
{
    public interface ITransactionService
    {
        Task<HttpResponseDto<TransactionDetailsDto>> GetDetails(string externalRef);
        Task<HttpResponseDto<CreateTransactionDto>> CreateTransaction(CreateTransactionDto createTransactionRequest);
        Task<HttpResponseDto<TransactionSummaryDto>> GetSummary(string externalRef);
        Task<HttpResponseDto<UpdateTransactionDto>> UpdateTransaction(string externalRef,UpdateTransactionDto updateTransactionRequest);
        Task<HttpResponseDto<List<TransactionDownloadDetails>>> GetDownloadDetails(string externalRef);
        Task<bool> AddDocumentToTransaction(IFormFile File, string DocTitle, string externalRef);
        Task<HttpResponseDto<AssignStaffDesignateForemanDto>> AssignStaffDesignateForeman(AssignStaffDesignateForemanDto request, string externalRef);
        Task<HttpResponseDto<bool>> RemoveResourceFromTransaction(string externalRef);
        Task<HttpResponseDto<ResourceCodesForTransactionDto>> AssignResourcesToTransaction(ResourceCodesForTransactionDto request, string externalRef);
        Task<HttpResponseDto<AssignMaterialsToTransactionDto>> AssignMaterialsToTransaction(AssignMaterialsToTransactionDto request, string externalRef);
        Task<HttpResponseDto<bool>> RemoveMaterialsFromTransaction(string externalRef);
    }
}
