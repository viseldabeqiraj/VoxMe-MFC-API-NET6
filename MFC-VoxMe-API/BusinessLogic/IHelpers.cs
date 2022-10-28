using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe_API.BusinessLogic
{
    public interface IHelpers
    {
        AssignMaterialsToTransactionDto GetTransactionMaterials();
        CreateTransactionDto CreateTransactionObjectFromXml();
        CreateJobDto CreateJobObjectFromXml();
        MovingData XMLParse(string xml);
        AssignStaffDesignateForemanDto GetTransactionResources();
    }
}
