using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe_API.BusinessLogic.JimToVoxMe
{
    public interface IHelpers
    {
        AssignMaterialsToTransactionDto GetTransactionMaterials();
        CreateTransactionDto CreateTransactionObjectFromXml();
        CreateJobDto CreateJobObjectFromXml();
        MovingDataDto XMLParse(string xml);
        AssignStaffDesignateForemanDto GetTransactionResources();
        //void InsertMovingDataRecords<T>() where T : class;
     }
}
