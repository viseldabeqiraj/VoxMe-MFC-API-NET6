using MFC_VoxMe.Core.Dtos.Common;
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
        Task<MovingDataDto> XMLParseAsync(string xml);
        AssignStaffDesignateForemanDto GetTransactionResources();
<<<<<<<< HEAD:MFC-VoxMe-API/BusinessLogic/JimToVoxMe/IHelpers.cs
        public byte[] GetDoc();
========
        Task InsertTableRecords();
        byte[] GetDoc();

>>>>>>>> origin/feature/MFCToJIM:MFC-VoxMe-API/BusinessLogic/JimToVoxMe/IJimToVoxmeHelper.cs
    }
}
