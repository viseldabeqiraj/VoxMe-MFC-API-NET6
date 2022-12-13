using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe_API.BusinessLogic.VoxMeToJim
{
    public interface IVoxmeToJimHelper
    {
        Task<dynamic> GetMovingDataId(string externalRef);
        List<KeyValuePair<string, string>> GetImages(HttpResponseDto<TransactionDetailsDto> transactiondetails);
        Task<string> GetItemsPath(int movingDataId);
        Task UpdateMovingData(string externalRef);
        Task InsertDataFromJobDetails(JobDetailsDto jobDetails, int movingDataId);
        string GetValueFromJsonConfig(string key);
        Task<int> testc();
    }
}
