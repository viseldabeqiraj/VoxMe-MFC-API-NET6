using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe_API.BusinessLogic.VoxMeToJim
{
    public interface IVoxmeToJimHelper
    {
        Task<dynamic> GetMovingDataId(string externalRef);
        void CreateFileInFolder(string filePath, byte[] bytes);
        void DeleteFilesFromFolder(List<string> serverPaths);
        List<KeyValuePair<string, string>> GetImages(JobDetailsDto jobDetails,TransactionDetailsDto transactiondetails);
        Task<string> GetItemsPath(int movingDataId);
        Task UpdateMovingData(string externalRef);
        Task InsertDataFromJobDetails(JobDetailsDto jobDetails,int movingDataId);
        Task InsertDataFromTransactionDetails(TransactionDetailsDto details, int movingDataId);
        string GetValueFromJsonConfig(string key);
        Task DeleteTables(int movingDataID);
        Task UpdateMovingDataStatus(int state, int movingDataID);
        Task<int> testc();
    }
}
