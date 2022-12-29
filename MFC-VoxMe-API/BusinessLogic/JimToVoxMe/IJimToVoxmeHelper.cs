using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Infrastructure.Models;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe_API.BusinessLogic.JimToVoxMe
{
    public interface IJimToVoxmeHelper
    {
        AssignMaterialsToTransactionDto GetTransactionMaterials();
        CreateTransactionDto CreateTransactionObjectFromXml();
        CreateJobDto CreateJobObjectFromXml();
        MovingDataDto XMLParse(string xml);
        AssignStaffDesignateForemanDto GetTransactionResources();
        Task InsertTableRecords();
        byte[] GetDoc();
        List<string> GetCorrelatingDocuments(MovingDataDto movingData);
    }
}
